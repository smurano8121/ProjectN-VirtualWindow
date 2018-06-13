using MjpegProcessor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Streaming.Adaptive;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using VLC;


namespace VirtualWindowUWP
{
    public sealed partial class LivePage : Page
    {
        private MjpegDecoder mjpegDecoder;

        public LivePage()
        {
            this.InitializeComponent();
            mjpegDecoder = new MjpegDecoder();
            mjpegDecoder.FrameReady += mjpeg_FrameReady;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            mjpegDecoder.ParseStream(new Uri("http://172.20.11.46/-wvhttp-01-/video.cgi"));
            // Project N camera url
            // http://192.168.0.10/cgi-bin/mjpeg?framerate=15&resolution=640x480
        }

        private async void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            using (InMemoryRandomAccessStream ms = new InMemoryRandomAccessStream())
            {
                await ms.WriteAsync(e.FrameBuffer);
                ms.Seek(0);

                var bmp = new BitmapImage();
                await bmp.SetSourceAsync(ms);

                //image is the Image control in XAML
                image.Source = bmp;
            }
        }

        //public LivePage()
        //{
        //    this.InitializeComponent();
        //    vlc_player.HardwareAcceleration = true;

        //    // digest??
        //    // vlc_player.Options.Add("rtsp-host", "192.168.0.10");
        //    // vlc_player.Options.Add("network-caching", "1000");
        //    // vlc_player.Options.Add("rtsp-user", "projectn");
        //    // vlc_player.Options.Add("rtsp-pwd", "projectn2018");
        //    // vlc_player.Source = "rtsp://projectn:projectn2018@192.168.0.10/MediaInput/h264";
        //    /* 暫定的にMJpegを指定 */
        //    vlc_player.Source = "http://192.168.10.13/cgi-bin/mjpeg?stream=1&resolution=4000x3000&quality=1&page=1520654672323&Language=1";

        //    /* DEBUG */
        //    // vlc_player.Source = "rtsp://184.72.239.149/vod/mp4:BigBuckBunny_175k.mov";
        //    // vlc_player.Options.Add("rtsp-host", "184.72.239.149");

        //    vlc_player.Stretch = Stretch.UniformToFill;
        //    vlc_player.Play();
        //}

    }
}
