using System.Collections.Generic;
using System.Linq;

using Voracious.EPub.Enum;

namespace Voracious.EPub.Format;

public class ContentType
{
    public static readonly IReadOnlyDictionary<string, EpubContentEnum> MimeTypeToContentType = new Dictionary<string, EpubContentEnum>
    {
        { "application/xhtml+xml", EpubContentEnum.Xhtml11 },
        { "application/x-dtbook+xml", EpubContentEnum.Dtbook },
        { "application/x-dtbncx+xml", EpubContentEnum.DtbookNcx },
        { "text/x-oeb1-document", EpubContentEnum.Oeb1Document },
        { "application/xml", EpubContentEnum.Xml },
        { "text/css", EpubContentEnum.Css },
        { "text/x-oeb1-css", EpubContentEnum.Oeb1Css },
        { "image/gif", EpubContentEnum.ImageGif },
        { "image/jpeg", EpubContentEnum.ImageJpeg },
        { "image/png", EpubContentEnum.ImagePng },
        { "image/svg+xml", EpubContentEnum.ImageSvg },
        { "font/truetype", EpubContentEnum.FontTruetype },
        { "font/opentype", EpubContentEnum.FontOpentype },
        { "application/vnd.ms-opentype", EpubContentEnum.FontOpentype }
    };

    public static readonly IReadOnlyDictionary<EpubContentEnum, string> ContentTypeToMimeType = MimeTypeToContentType
        .Where(pair => pair.Key != "application/vnd.ms-opentype") // Because it's defined twice.
        .ToDictionary(pair => pair.Value, pair => pair.Key);
}
