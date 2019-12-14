using System.Globalization;

namespace Iconic.Data
{
    public class Settings
    {
        /// <summary>
        /// Default Culture Info
        /// </summary>
        public static readonly CultureInfo CultureInfo = new CultureInfo("tr-TR");

        public static readonly string ImageFilter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|All files (*.*)|*.*";
    }
}