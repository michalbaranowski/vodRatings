using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Utils.HtmlSource.Deserialize
{
    public class HtmlSourceDeserializer : IHtmlSourceDeserializer
    {
        public IEnumerable<Movie> Deserialize(HtmlDocument html)
        {
            return html.DocumentNode.Descendants("div")
                .Where(x => x.Attributes.Contains("title"))
                .Select(n => 
                    new Movie()
                    {
                        Title = n.Attributes?
                            .FirstOrDefault(p => p.Name == "title")?
                            .Value
                            .Replace("(HD)", string.Empty)
                            .Replace("(SD)", "")
                    });
        }
    }
}
