﻿using System;
using System.Text;

using Newtonsoft.Json;
using Voracious.Core.Interface;

namespace Voracious.Core.Model;

/// <summary>
/// Is the location within a book. Does not include the book ID.
/// </summary>
public class BookLocationCore : IBookLocation
{
    public BookLocationCore()
    {

    }

    public BookLocationCore(int htmlIndex, string location)
    {
        HtmlIndex = htmlIndex;
        Location = location ?? "";
    }

    public BookLocationCore(int htmlIndex, double scrollPercent)
    {
        HtmlIndex = htmlIndex;
        ScrollPercent = scrollPercent;
    }

    /// <summary>
    /// Location via unique ID in the book OR to a JSON 
    /// representations of a BookLocation
    /// </summary>
    public string Location { get; set; } = "";

    /// <summary>
    /// Location via the current scroll position. This requires 
    /// both the scroll position and the index of the html
    /// </summary>
    public double ScrollPercent { get; set; } = double.NaN;
    public int HtmlIndex { get; set; } = -1;
    public string HtmlFileName { get; set; } = "";

    public double HtmlPercent
    {
        get
        {
            if (HtmlIndex < 0 || double.IsNaN(ScrollPercent))
            {
                return -1.0;
            }
            var retval = HtmlIndex * 1000 + ScrollPercent; // scrollpercent is 0..100 inclusive
            return retval;
        }
    }

    public string ToJson()
    {
        string retval = JsonConvert.SerializeObject(this);
        return retval;
    }

    public static BookLocationCore FromJson(string json)
    {
        // is old-style; the string is just e.g. pgepubid00000
        if (!json.Contains("{"))
        {
            return new BookLocationCore(-1, json);
        }


        try
        {
            var retval = JsonConvert.DeserializeObject<BookLocationCore>(json);
            if (double.IsNaN(retval.ScrollPercent) && string.IsNullOrEmpty(retval.Location))
            {
                retval = null; // it's actually blank; nuke it!
            }
            return retval;
        }
        catch (Exception)
        {
            ;
        }
        return null;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.Append("Location:");
        if (!double.IsNaN(ScrollPercent)) sb.Append($" Percent={ScrollPercent:F3}");
        if (!string.IsNullOrEmpty(Location)) sb.Append($" NamedLocation={Location}");
        if (HtmlIndex >= 0) sb.Append($" HtmlIndex={HtmlIndex}");
        if (!string.IsNullOrEmpty(HtmlFileName)) sb.Append($" HtmlFileName={HtmlFileName}");
        return sb.ToString();
    }
}
