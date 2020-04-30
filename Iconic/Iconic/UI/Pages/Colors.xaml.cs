using Iconic.ViewModel.Color;

namespace Iconic.UI.Pages
{
    /// <summary>
    /// Interaction logic for Colors.xaml
    /// </summary>
    public partial class Colors : BasePage<ColorsViewModel>
    {
        public Colors() : base()
        {
            InitializeComponent();
        }

        public Colors(ColorsViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}