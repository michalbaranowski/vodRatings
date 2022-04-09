namespace vod.Domain.Services.Utils.HtmlSource.Extension
{
    public static class PolishSignsHelper
    {
        public static string FixPlLetters(this string str)
        {
            return str.Replace("&#260;", "Ą")
                .Replace("&#261;", "ą")
                .Replace("&#262;", "Ć")
                .Replace("&#263;", "ć")
                .Replace("&#280;", "Ę")
                .Replace("&#281;", "ę")
                .Replace("&#321;", "Ł")
                .Replace("&#322;", "ł")
                .Replace("&#323;", "Ń")
                .Replace("&#324;", "ń")
                .Replace("&Oacute;", "Ó")
                .Replace("&#211;", "Ó")
                .Replace("&oacute;", "ó")
                .Replace("&#243;", "ó")
                .Replace("&#198;", "AE")
                .Replace("&#346;", "Ś")
                .Replace("&#347;", "ś")
                .Replace("&#377;", "Ź")
                .Replace("&#378;", "ź")
                .Replace("&#379;", "Ż")
                .Replace("&#380;", "ż")
                .Replace("&quot;", "\"")
                .Replace("&#39;", "'");
        }
    }
}
