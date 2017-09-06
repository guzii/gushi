using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Code
{
    public class StringHelper
    {
        public static bool IsNullOrEmptyNoHtml(string value)
        {
            string text = WebHelper.NoHtml(value + "");
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }
            else
                return false;
        }

    }
}
