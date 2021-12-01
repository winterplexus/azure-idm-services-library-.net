//
//  IdentityManager.cs
//
//  Wiregrass Code Technology 2020-2022
//
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using Azure.Core.Pipeline;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace IdentityManagement.Services
{
    public class IdentityManager : IIdentityManager
    {
        private IConfigurationSection configuration;
        private GraphServiceClient client;

        public string Tenant { get; set; }

        public IUserManagement UserServices { get; set; }

        public IGroupManagement GroupServices { get; set; }

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
            var proxyAddress = configuration["ProxyAddress"];

            TokenCredentialOptions tokenCredentialOptions;

            if (!string.IsNullOrEmpty(proxyAddress))
            {
                var handler = new HttpClientHandler
                {
                    Proxy = new WebProxy(new Uri(proxyAddress))
                };

                tokenCredentialOptions = new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                    Transport = new HttpClientTransport(handler)
                };
            }
            else
            {
                tokenCredentialOptions = new TokenCredentialOptions
                {
                    AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
                };
            }

            var clientSecretCredential = new ClientSecretCredential(configuration["Tenant"],
                                                                    configuration["ClientId"],
                                                                    configuration["ClientSecret"],
                                                                    tokenCredentialOptions);

            var scopes = new[] { "https://graph.microsoft.com/.default" };

            client = new GraphServiceClient(clientSecretCredential, scopes) { HttpProvider = { OverallTimeout = GetTimeout() } };
        }

        private void SetPublicProperties()
        {
            Tenant = configuration["Tenant"];
            UserServices = new UserManagement(client, configuration);
            GroupServices = new GroupManagement(client, UserServices);
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