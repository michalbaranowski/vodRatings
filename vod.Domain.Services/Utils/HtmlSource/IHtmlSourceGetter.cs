using System.Threading.Tasks;
using HtmlAgilityPack;

namespace vod.Domain.Services.Utils.HtmlSource
{
    public interface IHtmlSourceGetter
    {
        HtmlDocument GetHtmlFrom(string url);
        void SerializeFilmDetails(HtmlDocument moreInfoHtml, Boundary.Models.Movie movie);
    }
}