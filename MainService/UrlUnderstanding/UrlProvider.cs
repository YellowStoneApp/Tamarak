using System;
using Microsoft.Extensions.Logging;

namespace MainService.UrlUnderstanding
{
    /// <summary>
    /// Currently this class just wraps Uri but this is intended to do more.
    /// Eventually we're going to want to resolve urls to base components that represent a unique product at a
    /// particular vendor. This way we can understand which core products customers want
    ///
    /// This is also intended to provide affiliate links for a particular product.
    ///
    /// How I imaging this working in the futures is we load in parsing configs from a db table at startup that we use
    /// to parse each url / generate affiliate links. 
    /// 
    ///
    /// There's going to be tonnes to come here. This is just the entry point for now.
    ///
    /// This probably should be dependency injected at startup and passed into the Controllers
    /// </summary>
    public class UrlProvider : IUrlProvider
    {
        private readonly ILogger<UrlProvider> _logger;

        public UrlProvider(ILogger<UrlProvider> logger)
        {
            _logger = logger;
        }
            
        
        /// <summary>
        /// Main entry point into getting url information
        /// </summary>
        /// <param name="urlString"></param>
        /// <returns></returns>
        public UrlResult Extract(string urlString)
        {
            var url = new Uri(urlString);

            return new UrlResult()
            {
                Url = urlString,
                Vendor = ExtractVendor(url.Authority),
            };
        }

        /// <summary>
        /// This should just parse off the www. and .com from the authority.
        ///
        /// might have other things to do here.
        /// </summary>
        /// <param name="authority"></param>
        /// <returns></returns>
        private string ExtractVendor(string authority)
        {
            string[] parts = authority.Split(".");

            if (parts.Length == 0)
            {
                _logger.LogError($"Unable to parse url given authority: {authority}");
                return "";
            }

            if (parts[0] == "www" && parts.Length >= 2)
            {
                return parts[1];
            }
            
            return parts[0];
        }
        
    }
}