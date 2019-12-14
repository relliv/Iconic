using System.Collections.Generic;

namespace Iconic.Models.Common.Icon
{
    public class User
    {
        public string name { get; set; }
        public string twitter { get; set; }
    }

    public class Tag
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class Icon
    {
        public string id { get; set; }
        public User user { get; set; }
        public List<object> comments { get; set; }
        public List<Tag> tags { get; set; }
        public bool isAuthor { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string data { get; set; }
        public string date { get; set; }
    }
}