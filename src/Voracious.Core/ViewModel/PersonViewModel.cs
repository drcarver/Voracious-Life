using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;

using Voracious.Core.Enum;
using Voracious.Core.Interface;

namespace Voracious.Core.ViewModel;

/// <summary>
/// Person can be used for Author, Illustrator, Editor, Translator, etc.
/// </summary>
public partial class PersonViewModel : ObservableObject, IPerson
{
    /// <summary>
    /// The primary key
    /// </summary>
    [ObservableProperty]
    private string about;

    /// <summary>
    /// The name of the person
    /// </summary>
    [ObservableProperty]
    private string? name;

    /// <summary>
    /// The person's alias
    /// </summary>
    [ObservableProperty]
    private string? aliases;

    /// <summary>
    /// The date of birth
    /// </summary>
    [ObservableProperty]
    private int? birthDate;

    /// <summary>
    /// The date of death
    /// </summary>
    [ObservableProperty]
    private int? deathDate;

    /// <summary>
    /// THe author's web page
    /// </summary>
    [ObservableProperty]
    private string? webpage;

    /// <summary>
    /// The sortable version of the author
    /// </summary>
    [ObservableProperty]
    private string? fileAs;

    /// <summary>
    /// The relator's for the book (creator, illustrator, etc..)
    /// </summary>
    [ObservableProperty]
    private RelatorEnum? relator;

    /// <summary>
    /// People include authors, illustrators, etc.
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<ResourceViewModel> books = [];

    /// <summary>
    /// The logger
    /// </summary>
    private readonly ILogger logger;

