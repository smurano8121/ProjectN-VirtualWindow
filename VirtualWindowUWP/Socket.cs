using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;

namespace VirtualWindowUWP
{
    class Socket
    {
        private Windows.Networking.Sockets.StreamSocketListener socketListener;
        private Frame rootFrame;

        public async void CreateSocketListener(String portNum)
        {
            try
            {
                // Create a StreamSocketListener to start listening for TCP connections.
                socketListener = new Windows.Networking.Sockets.StreamSocketListener();

                // Hook up an event handler to call when connections are received.
                socketListener.ConnectionReceived += SocketListener_ConnectionReceived;

                // Start listening for incoming TCP connections on the specified port. You can specify any port that' s not currently in use.
                await socketListener.BindServiceNameAsync(portNum);
            }
            catch (Exception e)
            {
                Debug.WriteLine("CreateSocketListener() fault...");
                // Handle exception.
            }
        }

        public void setRootFrame(Frame frame)
        {
            rootFrame = frame;
        }

        private async void SocketListener_ConnectionReceived(Windows.Networking.Sockets.StreamSocketListener sender,
        Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            // Read line from the remote client.
            Stream inStream = args.Socket.InputStream.AsStreamForRead();
            StreamReader reader = new StreamReader(inStream);
            string request = await reader.ReadLineAsync();

            try
            {
                await CheckCommand(request, args);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Command Execution Fault...");
                Debug.WriteLine(e);
            }
        }

        // http://garicchi.com/?p=5891
        // string CheckCommand(String msg)
        private async Task CheckCommand(String msg, Windows.Networking.Sockets.StreamSocketListenerConnectionReceivedEventArgs args)
        {
            await Task.Run(async () =>
             {
                 string result = "";
                 Stream outStream = args.Socket.OutputStream.AsStreamForWrite();
                 StreamWriter writer = new StreamWriter(outStream);

                 await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 () =>
                 {
                     try
                     {
                         if (msg.IndexOf("IMAGE") >= 0)
                         {
                            // change mode to image mode and set the specified picture
                            rootFrame.ContentTransitions = new TransitionCollection();
                            rootFrame.ContentTransitions.Add(new NavigationThemeTransition());
                            rootFrame.Navigate(typeof(ImagePage));
                            result = "OK";
                        }
                         else if (msg.IndexOf("VIDEO") >= 0)
                         {
                            // change mode to video mode and set the specified video
                            rootFrame.ContentTransitions = new TransitionCollection();
                            rootFrame.ContentTransitions.Add(new NavigationThemeTransition());
                            rootFrame.Navigate(typeof(VideoPage));
                            result = "OK";
                        }
                         else if (msg.IndexOf("LIVE") >= 0)
                         {
                            // change mode to live mode
                            rootFrame.ContentTransitions = new TransitionCollection();
                            rootFrame.ContentTransitions.Add(new NavigationThemeTransition());
                            rootFrame.Navigate(typeof(LivePage));
                            result = "OK";
                        }
                         else if (msg.IndexOf("BLANK") >= 0)
                         {
                            // change mode to blank mode
                            rootFrame.ContentTransitions = new TransitionCollection();
                            rootFrame.ContentTransitions.Add(new NavigationThemeTransition());
                            rootFrame.Navigate(typeof(BlankPage));
                            result = "OK";
                        }
                         else if (msg.IndexOf("NEXT") >= 0)
                         {
                             switch (App.GetMode())
                             {
                                 case "ImagePage":
                                     ImagePage.NextImage(); break;
                                 case "VideoPage":
                                     VideoPage.NextVideo(); break;
                                 default:
                                     break;
                             }
                             result = "OK";
                         }
                         else if (msg.IndexOf("PREVIOUS") >= 0)
                         {
                             switch (App.GetMode())
                             {
                                 case "ImagePage":
                                     ImagePage.PreviousImage(); break;
                                 case "VideoPage":
                                     VideoPage.PreviousVideo(); break;
                                 default:
                                     break;
                             }
                             result = "OK";
                         }
                         else if (msg.IndexOf("GET_MODE") >= 0)
                         {
                             result = App.GetMode();
                         }
                         else
                         {
                             result = "Invalid command.";
                         }
                     }
                     catch
                     {
                         Debug.WriteLine("Failed to send command.");
                         result = "Failed to send command.";
                     }
                 });

                 // send back result strings
                 await writer.WriteLineAsync(result);
                 await writer.FlushAsync();
             });

        }

        public void CloseSocket()
        {
            socketListener.Dispose();
        }
    }
}
