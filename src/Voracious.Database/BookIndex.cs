using System.Text;

using Voracious.Core.Interface;
using Voracious.Core.Model;

namespace Voracious.Database;

public class BookIndex
{
    public string? BookId { get; set; }
    public string? Text { get; set; }

    public override string ToString()
    {
        return $"{BookId}\t{Text}"; // assumes bookId will never include a tab.
    }

    public static BookIndex FromBookData(ResourceModel resource)
    {
        var sb = new StringBuilder();
        Append(sb, resource.Title);
        Append(sb, resource.TitleAlternative);
        Append(sb, resource.BookSeries);
        Append(sb, resource.Imprint);
        Append(sb, resource.LCC);
        Append(sb, resource.LCCN);
        Append(sb, resource.LCSH);
        //foreach (var people in resource.People)
        //{
        //    Append(sb, people.Aliases);
        //    Append(sb, people.Name);
        //}

        var retval = new BookIndex()
        {
            BookId = resource.About,
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