    /// <summary>
    /// Return the web page as a Uri
    /// </summary>
    [IgnoreDataMember]
    public Uri WebPageUri
    {
        get
        {
            try
            {
                // Some web pages are malformed e.g. eebooks/3453 and won't be
                // converted into URI.  May as well filter these out since they
                // will never work.
                if (Webpage.StartsWith("http"))
                {
                    return new Uri(Webpage);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Unable to convert {Webpage} to a URI");
            }
            return new Uri("http://wikipedia.com");
        }
    }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="loggerFactory">The logger factory</param>
    public PersonViewModel(ILoggerFactory loggerFactory)
    {
        this.logger = loggerFactory.CreateLogger<PersonViewModel>();
    }

    /// <summary>
    /// Add the alias for the person
    /// </summary>
    /// <param name="value"></param>
    public void AddAlias(string value)
    {
        if (value.Contains("+"))
        {
            value = value.Replace('+', '&');
        }
        if (string.IsNullOrEmpty(Aliases))
        {
            Aliases = value;
        }
        else Aliases = "+" + value;
    }

    /// <summary>
    /// Get the importance of the creator as a number so we can order the
    /// persons associated with resource
    /// </summary>
    /// <returns>return the importance of person to the resource</returns>
    public int GetImportance()
    {
        switch (Relator)
        {
            case RelatorEnum.author: 
                return 10;
            case RelatorEnum.artist: 
                return 20;
            case RelatorEnum.editor: 
                return 30;
            case RelatorEnum.photographer: 
                return 40;
            case RelatorEnum.translator: 
                return 50;
            case RelatorEnum.illustrator: 
                return 60;
            case RelatorEnum.adapter: 
                return 70;
            case RelatorEnum.annotator: 
                return 80;
            case RelatorEnum.authorOfAfterward: 
                return 90;
            case RelatorEnum.arranger: 
                return 100;
            case RelatorEnum.compiler: 
                return 110;
            case RelatorEnum.composer: 
                return 120;
            case RelatorEnum.conductor: 
                return 130;
            case RelatorEnum.performer: 
                return 140;
            case RelatorEnum.librettist: 
                return 150;
            case RelatorEnum.authorOfIntroduction: 
                return 160;
            case RelatorEnum.collaborator: 
                return 170;
            case RelatorEnum.commentator: 
                return 180;
            case RelatorEnum.contributor: 
                return 190;
            case RelatorEnum.dubiousAuthor: 
                return 200;
            case RelatorEnum.editorOfCompilation: 
                return 210;
            case RelatorEnum.engraver: 
                return 220;
            case RelatorEnum.other: 
                return 230;
            case RelatorEnum.publisher: 
                return 240;
            case RelatorEnum.researcher: 
                return 250;
            case RelatorEnum.transcriber: 
                return 260;
            case RelatorEnum.unknown: 
                return 270;
            case RelatorEnum.otherError: 
                return 280;
        }
        return 999;
    }

    /// <summary>
    /// Examples:
    /// by Samantha Jones
    /// by Samantha Jones (dubious) 1777-1810
    /// illustrated by Samantha Jones 
    /// </summary>
    public override string ToString()
    {
        var dates = "";
        if (BirthDate != -999999 && DeathDate != 999999) dates = $"{BirthDate}—{DeathDate}";
        else if (BirthDate != -999999) dates = $"{BirthDate}—";
        else if (DeathDate != 999999) dates = $"—{DeathDate}";
        string retval;
        switch (Relator)
        {
            case RelatorEnum.adapter: 
                retval = $"adapted by {Name}"; 
                break;
            case RelatorEnum.annotator: 
                retval = $"annotated by {Name}"; 
                break;
            case RelatorEnum.arranger: 
                retval = $"arranged by {Name}"; 
                break;
            case RelatorEnum.artist: 
                retval = $"{Name} (artist)"; 
                break;
            case RelatorEnum.author: 
                retval = $"by {Name}"; 
                break;
            case RelatorEnum.authorOfAfterward: 
                retval = $"afterword by {Name}"; 
                break;
            case RelatorEnum.authorOfIntroduction: 
                retval = $"introduction by {Name}"; 
                break;
            case RelatorEnum.collaborator: 
                retval = $"{Name} (collaborator)"; 
                break;
            case RelatorEnum.commentator: 
                retval = $"{Name} (commentator)"; 
                break;
            case RelatorEnum.compiler: 
                retval = $"compiled by {Name}"; 
                break;
            case RelatorEnum.composer: 
                retval = $"composed by {Name}"; 
                break;
            case RelatorEnum.conductor: 
                retval = $"conducted by {Name}"; 
                break;
            case RelatorEnum.contributor: 
                retval = $"{Name} (contributor)"; 
                break;
            case RelatorEnum.dubiousAuthor: 
                retval = $"by {Name} (dubious)"; 
                break;
            case RelatorEnum.editor: 
                retval = $"edited by {Name}"; 
                break;
            case RelatorEnum.editorOfCompilation: 
                retval = $"edited by {Name} (compilation)"; 
                break;
            case RelatorEnum.engraver: 
                retval = $"engraved by {Name}"; 
                break;
            case RelatorEnum.illustrator: 
                retval = $"illustrated by {Name}"; 
                break;
            case RelatorEnum.librettist: 
                retval = $"{Name} (librettist)"; 
                break;
            case RelatorEnum.otherError: 
                retval = $"{Name}"; 
                break;
            case RelatorEnum.performer: 
                retval = $"performed by {Name}"; 
                break;
            case RelatorEnum.photographer: 
                retval = $"{Name} (photographer)"; 
                break;
            case RelatorEnum.publisher: 
                retval = $"published by {Name}"; 
                break;
            case RelatorEnum.researcher: 
                retval = $"{Name} (researcher)"; 
                break;
            case RelatorEnum.transcriber: 
                retval = $"transcribed by {Name}"; 
                break;
            case RelatorEnum.translator: 
                retval = $"translated by {Name}"; 
                break;
            case RelatorEnum.unknown: 
                retval = $"{Name}"; 
                break;
            default: 
                retval = $"{Name}"; 
                break;
        }
        if (dates != "")
        {
            retval = retval + " " + dates;
        }
        if (Aliases != "")
        {
            retval += $" ({Aliases})";
        }
        return retval;
    }
}
