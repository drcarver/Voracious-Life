using System.Collections.Generic;

using Voracious.EPub.Extensions;

namespace Voracious.EPub.Interface;

public interface ISetChapters
{
    void SetChapters(EpubBookExt book, List<EpubChapter> chapters);
}
