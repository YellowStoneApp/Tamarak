using System;
using MainService.UrlUnderstanding;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace TestMainService
{
    [TestFixture]
    public class UriParseTest
    {
        private Mock<ILogger<UrlProvider>> _loggerMoq;

        private UrlProvider _underTest;

        /// <summary>
        /// This will run before each test is run. This clears out the logger object.
        /// </summary>
        [SetUp]
        public void ParseSetup()
        {
            _loggerMoq = new Mock<ILogger<UrlProvider>>();

            _underTest = new UrlProvider(_loggerMoq.Object);
        }
        
        [Test]
        public void ParseUrlGetVendor()
        {
            var urlString =
                "https://www.nike.com/t/blazer-mid-77-vintage-mens-shoe-nw30B2/BQ6806-100?nikemt=true&cp=71606177307_search_%7CPRODUCT_GROUP%7CGOOGLE%7C71700000041489779%7CAll_X_X_X_X-Device_X_Nike-FullPrice_X%7C%7Cc&gclsrc=aw.ds&&gclid=CjwKCAjwzMeFBhBwEiwAzwS8zP1RI8i_S0IVJIPhi7fJGil-UxGuQoSx2sicJLLNAaD2qgt5pmlPrRoCEmIQAvD_BwE&gclsrc=aw.ds";
            
            var result = _underTest.Extract(urlString);

            Assert.AreEqual("nike", result.Vendor);
        }
        
        [Test]
        public void ParseUrlGetVendorNoWww()
        {
            var urlString =
                "https://nike.com/t/blazer-mid-77-vintage-mens-shoe-nw30B2/BQ6806-100?nikemt=true&cp=71606177307_search_%7CPRODUCT_GROUP%7CGOOGLE%7C71700000041489779%7CAll_X_X_X_X-Device_X_Nike-FullPrice_X%7C%7Cc&gclsrc=aw.ds&&gclid=CjwKCAjwzMeFBhBwEiwAzwS8zP1RI8i_S0IVJIPhi7fJGil-UxGuQoSx2sicJLLNAaD2qgt5pmlPrRoCEmIQAvD_BwE&gclsrc=aw.ds";
            
            var result = _underTest.Extract(urlString);

            Assert.AreEqual("nike", result.Vendor);
        }
    }
}