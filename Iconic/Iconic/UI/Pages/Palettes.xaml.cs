using Iconic.ViewModel.Color;

namespace Iconic.UI.Pages
{
    /// <summary>
    /// Interaction logic for Palettes.xaml
    /// </summary>
    public partial class Palettes : BasePage<PalettesViewModel>
    {
        public Palettes()
        {
            InitializeComponent();
        }

        public Palettes(PalettesViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}