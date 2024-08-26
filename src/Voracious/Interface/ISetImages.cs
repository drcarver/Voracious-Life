using Voracious.EpubSharp;

namespace Voracious.Interface
{
    public interface ISetImages
    {
        Task SetImagesAsync(ICollection<EpubByteFile> images);
    }
}
