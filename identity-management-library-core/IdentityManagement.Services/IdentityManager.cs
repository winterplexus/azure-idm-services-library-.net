//
//  IdentityManager.cs
//
//  Wiregrass Code Technology 2020-2023
//
using System.Globalization;
using System.Net;
using Azure.Core.Pipeline;
using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace IdentityManagement.Services
{
    public class IdentityManager : IIdentityManager
    {
        private IConfigurationSection? configuration;
        private GraphServiceClient? client;

        public string? Tenant { get; set; }

        public IUserManagement? UserServices { get; set; }

        public IGroupManagement? GroupServices { get; set; }

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
            if (configuration == null)
            {
                throw new IdentityManagerException("configuration object is null");
            }

            var proxyAddress = configuration["ProxyAddress"];

            TokenCredentialOptions tokenCredentialOptions;

            if (!string.IsNullOrEmpty(proxyAddress))
            {
                using var handler = new HttpClientHandler();
                handler.Proxy = new WebProxy(new Uri(proxyAddress));

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
            if (configuration == null)
            {
                throw new IdentityManagerException("configuration object is null");
            }
            if (client == null)
            {
                throw new IdentityManagerException("client object is null");
            }

            Tenant = configuration["Tenant"];
            UserServices = new UserManagement(client, configuration);
            GroupServices = new GroupManagement(client, UserServices);
        }

        private TimeSpan GetTimeout()
        {
            if (configuration == null)
            {
                throw new IdentityManagerException("configuration object is null");
            }

            var timeout = configuration["Timeout"];

            double seconds = 5;
            if (!string.IsNullOrEmpty(timeout))
            {
                seconds = double.Parse(timeout, CultureInfo.InvariantCulture);
            }
            return TimeSpan.FromSeconds(seconds);
        }
    }
}