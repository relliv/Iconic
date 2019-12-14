using Dna;
using Iconic.Models.Common;
using Iconic.ViewModel.App;

namespace Iconic.DI
{
    /// <summary>
    /// A shorthand access class to get DI services with nice clean short code
    /// </summary>
    public static class DI
    {
        /// <summary>
        /// A shortcut to access the <see cref="ApplicationViewModel"/>
        /// </summary>
        public static ApplicationViewModel ViewModelApplication => Framework.Service<ApplicationViewModel>();

        public static AppSettings AppSettings => Framework.Service<AppSettings>();
    }
}