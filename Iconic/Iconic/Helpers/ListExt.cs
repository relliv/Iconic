using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Iconic.Helpers
{
    public static class ListExt
    {
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        static extern int StrCmpLogicalW(string lhs, string rhs);

        public static void SortNatural<T>(this List<T> self, Func<T, string> stringSelector)
        {
            self.Sort((lhs, rhs) => StrCmpLogicalW(stringSelector(lhs), stringSelector(rhs)));
        }

        public static void SortNatural(this List<string> self)
        {
            self.Sort(StrCmpLogicalW);
        }
    }
}
