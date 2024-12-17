using ApiSdk;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProviderOneLib
{
    public class ProviderOneClientFactory
    {
        private readonly IAuthenticationProvider _authenticationProvider;
        private readonly HttpClient _httpClient;

        public ProviderOneClientFactory(HttpClient httpClient)
        {
            _authenticationProvider = new AnonymousAuthenticationProvider();
            _httpClient = httpClient;
        }

        public ProviderOneApiClient GetClient()
        {
            return new ProviderOneApiClient(new HttpClientRequestAdapter(_authenticationProvider, httpClient: _httpClient));
        }
    }
}
