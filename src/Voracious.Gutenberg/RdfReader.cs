using System.Xml;

using Microsoft.Extensions.Logging;

using Voracious.Core.Enum;
using Voracious.Core.ViewModel;
using Voracious.Database;

namespace Voracious.Gutenberg;

public partial class RdfReader
{
    private ILogger<RdfReader> logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    /// <param name="bookdb">The database context for the application</param>
    public RdfReader(
        ILoggerFactory loggerFactory,
        VoraciousDataContext bookdb)
    {
        logger = loggerFactory.CreateLogger<RdfReader>();
        Bookdb = bookdb;
    }

    /// <summary>
    /// Gets the zipped tar (catalog) 
    /// file and stuff data into database
    /// </summary>
    /// <returns></returns>
    public async Task<int> ReadZipTarRdfFile()
    {
        await Task.Run(async () =>
        {
            var cts = new CancellationTokenSource();
            retval = await ReadZipTarRdfFileAsync(ui, bookdb, filepick, cts.Token);
            ;
        });
        return retval;
    }


    public enum UpdateType { Full, Fast }


    public List<BookViewModel> ReadZipTarRdfFileAsync()
    {
        SaveAfterNFiles = SaveSkipCount;
        UiAfterNNodes = NodeReadCount;

        // FAIL: Gutenberg includes bad files
        HashSet<string> KnownBadFiles = new HashSet<string>()
        {
            "cache/epub/0/pg0.rdf",
            "cache/epub/999999/pg999999.rdf",
        };
        var startTime = DateTime.Now;
        int nnewfiles = 0;
        int nnodes = 0;
        List<BookViewModel> newBooks = [];

        string folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)
        try
        {
            //using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            //{
            //}
            //    while (reader.MoveToNextEntry())
            //        {
            //            if (token.IsCancellationRequested)
            //            {
            //                break;
            //            }
            //            System.Diagnostics.Debug.WriteLine($"ZIPREAD: {reader.Entry.Key} size {reader.Entry.Size}");

            //            // Is the rdf-files.tar file that Gutenberg uses. 
            //            // The zip file has one giant TAR file (rdf-files.tar) embedded in it.
            //            if (reader.Entry.Key.EndsWith(".tar"))
            //            {
            //                using (var tarStream = reader.OpenEntryStream())
            //                {
            //                    using (var tarReader = ReaderFactory.Open(tarStream))
            //                    {
            //                        while (tarReader.MoveToNextEntry())
            //                        {
            //                            MemoryStream ms = new MemoryStream((int)tarReader.Entry.Size);
            //                            tarReader.WriteEntryTo(ms);
            //                            ms.Position = 0;
            //                            var sr = new StreamReader(ms);
            //                            var text = sr.ReadToEnd();
            //                            nnodes++;
            //                            if (token.IsCancellationRequested)
            //                            {
            //                                break;
            //                            }

            //                            if (KnownBadFiles.Contains(tarReader.Entry.Key))
            //                            {
            //                                // Skip known bad files like entry 999999 -- has weird values for lots of stuff!
            //                            }
            //                            else
            //                            {
            //                                // Got a book; let the UI know.
            //                                newBooks.Clear();
            //                                if (tarReader.Entry.Key.Contains("69203"))
            //                                {
            //                                    ; // useful hook for debugging.
            //                                }

            //                                // Reads and saves to database. And does a fancy merge if needed.
            //                                int newCount = 0;
            //                                try
            //                                {
            //                                    newCount = ReadRdfFileAndInsert(bookdb, tarReader.Entry.Key, text, newBooks, updateType);
            //                                }
            //                                catch (Exception rdfex)
            //                                {
            //                                    // Do what on exception?
            //                                    logger.LogError($"Error: file {file.Name} name {tarReader.Entry.Key} exception {rdfex.Message}");
            //                                    newCount = 0;
            //                                }
            //                                nnewfiles += newCount;

            //                                if (nnewfiles > 6000 && nnewfiles < 9000)
            //                                {
            //                                    SaveSkipCount = 100;
            //                                }
            //                                else
            //                                {
            //                                    SaveSkipCount = 100; // save very frequently. Otherwise, ka-boom!
            //                                }

            //                                if (nnewfiles >= SaveAfterNFiles)
            //                                {
            //                                    // FAIL: must save periodically. Can't accumulate a large number
            //                                    // of books (e..g, 60K books in the catalogger.LogError) and then save all at
            //                                    // once; it will take up too much memory and will crash.
            //                                    logger.LogError($"At index {CommonQueries.BookCount(bookdb)} file {file.Name} nfiles {nnewfiles}");
            //                                    CommonQueries.BookSaveChanges(bookdb);

            //                                    // Try resetting the singleton to reduce the number of crashes.
            //                                    BookDataContext.ResetSingleton("InitialBookData.Db");
            //                                    await Task.Delay(100); // Try a pause to reduce crashes.

            //                                    SaveAfterNFiles += SaveSkipCount;

            //                                }
            //                                if (newCount > 0)
            //                                {
            //                                    foreach (var bookData in newBooks)
            //                                    {
            //                                        await ui.OnAddNewBook(bookData);
            //                                    }
            //                                }
            //                                if (nnodes >= UiAfterNNodes)
            //                                {
            //                                    //await ui.logger.LogErrorAsync($"Book: file {tarReader.Entry.Key}\nNNew: {nfiles} NProcesses {nnodes}\n");
            //                                    await ui.OnTotalBooks(nnodes);
            //                                    UiAfterNNodes += NodeReadCount;
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }
        catch (Exception readEx)
        {
            logger.LogError($"Error: reading Gutenberg ZIP file exception {readEx.Message}");
        }

