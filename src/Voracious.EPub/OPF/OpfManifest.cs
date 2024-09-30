using System.Collections.Generic;
using System.Linq;

namespace Voracious.EPub.OPF;

public class OpfManifest
{
    public const string ManifestItemCoverImageProperty = "cover-image";

    public List<OpfManifestItem> Items { get; set; } = new List<OpfManifestItem>();

    public OpfManifestItem FindCoverItem()
    {
        return Items.FirstOrDefault(e => e.Properties.Contains(ManifestItemCoverImageProperty));
    }

    public void DeleteCoverItem(string id = null)
    {
        var item = id != null ? Items.FirstOrDefault(e => e.Id == id) : FindCoverItem();
        if (item != null)
        {
            Items.Remove(item);
        }
    }
}
