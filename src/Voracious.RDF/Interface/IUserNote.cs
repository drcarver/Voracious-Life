using System;

namespace Voracious.RDF.Interface;

/// <summary>
/// A user note is also the bookmark system. 
/// </summary>
public interface IUserNote
{
    int Id { get; set; }

    IResource Book { get; set; }

    DateTimeOffset CreateDate { get; set; }

    DateTimeOffset MostRecentModificationDate { get; set; }

    string Location { get; set; }

    string Text { get; set; }

    string Tags { get; set; }

    string Icon { get; set; }

    string BackgroundColor { get; set; }

    string ForegroundColor { get; set; }

    string SelectedText { get; set; }

    double LocationNumericValue { get; }
}
