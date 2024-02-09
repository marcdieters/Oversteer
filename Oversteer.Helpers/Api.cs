using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Oversteer.Helpers
{
    public class Api
    {
        public static async Task<string> Get(string baseUri, string endpoint, string token)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                using (var client = new HttpClient(httpClientHandler))
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    client.BaseAddress = new Uri(baseUri);
                    var response = await client.GetAsync(endpoint);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        throw new Exception(response.StatusCode.ToString() + ": " + response.RequestMessage);
                    }
                }
            }
        }

        public static async Task<byte[]> GetFile(string baseUri, string endpoint, string token)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(baseUri);

                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    var response = await client.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();

                    byte[] buffer = new byte[16 * 1024];
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

        public static async Task<string> Post(string baseUri, string endpoint, dynamic body, string token, bool serialize = true)
        {
            using (var httpClientHandler = new HttpClientHandler())
            {
                using (var client = new HttpClient(httpClientHandler))
                {
                    if (!string.IsNullOrEmpty(token))
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    string jsonDoc = string.Empty;
                    if (serialize)
                    {
                        jsonDoc = JsonConvert.SerializeObject(body);
                    }
                    else
                    {
                        jsonDoc = body;
                    }

                    HttpContent httpContent = new StringContent(jsonDoc, Encoding.UTF8, "application/json");
                    client.BaseAddress = new Uri(baseUri);
                    var response = await client.PostAsync(endpoint, httpContent);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        throw new Exception(response.StatusCode.ToString() + ": " + response.RequestMessage);
                    }
                }
            }
        }

        public static async Task<string> Put(string baseUri, string endpoint, object body, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                client.BaseAddress = new Uri(baseUri);
                string jsonDoc = JsonConvert.SerializeObject(body);
                HttpContent httpContent = new StringContent(jsonDoc, Encoding.UTF8, "application/json");
                var response = await client.PutAsync(endpoint, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + ": " + response.RequestMessage);
                }
            }
        }

        public static async Task<string> Delete(string baseUri, string endpoint, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                client.BaseAddress = new Uri(baseUri);

                var response = await client.DeleteAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + ": " + response.RequestMessage);
                }
            }
        }

        public static async Task<string> Delete(string baseUri, string endpoint, object body, string token)
        {
            using (HttpClient client = new HttpClient())
            {
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                client.BaseAddress = new Uri(baseUri);
                string jsonDoc = JsonConvert.SerializeObject(body);
                HttpContent httpContent = new StringContent(jsonDoc, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Delete,
                    RequestUri = new Uri(baseUri + endpoint),
                    Content = httpContent
                };

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString() + ": " + response.RequestMessage);
                }
            }
        }
    }
}