        return newBooks;
    }

    static int SaveAfterNFiles = 0;
    static int SaveSkipCount = 100;

    static int UiAfterNNodes = 0;
    static int NodeReadCount = 100;

    const int MaxFilesChecked = 9999999;
    private static PersonViewModel ExtractCreator(string logger.LogErrorname, XmlNode parentnode)
    {
        var retval = new PersonViewModel();
        try
        {
            var node = parentnode["pgterms:agent"] as XmlNode;
            if (node == null)
            {
                // Some books (like 1813) just don't have all this set up.
                //logger.LogError($"Error: ExtractCreator doesn't have a pgterms:agent for {parentnode.InnerText} for {logger.LogErrorname}");
                return null;
            }
            string str = "";
            var relator = PersonViewModel.ToRelator(parentnode.Name); // e.g. marcrel:aui == author of introductions
            if (relator == RelatorEnum.otherError)
            {
                logger.LogError($"ERROR: ExtractCreator has unknown relator {parentnode.Name} for {logger.LogErrorname}");
                return null;
            }
            retval.PersonType = relator;
            str = (node["pgterms:deathdate"] as XmlNode)?.InnerText;
            if (!string.IsNullOrEmpty(str)) retval.DeathDate = int.Parse(str);
            str = (node["pgterms:birthdate"] as XmlNode)?.InnerText;
            if (!string.IsNullOrEmpty(str)) retval.BirthDate = int.Parse(str);
            str = (node["pgterms:webpage"] as XmlNode)?.Attributes?.GetNamedItem("rdf:resource")?.Value;
            if (!string.IsNullOrEmpty(str)) retval.Webpage = str;
            str = (node["pgterms:alias"] as XmlNode)?.InnerText;
            if (!string.IsNullOrEmpty(str)) retval.AddAlias(str);
            str = (node["pgterms:name"] as XmlNode)?.InnerText;
            if (!string.IsNullOrEmpty(str)) retval.Name = str;
        }
        catch (Exception)
        {
            logger.LogError($"ERROR: unable to extract person from {parentnode.Value} for {logger.LogErrorname}");
            return null;
        }
        return retval;
    }
    /// <summary>
    /// Given a <dcterms:hasFormat> blob, extract the file format + file location data.
    /// </summary>
    private static FilenameAndFormatDataViewModel ExtractHasFormat(string logger.LogErrorname, XmlNode node)
    {
        bool extentIsZero = false;
        var retval = new FilenameAndFormatDataViewModel();
        int nchild = 0;
        try
        {
            foreach (var childFileObj in node.ChildNodes)
            {
                var childFile = childFileObj as XmlNode;
                if (childFile == null)
                {
                    logger.LogError($"ERROR: hasFormat child isn't an XmlNode with {node.InnerText} for {logger.LogErrorname}");
                    continue;
                }
                nchild++;
                if (nchild > 1)
                {
                    logger.LogError($"ERROR: hasFormat has too many child with {node.InnerText} for {logger.LogErrorname}");
                    continue;
                }
                retval.FileName = childFile.Attributes["rdf:about"].Value; // The super critical part!
                foreach (var valueObj in childFile.ChildNodes)
                {
                    var value = valueObj as XmlNode;
                    if (value == null)
                    {
                        logger.LogError($"ERROR: hasFormat grandchild isn't an XmlNode with {childFile.InnerText} for {logger.LogErrorname}");
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
                                    logger.LogError($"ERROR: hasFormat description grandchild isn't an XmlNode with {value.InnerText} for {logger.LogErrorname}");
                                    continue;
                                }
                                foreach (var dvalueObj in description.ChildNodes)
                                {
                                    var dvalue = dvalueObj as XmlNode;
                                    if (dvalue == null)
                                    {
                                        logger.LogError($"ERROR: hasFormat description grandchild isn't an XmlNode with {description.InnerText} for {logger.LogErrorname}");
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
                                            logger.LogError($"ERROR: Unknown member {dvalue.Name} for {logger.LogErrorname}");
                                            break;
                                    }
                                }
                            }
                            break;
                        case "dcterms:modified":
                            if (retval.LastModified != "")
                            {
                                // 2024-05-05: extent and modified commonly have multiple values. As of 2024-05-05, the most recent is always 
                                // the first value, so that's the one to keep.
                                // logger.LogError($"ERROR: hasFormat has multiple {value.Name} for {logger.LogErrorname}");
                            }
                            else
                            {
                                retval.LastModified = value.InnerText;
                            }
                            break;
                        case "dcterms:extent":
                            if (retval.Extent != -1)
                            {
                                // 2024-05-05: extent and modified commonly have multiple values. As of 2024-05-05, the most recent is always 
                                // the first value, so that's the one to keep.
                                //logger.LogError($"ERROR: hasFormat has multiple {value.Name} for {logger.LogErrorname}");
                            }
                            else
                            {
                                retval.Extent = int.Parse(value.InnerText);
                            }
                            if (retval.Extent == 0) extentIsZero = true;
                            break;
                        case "dcterms:isFormatOf":
                            if (retval.BookId != "")
                            {
                                logger.LogError($"ERROR: hasFormat has multiple {value.Name} for {logger.LogErrorname}");
                            }
                            retval.BookId = value.Attributes["rdf:resource"].Value;
                            break;
                        default:
                            logger.LogError($"ERROR: hasFormat unknown child {value.Name} for {logger.LogErrorname}");
                            break;
                    }
                }
            }
        }
        catch (Exception)
        {
            logger.LogError($"ERROR: unable to extract hasFormat from {node.Value} for {logger.LogErrorname}");
        }

        if (retval.FileName == "") logger.LogError($"ERROR: hasFormat: doesn't include filename for {logger.LogErrorname}");
        if (retval.Extent < 1 && !extentIsZero) logger.LogError($"ERROR: hasFormat: doesn't include extent for {logger.LogErrorname} for {retval.FileName}");
        if (retval.LastModified == "") logger.LogError($"ERROR: hasFormat: doesn't include modified for {logger.LogErrorname}");
        if (retval.BookId == "") logger.LogError($"ERROR: hasFormat: doesn't include bookId for {logger.LogErrorname}");
        if (!retval.IsKnownMimeType) logger.LogError($"ERROR: hasFormat: Unknown mime type {retval.MimeType} for {logger.LogErrorname}");
        return retval;
    }
    /// <summary>
    /// Creates an EPUB FilenameAndFormatData that's for an EPUB. Needed because Project Gutenberg RDF files
    /// as of 2022-10-22 are broken: they don't include EPUB files first time around (will be added a month later
    /// when they reprocess) and have bad links.
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private static FilenameAndFormatDataViewModel MakeEpubFromFormat(FilenameAndFormatDataViewModel source)
    {
        if (source == null) return null;
        FilenameAndFormatDataViewModel retval = new FilenameAndFormatDataViewModel(source);
        retval.MimeType = "application/epub+zip";
        // extent will be hopelessly wrong, but that's life.
        var gutIndex = source.GutenbergStyleIndexNumber;
        if (gutIndex < 1) return null;
        if (gutIndex < 50000) return null; // never for old things? TODO: make this more dynamic?

        retval.FileName = $"https://www.gutenberg.org/cache/epub/{gutIndex}/pg{gutIndex}-images.epub";
        return retval;
    }

    private static string ExtractLanguageValue(string logger.LogErrorname, XmlNode node, string defaultValue = "")
    {
        string retval = defaultValue;
        try
        {
            var descrNode = node["rdf:Description"] as XmlNode;
            if (descrNode != null)
            {
                var language = descrNode["rdf:value"];
                retval = language.InnerText.ToLower(); // APRESS can have this as En instead of en
            }
            else
            {
                var inner = node.InnerText;
                if (inner.Length >= 2 && inner.Length <= 2)
                {
                    retval = inner.ToLower(); // APRESS can have this as En instead of en
                }
                else
                {
                    logger.LogError($"ERROR: unrecognized inner-text language {inner} from {node.Value} for {logger.LogErrorname}");
                }
            }
        }
        catch (Exception)
        {
            logger.LogError($"ERROR: unable to extract language from {node.Value} for {logger.LogErrorname}");
        }
        return retval;
    }
    private static (SubjectTypeEnum, string) ExtractSubjectValue(string logger.LogErrorname, XmlNode node, string defaultValue = "")
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
                    logger.LogError("$ERROR: unknown subject type {subjectString} for {logger.LogErrorname}"); break;
            }
        }
        catch (Exception)
        {
            logger.LogError($"ERROR: unable to extract subject from {node.Value} for {logger.LogErrorname}");
        }
        return (subjectType, retval);
    }

    private static string ExtractType(string logger.LogErrorname, XmlNode node, string defaultValue = "")
    {
        string retval = defaultValue;
        try
        {
            var language = (node["rdf:Description"] as XmlNode)["rdf:value"];
            retval = language.InnerText;
        }
        catch (Exception)
        {
            logger.LogError($"ERROR: unable to extract type from {node.Value} for {logger.LogErrorname}");
        }
        return retval;
    }


    private static int NUnknownMarcRecords = 0;
    private static BookViewModel ExtractBook(string logger.LogErrorname, XmlNode node)
    {
        var book = new BookViewModel();
        var id = node.Attributes["rdf:about"]?.Value;
        if (!string.IsNullOrEmpty(id))
        {
            book.BookId = id;
        }
        else
        {
            logger.LogError($"ERROR: BookId: missing rdf:about with an id? for {logger.LogErrorname}");
            return null;
        }
        FilenameAndFormatDataViewModel txtFormat = null;
        var haveEpub = false;
        foreach (var cn in node.ChildNodes)
        {
            var value = cn as XmlNode;
            if (value == null) continue;
            PersonViewModel person = null;
            switch (value.Name)
            {
                case "dcterms:alternative": // <...>Alice in Wonderland</...>
                    book.TitleAlternative = value.InnerText;
                    break;
                case "dcterms:creator":
                    person = ExtractCreator(logger.LogErrorname, value);
                    if (person != null) book.People.Add(person);
                    break;
                case "dcterms:description": // <...>Illustrated by the author.</...>
                    book.Description = value.InnerText;
                    break;
                case "dcterms:hasFormat": //  direct info about how to download a book
                    var format = ExtractHasFormat(logger.LogErrorname, value);
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
                        book.Files.Add(format);
                    }
                    break;
                case "dcterms:issued": // <... rdf:datatype="http://www.w3.org/2001/XMLSchema#date">2015-03-06</...>
                    book.Issued = value.InnerText; // e.g. 1997-12-01 or None
                    break;
                case "dcterms:language":
                    book.Language = ExtractLanguageValue(logger.LogErrorname, value);
                    break;
                case "dcterms:license": // <dcterms:license rdf:resource="license"/>
                    break;
                case "dcterms:publisher": // <dcterms:publisher>Project Gutenberg</dcterms:publisher>
                    break;
                case "dcterms:rights": // <dcterms:rights>Public domain in the USA.</dcterms:rights>
                    break;
                case "dcterms:subject": // <dcterms:rights>Public domain in the USA.</dcterms:rights>
                    var (subjectType, subject) = ExtractSubjectValue(logger.LogErrorname, value);
                    switch (subjectType)
                    {
                        case SubjectType.LCC:
                            if (!string.IsNullOrEmpty(book.LCC)) book.LCC += ",";
                            book.LCC += subject;
                            break;
                        case SubjectType.LCSH:
                            if (!string.IsNullOrEmpty(book.LCSH)) book.LCSH += ",";
                            book.LCSH += subject;
                            break;
                        default:
                            logger.LogError($"ERROR: unable to understand dcterms:subject {value.InnerText} for {logger.LogErrorname}");
                            break;
                    }
                    break;
                case "dcterms:tableOfContents":
                    // Alas: these are often not actually very useful. The epubs have correct TOC 
                    // with links set up.
                    break;
                case "dcterms:title": // <dcterms:title>Three Little Kittens</dcterms:title>
                    book.Title = value.InnerText;
                    break;
                case "dcterms:type":
                    var bookType = ExtractType(logger.LogErrorname, value, "???");
                    switch (bookType)
                    {
                        case "Collection": book.BookType = BookViewModel.FileType.Collection; break;
                        case "Dataset": book.BookType = BookViewModel.FileType.Dataset; break;
                        // Dataset from e.g. http://www.gutenberg.org/ebooks/3503
                        // These are e.g. the human genome project. They are 100% uninteresting.
                        case "Image": book.BookType = BookViewModel.FileType.Image; break;
                        case "MovingImage": book.BookType = BookViewModel.FileType.MovingImage; break;
                        case "Sound": book.BookType = BookViewModel.FileType.Sound; break;
                        case "StillImage": book.BookType = BookViewModel.FileType.StillImage; break;
                        case "Text": // OK, this is normal
                            break;
                        default:
                            logger.LogError($"ERROR: Unknown book type {bookType} for {logger.LogErrorname}");
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
                    person = ExtractCreator(logger.LogErrorname, value);
                    if (person != null) book.People.Add(person);
                    break;
                case "pgterms:bookshelf":
                    break;
                case "pgterms:downloads": // <... rdf:datatype="http://www.w3.org/2001/XMLSchema#integer">11</...>
                    break;

                // almost always the LCCN number
                case "pgterms:marc010": book.LCCN = value.InnerText; break;

                // e.g. The Charles Dickens Edition for pg766.rdf
                case "pgterms:marc250": book.PGEditionInfo = value.InnerText; break;

                // <...>Houston: Advantage International, The PaperLess Readers Club, 1992</...>
                case "pgterms:marc260": book.Imprint = value.InnerText; break;

                // e.g. The Pony Rider Boys, number 1 for pg6067.rdf
                case "pgterms:marc440": book.BookSeries = value.InnerText; break;

                // e.g. EBook produced by David Starner and Heather Martino for pg12962.rdf
                case "pgterms:marc508": book.PGProducedBy = value.InnerText; break;

                // e.g. This ebook uses a beginning of the 20th century spelling. for pg17193.rdf
                case "pgterms:marc546": book.PGNotes = value.InnerText; break;

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
                case "pgterms:marc901": break;

                default:
                    if (NUnknownMarcRecords == 0)
                    {
                        logger.LogError($"XML: unknown XML item. Individual RDF files are at e.g. https://www.gutenberg.org/cache/epub/35426/pg35426.rdf");
                        logger.LogError($"XML: Gutenberg offline catalogger.LogError information at https://www.gutenberg.org/ebooks/offline_catalogger.LogErrors.html#xmlrdf");
                    }
                    var logger.LogErrorvalue = value.InnerText;
                    if (logger.LogErrorvalue.Length > 40) logger.LogErrorvalue = logger.LogErrorvalue.Substring(0, 40);
                    logger.LogError($"XML: {value.Name} but expected e.g. dcterms:hasFormat or dcterms:language etc. for {logger.LogErrorname}; value={logger.LogErrorvalue}");
                    NUnknownMarcRecords++;
                    break;
            }
        }
        // Create an EPUB for all books > Id 50000.
        // FAIL: Gutenberg creates incorrect catalogger.LogErrors. They don't include the EPUB until 
        //  month after the initial creation thanks to an incorrect RDF creation script. And the 
        // links are just wrong; the links like https://www.gutenberg.org/ebooks/39198.epub.images
        // are really just redirects(!)
        if (txtFormat != null && haveEpub == false)
        {
            var epubFormat = MakeEpubFromFormat(txtFormat);
            if (epubFormat != null)
            {
                // add to the list of formats.
                book.Files.Add(epubFormat);
            }
        }

        // Do a little validation
        // Correction: don't do this validation. Too many Gutenberg "books" in the catalogger.LogError are just plain
        // errors. OTOH, they are o
        if (book.Issued != "None")
        {
            if (book.Files.Count == 0)
            {
                logger.LogError($"XML: book has no files for {logger.LogErrorname}");
            }
        }
        return book;
    }
    private static BookViewModel[] BookDataArray = new BookViewModel[10];
    private static int BookDataArrayIndex = -1;

    public VoraciousDataContext Bookdb { get; }

    /// <summary>
    /// Reads in data form the xml file and potentially adds it to the database via a fancy merge.
    /// </summary>
    /// <param name="bookdb"></param>
    /// <param name="logger.LogErrorname"></param>
    /// <param name="xmlData"></param>
    /// <param name="newBooks"></param>
    /// <returns></returns>
    private static int ReadRdfFileAndInsert(BookDataContext bookdb, string logger.LogErrorname, string xmlData, IList<BookViewModel> newBooks, UpdateType updateType)
    {
        // This is a state machine, which means that some pretty important parts are just a little
        // bit buried. When a book is done, take a look at "pgterms:ebook" which has ExtractBook,
        // some fixup, and then saves the book in the database but only if it's valid.
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
                                var book = ExtractBook(logger.LogErrorname, bookNode);

                                //
                                // Set the denorm data
                                //
                                book.DenormPrimaryAuthor = (book.BestAuthorDefaultIsNull ?? "-Unknown").ToLower();
                                var idnumber = GutenbergFileWizard.GetBookIdNumber(book.BookId, -365);
                                var dto = new DateTimeOffset(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);
                                dto = dto.AddMinutes(idnumber);
                                book.DenormDownloadDate = dto.ToUnixTimeSeconds();

                                // A book might be invalid. For example, Gutenberg includes
                                // ebooks/0 in the RDF catalogger.LogError even though it doesn't exist.
                                // Books with no downloads aren't considered real books.
                                if (book != null && book.Validate() == "") //"" is OK (yes, it's weird)
                                {
                                    // If it already exists, we don't need to add it.
                                    // NOTE: At some point in the future we might want to see if the
                                    // record has changed at all. 

                                    // TODO: 
                                    // Especially if we read from a bookmark file from a computer with a more
                                    // advanced index, so our book data is actually pretty sketchy.
                                    // Sketchy == many fields are made up. The source should be 
                                    // BookSourceBookMarkFile = "From-bookmark-file:"; so that we know when we're 
                                    // in this situation.
                                    // See if source in the database is BookSourceBookMarkFile
                                    // in order to fix this.

                                    // Save an indicator of the books
                                    // The current index is always the last location
                                    // This is really just used for debugging.
                                    BookDataArrayIndex = (BookDataArrayIndex + 1) % BookDataArray.Length;
                                    BookDataArray[BookDataArrayIndex] = book;

                                    var dbbook = bookdb.Books.Find(book.BookId);

                                    // Actually save the book! (possibly with a fancy merge)
                                    // A fast update just adds new books; a full update will merge in data as needed.
                                    var existHandling = updateType == UpdateType.Full
                                        ? CommonQueries.ExistHandling.SmartCatalogger.LogErrorOverride
                                        : CommonQueries.ExistHandling.Catalogger.LogErrorOverrideFast;

                                    var nAdded = CommonQueries.BookAdd(bookdb, book, existHandling);
                                    if (nAdded > 0 && newBooks != null)
                                    {
                                        newBooks.Add(book);
                                    }
                                    retval += nAdded;
                                }
                                break;
                            case "cc:Work": // cc:Work rdf:about=""> 
                                            // <... rdf:resource="https://creativecommons.org/publicdomain/zero/1.0/"/>
                                break;
                            case "rdf:Description":
                                // <<... rdf:about="http://en.wikipedia.org/wiki/Katharine_Pyle">
                                break;
                            default:
                                logger.LogError($"XML: ebook: {bookNode.Name} but just expected pgterms:ebook for {logger.LogErrorname}");
                                break;
                        }
                    }
                    break;
                case "xml": // <?xml version="1.0" encoding="utf-8"?>
                    break;

                default:
                    logger.LogError($"XML: rdf:rdf: {node.Name} but just expected rdf:RDF for {logger.LogErrorname}");
                    break;
            }
        }
        return retval;
    }
}
