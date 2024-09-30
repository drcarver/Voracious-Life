using System.Collections.Generic;
using System.Linq;

namespace Voracious.EPub.OPF;

public class OpfMetadata
{
    public List<string> Titles { get; set; } = new List<string>();
    public List<string> Subjects { get; set; } = new List<string>();
    public List<string> Descriptions { get; set; } = new List<string>();
    public List<string> Publishers { get; set; } = new List<string>();
    public List<OpfMetadataCreator> Creators { get; set; } = new List<OpfMetadataCreator>();
    public List<OpfMetadataCreator> Contributors { get; set; } = new List<OpfMetadataCreator>();
    public List<OpfMetadataDate> Dates { get; set; } = new List<OpfMetadataDate>();
    public List<string> Types { get; set; } = new List<string>();
    public List<string> Formats { get; set; } = new List<string>();
    public List<OpfMetadataIdentifier> Identifiers { get; set; } = new List<OpfMetadataIdentifier>();
    public List<string> Sources { get; set; } = new List<string>();
    public List<string> Languages { get; set; } = new List<string>();
    public List<string> Relations { get; set; } = new List<string>();
    public List<string> Coverages { get; set; } = new List<string>();
    public List<string> Rights { get; set; } = new List<string>();
    public List<OpfMetadataMeta> Metas { get; set; } = new List<OpfMetadataMeta>();

    public OpfMetadataMeta FindCoverMeta()
    {
        return Metas.FirstOrDefault(metaItem => metaItem.Name == "cover");
    }

    public OpfMetadataMeta FindAndDeleteCoverMeta()
    {
        var meta = FindCoverMeta();
        if (meta == null) return null;
        Metas.Remove(meta);
        return meta;
    }
}
