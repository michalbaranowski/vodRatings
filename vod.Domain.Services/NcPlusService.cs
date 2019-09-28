using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Deserialize;

namespace vod.Domain.Services
{
    public class NcPlusService : INcPlusService
    {
        private readonly IHtmlSourceGetter _sourceGetter;
        private readonly IHtmlSourceDeserializer _deserializer;

        public NcPlusService(
            IHtmlSourceGetter sourceGetter,
            IHtmlSourceDeserializer deserializer)
        {
            _sourceGetter = sourceGetter;
            _deserializer = deserializer;
        }

        public async Task<IEnumerable<Movie>> GetMoviesOfType(MovieTypes type)
        {
            var cplusHtml = await _sourceGetter.GetHtmlFrom(NcPlusUrls.VodCplusBaseUrl);
            var cplusResult = _deserializer.DeserializeMovies(cplusHtml);

            var premieryHtml = await _sourceGetter.GetHtmlFrom(NcPlusUrls.VodPremieryBaseUrl);
            var premieryResult = _deserializer.DeserializeMovies(premieryHtml);

            var hboHtml = await _sourceGetter.GetHtmlFrom(NcPlusUrls.VodHboBaseUrl);
            var hboResult = _deserializer.DeserializeMovies(hboHtml);

            var result = cplusResult
                .Concat(hboResult)
                .Concat(premieryResult)
                .DistinctBy(n=>n.Title);

            return result;
        }
    }
}
