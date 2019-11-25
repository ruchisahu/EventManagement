using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HttpHelper
{
    public class Class1
    {
        static string Baseurl = "https://crud-api.azurewebsites.net";
        Class1()
        {
           
        }
        public static async System.Threading.Tasks.Task GetAllAsync()
        {
         //   List<DomainEvent> eventitems = new List<DomainEvent>();

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Add("X-API-KEY", "d376f4e6-8150-407d-a694-1cd25cb11270");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("/api/peek/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EventResponse = Res.Content.ReadAsStringAsync().Result;
                    dynamic json = JValue.Parse(EventResponse);
                    var jsonmessage = json.message;

                    foreach (var item in jsonmessage)
                    {
                        var data = item.data;
                        var data1 = data.ToString();
                      //  DomainEvent domain = new DomainEvent();
                      //  domain = JsonConvert.DeserializeObject<DomainEvent>(data1);
                       // eventitems.Add(domain);
                    }
                    return eventitems;

                }
            }
        public static void Create()
        {

        }
        public static void Delete()
        {

        }
        public static void Edit()
        {

        }

        public static void Details()
        {

        }
    }
}
