using System.Collections.ObjectModel;
using System.Formats.Tar;
using System.IO.Compression;
using System.Xml;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Voracious.Database.Interface;
using Voracious.RDF.Enum;
using Voracious.RDF.Extension;
using Voracious.RDF.Model;

namespace Voracious.Database;

public partial class CardCatalog : ICardCatalog
{
    private ILogger<CardCatalog> logger;

    private CatalogDataContext Catalogdb { get; }

    private Resource book;

    private int SaveAfterNFiles = 0;
    private int SaveSkipCount = 100;

    private int UiAfterNNodes = 0;
    private int NodeReadCount = 100;

    const int MaxFilesChecked = 9999999;

    private List<Resource> Books = [];
    private List<Resource> ExistingBooks;

    private List<Creator> Creators = [];

    private ObservableCollection<FileFormat> Files = [];

    public static DateOnly LastUpdateDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(-31);

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    /// <param name="bookdb">The database context for the application</param>
    public CardCatalog(
        ILoggerFactory factory,
        CatalogDataContext bookdb)
    {
        logger = factory.CreateLogger<CardCatalog>();
        Catalogdb = bookdb;
        Catalogdb.SaveChangesFailed += Bookdb_SaveChangesFailed;
    }

    /// <summary>
    /// Raised when a commit fails
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Bookdb_SaveChangesFailed(object? sender, Microsoft.EntityFrameworkCore.SaveChangesFailedEventArgs e)
    {
        var ex = e.Exception;
        throw ex;
    }

    #region Update the Card Catalog from Project Gutenberg
    /// <summary>
    /// Read through the Gutenberg catalog and update new issues or
    /// add new entries.
    /// </summary>
    /// <returns>The count of updated entries</returns>
    public async Task<int> UpdateCatalogAsync()
    {
        // FAIL: Gutenberg includes bad files
        HashSet<string> KnownBadFiles =
        [
            "cache/epub/0/pg0.rdf",
            "cache/epub/999999/pg999999.rdf",
        ];

        var startTime = DateTime.Now;
        await DownloadCatalog();
        var downloadTime = startTime - DateTime.Now;

        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Voracious");
        Directory.CreateDirectory(folder);
        Directory.SetCurrentDirectory(folder);

        string zipPath = "rdf-files.tar.zip";
        try
        {
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                archive.Entries[0].ExtractToFile("rdf-files.tar", true);
                using (StreamReader sr = new StreamReader(File.OpenRead("rdf-files.tar")))
                {
                    TarReader tarReader = new(sr.BaseStream);
                    TarEntry entry = null;
                    if (Catalogdb.Database.HasPendingModelChanges())
                    {
                        await Catalogdb.Database.MigrateAsync();
                    }
                    Catalogdb.Database.EnsureCreated();
                    startTime = DateTime.Now;
                    ExistingBooks = Catalogdb.Resources.ToList();
                    Books = ExistingBooks;
                    var catalogLoadTime = startTime - DateTime.Now;

                    int TotalBooks = Books.Count;
                    while ((entry = tarReader.GetNextEntry()) != null)
                    {
                        if (!KnownBadFiles.Contains(entry.Name))
                        {
                            if (entry.Name.ToLower().Contains("epub"))
                            {
                                // Reads and saves to database. And does a fancy merge if needed.
                                int newCount = Books.Count;
                                if (newCount % 1000 == 0)
                                {
                                    Catalogdb.SaveChanges();
                                }
                                try
                                {
                                    newCount = ReadRdfFileAndInsert(entry.DataStream.ReadAllText());
                                }
                                catch (Exception rdfex)
                                {
                                    // Do what on exception?
                                    logger.LogError($"Name: {entry.Name} exception {rdfex.Message}");
                                    newCount = 0;
                                }
                            }
                        }
                    }
                    Catalogdb.SaveChanges();
                }
            }
        }
        catch (Exception readEx)
        {
            logger.LogError($"Reading Gutenberg ZIP file exception {readEx.Message}");
        }

