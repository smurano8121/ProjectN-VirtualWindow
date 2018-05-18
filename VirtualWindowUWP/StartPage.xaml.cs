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
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Animation;
using System.Diagnostics;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VirtualWindowUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StartPage : Page
    {

        public StartPage()
        {
            this.InitializeComponent();
            // create socket datagram
            CreateSocketListener();
        }

        public void BlankButton_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = this.Frame;
            frame.ContentTransitions = new TransitionCollection();
            frame.ContentTransitions.Add(new NavigationThemeTransition());
            frame.Navigate(typeof(Blank));
        }

        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = this.Frame;
            frame.ContentTransitions = new TransitionCollection();
            frame.ContentTransitions.Add(new NavigationThemeTransition());
            frame.Navigate(typeof(ImagePage));
        }

        private void VideoButton_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = this.Frame;
            frame.ContentTransitions = new TransitionCollection();
            frame.ContentTransitions.Add(new NavigationThemeTransition());
            frame.Navigate(typeof(VideoPane));
        }

        private void LiveButton_Click(object sender, RoutedEventArgs e)
        {
            Frame frame = this.Frame;
            frame.ContentTransitions = new TransitionCollection();
            frame.ContentTransitions.Add(new NavigationThemeTransition());
            frame.Navigate(typeof(LivePage));
        }

        private void TextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        async void CreateSocketListener()
        {
            try
            {
                //Create a StreamSocketListener to start listening for TCP connections.
                Windows.Networking.Sockets.StreamSocketListener socketListener = new Windows.Networking.Sockets.StreamSocketListener();

                //Hook up an event handler to call when connections are received.
                socketListener.ConnectionReceived += SocketListener_ConnectionReceived;

                //Start listening for incoming TCP connections on the specified port. You can specify any port that' s not currently in use.
                await socketListener.BindServiceNameAsync("50005");
                Debug.WriteLine("OK.");
            }
            catch (Exception e)
            {
                //Handle exception.
            }
        }

        private async void SocketListener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender,
        Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            //Read line from the remote client.
            Stream inStream = args.Socket.InputStream.AsStreamForRead();
            StreamReader reader = new StreamReader(inStream);
            string request = await reader.ReadLineAsync();

            // string check = CheckCommand(request);
            await CheckCommand(request);
        }

        // http://garicchi.com/?p=5891
        // string CheckCommand(String msg)
        private async Task CheckCommand(String msg)
        {
            await Task.Run(() =>
            {
                // Frame rootFrame = Window.Current.Content as Frame;
                Frame rootFrame = this.Frame;
                Debug.WriteLine("check command.");
                if (msg.IndexOf("IMAGE") >= 0)
                {
                    Debug.WriteLine("Image!");
                    // change mode to image mode and set the specified picture
                    rootFrame.ContentTransitions = new TransitionCollection();
                    rootFrame.ContentTransitions.Add(new NavigationThemeTransition());
                    rootFrame.Navigate(typeof(ImagePage));
                    // return "OK";
                }
                else if (msg.IndexOf("VIDEO") >= 0)
                {
                    // change mode to video mode and set the specified video
                    rootFrame.Navigate(typeof(VideoPane));
                    // return "OK";
                }
                else if (msg.IndexOf("LIVE") >= 0)
                {
                    // change mode to live mode

                    // return "OK";
                }
                else if (msg.IndexOf("BLANK") >= 0)
                {
                    // change mode to blank mode
                    Debug.WriteLine("Blank accepted.");
                    // return "OK";
                }
                // return "NG";
            });
            
        }
    }
}
