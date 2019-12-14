using GalaSoft.MvvmLight;
using Iconic.Data;
using Iconic.Dialogs.Icon;
using Iconic.Helpers;
using Iconic.Models.Icon;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Iconic.ViewModel.Icon
{
    public class IconsViewModel : ViewModelBase
    {
        public IconsViewModel()
        {
            GoToListPageCommand = new RelayParameterizedCommand(GoToListPage);
            SearchChangedCommand = new RelayParameterizedCommand(SearchChanged);
            PaginationChangedCommand = new RelayCommand(p => LoadIconItems());
            LoadIconItems();
            LoadColors();
        }

        #region Commands

        public ICommand PaginationChangedCommand { get; set; }
        public ICommand SearchChangedCommand { get; set; }
        public ICommand GoToListPageCommand { get; set; }

        #endregion

        #region Properties

        public ObservableCollection<IconItem> IconItems { get; set; } = new ObservableCollection<IconItem>();
        public ObservableCollection<ColorItem> ColorItems { get; set; } = new ObservableCollection<ColorItem>();
        public int PageLimit { get; set; } = 100;
        public int CurrentPage { get; set; } = 1;
        public string SearchTerm { get; set; }
        public Pagination Pagination { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Load material icons
        /// </summary>
        public void LoadIconItems()
        {
            using var db = new AppDbContext();

            var totalSize = db.Icons.Where(x => EF.Functions.Like(x.Description, $"%{SearchTerm}%")).Count();
            totalSize = totalSize > 0 ? totalSize : 1;

            Pagination = new Pagination(totalSize, CurrentPage, PageLimit, 10);

            IconItems = db.Icons.Where(x => EF.Functions.Like(x.Description, $"%{SearchTerm}%"))
            .Select(icon => new IconItem
            {
                Icon = icon,
                CopyDataCommand = new RelayParameterizedCommand(CopyData),
                CopySVGCommand = new RelayParameterizedCommand(CopySVG),
                CopySVGforHTMLCommand = new RelayParameterizedCommand(CopySVGforHTML),
                CopyXMLCommand = new RelayParameterizedCommand(CopyXML),
                CopyXAMLCanvasCommand = new RelayParameterizedCommand(CopyXAMLCanvas),
                CopyXAMLGeometryCommand = new RelayParameterizedCommand(CopyXAMLGeometry),
                CopyXAMLPathCommand = new RelayParameterizedCommand(CopyXAMLPath),

                OpenIconEditorCommand = new RelayParameterizedCommand(OpenIconEditor)
            })
            .OrderBy(x => x.Icon.Name)
            .Skip((CurrentPage - 1) * PageLimit)
            .Take(PageLimit)
            .ToObservableCollection();
        }

        /// <summary>
        /// Load material colors
        /// </summary>
        public void LoadColors()
        {
            var materialColorsResourceDictionary = new ResourceDictionary
            {
                Source = new Uri("/Iconic;component/UI/ColorAndIcons/MaterialColors.xaml", UriKind.RelativeOrAbsolute)
            };

            foreach (var materialColor in materialColorsResourceDictionary)
            {
                var dictionaryEntry = (DictionaryEntry)materialColor;

                if (dictionaryEntry.Key.ToString().IndexOf("Brush") > -1)
                {
                    ColorItems.Add(new ColorItem
                    {
                        ColorName = dictionaryEntry.Key.ToString(),
                        ColorHex = dictionaryEntry.Value.ToString(),

                        CopyHexCommand = new RelayParameterizedCommand(CopyHex)
                    });
                }
            }

            ColorItems = ColorItems.OrderBy(x => x.ColorName).ToObservableCollection();
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Go to page
        /// </summary>
        /// <param name="sender"></param>
        public void GoToListPage(object sender)
        {
            CurrentPage = (int)(sender as Button).DataContext;
            LoadIconItems();
        }

        /// <summary>
        /// Search term changed
        /// </summary>
        /// <param name="sender"></param>
        public void SearchChanged(object sender)
        {
            SearchTerm = (sender as TextBox).Text;
            CurrentPage = 1;
            LoadIconItems();
        }

        #region Icon Copy

        /// <summary>
        /// Copy as plain data text
        /// </summary>
        /// <param name="sender"></param>
        public void CopyData(object sender)
        {
            var iconItem = ((Grid)sender).DataContext as IconItem;

            Clipboard.SetText(iconItem.Icon.Data);
        }

        /// <summary>
        /// Copy as SVG
        /// </summary>
        /// <param name="sender"></param>
        public void CopySVG(object sender)
        {
            var iconItem = ((Grid)sender).DataContext as IconItem;

            var svg = $"<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" version=\"1.1\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"{iconItem.Icon.Data}\" /></svg>";

            Clipboard.SetText(svg);
        }

        /// <summary>
        /// Copy as SVG for HTML
        /// </summary>
        /// <param name="sender"></param>
        public void CopySVGforHTML(object sender)
        {
            var iconItem = ((Grid)sender).DataContext as IconItem;

            var svg = $"<?xml version=\"1.0\" encoding=\"UTF - 8\"?><!DOCTYPE svg PUBLIC \" -//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"><svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" version=\"1.1\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"{iconItem.Icon.Data}\" /></svg>";

            Clipboard.SetText(svg);
        }

        /// <summary>
        /// Copy as XML
        /// </summary>
        /// <param name="sender"></param>
        public void CopyXML(object sender)
        {
            var iconItem = ((Grid)sender).DataContext as IconItem;

            var xml = $"<!-- drawable/shape.xml -->\r\n<vector xmlns:android=\"http://schemas.android.com/apk/res/android\"\r\n    android:height=\"24dp\"\r\n    android:width=\"24dp\"\r\n    android:viewportWidth=\"24\"\r\n    android:viewportHeight=\"24\">\r\n    <path android:fillColor=\"#000\" android:pathData=\"{iconItem.Icon.Data}\" />\r\n</vector>";

            Clipboard.SetText(xml);
        }

        /// <summary>
        /// Copy as XAML-Canvas
        /// </summary>
        /// <param name="sender"></param>
        public void CopyXAMLCanvas(object sender)
        {
            var iconItem = ((Grid)sender).DataContext as IconItem;

            var xaml = $"<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Width=\"24\" Height=\"24\"><Path Data=\"{iconItem.Icon.Data}\" /></Canvas>";

            Clipboard.SetText(xaml);
        }

        /// <summary>
        /// Copy as XAML-Geometry
        /// </summary>
        /// <param name="sender"></param>
        public void CopyXAMLGeometry(object sender)
        {
            var iconItem = ((Grid)sender).DataContext as IconItem;

            var key = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(iconItem.Icon.Name.Replace("-", " ")).Replace(" ", "").Replace("İ", "I");

            var xaml = $"<PathGeometry x:Key=\"{key}\" Figures=\"{iconItem.Icon.Data}\" />";

            Clipboard.SetText(xaml);
        }

        /// <summary>
        /// Copy as XAML-Path
        /// </summary>
        /// <param name="sender"></param>
        public void CopyXAMLPath(object sender)
        {
            var iconItem = ((Grid)sender).DataContext as IconItem;

            var key = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(iconItem.Icon.Name.Replace("-", " ")).Replace(" ", "").Replace("İ", "I");

            var xaml = $"<Path x:Key=\"{key}\" Data=\"{iconItem.Icon.Data}\" Fill=\"Black\" />";

            Clipboard.SetText(xaml);
        }

        #endregion

        #region Color Copy

        /// <summary>
        /// Copy as plain data text
        /// </summary>
        /// <param name="sender"></param>
        public void CopyHex(object sender)
        {
            var colorItem = ((Grid)sender).DataContext as ColorItem;

            Clipboard.SetText(colorItem.ColorHex);
        }

        #endregion

        /// <summary>
        /// Open with icon editor
        /// </summary>
        /// <param name="sender"></param>
        public void OpenIconEditor(object sender)
        {
            var iconItem = ((Grid)sender).DataContext as IconItem;

            var dialog = new IconEditorDialog();

            dialog.ShowDialogWindow(new IconEditorViewModel(dialog, iconItem.Icon));
        }

        #endregion
    }
}