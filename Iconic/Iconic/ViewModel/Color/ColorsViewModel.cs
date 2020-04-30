using GalaSoft.MvvmLight;
using Iconic.Models.Color;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using static Iconic.Helpers.ListExt;
using Iconic.Helpers;
using KOR.ColorConversions;
using System.Windows.Input;
using System.Windows.Controls;

namespace Iconic.ViewModel.Color
{
    public class ColorsViewModel : ViewModelBase
    {
        public ColorsViewModel()
        {
            MaterialColorPalettes = new List<MaterialColorPalette>();

            CopyColorCodeCommand = new RelayParameterizedCommand(CopyColorCode);
            CopyAsCodeCommand = new RelayParameterizedCommand(CopyAsCode);

            LoadMaterialColors();
        }

        #region Commands

        public ICommand CopyColorCodeCommand { get; set; }
        public ICommand CopyAsCodeCommand { get; set; }

        #endregion


        #region Public Properties

        public List<MaterialColorPalette> MaterialColorPalettes { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Loads and organize material colors from resource dictionary
        /// </summary>
        public void LoadMaterialColors()
        {
            // get material colors from resource dictionary
            var materialColorsResourceDictionary = new ResourceDictionary
            {
                Source = new Uri("/Iconic;component/UI/ColorAndIcons/MaterialColors.xaml", UriKind.RelativeOrAbsolute)
            };

            var materialColors = new List<MaterialColor>();

            // loop in all resources
            foreach (var materialColor in materialColorsResourceDictionary)
            {
                var dictionaryEntry = (DictionaryEntry)materialColor;

                // if is it brush
                if (dictionaryEntry.Key.ToString().IndexOf("Brush") > -1)
                {
                    // convert to colors
                    var colors = Conversions.ConvertColors(dictionaryEntry.Value.ToString());

                    // set some values
                    var mColor = new MaterialColor
                    {
                        ColorName = dictionaryEntry.Key.ToString().Replace("Brush", null),
                        ColorHexCode = dictionaryEntry.Value.ToString(),
                        InvertColor = colors.InverseColorHex_2,
                        Colors = colors
                    };

                    materialColors.Add(mColor);
                }
            }

            // loop in materialColors
            foreach (var mColor in materialColors)
            {
                // extract pure color name
                var colorName = Regex.Match(mColor.ColorName, @"(([A-Z]{1}[a-z]+){1,2})").Value;

                // add or insert MaterialColors to MaterialColorPalettes
                if (MaterialColorPalettes.Any(x => x.PaletteName == colorName))
                {
                    var palette = MaterialColorPalettes.First(x => x.PaletteName == colorName);

                    if (!palette.MaterialColors.Any(x => x == mColor))
                    {
                        palette.MaterialColors.Add(mColor);
                    }
                }
                else
                {
                    MaterialColorPalettes.Add(new MaterialColorPalette
                    {
                        PaletteName = colorName,
                        MaterialColors = new List<MaterialColor>
                            {
                                new MaterialColor
                                {
                                    ColorName = mColor.ColorName,
                                    ColorHexCode = mColor.ColorHexCode
                                }
                            }
                    });
                }
            }

            // sort as natural MaterialColors 
            foreach (var mPalette in MaterialColorPalettes)
            {
                mPalette.MaterialColors.SortNatural(x => x.ColorName);
            }

            // sort as MaterialColors.Count then PaletteName  MaterialColorPalettes
            MaterialColorPalettes = MaterialColorPalettes
                .OrderByDescending(x => x.MaterialColors.Count)
                .ThenBy(x => x.PaletteName)
                .ToList();

            materialColors = materialColors.OrderBy(x => Regex.Match(x.ColorName, @"(([A-Z]{1}[a-z]+){1,2})").Value).ToList();
        }

        /// <summary>
        /// Copy selected color code
        /// </summary>
        /// <param name="parameter"></param>
        public void CopyColorCode(object parameter)
        {
            var values = (object[])parameter;

            var materialColor = (MaterialColor)((Border)values[0]).DataContext;
            var colorType = (string)values[1];

            var color = materialColor.ColorHexCode;

            if (colorType == "Hex")
            {
                color = materialColor.ColorHexCode;
            }
            else if (colorType == "Rgb")
            {
                color = materialColor.Colors.Rgb;
            }
            else if (colorType == "Argb")
            {
                color = materialColor.Colors.Argb;
            }
            else if (colorType == "Invers1")
            {
                color = materialColor.Colors.InverseColorHex_1;
            }
            else if (colorType == "Invers2")
            {
                color = materialColor.Colors.InverseColorHex_2;
            }

            Clipboard.SetText(color);
        }

        /// <summary>
        /// Copy as selected code
        /// </summary>
        /// <param name="parameter"></param>
        public void CopyAsCode(object parameter)
        {
            var values = (object[])parameter;

            var materialColor = (MaterialColor)((Border)values[0]).DataContext;
            var colorType = (string)values[1];

            var code = "";

            if (colorType == "CssBgColor")
            {
                code = $"background-color: {materialColor.ColorHexCode};";
            }
            else if (colorType == "XamlColor")
            {
                code = $"<Color x:Key=\"{materialColor.ColorName}\">{materialColor.ColorHexCode}</Color>";
            }
            else if (colorType == "XamlBrushColor")
            {
                code = $"<Color x:Key=\"{materialColor.ColorName}\">{materialColor.ColorHexCode}</Color>\r\n" +
                       $"<SolidColorBrush x:Key=\"{materialColor.ColorName}Brush\" Color=\"{{StaticResource {materialColor.ColorName}}}\" />";
            }

            Clipboard.SetText(code);
        }

        #endregion
    }
}