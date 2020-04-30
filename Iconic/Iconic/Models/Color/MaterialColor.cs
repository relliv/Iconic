using System.Windows.Media;

namespace Iconic.Models.Color
{
    public class MaterialColor
    {
        public string ColorName { get; set; }
        public string ColorHexCode { get; set; }
        public string InvertColor { get; set; }
        public KOR.ColorConversions.Colors Colors { get; set; }
    }
}