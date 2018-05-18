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
    public sealed partial class StartPage : Page
    {

        public StartPage()
        {
            this.InitializeComponent();
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

        
    }
}
