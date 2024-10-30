using System.Text;

using ExCSS;

using Microsoft.Extensions.Logging;

using Voracious.EPub;

namespace Voracious.Control.Extensions;

public class CssFixup
{
    private ILogger<CssFixup> logger;

    CssFixup(ILoggerFactory loggerFactory)
    {
        logger = loggerFactory.CreateLogger<CssFixup>();
    }

    public static bool FixupCss(EpubFile cssFile)
    {
        var changed = false;
        var css = Encoding.UTF8.GetString(cssFile.Content);
        var parser = new StylesheetParser();
        var sheet = parser.Parse(css);
        var cssBodyList = sheet.StyleRules.Where(rule => rule.SelectorText == "body");
        foreach (var cssBody in cssBodyList)
        {
            var margin = cssBody.Style.Margin;
            if (margin != null && margin.Contains("%"))
            {
                changed = true;
                cssBody.Style.Margin = "1em";
            }
            var marginLeft = cssBody.Style.MarginLeft;
            if (marginLeft != null && marginLeft.Contains("%"))
            {
                changed = true;
                cssBody.Style.MarginLeft = "0.5em";
            }
            var marginRight = cssBody.Style.MarginRight;
            if (marginRight != null && marginRight.Contains("%"))
            {
                changed = true;
                cssBody.Style.MarginRight = "0.5em";
            }


        }

        if (changed)
        {
            var newCss = sheet.ToCss();
            var newBuffer = Encoding.UTF8.GetBytes(newCss);
            cssFile.Content = newBuffer;
        }

        return changed;
    }
}
