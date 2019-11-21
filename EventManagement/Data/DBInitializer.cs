using EventManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManagement.Data
{
    public class DBInitializer
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureCreated();
            if (context.EventItem.Any())
            {
                return;
            }
            var EventItem = new EventItem[]
                {
                    new EventItem{ Name="Java", StartDate = DateTime.Parse("2019-11-24 08:15:00 AM"), Duration=60, Brief="about event100"},
                    new EventItem{ Name="C++", StartDate = DateTime.Parse("2019-11-24 12:15:12 PM"), Duration=30, Brief="Discussion C++"},
                    new EventItem{ Name="Unity", StartDate = DateTime.Parse("2019-11-24 5:00:00 PM"), Duration=100, Brief="Game Devlopment workshop"},
                    new EventItem{ Name="Dot Net", StartDate = DateTime.Parse("2019-11-25 5:00:00 PM"), Duration=60, Brief="Dicsussing new features in ASP.Net"},
                    new EventItem{ Name="Machine learning", StartDate = DateTime.Parse("2019-11-26 11:00:00 AM"), Duration=60, Brief="about event Machine learning"},
                    new EventItem{ Name="EventTech", StartDate = DateTime.Parse("2019-11-26 5:00:00 PM"), Duration=60, Brief="New Innovations"}
                };

            foreach (EventItem e in EventItem)
            {
                context.EventItem.Add(e);
            }
            context.SaveChanges();
        }
    }
}
