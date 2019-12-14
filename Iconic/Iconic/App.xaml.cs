using Dna;
using Iconic.DI;
using Iconic.Models.Common;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Iconic.Data;
using Iconic.Dialogs;
using Iconic.ViewModel;
using Iconic.Helpers;
using static Iconic.DI.DI;

namespace Iconic
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            new IconImporter();

            _ = new AppDbContext();

            ApplicationSetup();

            ViewModelApplication.GoToPage(ApplicationPage.Icons);

            Current.MainWindow = new MainWindow();
            Current.MainWindow.DataContext = new WindowViewModel(Current.MainWindow);
            Current.MainWindow.Show();
        }

        private void ApplicationSetup()
        {
            Framework.Construct<DefaultFrameworkConstruction>()
                .AddFileLogger()
                .AddAppViewModels()
                .Build();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            var dialog = new MessageDialog();
            //dialog.ShowDialogWindow(new MessageDialogViewModel(dialog, "Error: Application_DispatcherUnhandledException", args.Exception.ToString()));

            args.Handled = true;
        }

        private void TaskSchedulerOnUnobservedTaskException(UnobservedTaskExceptionEventArgs args)
        {
            var dialog = new MessageDialog();
            //dialog.ShowDialogWindow(new MessageDialogViewModel(dialog, "Error: TaskSchedulerOnUnobservedTaskException", args.Exception.ToString()));
        }

        private void CurrentOnDispatcherUnhandledException(DispatcherUnhandledExceptionEventArgs args)
        {
            var dialog = new MessageDialog();
            //dialog.ShowDialogWindow(new MessageDialogViewModel(dialog, "Error: CurrentOnDispatcherUnhandledException", args.Exception.ToString()));

            args.Handled = true;
        }
    }
}