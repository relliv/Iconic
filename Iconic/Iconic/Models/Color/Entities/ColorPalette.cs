using System.ComponentModel;

namespace Iconic.Models.Color.Entities
{
    public class ColorPalette : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }

        public int PaletteNumber { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Color3 { get; set; }
        public string Color4 { get; set; }
        public string Color5 { get; set; }
    }
}