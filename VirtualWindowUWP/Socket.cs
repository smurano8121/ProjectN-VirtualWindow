using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;

namespace VirtualWindowUWP
{
    class Socket
    {
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
                Debug.WriteLine("NG");
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

            Debug.WriteLine("Accept");
            // string check = CheckCommand(request);
            await CheckCommand(request);
        }

        // http://garicchi.com/?p=5891
        // string CheckCommand(String msg)
        private async Task CheckCommand(String msg)
        {
            await Task.Run(() =>
            {
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
