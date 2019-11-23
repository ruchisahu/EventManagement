﻿using System;
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
using Newtonsoft.Json.Linq;

namespace EventBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainEventsController : ControllerBase
    {
        string Baseurl = "https://crud-api.azurewebsites.net";
        
        
       // List<DomainEvent> eventitem = new List<DomainEvent>();
        
        public async Task<IActionResult> AllEvents()
        {
            List<DomainEvent> eventitems = new List<DomainEvent>();
            List<DomainEvent> eventitem1 = new List<DomainEvent>();
            List<int> rowlist=new List<int>();
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
                    dynamic json = JValue.Parse(EventResponse);
                    var jsonmessage = json.message;

                    foreach (var item in jsonmessage)
                    {
                        var data = item.data;
                        var data1 = data.ToString();
                        DomainEvent domain = new DomainEvent();
                        domain =JsonConvert.DeserializeObject<DomainEvent>(data1);
                        eventitems.Add(domain);
                    }
                   

                        //Deserializing the response recieved from web api and storing into the Employee list  
                        //   eventitem = JsonConvert.DeserializeObject<List<DomainEvent>>(EventResponse);
                        //string name=eventitem[0].Name;
                        //  eventitem1 = eventitem;
                        // Console.WriteLine(eventitem);

                    }
                //returning the employee list to view  
                 return Ok(eventitems);
               // return eventitems;
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
            Guid Id = eventItem.Id;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Add("X-API-KEY", "d376f4e6-8150-407d-a694-1cd25cb11270");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string eventobject = JsonConvert.SerializeObject(eventItem);
                string data = "{\"" + "data\":" + eventobject + "}";



                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync("/api/create/" +Id , content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        //string apiResponse = await response.Content.ReadAsStringAsync();
                        //return 
                        //receivedEvent = JsonConvert.DeserializeObject<DomainEvent>(apiResponse);
                    }
                    
                }
            }
            return Ok(eventItem);
        }

    }
}