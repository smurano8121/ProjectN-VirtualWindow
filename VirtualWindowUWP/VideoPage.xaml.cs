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
using Windows.Storage.FileProperties;

namespace VirtualWindowUWP
{
    public sealed partial class VideoPage : Page
    {
        // To get video library, we have to declare the function in app manifest.
        private static StorageFolder videoLibrary = KnownFolders.VideosLibrary;
        // The list which contains stored videos in video library.
        private static IReadOnlyList<StorageFile> storedVideo;
        // File number index of stored video which is shown in Media Element.
        private static int videoIndex = 0;
        // Media element static object
        private static MediaElement videoObject;
        // Thumbnail object
        private static List<StorageItemThumbnail> thumbnailList;

        public VideoPage()
        {
            this.InitializeComponent();

            // Add KeyDown event handler into CoreWindow
            // Have to remove this handler when this page is unloaded.
            Window.Current.CoreWindow.KeyDown += KeyDownHandle;
            this.Unloaded += (sender, e) =>
            {
                Window.Current.CoreWindow.KeyDown -= KeyDownHandle;
            };

            // set MediaElement into static variable
            videoObject = videoPlayer;

            // Show first image file stored in picture library.
            // Note: "first image" means the top file when files are sorted by Name.
            ReadVideo();
        }
        public static async void GetVideoList()
        {
            // load image files upto 100.
            videoLibrary = await videoLibrary.GetFolderAsync("VirtualWindow");
            storedVideo = await videoLibrary.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName, 0, 100);

            // // get tumbnails
            GetThumbs();
        }

        private static async void ReadVideo()
        {
            StorageFile video = storedVideo[videoIndex];
            var stream = await video.OpenAsync(Windows.Storage.FileAccessMode.Read);

            videoObject.SetSource(stream, video.ContentType);
        }

        // CoreWindow.KeyDown event handler only used in this page.
        private void KeyDownHandle(object send, Windows.UI.Core.KeyEventArgs e)
        {
            switch (e.VirtualKey)
            {
                case VirtualKey.Right:
                    NextVideo();
                    break;
                case VirtualKey.Left:
                    PreviousVideo();
                    break;
            }
        }

        public static void NextVideo()
        {
            videoIndex = videoIndex == storedVideo.Count - 1 ? 0 : videoIndex + 1;
            ReadVideo();
        }

        public static void PreviousVideo()
        {
            videoIndex = videoIndex == 0 ? storedVideo.Count - 1 : videoIndex - 1;
            ReadVideo();
        }

        public static async void GetThumbs()
        {
            thumbnailList = new List<StorageItemThumbnail>();
            foreach (StorageFile file in storedVideo)
            {
                // Get thumbnail
                const uint requestedSize = 450;
                const ThumbnailMode thumbnailMode = ThumbnailMode.VideosView;
                const ThumbnailOptions thumbnailOptions = ThumbnailOptions.UseCurrentScale;
                var tmp = await file.GetThumbnailAsync(thumbnailMode, requestedSize, thumbnailOptions);
                thumbnailList.Add(tmp);
            }
        }

        public static List<StorageItemThumbnail> GetThumbnailList()
        {
            return thumbnailList;
        }

        public static void SetVideoIndex(int i)
        {
            videoIndex = i;
            ReadVideo();
        }
    }
}
