using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace AnoviSoftSiteFun.Util
{
    public class Album
    {
        public static string Photos(string path)
        {
            var lst = GetListOfFiles(path).Select((x, i) => ColImg(x, i, path));
            var sb = new StringBuilder();
            while (lst.Any())
            {
                sb.Append(RowStart);
                var tmp = lst.Take(4);
                tmp.ToList().ForEach(x => sb.Append(x));
                lst = lst.Skip(4).ToList();
                sb.Append(RowEnd);
            }
            return sb.ToString();
        }
        
        public static List<string> GetListOfFiles(string path)
        {
            var localPath = HttpContext.Current.Server.MapPath("~/Content/Album/mini");
            path = Path.Combine(localPath, path);
            return Directory.GetFiles(path)
                .Select(x => x.Substring(x.IndexOf("\\Content\\Album\\mini", StringComparison.Ordinal)))
                .Select(x => x.Replace("\\", "/"))
                .OrderBy(x => x)
                .ToList();
        }

        public static List<string> GetListOfFilesBig(string path) =>
            GetListOfFiles(path).Select(x => $"'{x}'").Select(x =>
            {
                var regex = new Regex(Regex.Escape("mini/"));
                return regex.Replace(x, "", 1);
            }).ToList();

        public static string GetListOfFilseBigJS(string path) =>
            $"[{string.Join(", ", GetListOfFilesBig(path))}]";

        private static string RowStart => "<div class=\"row\">";
        private static string RowEnd => "</div>";
        private static string ColImg(string value, int i, string name) => Col(Img(value, i, name));
        private static string Col(string value) => $"<div class=\"col-1-4\">{value}</div>";

        private static string Img(string value, int i, string name)
            => $"<img class=\"aphoto\" alt=\"{value.Replace('\\', '/').Split('/').Last()}\" src=\"{value}\" onclick=\"ImageClick({i}, '{name}');\"/>";
    }
}
