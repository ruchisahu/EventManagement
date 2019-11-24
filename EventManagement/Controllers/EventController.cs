using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventManagement.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;

namespace EventManagement.Controllers
{
    public class EventController : Controller
    {
        private readonly Context _context;
        string Baseurl = "https://localhost:44314/";
        public EventController(Context context)
        {
            _context = context;
        }

        // GET: Event
        public async Task<IActionResult> Index()
        {
            // return View(await _context.EventItem.ToListAsync());
            List<DomainEvent> eventitem = new List<DomainEvent>();
            using (var client = new HttpClient())
            {
                //Passing service base url  
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Clear();
                //Define request data format  
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage Res = await client.GetAsync("api/DomainEvents/");

                if (Res.IsSuccessStatusCode)
                {
                    var EventResponse = Res.Content.ReadAsStringAsync().Result;

                    eventitem = JsonConvert.DeserializeObject<List<DomainEvent>>(EventResponse);

                }
                //returning the employee list to view  
                return View(eventitem);
            }
            }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DomainEvent Event = new DomainEvent();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await client.GetAsync("/api/DomainEvents/" + id))
                {

                    if (!response.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    //Event = JsonConvert.DeserializeObject<DomainEvent>(apiResponse);

                    dynamic json = JValue.Parse(apiResponse);
                   // var jsonmessage = json.message;

                   Event = JsonConvert.DeserializeObject<DomainEvent>(json.ToString());

                }
            }

                return View(Event);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
       
        [HttpPost]
         public async Task<IActionResult> Create([Bind("Id,Name,StartDate,Duration,Brief")] DomainEvent eventItem)
          {
            DomainEvent receivedEvent = new DomainEvent();
            DomainEvent domainEvent = new DomainEvent();
            Guid obj = Guid.NewGuid();
            domainEvent.Name = eventItem.Name;
            domainEvent.StartDate = eventItem.StartDate;
            domainEvent.Duration = eventItem.Duration;
            domainEvent.Brief = eventItem.Brief;
            domainEvent.Id = obj;


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string postitem = JsonConvert.SerializeObject(domainEvent);
                StringContent content = new StringContent(postitem, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync("api/DomainEvents/", content))
                {
                   
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.Message = apiResponse.ToString();
                        return View("Status");
                    }
                    receivedEvent = JsonConvert.DeserializeObject<DomainEvent>(apiResponse);
                }
            }
            return View(eventItem);

        }
        // GET: Event/Edit/5
        
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DomainEvent item = new DomainEvent();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                using (var response = await client.GetAsync("/api/DomainEvents/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    item = JsonConvert.DeserializeObject<DomainEvent>(apiResponse);
                }
            }
            return View(item);
           
        }

        // POST: Event/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,StartDate,Duration,Brief")] DomainEvent domainEvent)
        {
           
            DomainEvent receivedevent = new DomainEvent();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                // client.DefaultRequestHeaders.Add("X-API-KEY", "d376f4e6-8150-407d-a694-1cd25cb11270");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string postitem = JsonConvert.SerializeObject(domainEvent);
                StringContent content = new StringContent(postitem, Encoding.UTF8, "application/json");

                using (var response = await client.PutAsync("/api/DomainEvents/" +id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        ViewBag.Message = apiResponse.ToString();
                        return View("Status");
                    }
                    //ViewBag.Result = "Success";
                    receivedevent = JsonConvert.DeserializeObject<DomainEvent>(apiResponse);
                }
            }
            return View(domainEvent);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

           
            DomainEvent receivedEvent = new DomainEvent();
            DomainEvent domainEvent = new DomainEvent();

            Guid Id = (Guid)id;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                using (var response = await client.DeleteAsync("/api/DomainEvents/" + Id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }


            return View(receivedEvent);
        }

   
        private bool EventItemExists(int id)
        {
            return _context.EventItem.Any(e => e.Id == id);
        }
    }
}
