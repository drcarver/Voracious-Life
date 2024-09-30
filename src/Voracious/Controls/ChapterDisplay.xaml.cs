using System.Collections.ObjectModel;

using Voracious.Core.Model;
using Voracious.EPub;
using Voracious.EPub.Extensions;
using Voracious.EPub.Interface;

namespace Voracious.Reader.Controls;

public sealed partial class ChapterDisplay : ContentView, INavigateTo, ISetChapters
{
    // We always track which one we are
    const NavigateControlId ControlId = NavigateControlId.ChapterDisplay;
    public ChapterDisplay()
    {
        this.DataContext = this;
        this.InitializeComponent();
    }

    public ObservableCollection<EpubChapter> Chapters { get; } = new ObservableCollection<EpubChapterData>();
    private EpubBookExt Book { get; set; }
    public void SetChapters(EpubBookExt book, IList<EpubChapter> chapters)
    {
        Book = book;
        Chapters.Clear();
        SetChaptersHelper(chapters, 1);
    }

    const int MAX_CHAPTER_DEPTH = 3;
    /// <summary>
    /// Proper recursion for adding chapters.
    /// </summary>
    private void SetChaptersHelper(IList<EpubChapter> chapters, int level)
    {
        if (chapters == null || chapters.Count == 0)
        {
            return; // not an error, just a fact. If there are no chapters, don't add them.
        }


        foreach (var chapter in chapters)
        {
            Chapters.Add(new EpubChapter(chapter, level));
            if (chapter.SubChapters != null && chapter.SubChapters.Count > 0)
            {
                if (level >= MAX_CHAPTER_DEPTH)
                {
                    Logger.Log($"ChapterDisplay:SetChapter: too many levels under chapter {chapter.Title}");
                }
                else
                {
                    SetChaptersHelper(chapter.SubChapters, level + 1);
                }
            }
        }
    }

    /// <summary>
    /// Called when the user has navigated to somewhere in the book. The chapter display
    /// tries to sync itself to the value. The chapter display depends on the caller being
    /// fully initialized first!
    /// </summary>
    /// <param name="sourceId"></param>
    /// <param name="location"></param>
    public async void NavigateTo(NavigateControlId sourceId, BookLocation location)
    {
        string chapterid = "";
        var nav = Navigator.Get();
        if (!double.IsNaN(location.ScrollPercent))
        {
            chapterid = await nav.MainBookHandler.GetChapterBeforePercentAsync(location);
        }
        else
        {
            chapterid = nav.MainBookHandler.GetChapterContainingId(location.Location, location.HtmlIndex);
        }
        EpubChapter foundChapter = null;

        if (foundChapter == null && location.HtmlIndex >= 0)
        {
            var html = Book.ResourcesHtmlOrdered[location.HtmlIndex];
            foreach (var chapter in Chapters)
            {
                // FAIL: Intro to Planetary Nebulae the location is html 8, id tit1 which is shared by multiple chapters.
                if (html.Href.EndsWith(chapter.FileName) && chapter.Anchor == chapterid)
                {
                    foundChapter = chapter;
                    break;
                }
            }
        }

        // Most common: there's an id, and it matches a single chapter.
        if (foundChapter == null)
        {
            foreach (var chapter in Chapters)
            {
                if (chapter.Anchor == chapterid || chapter.FileName == chapterid)
                {
                    foundChapter = chapter;
                    break;
                }
            }
        }

        if (foundChapter == null && location.HtmlIndex >= 0)
        {
            var html = Book.ResourcesHtmlOrdered[location.HtmlIndex];
            foreach (var chapter in Chapters)
            {
                // FAIL: Intro to Planetary Nebulae the location is html 8, id tit1 which is shared by multiple chapters.
                if (html.Href.EndsWith(chapter.FileName))
                {
                    foundChapter = chapter;
                    break;
                }
            }
        }

        // Worse case scenario, but it's better to display something
        if (foundChapter == null)
        {
            foreach (var chapter in Chapters)
            {
                if (string.IsNullOrEmpty(chapterid))
                {
                    App.Error($"ChapterDisplay:Navigate({location}) was asked to find an empty chapter");
                    foundChapter = chapter;
                    break;
                }
            }
        }

        if (foundChapter == null)
        {
            // Truly desperate.
            if (Chapters.Count > 0)
            {
                App.Error($"ChapterDisplay:Navigate({location}) completely failed");
                foundChapter = Chapters[0];
            }
            else
            {
                App.Error($"ChapterDisplay:Navigate({location}) last ditch completely failed -- no chapters at all!");
            }
        }

        if (foundChapter != null)
        {
            // Select this one
            uiChapterList.SelectedItem = foundChapter;
            uiChapterList.ScrollIntoView(foundChapter);
            return; // all done!
        }
    }

    private void OnSelectionTapped(object sender, TappedEventArgs e)
    {
        var chapter = (sender as FrameworkElement).DataContext as EpubChapterData;
        UserDidNavigationRequest(chapter);
    }

    private void UserDidNavigationRequest(EpubChapter chapter)
    {
        if (chapter == null) return;
        var nav = Navigator.Get();
        BookLocation location = null;
        if (chapter == Chapters[0])
        {
            // First one is special: we always go to the very start of the book, including stuff that's not in
            // the classic table of contents. For example, Gutenberg Campfire Girls Station Island has a cover that's
            // not in the TOC but which is in the inner.SpecialResources.HtmlInReadingOrder and which is placed first.
            // Additionally, that book also has the weird thing that the ID for the first section (in the TOC) is
            // near the end of the section, so when you click on it, it scrolls to there, but then the next TOC entry
            // (chapter 1) is visible near the top, and so the selection jumps to there.
            location = new BookLocation(0, 0); // first HTML page, at the very top.
        }
        else
        {
            location = EpubChapter.FromChapter(chapter);
        }
        nav.UserNavigatedTo(ControlId, location);
    }
}
