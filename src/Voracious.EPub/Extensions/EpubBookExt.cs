﻿using System;
using System.Collections.Generic;

namespace Voracious.EPub.Extensions;

public class EpubBookExt
{
    public EpubBookExt(EpubBook originalBook)
    {
        inner = originalBook;
        if (inner == null)
        {
            TableOfContents = new List<EpubChapter>();
        }
    }

    private EpubBook inner;

    /// <summary>
    /// The original EpubBook has only a getter for the Resources value, 
    /// but I need more than that.
    /// </summary>
    EpubResources _ResourcesExt = null;

    public EpubResources Resources
    {
        get { return _ResourcesExt ?? inner.Resources; }
        set { _ResourcesExt = value; }
    }

    public List<EpubTextFile> ResourcesHtmlOrdered
    {
        get
        {
            // This is often filled in
            if (inner != null && inner.SpecialResources != null)
            {
                var list = inner.SpecialResources.HtmlInReadingOrder;
                if (list != null && list.Count > 0)
                {
                    return (List<EpubTextFile>)inner.SpecialResources.HtmlInReadingOrder;
                }
            }

            // Kind of a fail -- this can only be hit if we craft an epub but
            // then don't call the fixUp.
            if (_ResourcesHtmlOrdered == null)
            {
                FixupHtmlOrdered();
            }
            return _ResourcesHtmlOrdered;
        }
    }

    // FAIL: Intro to Planetary Nebula is the first book where the set of
    // HTML files in the <manifest> doesn't match the order of files in
    // the <spine toc="ncx">. The order is almost the same, but almost
    // only counts in horseshoes and hand grenades. I have to skip
    // through the spine and for each item, find it in the Resources.Html
    // value.
    private List<EpubTextFile> _ResourcesHtmlOrdered = null;

    /// <summary>
    /// Creates an ordered set of HTML files for the chapters in the book. 
    /// For many books this will be the files in the manifest BUT for 
    /// books like Intro to Planetary Nebula, it's a different order and 
    /// doesn't have all of the files in the manifest.
    /// </summary>
    public void FixupHtmlOrdered()
    {
        _ResourcesHtmlOrdered = new List<EpubTextFile>();
        foreach (var chapter in TableOfContents)
        {
            FixupHtmlOrderedAdd(chapter);
        }

        // FAIL: Programming In Go includes a cover, title page and copyrights
        // page, none of which are in the table of contents. Walk through
        // all of the HTML pages, looking for pages that haven't been added
        // yet and put them in.
        int addIndex = 0; // if in front, the index to add to; if negative then add to back.
        foreach (var html in Resources.Html)
        {
            if (IsInFixup(html.AbsolutePath))
            {
                addIndex = -1;
            }
            else if (html.AbsolutePath == inner.Format.Paths.NavAbsolutePath)
            {
                // FAIL: we have to add in all of the items in the manifest BUT
                // we should ignore the (Programming in Go) the nav.xhtml file
                // because it's useless. Luckily that file's path is marked in
                // inner.Format.Paths.NavAbsolutePath. 
                ; // Skip over the useless nax.xhtml file
            }
            else
            {
                if (addIndex < 0)
                {
                    _ResourcesHtmlOrdered.Add(html);
                }
                else
                {
                    _ResourcesHtmlOrdered.Insert(addIndex, html);
                    addIndex++;
                }
            }
        }
    }

    private void FixupHtmlOrderedAdd(EpubChapter chapter)
    {
        if (chapter == null) return;

        if (!IsInFixup(chapter.AbsolutePath))
        {
            var htmlForChapter = FindHtml(chapter);
            if (htmlForChapter == null)
            {
                throw new Exception($"ERROR: unable to find match HTML for chapter {chapter.Title}");
            }
            else
            {
                _ResourcesHtmlOrdered.Add(htmlForChapter);
            }
        }

        if (chapter.SubChapters != null)
        {
            foreach (var subChapter in chapter.SubChapters)
            {
                FixupHtmlOrderedAdd(subChapter);
            }
        }
    }

    private bool IsInFixup(string absolutePath)
    {
        foreach (var html in _ResourcesHtmlOrdered)
        {
            if (html.AbsolutePath == absolutePath)
            {
                return true;
            }
        }
        return false;
    }

    private EpubTextFile FindHtml(EpubChapter chapter)
    {
        foreach (var html in Resources.Html)
        {
            if (html.AbsolutePath == chapter.AbsolutePath)
            {
                return html;
            }
        }
        return null;
    }

    List<EpubChapter> _TableOfContentsExt = null;
    public List<EpubChapter> TableOfContents
    {
        get { return (List<EpubChapter>)(_TableOfContentsExt ?? inner?.TableOfContents); }
        set { _TableOfContentsExt = value; }
    }
}