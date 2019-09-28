using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace vod.Domain.Services.Utils.HtmlSource
{
    public class HtmlSourceGetter : IHtmlSourceGetter
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HtmlSourceGetter(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HtmlDocument> GetHtmlFrom(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");

            var response = await client.SendAsync(request);
            var htmlString = await response.Content.ReadAsStringAsync();
            var html = new HtmlDocument();
            html.LoadHtml(htmlString);
            return html;
        }
    }
}
