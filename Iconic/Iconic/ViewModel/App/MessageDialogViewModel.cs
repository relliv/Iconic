using Iconic.Models.Common;
using System.Windows;

namespace Iconic.ViewModel.App
{
    public class MessageDialogViewModel : WindowViewModel
    {
        public MessageDialogViewModel(Window window, string title, string content) : base(window)
        {
            Title = title;
            Content = content;
        }

        public MessageDialogType MessageDialogType { get; set; }

        public string Content { get; set; }
    }
}