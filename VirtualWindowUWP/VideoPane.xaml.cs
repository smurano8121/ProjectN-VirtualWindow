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
using Windows.System;

namespace VirtualWindowUWP
{
    public sealed partial class VideoPane : Page
    {
        // To get video library, we have to declare the function in app manifest.
        private static StorageFolder videoLibrary;
        // The list which contains stored videos in video library.
        private IReadOnlyList<StorageFile> storedVideo;
        // File number index of stored video which is shown in Media Element.
        private int videoIndex = 0;


        public VideoPane()
        {
            this.InitializeComponent();

            videoLibrary = KnownFolders.VideosLibrary;

            // Read Image File from picture library.
            GetVideoList();

            // Add KeyDown event handler into CoreWindow
            // Have to remove this handler when this page is unloaded.
            Window.Current.CoreWindow.KeyDown += KeyDownHandle;
            this.Unloaded += (sender, e) =>
            {
                Window.Current.CoreWindow.KeyDown -= KeyDownHandle;
            };
        }
        private async void GetVideoList()
        {
            // load image files upto 100.
            videoLibrary = await videoLibrary.GetFolderAsync("VirtualWindow");
            storedVideo = await videoLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName, 0, 100);

            // Show first image file stored in picture library.
            // Note: "first image" means the top file when files are sorted by Name.
            ReadVideo();
        }

        private async void ReadVideo()
        {
            StorageFile video = storedVideo[videoIndex];
            var stream = await video.OpenAsync(Windows.Storage.FileAccessMode.Read);

            videoPlayer.SetSource(stream, video.ContentType);
        }

        // CoreWindow.KeyDown event handler only used in this page.
        private void KeyDownHandle(object send, Windows.UI.Core.KeyEventArgs e)
        {
            switch (e.VirtualKey)
            {
                case VirtualKey.Right:
                    videoIndex = videoIndex == storedVideo.Count - 1 ? 0 : videoIndex + 1;
                    break;
                case VirtualKey.Left:
                    videoIndex = videoIndex == 0 ? storedVideo.Count - 1 : videoIndex - 1;
                    break;
            }
            ReadVideo();
        }
    }
}
