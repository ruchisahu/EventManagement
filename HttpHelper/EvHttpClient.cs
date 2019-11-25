using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HttpHelper
{
    public class EvHttpClient
    {
        string Baseurl;
        string apikey;
        string getallEndPoint;
        string createEndpoint;
        string deleteEndpoint;
        string updateEndpoint;
        string detailEndpoint;

        HttpClient client;

        public EvHttpClient(String baseUrl, String apikey,string getallEndPoint, string createEndpoint, string deleteEndpoint, string updateEndpoint, string detailEndpoint)
        {
            this.Baseurl = baseUrl;
            this.apikey = apikey;
            this.createEndpoint = createEndpoint;
            this.deleteEndpoint = deleteEndpoint;
            this.updateEndpoint = updateEndpoint;
            this.detailEndpoint = detailEndpoint;
            this.getallEndPoint = getallEndPoint;

            client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            if (!String.IsNullOrEmpty(apikey))
            {
                client.DefaultRequestHeaders.Add("X-API-KEY", apikey);
            }
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            }
        public async Task<String> GetAllEvents()
        {
            String response = null;
            try
            {
                HttpResponseMessage Res = await client.GetAsync(getallEndPoint);
                if (Res.IsSuccessStatusCode)
                {
                    response = await Res.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetAllEvents failed");
            }

            return response;
        }
        public async Task<String> Create(String Id, String data)
        {
            String response = null;
            try
            {
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PostAsync(createEndpoint + Id, content);
                
                if (Res.IsSuccessStatusCode)
                {
                    response = await Res.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Create failed");
            }

            return response;
        }
        public async Task<String> Delete(String Id)
            {

            String response = null;
            try
            {
                HttpResponseMessage Res = await client.DeleteAsync("/api/remove/" + Id);
                if (Res.IsSuccessStatusCode)
                {
                    response = await Res.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Delete failed");
            }

            return response;
        }
        public async Task<String> Edit(String Id, String data)
            {

            String response = null;
            try
            {
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await client.PutAsync("/api/update/" + Id, content);
                  if (Res.IsSuccessStatusCode)
                {
                    response = await Res.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Edit failed");
            }

            return response;
        }

        public async Task<String> Details(string id)
            {
            String response = null;
            try
            {
                HttpResponseMessage Res = await client.GetAsync("/api/read/" + id);

                if (Res.IsSuccessStatusCode)
                {
                    response = await Res.Content.ReadAsStringAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Details failed");
            }

            return response;

        }
                }
            }
        
    
