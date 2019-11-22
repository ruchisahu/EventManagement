using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EventBackend.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EventBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainEventsController : ControllerBase
    {
        string Baseurl = "https://crud-api.azurewebsites.net";
        
        
        List<DomainEvent> eventitem = new List<DomainEvent>();
        
        public async Task<IActionResult> AllEvents()
        {
            List<DomainEvent> eventitem = new List<DomainEvent>();
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(Baseurl);

                 client.DefaultRequestHeaders.Add("X-API-KEY", "d376f4e6-8150-407d-a694-1cd25cb11270");
                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("/api/peek/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EventResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    eventitem = JsonConvert.DeserializeObject<List<DomainEvent>>(EventResponse);

                }
                //returning the employee list to view  
                return Ok(eventitem);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Add("X-API-KEY", "d376f4e6-8150-407d-a694-1cd25cb11270");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await client.DeleteAsync("/api/remove/" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }
            return RedirectToAction("AllEvents");

        }

        [HttpPost]
        
        public async Task<IActionResult> Create([FromBody]DomainEvent eventItem)
        {
            DomainEvent receivedEvent = new DomainEvent();
            int Id = 56;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Add("X-API-KEY", "d376f4e6-8150-407d-a694-1cd25cb11270");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(eventItem), Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync("/api/create/" + Id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedEvent = JsonConvert.DeserializeObject<DomainEvent>(apiResponse);
                }
            }
            return Ok(receivedEvent);
        }

    }
}