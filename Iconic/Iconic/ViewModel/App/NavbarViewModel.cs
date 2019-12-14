using System.Collections.Generic;
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
            LeftSideBarItems = new List<NavbarItem>()
            {
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.Home,
                    IconData = (System.Windows.Application.Current.FindResource("Home") as Path)?.Data,
                    IsChecked = true
                }
            };

            GoToCommand = new RelayParameterizedCommand(GoTo);
        }

        public List<NavbarItem> LeftSideBarItems { get; set; }

        public ICommand GoToCommand { get; set; }

        public void GoTo(object sender)
        {
            if (sender == null || !(sender is ToggleButton toggleButtonbutton)) return;

            if (!(toggleButtonbutton.DataContext is NavbarItem leftSideBarItem)) return;

            if (ViewModelApplication.CurrentPage != leftSideBarItem.ApplicationPage)
            {
                ViewModelApplication.GoToPage(leftSideBarItem.ApplicationPage);
            }
        }
    }
}
