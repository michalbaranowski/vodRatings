using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using RichardSzalay.MockHttp;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Tests.Resources;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using static vod.Domain.Services.Utils.NcPlusUrls;

namespace vod.Domain.Services.Tests
{
    public class HtmlSourceGetterUT
    {
        private HtmlSourceGetter _htmlSourceGetter;

        private void BaseArrange()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(VodHboBaseUrl.GetUrlWithType(MovieTypes.Comedy)).Respond("application/html", HtmlResources.HboComediesResultHtml());
            var fakeClient = new HttpClient(mockHttp);

            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(fakeClient);
            _htmlSourceGetter = new HtmlSourceGetter(httpClientFactoryMock.Object);
        }

        [Test]
        public void GetHtmlFrom_ShouldReturnHtmlDocument()
        {
            BaseArrange();
            var result = _htmlSourceGetter.GetHtmlFrom(VodHboBaseUrl.GetUrlWithType(MovieTypes.Comedy));

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.DocumentNode);
            Assert.IsInstanceOf<HtmlDocument>(result);
        }
    }
}
