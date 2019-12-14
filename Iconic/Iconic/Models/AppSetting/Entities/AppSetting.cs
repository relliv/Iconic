using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Iconic.Models.AppSetting
{
    public class AppSetting : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public string SettingName { get; set; }
        [Required]
        public string Value { get; set; }
        [Required]
        public bool IsEditable { get; set; }
        public string DefaultValue { get; set; }
    }
}