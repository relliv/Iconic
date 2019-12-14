using System.ComponentModel;
using System.Windows.Input;

namespace Iconic.Models.Icon
{
    public class ColorItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CopyHexCommand { get; set; }

        public string ColorName { get; set; }
        public string ColorHex { get; set; }
    }
}