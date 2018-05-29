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
        private static StorageFolder pictureLibrary;
        // The list which contains stored pictures in picture library.
        private static IReadOnlyList<StorageFile> storedPicture;
        // File number index of stored picture which is shown in Image view.
        private static int imageIndex = 0;

        private static Image imageViewObject;

 

        public ImagePage()
        {
            this.InitializeComponent();

            pictureLibrary = KnownFolders.PicturesLibrary;

            // Read Image File from picture library.
            GetImageList();

            // Add KeyDown event handler into CoreWindow
            // Have to remove this handler when this page is unloaded.
            Window.Current.CoreWindow.KeyDown += KeyDownHandle;
            this.Unloaded += (sender, e) =>
            {
                Window.Current.CoreWindow.KeyDown -= KeyDownHandle;
            };

            imageViewObject = imageView;
        }

        private async void GetImageList()
        {
            // load image files upto 100.
            pictureLibrary = await pictureLibrary.GetFolderAsync("VirtualWindow");
            storedPicture = await pictureLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName, 0, 100);

            // Show first image file stored in picture library.
            // Note: "first image" means the top file when files are sorted by Name.
            ReadImage();
        }

        private static async void ReadImage()
        {
            Windows.Storage.StorageFile pic = storedPicture[imageIndex];

            BitmapImage img = new BitmapImage();
            img = await LoadImage(pic);

            imageViewObject.Source = img;
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
            switch (e.VirtualKey)
            {
                case VirtualKey.Right:
                    NextImage();
                    break;
                case VirtualKey.Left:
                    PreviousImage();
                    break;
            }
        }

        public static void NextImage()
        {
            imageIndex = imageIndex == storedPicture.Count - 1 ? 0 : imageIndex + 1;
            ReadImage();
        }

        public static void PreviousImage()
        {
            imageIndex = imageIndex == 0 ? storedPicture.Count - 1 : imageIndex - 1;
            ReadImage();
        }
    }
}
