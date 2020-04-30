using GalaSoft.MvvmLight;
using Iconic.Data;
using Iconic.Dialogs.Icon;
using Iconic.Helpers;
using Iconic.Models.User.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            GoToPageCommand = new RelayParameterizedCommand(GoToPage);
            SearchChangedCommand = new RelayParameterizedCommand(SearchChanged);
            PaginationChangedCommand = new RelayCommand(p => LoadIcons());

            OpenIconEditorCommand = new RelayParameterizedCommand(OpenIconEditor);
            CopyIconDataCommand = new RelayParameterizedCommand(CopyIconData);

            Icons = new ObservableCollection<Models.Icon.Entities.Icon>();

            LoadIcons();

            if (ImportIcons())
            {
                LoadIcons();
            }
        }

        #region Commands

        public ICommand CopyIconDataCommand { get; set; }
        public ICommand OpenIconEditorCommand { get; set; }

        public ICommand PaginationChangedCommand { get; set; }
        public ICommand SearchChangedCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }

        #endregion


        #region Properties

        public ObservableCollection<Models.Icon.Entities.Icon> Icons { get; set; }
        public string ImportStatus { get; set; }
        #endregion


        #region Pagination

        public int PageLimit { get; set; } = 100;
        public int CurrentPage { get; set; } = 1;
        public string SearchTerm { get; set; }
        public Pagination Pagination { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Load material icons
        /// </summary>
        public void LoadIcons()
        {
            using var db = new AppDbContext();

            var totalSize = db.Icons.Where(x => EF.Functions.Like(x.Description, $"%{SearchTerm}%")).Count();
            totalSize = totalSize > 0 ? totalSize : 1;

            Pagination = new Pagination(totalSize, CurrentPage, PageLimit, 10);

            Icons = db.Icons.Where(x => EF.Functions.Like(x.Description, $"%{SearchTerm}%"))
            .OrderBy(x => x.Name)
            .Skip((CurrentPage - 1) * PageLimit)
            .Take(PageLimit)
            .ToObservableCollection();
        }

        /// <summary>
        /// Go to page
        /// </summary>
        /// <param name="sender"></param>
        public void GoToPage(object sender)
        {
            var page = (Models.Common.Page)(sender as Button).DataContext;

            CurrentPage = page.PageNumber;
            LoadIcons();
        }

        /// <summary>
        /// Search term changed
        /// </summary>
        /// <param name="sender"></param>
        public void SearchChanged(object sender)
        {
            SearchTerm = (sender as TextBox).Text;
            CurrentPage = 1;
            LoadIcons();
        }

        /// <summary>
        /// Copy icon with selected data format
        /// </summary>
        /// <param name="parameter"></param>
        public void CopyIconData(object parameter)
        {
            var values = (object[])parameter;

            var materialIcon = (Models.Icon.Entities.Icon)((Grid)values[0]).DataContext;
            var iconType = (string)values[1];

            var icon = materialIcon.Data;

            if (iconType == "Data")
            {
                Clipboard.SetText(materialIcon.Data);
            }
            else if (iconType == "SVG")
            {
                var svg = $"<svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" version=\"1.1\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"{materialIcon.Data}\" /></svg>";

                Clipboard.SetText(svg);
            }
            else if (iconType == "SVGHTML")
            {
                var svg = $"<?xml version=\"1.0\" encoding=\"UTF - 8\"?><!DOCTYPE svg PUBLIC \" -//W3C//DTD SVG 1.1//EN\" \"http://www.w3.org/Graphics/SVG/1.1/DTD/svg11.dtd\"><svg xmlns=\"http://www.w3.org/2000/svg\" xmlns:xlink=\"http://www.w3.org/1999/xlink\" version=\"1.1\" width=\"24\" height=\"24\" viewBox=\"0 0 24 24\"><path d=\"{materialIcon.Data}\" /></svg>";

                Clipboard.SetText(svg);
            }
            else if (iconType == "XML")
            {
                var xml = $"<!-- drawable/shape.xml -->\r\n<vector xmlns:android=\"http://schemas.android.com/apk/res/android\"\r\n    android:height=\"24dp\"\r\n    android:width=\"24dp\"\r\n    android:viewportWidth=\"24\"\r\n    android:viewportHeight=\"24\">\r\n    <path android:fillColor=\"#000\" android:pathData=\"{materialIcon.Data}\" />\r\n</vector>";

                Clipboard.SetText(xml);
            }
            else if (iconType == "XAMLCanvas")
            {
                var xaml = $"<Canvas xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\" xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\" Width=\"24\" Height=\"24\"><Path Data=\"{materialIcon.Data}\" /></Canvas>";

                Clipboard.SetText(xaml);
            }
            else if (iconType == "XAMLGeometry")
            {
                var key = Thread.CurrentThread.CurrentCulture.TextInfo
                    .ToTitleCase(materialIcon.Name
                    .Replace("-", " "))
                    .Replace(" ", "")
                    .Replace("İ", "I");

                var xaml = $"<PathGeometry x:Key=\"{key}\" Figures=\"{materialIcon.Data}\" />";

                Clipboard.SetText(xaml);
            }
            else if (iconType == "XAMLPath")
            {
                var key = Thread.CurrentThread.CurrentCulture.TextInfo
                    .ToTitleCase(materialIcon.Name
                    .Replace("-", " "))
                    .Replace(" ", "")
                    .Replace("İ", "I");

                var xaml = $"<Path x:Key=\"{key}\" Data=\"{materialIcon.Data}\" Fill=\"Black\" />";

                Clipboard.SetText(xaml);
            }
        }

        /// <summary>
        /// Open with icon editor
        /// </summary>
        /// <param name="sender"></param>
        public void OpenIconEditor(object sender)
        {
            var icon = ((Grid)sender).DataContext as Models.Icon.Entities.Icon;

            var dialog = new IconEditorDialog();

            dialog.ShowDialogWindow(new IconEditorViewModel(dialog, icon));
        }

        /// <summary>
        /// Icon importer
        /// source: https://materialdesignicons.com/
        /// </summary>
        public bool ImportIcons()
        {
            var status = false;

            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                using var db = new AppDbContext();

                var lastUpdate = db.AppSettings.First(x => x.SettingName == "IconsLastUpdate");
                DateTime.TryParse(lastUpdate.Value, out DateTime date);

                if (date.AddDays(7) > DateTime.Now.Date)
                {
                    ImportStatus = null;
                    return;
                }

                ImportStatus = "Import Starting...";
                Thread.Sleep(2000);

                var source = db.AppSettings.First(x => x.SettingName == "IconsSource");
                var iconsGuId = db.AppSettings.First(x => x.SettingName == "IconsGUID");

                ImportStatus = "Retrieving Icons from API...";

                // get icons list
                var iconsData = HttpHelpers.Get($"{source.Value}/package/{iconsGuId.Value}");

                // parse icons list
                var jsonObjIcons = JsonConvert.DeserializeObject<Models.Common.Icons.Icons>(iconsData);

                if (db.Icons.Count() < jsonObjIcons.icons.Count)
                {
                    var icons = new List<Models.Icon.Entities.Icon>();

                    ImportStatus = $"Found {jsonObjIcons.icons.Count} Icons...";
                    Thread.Sleep(2000);

                    // loop in icons list
                    foreach (var icon in jsonObjIcons.icons.Select((value, i) => new { i, value }))
                    {
                        if (!db.Icons.Any(x => x.GUId == icon.value.id))
                        {
                            // get current icon from api
                            var iconData = HttpHelpers.Get($"{source.Value}/icon/{icon.value.id}");
                            var jsonObjIcon = JsonConvert.DeserializeObject<Models.Common.Icon.Icon>(iconData);

                            // check user
                            var user = db.Users.FirstOrDefault
                                (x => x.UserName == jsonObjIcon.user.name && x.Twitter == jsonObjIcon.user.twitter) 
                            ?? new User();

                            // save new user
                            if (user == null)
                            {
                                user = new User
                                {
                                    UserName = jsonObjIcon.user.name,
                                    Twitter = jsonObjIcon.user.twitter
                                };

                                db.Users.Add(user);
                                db.SaveChanges();
                            }

                            // create new icon
                            DateTime.TryParse(jsonObjIcon.date, out DateTime addedDate);

                            // add new icon
                            db.Icons.Add(new Models.Icon.Entities.Icon
                            {
                                GUId = jsonObjIcon.id,
                                UserId = user.Id,

                                Name = jsonObjIcon.name,
                                Description = jsonObjIcon.description,

                                Data = jsonObjIcon.data,
                                Date = addedDate
                            });
                            db.SaveChanges();

                            ImportStatus = $"Getting Icons... Progress: {icon.i + 1}/{jsonObjIcons.icons.Count}";
                        }
                    }

                    status = true;
                    ImportStatus = $"";

                    lastUpdate.Value = DateTime.Now.Date.ToString();
                    db.AppSettings.Update(lastUpdate);
                    db.SaveChanges();

                }
            }).Start();

            return status;
        }

        #endregion
    }
}