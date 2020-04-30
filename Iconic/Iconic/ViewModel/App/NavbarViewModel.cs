using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using Iconic.Models.Common;
using static Iconic.DI.DI;

namespace Iconic.ViewModel.App
{
    public class NavbarViewModel : ViewModelBase
    {
        public NavbarViewModel()
        {
            NavbarItems = new ObservableCollection<NavbarItem>()
            {
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.Icons,
                    IconData = (System.Windows.Application.Current.FindResource("ArrangeSendToBack") as Path)?.Data,
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.Colors,
                    IconData = (System.Windows.Application.Current.FindResource("FormatColorFill") as Path)?.Data,
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.Palettes,
                    IconData = (System.Windows.Application.Current.FindResource("Home") as Path)?.Data,
                }
            };

            foreach (var item in NavbarItems)
            {
                if (item.ApplicationPage == ViewModelApplication.CurrentPage)
                {
                    item.IsChecked = true;
                    break;
                }
            }

            GoToCommand = new RelayParameterizedCommand(GoTo);
        }

        public ObservableCollection<NavbarItem> NavbarItems { get; set; }

        public ICommand GoToCommand { get; set; }

        public void GoTo(object sender)
        {
            if (sender == null || !(sender is ToggleButton toggleButtonbutton)) return;

            if (!(toggleButtonbutton.DataContext is NavbarItem navbarItem)) return;

            foreach (var item in NavbarItems)
            {
                item.IsChecked = false;
            }

            navbarItem.IsChecked = true;

            if (ViewModelApplication.CurrentPage != navbarItem.ApplicationPage)
            {
                ViewModelApplication.GoToPage(navbarItem.ApplicationPage);
            }
        }
    }
}
