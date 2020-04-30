using System.Collections.Generic;

namespace Iconic.Models.Color
{
    public class MaterialColorPalette
    {
        public MaterialColorPalette()
        {
            MaterialColors = new List<MaterialColor>();
        }

        public string PaletteName { get; set; }
        public List<MaterialColor> MaterialColors { get; set; }
    }
}