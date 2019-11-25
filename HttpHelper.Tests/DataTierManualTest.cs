using Microsoft.VisualStudio.TestTools.UnitTesting;
using HttpHelper;
using System;

namespace EvHttpClient.Tests
{
    [TestClass]
    public class DataTierManualTests
    {
        [TestMethod]
        public void ManualAPIverification()
        {

           //String baseUrl, String apikey, string getallEndPoint, string createEndpoint, string deleteEndpoint, string updateEndpoint, string detailEndpoint)
            HttpHelper.EvHttpClient helper = new HttpHelper.EvHttpClient("https://crud-api.azurewebsites.net", "d376f4e6-8150-407d-a694-1cd25cb11270", "/api/peek/", "/api/create/", "/api/remove/", "/api/update/", "/api/read/");
           String response= helper.GetAllEvents().Result;

            String detailresponse = helper.Details("693d1666-56f5-4ecb-90fe-7409781f9967").Result;
            String postbody = "{\"data\": {\"Id\": \"693d1666-56f5-4ecb-90fe-7409781f9969\",\"Name\": \"C++ training\",\"StartDate\": \"2019-11-23T09:38:00\",\"Duration\": 30,\"Brief\": \"learning c++24\"}}";

            String postbody_update = "{\"data\": {\"Id\": \"693d1666-56f5-4ecb-90fe-7409781f9969\",\"Name\": \"C++ training2\",\"StartDate\": \"2019-11-23T09:38:00\",\"Duration\": 30,\"Brief\": \"learning c++24\"}}";

            String createresponse = helper.Create("693d1666-56f5-4ecb-90fe-7409781f9969", postbody).Result;

            String updateresponse = helper.Edit("693d1666-56f5-4ecb-90fe-7409781f9969", postbody_update).Result;

            String deleteresponse = helper.Delete("693d1666-56f5-4ecb-90fe-7409781f9969").Result;
        }
    }
}
