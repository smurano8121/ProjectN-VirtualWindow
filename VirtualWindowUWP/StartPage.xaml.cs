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

        public void NavigateButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Type pageType = null;
            switch (button.Name)
            {
                case "liveButton":
                    pageType = typeof(LivePage); break;
                case "blankButton":
                    pageType = typeof(BlankPage); break;
                case "imageButton":
                    pageType = typeof(ImagePage); break;
                case "videoButton":
                    pageType = typeof(VideoPage); break;
            }
            App.NavigateTo(pageType);
        }
    }
}
