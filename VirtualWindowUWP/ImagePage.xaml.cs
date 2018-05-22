using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Diagnostics;
using Windows.System;
using Windows.UI.Core;

namespace VirtualWindowUWP
{
    public sealed partial class ImagePage : Page
    {
        // To get picture library, we have to declare the function in app manifest.
        private static StorageFolder pictureLiblary;

        public ImagePage()
        {
            this.InitializeComponent();
            pictureLiblary = KnownFolders.PicturesLibrary;

            // Add KeyDown event handler into CoreWindow
            // Have to remove this handler when this page is unloaded.
            Window.Current.CoreWindow.KeyDown += KeyDownHandle;
            this.Unloaded += (sender, e) =>
            {
                Window.Current.CoreWindow.KeyDown -= KeyDownHandle;
            };
        }

        private async void ReadImage()
        {
            Debug.WriteLine("OK.");
            // for debug
            Windows.Storage.StorageFile pic = await pictureLiblary.GetFileAsync("virtualWindow\\pic_01.jpg");

            BitmapImage img = new BitmapImage();
            img = await LoadImage(pic);

            imageView.Source = img;
        }

        private static async Task<BitmapImage> LoadImage(StorageFile file)
        {
            BitmapImage bitmapImage = new BitmapImage();
            FileRandomAccessStream stream = (FileRandomAccessStream)await file.OpenAsync(FileAccessMode.Read);

            bitmapImage.SetSource(stream);

            return bitmapImage;

        }

        // CoreWindow.KeyDown event handler only used in this page.
        private void KeyDownHandle(object send, Windows.UI.Core.KeyEventArgs e)
        {
            Debug.WriteLine("Change");
            switch (e.VirtualKey)
            {
                case VirtualKey.Escape:
                    // ESCキーを押した時呼ばれる
                    break;
                case Windows.System.VirtualKey.Z:
                    // zキーを押した時呼ばれる
                    ReadImage();
                    break;

            }
        }
    }
}
