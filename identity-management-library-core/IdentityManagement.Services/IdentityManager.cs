//
//  IdentityManager.cs
//
//  Wiregrass Code Technology 2020-2021
//
using System;
using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;

namespace IdentityManagement.Services
{
    public class IdentityManager : IIdentityManager
    {
        private IConfigurationSection configuration;
        private GraphServiceClient client;

        public string Domain { get; set; }

        public IUserServices UserServices { get; set; }

        public IGroupServices GroupServices { get; set; }

        public IdentityManager()
        {
            GetConfiguration("appsettings.json");
            GetGraphClient();
            SetPublicProperties();
        }

        public IdentityManager(string settingsPath)
        {
            GetConfiguration(settingsPath);
            GetGraphClient();
            SetPublicProperties();
        }

        public IdentityManager(IConfigurationSection configurationParameter)
        {
            configuration = configurationParameter;
            GetGraphClient();
            SetPublicProperties();
        }

        private void GetConfiguration(string settingsPath)
        {
            var configurationRoot = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile(settingsPath)
                .Build();

            configuration = configurationRoot.GetSection("IdentityManagement");
        }

        private void GetGraphClient()
        {
            IConfidentialClientApplication confidentialClientApplication;

            var proxyUri = configuration["ProxyUri"];
            if (string.IsNullOrEmpty(proxyUri))
            {
                confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(configuration["ClientId"])
                    .WithTenantId(configuration["Domain"])
                    .WithClientSecret(configuration["ClientSecret"])
                    .Build();
            }
            else
            {
                IMsalHttpClientFactory httpClientFactory = new StaticClientWithProxyFactory(configuration);

                confidentialClientApplication = ConfidentialClientApplicationBuilder
                    .Create(configuration["ClientId"])
                    .WithTenantId(configuration["Domain"])
                    .WithClientSecret(configuration["ClientSecret"])
                    .WithHttpClientFactory(httpClientFactory)
                    .Build();
            }

            var authenticationProvider = new ClientCredentialProvider(confidentialClientApplication);

            client = new GraphServiceClient(authenticationProvider) { HttpProvider = { OverallTimeout = GetTimeout() } };
        }

        private void SetPublicProperties()
        {
            Domain = configuration["Domain"];
            UserServices = new UserServices(client, configuration);
            GroupServices = new GroupServices(client, UserServices);
        }

        private TimeSpan GetTimeout()
        {
            double seconds = 5;
            if (!string.IsNullOrEmpty(configuration["Timeout"]))
            {
                seconds = double.Parse(configuration["Timeout"], CultureInfo.InvariantCulture);
            }
            return TimeSpan.FromSeconds(seconds);
        }
    }
}