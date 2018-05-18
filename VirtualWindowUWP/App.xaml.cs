using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Threading.Tasks;

namespace VirtualWindowUWP
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>

    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(StartPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }

            var titleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;

            // Make titlebar as view
            CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

            // Set active window colors
            titleBar.ForegroundColor = Windows.UI.Colors.White;
            titleBar.BackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonHoverBackgroundColor = Windows.UI.Colors.LightGray;
            titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonPressedBackgroundColor = Windows.UI.Colors.DarkGray;

            // Enter to fullscreen mode
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();

            // Stretch window size to all working area
            var x = 0;
            var y = 0;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(x, y));
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryResizeView(new Size(x, y));

            // manage key listener
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;

            // create socket datagram
            CreateSocketListener();

        }

          

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs e)
        {
            Frame frame = Window.Current.Content as Frame;
            frame.ContentTransitions = new TransitionCollection();
            frame.ContentTransitions.Add(new NavigationThemeTransition());

            switch (e.VirtualKey)
            {
                // switch full screen mode
                case Windows.System.VirtualKey.F:
                    if (Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().IsFullScreenMode)
                    {
                        Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().ExitFullScreenMode();
                    }
                    else
                    {
                        Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TryEnterFullScreenMode();
                    }
                    break;
                // go blank screen
                case Windows.System.VirtualKey.B:
                    break;
                // go to start page
                case Windows.System.VirtualKey.Escape:
                    if (frame == null)
                        return;

                    // Navigate back if possible, and if the event has not 
                    // already been handled .
                    if (frame.CanGoBack && e.Handled == false)
                    {
                        e.Handled = true;
                        frame.GoBack();

                        // Clear all BackStack properties
                        frame.BackStack.Clear();
                    }
                    break;
            }
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
                Frame rootFrame = Window.Current.Content as Frame;
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
