using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using DataAccessLibrary;
using DataAccessLibrary.Sources.Common;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace EU4Editor.Views
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            
        }
        private NavigationViewItem currentNavigationViewItem;
        private void NavigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;
            if (selectedItem != null && selectedItem != currentNavigationViewItem)
            {
                currentNavigationViewItem = selectedItem;
                switch ((string)selectedItem.Tag)//Need to add assembly to Type.GetType
                {
                    case "ReligionsPage":
                        contentFrame.Navigate(typeof(ReligionsPage));
                        break;
                    case "CountriesPage":
                        contentFrame.Navigate(typeof(CountriesPage));
                        break;
                    case "ProvincesPage":
                        contentFrame.Navigate(typeof(ProvincesPage));
                        break;
                    case "IdeasPage":
                        contentFrame.Navigate(typeof(IdeasPage));
                        break;
                }
            }
        }
    }
}
