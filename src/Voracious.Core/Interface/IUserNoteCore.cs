using System;

using Voracious.Core.Model;

namespace Voracious.Core.Interface;

/// <summary>
/// A user note is also the bookmark system. 
/// </summary>
public interface IUserNoteCore
{
    int Id { get; set; }

    ResourceCore Book { get; set; }

    DateTime CreateDate { get; set; }

    DateTime MostRecentModificationDate { get; set; }

    string Location { get; set; }

    string Text { get; set; }

    string Tags { get; set; }

    string Icon { get; set; }

    string BackgroundColor { get; set; }

    string ForegroundColor { get; set; }

    string SelectedText { get; set; }

    double LocationNumericValue { get; }
}
