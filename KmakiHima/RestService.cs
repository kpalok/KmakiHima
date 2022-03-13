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
        private readonly Uri allAlertsUri;
        private readonly Uri newAlertsUri;
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

            allAlertsUri = new Uri(Constants.ALL_ALERTS_URS);
            newAlertsUri = new Uri(Constants.NEW_ALERTS_URS);
        }

        public async Task RefreshAlertsAsync()
        {
            DateTime start = DateTime.Now;
            try
            {
                HttpResponseMessage response = await client.GetAsync(newAlertsUri);

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

            Debug.WriteLine($"Alert count {ActiveAlerts.Count} refresh took {DateTime.Now.Subtract(start).TotalSeconds} seconds");
        }

        public async Task UpdateAlertStatus(AlertItem alert)
        {
            try
            {
                string json = JsonConvert.SerializeObject(alert);
                StringContent content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PutAsync(allAlertsUri, content);

                await RefreshAlertsAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
