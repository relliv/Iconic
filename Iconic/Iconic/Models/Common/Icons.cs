using System.Collections.Generic;

namespace Iconic.Models.Common.Icons
{
    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Icon
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<object> aliases { get; set; }
        public string data { get; set; }
        public User user { get; set; }
        public int commentCount { get; set; }
    }

    public class Icons
    {
        public List<Icon> icons { get; set; }
    }
}