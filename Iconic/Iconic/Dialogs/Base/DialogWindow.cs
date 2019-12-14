using System.Windows;
using System.Windows.Media.Effects;
using Iconic.ViewModel;

namespace Iconic.Dialogs
{
    public class DialogWindow : Window
    {
        public void ShowDialogWindow<T>(T viewModel, Window owner = null) where T : WindowViewModel
        {
            Owner = owner ?? Application.Current.MainWindow;
            DataContext = viewModel;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            ShowInTaskbar = false;

            if (Owner != null)
            {
                Owner.Effect = new BlurEffect()
                {
                    Radius = 7
                };
            }

            ShowDialog();

            if (Owner != null)
            {
                Owner.Effect = null;
            }
        }
    }
}
