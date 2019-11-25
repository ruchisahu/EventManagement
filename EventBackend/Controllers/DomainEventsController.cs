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
using Newtonsoft.Json.Linq;

namespace EventBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainEventsController : ControllerBase
    {
        string Baseurl = "https://crud-api.azurewebsites.net";

        enum ErrorCode
        {
            Overlap =1,
            TimeLimit=2,
            DaysLimit=3,
            Default=4
        }
        public async Task<List<DomainEvent>> GetAllEvents()
        {
            List<DomainEvent> eventitems = new List<DomainEvent>();
            List<int> rowlist = new List<int>();
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
                        DomainEvent domain = new DomainEvent();
                        domain = JsonConvert.DeserializeObject<DomainEvent>(data1);
                        eventitems.Add(domain);
                    }

                }
                //returning the event list to view  
                return eventitems;

            }
        }
        [HttpGet]
        public async Task<IActionResult> AllEvents()
        {
            List<DomainEvent> eventitems =  await GetAllEvents();
             return Ok(eventitems);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid Id)
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
            return Ok();

        }

        [HttpPost]
        
        public async Task<IActionResult> Create([FromBody]DomainEvent eventItem)
        {
            DomainEvent receivedEvent = new DomainEvent();
            List<DomainEvent> eventList = await GetAllEvents();
            Guid Id = eventItem.Id;
            var v= Validation(eventItem, eventList);
            if (Validation(eventItem, eventList) == ErrorCode.Overlap)
            {
                 return BadRequest(new { message = "Overlapping Events: Events should not overlaped." });
            }
            if (Validation(eventItem, eventList) == ErrorCode.TimeLimit)
            {
                return BadRequest(new { message = "Time limit exceed: Event can not be schedule after 8:00PM" });
            }
            if (Validation(eventItem, eventList) == ErrorCode.DaysLimit)
            {
                return BadRequest(new { message = "The schedule will span across 3 days." });
            }
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
                        return BadRequest(new { message = "bad request "});
                        
                    }
                }
            }
            return Ok(eventItem);
        }

        private ErrorCode Validation(DomainEvent eventItem, List<DomainEvent> eventlist,Guid? skipGuid=null)
        {
            var time = eventItem.StartDate.TimeOfDay;
            var newstartdate = eventItem.StartDate.Date;
            //case 1: overlapping events
            foreach (var item in eventlist)
            {
                if ((skipGuid!=null) && (item.Id == skipGuid))
                    continue;
                var oldstartdate = item.StartDate.Date;
                
                if (oldstartdate == newstartdate)
                {
                    var oldstarttime = item.StartDate.TimeOfDay;
                    var newstarttime = eventItem.StartDate.TimeOfDay;
                    var oldendtime = oldstarttime.Add(TimeSpan.FromMinutes(item.Duration));
                    var newendtime = newstarttime.Add(TimeSpan.FromMinutes(eventItem.Duration));
                    if (!((newstarttime > oldendtime && newendtime > oldstarttime) || (newstarttime < oldstarttime && newendtime < oldendtime)))
                    {
                        return ErrorCode.Overlap;
                    }
                }
            }
            // case2 : No event should start after 8:00PM
            var endtime = TimeSpan.Parse("20:00");

            if (TimeSpan.Compare(time, endtime) >= 1)
            {
                return ErrorCode.TimeLimit;
            }
            var val = (newstartdate - DateTime.Now).TotalDays;
            if ( val> 3)
            {
                return ErrorCode.DaysLimit;
            }
            return ErrorCode.Default;
        }

       

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            DomainEvent Event = new DomainEvent();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Add("X-API-KEY", "d376f4e6-8150-407d-a694-1cd25cb11270");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await client.GetAsync("/api/read/" + id))
                {

                    if (!response.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    dynamic json = JValue.Parse(apiResponse);
                    var jsonmessage = json.message;
                    Event = JsonConvert.DeserializeObject<DomainEvent>(jsonmessage.ToString());
                }
            }
            return Ok(Event);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(DomainEvent domainevent)
        {
            DomainEvent receivedevent = new DomainEvent();
            List<DomainEvent> eventList = await GetAllEvents();
            var v = Validation(domainevent, eventList);
            if (Validation(domainevent, eventList,domainevent.Id) == ErrorCode.Overlap)
            {
                return BadRequest(new { message = "Overlapping Events: Events should not overlaped." });
            }
            if (Validation(domainevent, eventList) == ErrorCode.TimeLimit)
            {
                return BadRequest(new { message = "Time limit exceed: Event can not be schedule after 8:00PM" });
            }
            if (Validation(domainevent, eventList) == ErrorCode.DaysLimit)
            {
                return BadRequest(new { message = "The schedule will span across 3 days." });
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Add("X-API-KEY", "d376f4e6-8150-407d-a694-1cd25cb11270");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string eventobject = JsonConvert.SerializeObject(domainevent);
                string data = "{\"" + "data\":" + eventobject + "}";
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                using (var response = await client.PutAsync("/api/update/"+ domainevent.Id.ToString(), content))
                {
                    if(!response.IsSuccessStatusCode)
                    {
                        return BadRequest();
                    }
                }
            }
            return Ok(domainevent);
        }

    }
}