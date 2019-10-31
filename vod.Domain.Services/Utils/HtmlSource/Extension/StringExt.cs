using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Domain.Services.Utils.HtmlSource.Extension
{
    public static class StringExt
    {
        public static string OnlyDigits(this string str)
        {
            var result = string.Empty;
            if (string.IsNullOrEmpty(str)) return result;

            foreach (char c in str)
            {
                if(c < '0' || c > '9')
                    continue;
                result += c;
            }

            return result;
        }
    }
}
