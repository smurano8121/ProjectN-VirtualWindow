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

// 空白ページの項目テンプレートについては、https://go.microsoft.com/fwlink/?LinkId=234238 を参照してください

namespace VirtualWindowUWP
{
    public sealed partial class VideoPane : Page
    {
        // To get video library, declare of function in app manifest is needed
        private static StorageFolder videoLibrary;

        public VideoPane()
        {
            this.InitializeComponent();

            videoLibrary = KnownFolders.VideosLibrary;

            ReadVideo();
        }

        private async void ReadVideo()
        {
            // for debug
            StorageFile video = await videoLibrary.GetFileAsync("virtualWindow\\video_06.mp4");
            var stream = await video.OpenAsync(Windows.Storage.FileAccessMode.Read);

            videoPlayer.SetSource(stream, video.ContentType);
        }

    }
}
