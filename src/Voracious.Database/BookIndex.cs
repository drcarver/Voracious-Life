using System.Text;

using Voracious.Core.ViewModel;

namespace Voracious.Database;

public class BookIndex
{
    public string? BookId { get; set; }
    public string? Text { get; set; }

    public override string ToString()
    {
        return $"{BookId}\t{Text}"; // assumes bookId will never include a tab.
    }

    public static BookIndex FromBookData(BookViewModel bookData)
    {
        var sb = new StringBuilder();
        Append(sb, bookData.Title);
        Append(sb, bookData.TitleAlternative);
        Append(sb, bookData.Review?.Tags);
        Append(sb, bookData.Review?.Review);
        Append(sb, bookData.BookSeries);
        Append(sb, bookData.Imprint);
        Append(sb, bookData.LCC);
        Append(sb, bookData.LCCN);
        Append(sb, bookData.LCSH);
        if (bookData.Notes != null)
        {
            foreach (var note in bookData.Notes.Notes)
            {
                Append(sb, note.Tags);
                Append(sb, note.Text);
            }
        }
        foreach (var people in bookData.People)
        {
            Append(sb, people.Aliases);
            Append(sb, people.Name);
        }

        var retval = new BookIndex()
        {
            BookId = bookData.BookId,
            Text = sb.ToString(),
        };
        return retval;
    }

    public static StringBuilder Append(StringBuilder sb, string field)
    {
        if (!string.IsNullOrEmpty(field))
        {
            sb.Append(" ");
            bool sawWS = true;
            foreach (var ch in field)
            {
                var newch = char.ToLower(ch);
                if (newch < '0'
                    || newch >= ':' && newch <= '@'
                    || newch >= '[' && newch <= '`'
                    || newch >= '{' && newch <= '~'
                    ) // higher chars stay the same. International support is ... iffy ... 
                    newch = ' ';
                if (newch != ' ' || !sawWS)
                {
                    sb.Append(newch);
                }
                sawWS = newch == ' ';
            }
        }
        return sb;
    }
}
