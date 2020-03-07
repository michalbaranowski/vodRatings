using System.Net.Http;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Tests.Resources;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;

namespace vod.Domain.Services.Tests
{
    public class HtmlSourceGetterUT
    {
        private HtmlSourceGetter _htmlSourceGetter;

        private void BaseArrange()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(NcPlusUrls.GetUrlWithType(NcPlusUrls.VodHboBaseUrl, MovieTypes.Comedy)).Respond("application/html", HtmlResources.HboComediesResultHtml());
            var fakeClient = new HttpClient(mockHttp);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(fakeClient);
            _htmlSourceGetter = new HtmlSourceGetter(httpClientFactoryMock.Object);
        }

        [Test]
        public void GetHtmlFrom_ShouldReturnHtmlDocument()
        {
            BaseArrange();
            var result = _htmlSourceGetter.GetHtmlFrom(NcPlusUrls.GetUrlWithType(NcPlusUrls.VodHboBaseUrl, MovieTypes.Comedy));

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DocumentNode);
            Assert.IsInstanceOf<HtmlDocument>(result);
        }
    }
}
