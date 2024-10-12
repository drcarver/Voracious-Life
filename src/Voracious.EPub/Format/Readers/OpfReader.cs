using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using Voracious.EPub.Enum;
using Voracious.EPub.Extensions;
using Voracious.EPub.OPF;

namespace Voracious.EPub.Format.Readers;

static class OpfReader
{
    public static OpfDocument Read(XDocument xml)
    {
        if (xml == null) throw new ArgumentNullException(nameof(xml));
        if (xml.Root == null) throw new ArgumentException("XML document has no root element.", nameof(xml));

        Func<XElement, OpfMetadataCreator> readCreator = elem => new OpfMetadataCreator
        {
            Role = (string)elem.Attribute(OpfMetadataCreator.Attributes.Role),
            FileAs = (string)elem.Attribute(OpfMetadataCreator.Attributes.FileAs),
            AlternateScript = (string)elem.Attribute(OpfMetadataCreator.Attributes.AlternateScript),
            Text = elem.Value
        };

        var epubVersion = GetAndValidateVersion((string)xml.Root.Attribute(OpfDocument.Attributes.Version));
        var metadata = xml.Root.Element(OpfElements.Metadata);
        var guide = xml.Root.Element(OpfElements.Guide);
        var spine = xml.Root.Element(OpfElements.Spine);

        var package = new OpfDocument
        {
            UniqueIdentifier = (string)xml.Root.Attribute(OpfDocument.Attributes.UniqueIdentifier),
            EpubVersion = epubVersion,
            Metadata = new OpfMetadata
            {
                Creators = (List<OpfMetadataCreator>)(metadata?.Elements(OpfElements.Creator).AsObjectList(readCreator)),
                Contributors = (List<OpfMetadataCreator>)(metadata?.Elements(OpfElements.Contributor).AsObjectList(readCreator)),
                Coverages = (List<string>)(metadata?.Elements(OpfElements.Coverages).AsStringList()),
                Dates = (List<OpfMetadataDate>)(metadata?.Elements(OpfElements.Date).AsObjectList(elem => new OpfMetadataDate
                {
                    Text = elem.Value,
                    Event = (string)elem.Attribute(OpfMetadataDate.Attributes.Event)
                })),
                Descriptions = (List<string>)(metadata?.Elements(OpfElements.Description).AsStringList()),
                Formats = (List<string>)(metadata?.Elements(OpfElements.Format).AsStringList()),
                Identifiers = (List<OpfMetadataIdentifier>)(metadata?.Elements(OpfElements.Identifier).AsObjectList(elem => new OpfMetadataIdentifier
                {
                    Id = (string)elem.Attribute(OpfMetadataIdentifier.Attributes.Id),
                    Scheme = (string)elem.Attribute(OpfMetadataIdentifier.Attributes.Scheme),
                    Text = elem.Value
                })),
                Languages = (List<string>)(metadata?.Elements(OpfElements.Language).AsStringList()),
                Metas = (List<OpfMetadataMeta>)(metadata?.Elements(OpfElements.Meta).AsObjectList(elem => new OpfMetadataMeta
                {
                    Id = (string)elem.Attribute(OpfMetadataMeta.Attributes.Id),
                    Name = (string)elem.Attribute(OpfMetadataMeta.Attributes.Name),
                    Refines = (string)elem.Attribute(OpfMetadataMeta.Attributes.Refines),
                    Scheme = (string)elem.Attribute(OpfMetadataMeta.Attributes.Scheme),
                    Property = (string)elem.Attribute(OpfMetadataMeta.Attributes.Property),
                    Text = epubVersion == EpubVersionEnum.Epub2 ? (string)elem.Attribute(OpfMetadataMeta.Attributes.Content) : elem.Value
                })),
                Publishers = (List<string>)(metadata?.Elements(OpfElements.Publisher).AsStringList()),
                Relations = (List<string>)(metadata?.Elements(OpfElements.Relation).AsStringList()),
                Rights = (List<string>)(metadata?.Elements(OpfElements.Rights).AsStringList()),
                Sources = (List<string>)(metadata?.Elements(OpfElements.Source).AsStringList()),
                Subjects = (List<string>)(metadata?.Elements(OpfElements.Subject).AsStringList()),
                Titles = (List<string>)(metadata?.Elements(OpfElements.Title).AsStringList()),
                Types = (List<string>)(metadata?.Elements(OpfElements.Type).AsStringList())
            },
            Guide = guide == null ? null : new OpfGuide
            {
                References = guide.Elements(OpfElements.Reference).AsObjectList(elem => new OpfGuideReference
                {
                    Title = (string)elem.Attribute(OpfGuideReference.Attributes.Title),
                    Type = (string)elem.Attribute(OpfGuideReference.Attributes.Type),
                    Href = (string)elem.Attribute(OpfGuideReference.Attributes.Href)
                })
            },
            Manifest = new OpfManifest
            {
                Items = xml.Root.Element(OpfElements.Manifest)?.Elements(OpfElements.Item).AsObjectList(elem => new OpfManifestItem
                {
                    Fallback = (string)elem.Attribute(OpfManifestItem.Attributes.Fallback),
                    FallbackStyle = (string)elem.Attribute(OpfManifestItem.Attributes.FallbackStyle),
                    Href = (string)elem.Attribute(OpfManifestItem.Attributes.Href),
                    Id = (string)elem.Attribute(OpfManifestItem.Attributes.Id),
                    MediaType = (string)elem.Attribute(OpfManifestItem.Attributes.MediaType),
                    Properties = ((string)elem.Attribute(OpfManifestItem.Attributes.Properties))?.Split(' ').ToList<string>() ?? new List<string>(),
                    RequiredModules = (string)elem.Attribute(OpfManifestItem.Attributes.RequiredModules),
                    RequiredNamespace = (string)elem.Attribute(OpfManifestItem.Attributes.RequiredNamespace)
                })
            },
            Spine = new OpfSpine
            {
                ItemRefs = (List<OpfSpineItemRef>)(spine?.Elements(OpfElements.ItemRef).AsObjectList(elem => new OpfSpineItemRef
                {
                    IdRef = (string)elem.Attribute(OpfSpineItemRef.Attributes.IdRef),
                    Linear = (string)elem.Attribute(OpfSpineItemRef.Attributes.Linear) != "no",
                    Id = (string)elem.Attribute(OpfSpineItemRef.Attributes.Id),
                    Properties = ((string)elem.Attribute(OpfSpineItemRef.Attributes.Properties))?.Split(' ').ToList() ?? new List<string>()
                })),
                Toc = spine?.Attribute(OpfSpine.Attributes.Toc)?.Value
            }
        };

        return package;
    }

    private static EpubVersionEnum GetAndValidateVersion(string version)
    {
        if (string.IsNullOrWhiteSpace(version)) throw new ArgumentNullException(nameof(version));

        if (version == "2.0")
        {
            return EpubVersionEnum.Epub2;
        }
        if (version == "3.0" || version == "3.0.1" || version == "3.1")
        {
            return EpubVersionEnum.Epub3;
        }

        throw new Exception($"Unsupported EPUB version: {version}.");
    }
}
