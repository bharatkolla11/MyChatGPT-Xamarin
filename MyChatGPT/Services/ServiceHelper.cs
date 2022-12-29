using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;

namespace MyChatGPT.Services
{
	public class ServiceHelper
	{
        private readonly JsonSerializerSettings serializerSettings;
        public ServiceHelper()
        {
            try
            {
                this.serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                    NullValueHandling = NullValueHandling.Ignore,
                };
                this.serializerSettings.Converters.Add(new StringEnumConverter());
            }
            catch (Exception ex)
            {

            }
        }
        public async Task<string> GetDataAsync(string baseUrl, string url)
        {
            string content = null;
            try
            {

                content = await HttpCallAsync(baseUrl, url, HttpMethod.Get);

            }
            catch (Exception ex)
            {

            }
            return content;
        }

        public async Task<string> PostDataAsync(string baseUrl, string url, string data)
        {
            string content = null;

            try
            {
   
                content = await HttpCallAsync(baseUrl, url, HttpMethod.Post, data);
            }
            catch (Exception ex)
            {

            }
            return content;
        }

        public async Task<HttpResponseMessage> CreateAsync(HttpClient client, string url, HttpMethod method, StringContent contentJson, Dictionary<string, string> parameters = null)
        {
            switch (method)
            {
                case HttpMethod m when m == HttpMethod.Post:
                    return await client.PostAsync(url, contentJson);
                case HttpMethod m when m == HttpMethod.Put:
                    return await client.PutAsync(url, contentJson);
                case HttpMethod m when m == HttpMethod.Delete:
                    return await client.DeleteAsync(url);
                default:
                    return await client.GetAsync(url);
            }
        }
        public async Task<string> PutDataAsync(string baseUrl, string url, string data)
        {
            string content = null;
            try
            {
                content = await HttpCallAsync(baseUrl, url, HttpMethod.Put, data);
            }
            catch (Exception ex)
            {

            }
            return content;
        }

        public async Task<string> HttpCallAsync(string baseUrl, string url, HttpMethod httpMethod, string data = null)
        {

            string content = null;
            using (var httpClientHandler = new HttpClientHandler())
            {
                //httpClientHandler.ServerCertificateCustomValidationCallback =
                //    (message, cert, chain, errors) => { return true; };

                using (var client = new HttpClient(httpClientHandler))
                {
                    client.BaseAddress = new Uri(baseUrl);


                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + "");

                    client.Timeout = TimeSpan.FromMilliseconds(20000);

                    StringContent contentJson = null;

                    if (data != null)
                    {
                        contentJson = new StringContent(data, Encoding.UTF8, "application/json");
                    }

                    try
                    {
               
                        HttpResponseMessage response = await CreateAsync(client, url, httpMethod, contentJson);
                        content = await response.Content?.ReadAsStringAsync();
                        switch (response.StatusCode)
                        {
                            case HttpStatusCode m when m == HttpStatusCode.OK:
                            case HttpStatusCode n when n == HttpStatusCode.NoContent:
                            case HttpStatusCode o when o == HttpStatusCode.Created:
                                break;
                            case HttpStatusCode m when m == HttpStatusCode.Unauthorized:
                                break;
                            case HttpStatusCode p when p == HttpStatusCode.BadRequest:
                                var objContactList = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(content);
                                var keyStatus = objContactList.Keys.First();
                                var errorMessage = objContactList[keyStatus];

                                if (errorMessage.Count > 0)
                                {
                                    Console.WriteLine(errorMessage);
                                    content = errorMessage.FirstOrDefault();
                                }
                                break;
                            case HttpStatusCode q when q == HttpStatusCode.NotFound:
                                var objContactList1 = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);
                                var keyStatus1 = objContactList1.Keys.First();
                                var errorMessage1 = objContactList1[keyStatus1];

                                if (!string.IsNullOrEmpty(errorMessage1))
                                {
                                    Console.WriteLine(errorMessage1);
                                    content = errorMessage1;
                                }
                                break;
                            default:
                                throw new APIException
                                {
                                    StatusCode = response.StatusCode.ToString(),
                                    Content = content,
                                };
                        }

                    }
                    catch (APIException ex)
                    {
                        return ex.InnerException?.Message;
                    }
                    catch (Exception ex)
                    {
                        return ex.InnerException?.Message;
                    }
                }
            }
            return content;
        }

    }
}

