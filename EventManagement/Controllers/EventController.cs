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

                //Sending request to find web api REST service resource GetAllEmployees using HttpClient  
                HttpResponseMessage Res = await client.GetAsync("api/DomainEvents/");

                //Checking the response is successful or not which is sent using HttpClient  
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var EmpResponse = Res.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    eventitem = JsonConvert.DeserializeObject<List<DomainEvent>>(EmpResponse);

                }
                //returning the employee list to view  
                return View(eventitem);
            }
            }

        // GET: Event/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.EventItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // GET: Event/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Event/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
         public async Task<IActionResult> Create([Bind("Id,Name,StartDate,Duration,Brief")] EventItem eventItem)
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
                // client.DefaultRequestHeaders.Add("X-API-KEY", "d376f4e6-8150-407d-a694-1cd25cb11270");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string postitem = JsonConvert.SerializeObject(domainEvent);
                StringContent content = new StringContent(postitem, Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync("api/DomainEvents/", content))
                { 
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedEvent = JsonConvert.DeserializeObject<DomainEvent>(apiResponse);
                }
            }
            return Ok(receivedEvent);

            //var time= eventItem.StartDate.TimeOfDay;
            //List<EventItem> eventlist = _context.EventItem.ToList();
            //foreach (var item in eventlist)
            //{
            //    var oldstartdate = item.StartDate.Date;
            //    var newstartdate = eventItem.StartDate.Date;
            //    if (oldstartdate == newstartdate)
            //    {
            //        var oldstarttime = item.StartDate.TimeOfDay;
            //        var newstarttime = eventItem.StartDate.TimeOfDay;
            //        var oldendtime = oldstarttime.Add (TimeSpan.FromMinutes(item.Duration));
            //        var newendtime=newstarttime.Add(TimeSpan.FromMinutes(eventItem.Duration));
            //        if (!((newstarttime > oldendtime && newendtime> oldstarttime) || (newstarttime< oldstarttime && newendtime < oldendtime)))
            //        {
            //            ViewBag.Message = "Event time/date overlapped/";
            //            return View("Status");
            //        }


            //    }


            //}
            //var endtime = TimeSpan.Parse("20:00");

            //if (TimeSpan.Compare(time, endtime) >= 1)
            //{
            //    ViewBag.Message = "Event start time shoud be less than 8:00Pm";
            //    return View("Status");

            //}


            //else
            //{
            //    if (ModelState.IsValid)
            //    {
            //        _context.Add(eventItem);
            //        await _context.SaveChangesAsync();
            //        return RedirectToAction(nameof(Index));
            //    }
            //    return View(eventItem);
            //}

        }

  
    /*    [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody]EventItem eventItem)
        {
            EventItem receivedEvent = new EventItem();
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
               // client.DefaultRequestHeaders.Add("X-API-KEY", "d376f4e6-8150-407d-a694-1cd25cb11270");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                StringContent content = new StringContent(JsonConvert.SerializeObject(eventItem), Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync("api/DomainEvents/", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedEvent = JsonConvert.DeserializeObject<EventItem>(apiResponse);
                }
            }
            return Ok(receivedEvent);
        }
        */
        // GET: Event/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.EventItem.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(eventItem);
        }

        // POST: Event/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,Duration,Brief")] EventItem eventItem)
        {
            if (id != eventItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(eventItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventItemExists(eventItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(eventItem);
        }

        // GET: Event/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.EventItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // POST: Event/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var eventItem = await _context.EventItem.FindAsync(id);
            _context.EventItem.Remove(eventItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventItemExists(int id)
        {
            return _context.EventItem.Any(e => e.Id == id);
        }
    }
}
