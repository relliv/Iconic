using GalaSoft.MvvmLight;
using Iconic.Data;
using Iconic.Helpers;
using Iconic.Models.Color.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Iconic.ViewModel.Color
{
    public class PalettesViewModel : ViewModelBase
    {
        public PalettesViewModel()
        {
            GoToPageCommand = new RelayParameterizedCommand(GoToPage);
            PaginationChangedCommand = new RelayCommand(p =>
            {
                CurrentPage = 1;
                LoadColorPalettes();
            });
            SearchChangedCommand = new RelayParameterizedCommand(SearchChanged);

            CopyPaletteColorCommand = new RelayParameterizedCommand(CopyPaletteColor);

            ColorPalettes = new ObservableCollection<ColorPalette>();

            LoadColorPalettes();
        }

        #region Commands

        public ICommand CopyPaletteColorCommand { get; set; }

        public ICommand GoToPageCommand { get; set; }
        public ICommand PaginationChangedCommand { get; set; }
        public ICommand SearchChangedCommand { get; set; }

        #endregion


        #region Public Properties

        public ObservableCollection<ColorPalette> ColorPalettes { get; set; }
        public string ImportStatus { get; set; }

        #endregion


        #region Pagination

        public Pagination Pagination { get; set; }
        public int PageLimit { get; set; } = 100;
        public int CurrentPage { get; set; } = 1;
        public string SearchTerm { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Load color palettes from database
        /// </summary>
        public void LoadColorPalettes()
        {
            using var db = new AppDbContext();

            var paletteCount = db.ColorPalettes
            .Where(x => EF.Functions.Like(x.Color1, $"%{SearchTerm}%"))
            .Count();

            if (paletteCount > 0)
            {
                var totalSize = paletteCount;
                totalSize = totalSize > 0 ? totalSize : 1;

                Pagination = new Pagination(totalSize, CurrentPage, PageLimit, 10);

                ColorPalettes = db.ColorPalettes
                .Where(x => EF.Functions.Like(x.Color1, $"%{SearchTerm}%"))
                .OrderBy(x => x.PaletteNumber)
                .Skip((CurrentPage - 1) * PageLimit)
                .Take(PageLimit)
                .ToObservableCollection();
            }
            else if (ImportPalettes())
            {
                LoadColorPalettes();
            }
        }

        /// <summary>
        /// Import color palettes
        /// </summary>
        /// <returns></returns>
        public bool ImportPalettes()
        {
            var status = false;

            ImportStatus = "Import Starting...";

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                using var db = new AppDbContext();

                var source = db.AppSettings.First(x => x.SettingName == "ColorPaletteSource");

                ImportStatus = "Retrieving Palettes from API...";

                var palettesData = HttpHelpers.Get($"{source.Value}");
                var jsonObjPalettes = JsonConvert.DeserializeObject<List<string[]>>(palettesData);

                ImportStatus = $"Found {jsonObjPalettes.Count} Palettes...";

                var colorPalettes = new List<ColorPalette>();

                for (int i = 0; i < jsonObjPalettes.Count; i++)
                {
                    if (!db.ColorPalettes.Any(x => x.PaletteNumber == i + 1))
                    {
                        var newPalette = new ColorPalette
                        {
                            PaletteNumber = i + 1,
                            Color1 = jsonObjPalettes[i][0],
                            Color2 = jsonObjPalettes[i][1],
                            Color3 = jsonObjPalettes[i][2],
                            Color4 = jsonObjPalettes[i][3],
                            Color5 = jsonObjPalettes[i][4]
                        };

                        colorPalettes.Add(newPalette);

                        Thread.Sleep(200);

                        ImportStatus = $"Importing Palette {i + 1}";
                    }
                }

                if (colorPalettes.Count > 0)
                {
                    db.ColorPalettes.AddRange(colorPalettes);
                    db.SaveChangesAsync();

                    status = true;

                    ImportStatus = $"Imported {colorPalettes.Count} Palettes";

                    Thread.Sleep(2000);

                    ImportStatus = $"";
                }
            }).Start();

            return status;
        }

        /// <summary>
        /// Search term changed
        /// </summary>
        /// <param name="sender"></param>
        public void SearchChanged(object sender)
        {
            SearchTerm = (sender as TextBox).Text;

            if (SearchTerm.Length < 3) return;

            CurrentPage = 1;
            LoadColorPalettes();
        }

        /// <summary>
        /// Go to seleceted page
        /// </summary>
        /// <param name="sender"></param>
        public void GoToPage(object sender)
        {
            var page = (Models.Common.Page)(sender as Button).DataContext;

            CurrentPage = page.PageNumber;
            LoadColorPalettes();
        }

        /// <summary>
        /// Copy selected color
        /// </summary>
        /// <param name="sender"></param>
        public void CopyPaletteColor(object sender)
        {
            var color = (string)sender;

            Clipboard.SetText(color);
        }

        #endregion
    }
}