// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Voracious.Controls;

public class EpubChapterDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate Toc1 { get; set; }
    public DataTemplate Toc2 { get; set; }
    public DataTemplate Toc3 { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        switch ((item as EpubChapterData)?.Indent ?? -1)
        {
            case -1:
                return Toc3;
            case 1: return Toc1;
            case 2: return Toc2;
            case 3: return Toc3;
        }
        return Toc3;
    }
}
