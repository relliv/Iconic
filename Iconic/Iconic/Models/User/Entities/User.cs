using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Iconic.Models.User.Entities
{
    public class User : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }

        [Required]
        public string UserName { get; set; }
        public string Twitter { get; set; }
    }
}