        return Books.Count;
    }

    /// <summary>
    /// A possibly long-running task to download a ZIP file 
    /// contains a TAR file or RDF files that provide the raw data 
    /// for each book. See the RdfReader for the companion 
    /// reader.
    /// </summary>
    /// <returns></returns>
    private async Task<bool> DownloadCatalog()
    {
        string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Voracious");
        Directory.CreateDirectory(folder);
        Directory.SetCurrentDirectory(folder);

        var catalog = new FileInfo("rdf-files.tar.zip");
        if (catalog.Exists)
        {
            // we only download the catalog every month
            if (DateOnly.FromDateTime(catalog.LastWriteTime) >= DateOnly.FromDateTime(DateTime.UtcNow))
            {
                return true;
            }
            //catalog.Delete();
        }

        using (var hc = new HttpClient())
        {
            try
            {
                var resp = await hc.GetAsync("https://www.gutenberg.org/cache/epub/feeds/rdf-files.tar.zip");
                if (resp.IsSuccessStatusCode)
                {
                    using FileStream outputStream = File.OpenWrite(catalog.Name);
                    {
                        using (var stream = resp.Content.ReadAsStream())
                        {
                            stream.CopyTo(outputStream);
                        }
                    }
                }
                else
                {
                    catalog.Delete();
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Trying to download latest catalog");
                catalog.Delete();
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Reads in data form the XML file and potentially adds it to the database 
    /// via a fancy merge.
    /// </summary>
    /// <param name="xmlData"></param>
    /// <returns></returns>
    private int ReadRdfFileAndInsert(string xmlData)
    {
        // This is a state machine, which means that some
        // pretty important parts are just a little bit
        // buried. When a book is done, take a look at
        // "pgterms:ebook" which has ExtractBook, some fix
        // up, and then saves the book in the database
        // but only if it's valid.
        int retval = 0;
        var doc = new XmlDocument();
        doc.LoadXml(xmlData);
        foreach (var childNode in doc.ChildNodes) // there is just the one
        {
            var node = childNode as XmlNode;
            switch (node.Name)
            {
                case "rdf:RDF":
                    foreach (var possibleBook in node.ChildNodes)
                    {
                        var bookNode = possibleBook as XmlNode;
                        if (bookNode == null) continue;
                        switch (bookNode.Name)
                        {
                            case "pgterms:ebook":
                                var book = ExtractResource(bookNode);

                                // A book might be invalid. For example, Gutenberg includes
                                // ebooks/0 in the RDF catalogger.LogError even though it doesn't exist.
                                // Books with no downloads aren't considered real books.
                                if (book != null) //"" is OK (yes, it's weird)
                                {
                                    // Actually save the book! (possibly with a fancy merge)
                                    // A fast update just adds new books; a full update will
                                    // merge in data as needed.
                                    var dbbook = ExistingBooks.FirstOrDefault(r => r.About == book.About);
                                    if (dbbook == null)
                                    {
                                        Books.Add(book);
                                        Catalogdb.Resources.Add(book);
                                    }
                                    else
                                    {
                                        if (book.Issued > dbbook.Issued)
                                        {
                                            Catalogdb.Resources.Update(book);
                                        }
                                    }
                                }
                                break;

                            // Ignored nodes is ignored
                            case "cc:Work":
                                break;
                            case "rdf:Description":
                                break;
                            default:
                                logger.LogError($"XML: ebook: {bookNode.Name} but just expected pgterms:ebook");
                                break;
                        }
                    }
                    break;
                case "xml": // <?xml version="1.0" encoding="utf-8"?>
                    break;
                default:
                    logger.LogError($"XML: rdf:rdf: {node.Name} but just expected rdf:RDF");
                    break;
            }
        }
        return retval;
    }
    #endregion

    #region Import pubs
    /// <summary>
    /// Import a folder of EPUB files into the card catalog
    /// </summary>
    /// <param name="folderPath">The folder of epubs to import</param>
    /// <returns></returns>
    public async Task ImportEpubFolder(string folderPath)
    {

    }
    #endregion

    #region Convert nodes to card catalog models and properties
    // Bad MARC records
    private int NUnknownMarcRecords = 0;

    /// <summary>
    /// Extract the resource entry from the Catalog
    /// </summary>
    /// <param name="node">The main xml node for the catalog</param>
    /// <returns>The resource view model</returns>
    private Resource ExtractResource(XmlNode node)
    {
        var book = new Resource();
        var id = node.Attributes["rdf:about"]?.Value;
        if (!string.IsNullOrEmpty(id))
        {
            book.About = id;
        }
        else
        {
            logger.LogError($"BookId: missing rdf:about with an id?");
            return null;
        }
        FileFormat txtFormat = null;
        var haveEpub = false;
        foreach (var cn in node.ChildNodes)
        {
            var value = cn as XmlNode;
            if (value == null) continue;
            Creator person = null;
            switch (value.Name)
            {
                case "dcterms:alternative": // <...>Alice in Wonderland</...>
                    book.TitleAlternative = value.InnerText;
                    break;
                case "dcterms:creator":
                    person = ExtractCreator(value);
                    if (person != null)
                    {
                        book.Creators.Add(person);
                    }
                    break;
                case "dcterms:description": // <...>Illustrated by the author.</...>
                    book.Description = value.InnerText;
                    break;
                case "dcterms:hasFormat": //  direct info about how to download a book
                    var format = ExtractHasFormat(value);
                    if (format.Extent > 0)
                    {
                        // Gutenberg has a bunch of badly-made files which end up as zero size.
                        // In all cases where the extent is zero, the actual file on project gutenberg in fact
                        // exists but has no bytes. There's no point in adding a catalogger.LogError entry for something that
                        // can't actually show up.
                        var ftype = format.GetFileType();
                        switch (ftype)
                        {
                            case ProcessedFileEnum.TextNotUtf8:
                            case ProcessedFileEnum.Text:
                                txtFormat = format; // stash this away to make the EPUB type
                                break;
                            case ProcessedFileEnum.EPub:
                            case ProcessedFileEnum.EPubNoImages:
                                haveEpub = true;
                                break;
                        }
                        //book.Files.Add(format);
                    }
                    break;
                case "dcterms:issued": // <... rdf:datatype="http://www.w3.org/2001/XMLSchema#date">2015-03-06</...>
                    book.Issued = Convert.ToDateTime(value.InnerText); // e.g. 1997-12-01 or None
                    break;
                case "dcterms:language":
                    book.Language = ExtractLanguageValue(value);
                    break;
                case "dcterms:license": // <dcterms:license rdf:resource="license"/>
                    book.License = value.Value;
                    break;
                case "dcterms:publisher": // <dcterms:publisher>Project Gutenberg</dcterms:publisher>
                    book.Publisher = value.Value;
                    break;
                case "dcterms:rights": // <dcterms:rights>Public domain in the USA.</dcterms:rights>
                    book.Rights = value.Value;
                    break;
                case "dcterms:subject": // <dcterms:rights>Public domain in the USA.</dcterms:rights>
                    (SubjectTypeEnum subjecttype, string subjectdata) subject = ExtractSubjectValue(value);
                    switch (subject.subjecttype)
                    {
                        case SubjectTypeEnum.LCC:
                            if (!string.IsNullOrEmpty(book.LCC)) book.LCC += ",";
                            book.LCC += subject.subjectdata;
                            break;
                        case SubjectTypeEnum.LCSH:
                            if (!string.IsNullOrEmpty(book.LCSH)) book.LCSH += ",";
                            book.LCSH += subject.subjectdata;
                            break;
                        default:
                            logger.LogError($"ERROR: unable to understand dcterms:subject {value.InnerText}");
                            break;
                    }
                    break;
                case "dcterms:tableOfContents":
                    // Alas: these are often not actually very useful. The epubs have correct TOC 
                    // with links set up.
                    break;
                case "dcterms:title":
                    // <dcterms:title>Three Little Kittens</dcterms:title>
                    book.Title = value.InnerText;
                    break;
                case "dcterms:type":
                    var bookType = ExtractType(value, "???");
                    switch (bookType)
                    {
                        case "Collection":
                            book.BookType = FileTypeEnum.Collection;
                            break;
                        case "Dataset":
                            book.BookType = FileTypeEnum.Dataset;
                            break;
                        // Dataset from e.g. http://www.gutenberg.org/ebooks/3503
                        // These are e.g. the human genome project. They are 100% uninteresting.
                        case "Image":
                            book.BookType = FileTypeEnum.Image;
                            break;
                        case "MovingImage":
                            book.BookType = FileTypeEnum.MovingImage;
                            break;
                        case "Sound":
                            book.BookType = FileTypeEnum.Sound;
                            break;
                        case "StillImage":
                            book.BookType = FileTypeEnum.StillImage;
                            break;
                        case "Text": // OK, this is normal
                            break;
                        default:
                            logger.LogError($"Unknown book type {bookType}");
                            break;
                    }
                    break;
                // https://www.loc.gov/marc/relators/relaterm.html
                case "marcrel:adp": // 
                case "marcrel:aft": // author forward
                case "marcrel:ann": // annotator
                case "marcrel:arr": // arranger
                case "marcrel:art": // 
                case "marcrel:aui": // author of introduction
                case "marcrel:aut": // author (not used?)
                case "marcrel:clb": // collaborator
                case "marcrel:cmm": // commentator
                case "marcrel:cmp": // composer
                case "marcrel:cnd": // 
                case "marcrel:com": // compiler
                case "marcrel:ctb": // contributor
                case "marcrel:dub": // 
                case "marcrel:edc": // editor of compilation
                case "marcrel:edt": // editor 
                case "marcrel:egr": // 
                case "marcrel:ill": // illustrator
                case "marcrel:lbt": // libretist
                case "marcrel:oth": // other
                case "marcrel:pbl": // publisher
                case "marcrel:pht": // 
                case "marcrel:prf": // performer
                case "marcrel:prt": // printer
                case "marcrel:res": // 
                case "marcrel:trc": // 
                case "marcrel:trl": // translator
                case "marcrel:unk": // 
                    person = ExtractCreator(value);
                    if (person != null)
                    {
                        book.Creators.Add(person);
                    }
                    break;
                case "pgterms:bookshelf":
                    break;
                case "pgterms:downloads": // <... rdf:datatype="http://www.w3.org/2001/XMLSchema#integer">11</...>
                    break;

                // almost always the LCCN number
                case "pgterms:marc010":
                    book.LCCN = value.InnerText;
                    break;

                // e.g. The Charles Dickens Edition for pg766.rdf
                case "pgterms:marc250":
                    book.PGEditionInfo = value.InnerText;
                    break;

                // <...>Houston: Advantage International, The PaperLess Readers Club, 1992</...>
                case "pgterms:marc260":
                    book.Imprint = value.InnerText;
                    break;

                // e.g. The Pony Rider Boys, number 1 for pg6067.rdf
                case "pgterms:marc440":
                    book.BookSeries = value.InnerText;
                    break;

                // e.g. EBook produced by David Starner and Heather Martino for pg12962.rdf
                case "pgterms:marc508":
                    book.PGProducedBy = value.InnerText;
                    break;

                // e.g. This ebook uses a beginning of the 20th century spelling. for pg17193.rdf
                case "pgterms:marc546":
                    book.CreationProductionCreditsNote = value.InnerText;
                    break;

                case "pgterms:marc300": // with 8 diagrams
                case "pgterms:marc520": // A fun and wonderfully illustrated version of 
                case "pgterms:marc902": // http://www.gutenberg.org/dirs/8/7/8/8789/8789-h/images/titlepage.jpg 
                case "pgterms:marc903": // http://www.gutenberg.org/files/22761/22761-page-images/cover.tif
                case "pgterms:marc904": // original: https://archive.org/details/worldsinmakingev00arrhuoft/page/n5/mode/2up
                case "pgterms:marc905": // uniqueified author? e.g. 20210306045855reynolds
                case "pgterms:marc906": // original year? e.g. 1923
                case "pgterms:marc907": // Country? e.g. US us UK GB FR NY es United States
                    // Not really unknown, just uninteresting to me: logger.LogError($"Unknown marc: {value.Name} == {value.InnerText} for {logger.LogErrorname}");
                    break;

                case "pgterms:marc020": // Mystery number: 0-397-00033-2
                    // Don't care: logger.LogError($"Unknown marc: {value.Name} == {value.InnerText} for {logger.LogErrorname}");
                    break;

                // Front cover e.g. http://www.gutenberg.org/files/3859/3859-h/images/cover.jpg
                case "pgterms:marc901":
                    break;

                default:
                    if (NUnknownMarcRecords == 0)
                    {
                        logger.LogError($"XML: unknown XML item. Individual RDF files are at e.g. https://www.gutenberg.org/cache/epub/35426/pg35426.rdf");
                        logger.LogError($"XML: Gutenberg offline catalogger.LogError information at https://www.gutenberg.org/ebooks/offline_catalogger.LogErrors.html#xmlrdf");
                    }
                    logger.LogError(value.InnerText);
                    NUnknownMarcRecords++;
                    break;
            }
            book.CleanupFileResourceAs();
        }

        // Do a little validation
        // Correction: don't do this validation. Too many Gutenberg
        // "books" in the catalog are just plain
        // errors. OTOH, they are o
        //if (book.Files.Count == 0)
        //{
        //    logger.LogError($"XML: book has no files");
        //}
        return book;
    }

    /// <summary>
    /// Get the creators of the book
    /// </summary>
    /// <param name="parentnode">The creator rdf node</param>
    /// <returns>The Person model for the creator</returns>
    private Creator ExtractCreator(XmlNode parentnode)
    {
        Creator retval = new();
        try
        {
            string? str = string.Empty;
            var node = parentnode["pgterms:agent"] as XmlNode;
            if (node == null)
            {
                // Some books (like 1813) just don't have all this set up.
                logger.LogError($"ExtractCreator doesn't have a pgterms:agent for {parentnode.InnerText}");
                return null;
            }
            else
            {
                retval.About = (node.Attributes["rdf:about"] as XmlAttribute).InnerText;
            }
            if (string.IsNullOrEmpty(retval.About.Trim()))
            {
                logger.LogError($"ExtractCreator has no key {parentnode.Name}");
                return null;
            }
            var exists = Creators.FirstOrDefault(p => p.About == retval.About);
            if (exists != null)
            {
                return exists;
            }

            // e.g. marcrel:aui == author of introductions
            RelatorEnum relator = retval.ToRelator(parentnode.Name);
            if (relator == RelatorEnum.otherError)
            {
                logger.LogError($"ExtractCreator has unknown realtor {parentnode.Name}");
                return null;
            }
            retval.Role = relator;

            // Get the death date
            str = (node["pgterms:deathdate"] as XmlNode)?.InnerText;
            if (!string.IsNullOrEmpty(str))
            {
                retval.DeathDate = int.Parse(str);
            }

            // Get the birthdate
            str = (node["pgterms:birthdate"] as XmlNode)?.InnerText;
            if (!string.IsNullOrEmpty(str))
            {
                retval.BirthDate = int.Parse(str);
            }

            // Get the author web page
            str = (node["pgterms:webpage"] as XmlNode)?.Attributes?.GetNamedItem("rdf:resource")?.Value;
            if (!string.IsNullOrEmpty(str))
            {
                retval.Webpage = str;
            }

            // The alias for the person
            str = (node["pgterms:alias"] as XmlNode)?.InnerText;
            if (!string.IsNullOrEmpty(str))
            {
                retval.AddAlias(str);
            }

            // The author name
            str = (node["pgterms:name"] as XmlNode)?.InnerText?.Trim();
            if (!string.IsNullOrEmpty(str))
            {
                retval.Name = str;
            }
            retval.FileAs = retval?.Name?.Trim().ToUpper();
        }
        catch (Exception)
        {
            logger.LogError($"Unable to extract person from {parentnode.Value}");
            return null;
        }
        Creators.Add(retval);
        return retval;
    }

    /// <summary>
    /// Given a dcterms:hasFormat blob, extract the file 
    /// format + file location data.
    /// </summary>
    private FileFormat ExtractHasFormat(XmlNode node)
    {
        bool extentIsZero = false;
        var retval = new FileFormat();
        int nchild = 0;
        try
        {
            foreach (var childFileObj in node.ChildNodes)
            {
                var childFile = childFileObj as XmlNode;
                if (childFile == null)
                {
                    logger.LogError($"HasFormat child isn't an XmlNode with {node.InnerText}");
                    continue;
                }
                nchild++;
                if (nchild > 1)
                {
                    logger.LogError($"HasFormat has too many child with {node.InnerText}");
                    continue;
                }

                retval.FileName = childFile.Attributes["rdf:about"].Value; // The super critical part!
                //var formatData = Catalogdb.Files.FirstOrDefault(f => f.FileName == retval.FileName);
                //if (formatData != null)
                //{
                //    return formatData;
                //}
                foreach (var valueObj in childFile.ChildNodes)
                {
                    var value = valueObj as XmlNode;
                    if (value == null)
                    {
                        logger.LogError($"HasFormat grandchild isn't an XmlNode with {childFile.InnerText}");
                        continue;
                    }
                    switch (value.Name)
                    {
                        case "dcterms:format":
                            foreach (var descriptionObj in value.ChildNodes)
                            {
                                var description = descriptionObj as XmlNode;
                                if (description == null)
                                {
                                    logger.LogError($"HasFormat description grandchild isn't an XmlNode with {value.InnerText}");
                                    continue;
                                }
                                foreach (var dvalueObj in description.ChildNodes)
                                {
                                    var dvalue = dvalueObj as XmlNode;
                                    if (dvalue == null)
                                    {
                                        logger.LogError($"HasFormat description grandchild isn't an XmlNode with {description.InnerText}");
                                        continue;
                                    }
                                    switch (dvalue.Name)
                                    {
                                        case "rdf:value":
                                            retval.MimeType = dvalue.InnerText;
                                            break;
                                        case "dcam:memberOf":
                                            break;
                                        default:
                                            logger.LogError($"Unknown member {dvalue.Name}");
                                            break;
                                    }
                                }
                            }
                            break;
                        case "dcterms:modified":
                            if (retval.LastModified != null)
                            {
                                retval.LastModified = Convert.ToDateTime(value.InnerText.Trim());
                            }
                            break;
                        case "dcterms:extent":
                            if (retval.Extent == -1)
                            {
                                retval.Extent = int.Parse(value.InnerText);
                            }
                            if (retval.Extent == 0) extentIsZero = true;
                            break;
                        default:
                            logger.LogError($"HasFormat unknown child {value.Name}");
                            break;
                    }
                }
            }
        }
        catch (Exception)
        {
            logger.LogError($"Unable to extract hasFormat from {node.Value}");
        }

        if (retval.FileName == "")
        {
            logger.LogError($"HasFormat: doesn't include filename");
        }

        if (retval.Extent < 1 && !extentIsZero)
        {
            logger.LogError($"HasFormat: doesn't include extent for {retval.FileName}");
        }

        if (retval.LastModified == null)
        {
            logger.LogError($"HasFormat: doesn't include modified");
        }
        if (!retval.IsKnownMimeType) 
        { 
            logger.LogError($"HasFormat: Unknown mime type {retval.MimeType}"); 
        }
        return retval;
    }

    /// <summary>
    /// Get the language of the epub
    /// </summary>
    /// <param name="node">The language node</param>
    /// <param name="defaultValue">THe default language</param>
    /// <returns>The string for the language</returns>
    private string ExtractLanguageValue(XmlNode node, string defaultValue = "en")
    {
        string returnValue = defaultValue;
        try
        {
            var descrNode = node["rdf:Description"] as XmlNode;
            if (descrNode != null)
            {
                var language = descrNode["rdf:value"];
                returnValue = language.InnerText.ToLower(); // APRESS can have this as En instead of en
            }
            else
            {
                var inner = node.InnerText;
                if (inner.Length >= 2 && inner.Length <= 2)
                {
                    returnValue = inner.ToLower(); // APRESS can have this as En instead of en
                }
                else
                {
                    logger.LogError($"Unrecognized inner-text language {inner} from {node.Value}");
                }
            }
        }
        catch (Exception)
        {
            logger.LogError($"Unable to extract language from {node.Value}");
        }
        return returnValue;
    }

    /// <summary>
    /// Return the SubjectTypeEnum for the epub
    /// </summary>
    /// <param name="node">The subject node</param>
    /// <param name="defaultValue">The default node</param>
    /// <returns>The subject type and string for the node</returns>
    private (SubjectTypeEnum, string) ExtractSubjectValue(XmlNode node, string defaultValue = "")
    {
        var subjectType = SubjectTypeEnum.Other;
        string retval = defaultValue;
        try
        {
            var description = node["rdf:Description"] as XmlNode;
            retval = description["rdf:value"].InnerText;
            var subjectString = description["dcam:memberOf"].GetAttribute("rdf:resource");
            switch (subjectString)
            {
                case "http://purl.org/dc/terms/LCSH":
                    subjectType = SubjectTypeEnum.LCSH;
                    break;
                case "http://purl.org/dc/terms/LCC":
                    subjectType = SubjectTypeEnum.LCC;
                    break;
                default:
                    logger.LogError($"Unknown subject type {subjectString}");
                    break;
            }
        }
        catch (Exception)
        {
            logger.LogError($"Unable to extract subject from {node.Value}");
        }
        return (subjectType, retval);
    }

    /// <summary>
    /// Extract the book type
    /// </summary>
    /// <param name="node">The type node</param>
    /// <param name="defaultValue">The default type</param>
    /// <returns>The type of epub</returns>
    private string ExtractType(XmlNode node, string defaultValue = "")
    {
        string retval = defaultValue;
        try
        {
            var epubtype = (node["rdf:Description"] as XmlNode)["rdf:value"];
            retval = epubtype.InnerText;
        }
        catch (Exception)
        {
            logger.LogError($"Unable to extract type from {node.Value}");
        }
        return retval;
    }
    #endregion
}
