using GalaSoft.MvvmLight;
using Iconic.Data;
using Iconic.Models.Common;
using Iconic.ViewModel.Base;
using System.ComponentModel;

namespace Iconic.ViewModel.App
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        public ApplicationViewModel()
        {
            using var db = new AppDbContext();

            AppSettings = new AppSettings();
        }

        #region Properties

        public ApplicationPage CurrentPage { get; private set; } = ApplicationPage.WelcomePage;

        public ViewModelBase CurrentPageViewModel { get; set; }

        public AppSettings AppSettings { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, ViewModelBase viewModel = null)
        {
            CurrentPageViewModel = viewModel;

            var different = CurrentPage != page;

            CurrentPage = page;

            if (!different)
                OnPropertyChanged(nameof(CurrentPage));
        }

        #endregion
    }
}
