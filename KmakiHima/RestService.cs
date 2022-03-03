using System;
using System.Net;
using System.Net.Http;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace KmakiHima
{
    public class RestService
    {
        private readonly HttpClient client;
        private readonly Uri alertItemsUri;
        public List<AlertItem> ActiveAlerts { get; private set; }

        public RestService()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    return (cert?.GetPublicKeyString() ?? string.Empty).Equals(Constants.PUBLIC_KEY);
                }
            };

            client = new HttpClient(handler, false)
            {
                BaseAddress = new Uri(Constants.BASE_URL)
            };

            alertItemsUri = new Uri("/alertitems", UriKind.Relative);
        }

        public async Task RefreshAlertsAsync()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(alertItemsUri);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    ActiveAlerts = JsonConvert.DeserializeObject<List<AlertItem>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task UpdateAlertStatus(AlertItem alert)
        {
            try
            {
                string json = JsonConvert.SerializeObject(alert);
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(alertItemsUri, content);

                await RefreshAlertsAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
