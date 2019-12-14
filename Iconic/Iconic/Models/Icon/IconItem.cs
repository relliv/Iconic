using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Iconic.Models.Icon
{
    public class IconItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CopyDataCommand { get; set; }
        public ICommand CopySVGforHTMLCommand { get; set; }
        public ICommand CopySVGCommand { get; set; }
        public ICommand CopyXMLCommand { get; set; }
        public ICommand CopyXAMLCanvasCommand { get; set; }
        public ICommand CopyXAMLGeometryCommand { get; set; }
        public ICommand CopyXAMLPathCommand { get; set; }

        public ICommand OpenIconEditorCommand { get; set; }

        public Icon Icon { get; set; }
    }
}