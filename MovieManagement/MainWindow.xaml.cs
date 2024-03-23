using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using MovieManagement.Views;


namespace MovieManagement
{

    public sealed partial class MainWindow : Window
    {
        // Init window mode
        // 0 -> User
        // 1 -> Admin
        public MainWindow(int mode)
        {
            this.InitializeComponent();

            // Set Backdrop
            SystemBackdrop = new MicaBackdrop() { Kind = MicaKind.BaseAlt };

            Page page = null;
            if (mode == 0)
            {
                page = new User_Frame();
            }
            else if (mode == 1)
            {
                var currentWindow = (Application.Current as App)?.m_window as MainWindow;
                var adminWindow = new AdminWindow();
                adminWindow.Activate();
                currentWindow.Close();
            }
            else
            {
                page = new User_Frame();
            }

            this.Content = page;

        }

    }


}
