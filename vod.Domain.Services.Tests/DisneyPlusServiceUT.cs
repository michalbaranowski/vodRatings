using NUnit.Framework;
using System.Linq;
using vod.Domain.Services.Tests.Resources;
using vod.Domain.Services.Utils.HtmlSource.Serialize;

namespace vod.Domain.Services.Tests
{
    public class DisneyPlusSerializerUT
    {
        [Test]
        public void SerializeUrls_HappyPath()
        {
            //arrange
            var html = HtmlResources.DisneyPlusSitemapExampleHtml();
            var serializer = new DisneyPlusSerializer();

            //act
            var results = serializer.SerializeUrls(sitemapHtml: html);

            //assert
            Assert.That(results.Any(n => n == "https://www.disney.pl/filmy/avengers-koniec-gry"));
        }

        [Test]
        public void SerializeMovie_HappyPath()
        {
            //arrange
            var html = HtmlResources.DisneyPlusMovieExampleHtml();
            var serializer = new DisneyPlusSerializer();

            //act
            var result = serializer.Serialize(html);

            //assert
            Assert.That(result.Title == "Mulan");
            Assert.That(result.MovieType == Boundary.Interfaces.Enums.MovieTypes.Action);
        }
    }
}
