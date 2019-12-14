using Iconic.Data;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Iconic.Helpers
{
    public class IconImporter
    {
        public IconImporter()
        {
            ImportIcons();
        }

        /// <summary>
        /// Icon importer
        /// source: https://materialdesignicons.com/
        /// </summary>
        public void ImportIcons()
        {
            using var db = new AppDbContext();

            var lastUpdate = db.AppSettings.First(x => x.SettingName == "LastUpdate");
            var grabSource = db.AppSettings.First(x => x.SettingName == "GrabSource");
            var grabIconsGuId = db.AppSettings.First(x => x.SettingName == "GrabGUID");

            DateTime.TryParse(lastUpdate.Value, out DateTime date);

            if (date.AddDays(7) < DateTime.Now.Date)
            {
                var iconsData = HttpHelpers.Get(grabSource.Value + "package/"+ grabIconsGuId.Value);
                var jsonObjIcons = JsonConvert.DeserializeObject<Models.Common.Icons.Icons>(iconsData);

                foreach (var icon in jsonObjIcons.icons)
                {
                    var user = new Models.User.User();

                    if (!db.Icons.Any(x => x.GUId == icon.id))
                    {
                        var iconData = HttpHelpers.Get(grabSource.Value + "icon/" + icon.id);
                        var jsonObjIcon = JsonConvert.DeserializeObject<Models.Common.Icon.Icon>(iconData);

                        user = db.Users.FirstOrDefault
                        (x => x.UserName == jsonObjIcon.user.name && x.Twitter == jsonObjIcon.user.twitter);

                        if (user == null)
                        {
                            user = new Models.User.User
                            {
                                UserName = jsonObjIcon.user.name,
                                Twitter = jsonObjIcon.user.twitter
                            };

                            db.Users.Add(user);
                            db.SaveChanges();
                        }

                        DateTime.TryParse(jsonObjIcon.date, out DateTime addedDate);

                        var _icon = new Models.Icon.Icon
                        {
                            GUId = jsonObjIcon.id,
                            UserId = user.Id,

                            Name = jsonObjIcon.name,
                            Description = jsonObjIcon.description,

                            Data = jsonObjIcon.data,
                            Date = addedDate
                        };

                        db.Icons.Add(_icon);
                        db.SaveChanges();
                    }
                }

                lastUpdate.Value = DateTime.Now.Date.ToString();
                db.AppSettings.Update(lastUpdate);
                db.SaveChanges();
            }
        }
    }
}