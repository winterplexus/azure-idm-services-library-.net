//
//  StaticClientWithProxyFactory.cs
//
//  Copyright (c) Wiregrass Code Technology 2020
//
using System;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;

namespace IdentityManagement.Services
{
    internal class StaticClientWithProxyFactory : IMsalHttpClientFactory
    {
        private static HttpClient httpClient;

        internal StaticClientWithProxyFactory(IConfigurationSection configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var webProxy = new WebProxy(new Uri(configuration["ProxyUri"]), BypassOnLocal: true)
            {
                Credentials = !string.IsNullOrEmpty(configuration["ProxyUserName"]) ? CreateCredential(configuration) : CredentialCache.DefaultCredentials
            };

            var httpClientHandler = new HttpClientHandler
            {
                Proxy = webProxy,
                UseProxy = true,
            };

            httpClient = new HttpClient(httpClientHandler);
        }

        public HttpClient GetHttpClient()
        {
            return httpClient;
        }

        private static NetworkCredential CreateCredential(IConfiguration configuration)
        {
            return new NetworkCredential(configuration["ProxyUserName"], configuration["ProxyUserPassword"]);
        }
    }
}