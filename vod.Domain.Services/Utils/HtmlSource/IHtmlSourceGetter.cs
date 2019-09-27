using System;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace vod.Domain.Services.Utils
{
    public interface IHtmlSourceGetter
    {
        Task<HtmlDocument> GetHtmlFrom(string url);
    }
}