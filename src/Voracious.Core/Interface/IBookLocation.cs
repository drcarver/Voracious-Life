namespace Voracious.Core.Interface;

public interface IBookLocation
{
    /// <summary>
    /// Location via unique ID in the book OR to a JSON 
    /// representations of a BookLocation
    /// </summary>
    public string Location { get; set; }

    /// <summary>
    /// Location via the current scroll position. This requires 
    /// both the scroll position and the index of the html
    /// </summary>
    public double ScrollPercent { get; set; }
    public int HtmlIndex { get; set; }
    public string HtmlFileName { get; set; }
    public double HtmlPercent { get; }
}