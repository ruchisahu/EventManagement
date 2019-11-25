using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace HttpHelper.Tests
{
    [TestClass]
    public class DomainControllerManualTests
    {
        [TestMethod]
        public void ManualAPIverificationDC()
        {

            HttpHelper.EvHttpClient helper = new HttpHelper.EvHttpClient("https://localhost:44314/", null, "api/DomainEvents/", "api/DomainEvents/", "api/DomainEvents/", "api/DomainEvents/", "api/DomainEvents/");
            String response = helper.GetAllEvents().Result;

            String detailresponse = helper.Details("693d1666-56f5-4ecb-90fe-7409781f9968").Result;
            String postbody = "{\"id\": \"8adaae04-fe7c-11e8-8eb2-f2801f1b5fd2\",\"name\": \"Java Training\",\"startDate\": \"2019-11-27T10:00:00\",\"duration\": 30,\"brief\": \"This is an event description\"}";
            String postbody_update = "{\"id\": \"8adaae04-fe7c-11e8-8eb2-f2801f1b5fd2\",\"name\": \"Java Training2\",\"startDate\": \"2019-11-27T10:00:00\",\"duration\": 30,\"brief\": \"This is an event description\"}";
            
            String createresponse = helper.Create("", postbody).Result;

            String updateresponse = helper.Edit("8adaae04-fe7c-11e8-8eb2-f2801f1b5fd2", postbody_update).Result;

            String deleteresponse = helper.Delete("8adaae04-fe7c-11e8-8eb2-f2801f1b5fd2").Result;
        }
    }
}
