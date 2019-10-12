using System.Collections.Generic;
using System.Linq;

namespace vod.Domain.Services.Utils
{
    public class UrlGetter : IUrlGetter
    {
        public IEnumerable<string> GetBaseUrls()
        {
            var props = typeof(NcPlusUrls).GetFields().Where(n => n.Name.ToLower().Contains("base"));
            return props.Select(n => n.GetValue(null).ToString());
        }
    }

    public interface IUrlGetter
    {
        IEnumerable<string> GetBaseUrls();
    }
}
