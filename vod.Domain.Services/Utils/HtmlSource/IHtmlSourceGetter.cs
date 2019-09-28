using System.Threading.Tasks;
using HtmlAgilityPack;

namespace vod.Domain.Services.Utils.HtmlSource
{
    public interface IHtmlSourceGetter
    {
        Task<HtmlDocument> GetHtmlFrom(string url);
    }
}