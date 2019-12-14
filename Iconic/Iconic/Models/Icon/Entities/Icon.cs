using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Iconic.Models.Icon
{
    public class Icon : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }

        [Required]
        public string GUId { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Data { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}