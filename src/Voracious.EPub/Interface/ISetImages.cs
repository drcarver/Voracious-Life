using System.Collections.Generic;
using System.Threading.Tasks;

namespace Voracious.EPub.Interface;

public interface ISetImages
{
    Task SetImagesAsync(ICollection<EpubByteFile> images);
}
