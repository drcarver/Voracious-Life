using System.ComponentModel;
using System.Runtime.CompilerServices;

using Voracious.EpubSharp;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Voracious.Controls
{
    public class ImageData : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string Href { get; set; }
        public EpubByteFile EpubImage { get; set; }
        public BitmapImage _ImageSource = null;
        public ImageSource ImageSource
        {
            get
            {
                if (_ImageSource != null)
                    return _ImageSource;
                return _ImageSource;
            }
        }
        public int NColumns
        {
            get
            {
                if (_ImageSource == null) return 1;
                var ratio = (double)_ImageSource.PixelWidth / (double)_ImageSource.PixelHeight;
                var nc = Math.Round(ratio);
                if (nc < 1) nc = 1;
                if (nc > 3) nc = 3;
                return (int)nc;
            }
        }
        public async Task SetImageAsync()
        {
            var bytes = EpubImage.Content;
            BitmapImage imageSource = new BitmapImage();
            var stream = bytes.AsBuffer().AsStream().AsRandomAccessStream();
            await imageSource.SetSourceAsync(stream);
            _ImageSource = imageSource;
            NotifyPropertyChanged("ImageSource");
        }

        // The stuff for INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
