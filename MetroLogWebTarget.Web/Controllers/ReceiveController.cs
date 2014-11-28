using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MetroLogWebTarget.Web.Models;
using Newtonsoft.Json;

namespace MetroLogWebTarget.Web.Controllers
{
    public class ReceiveController : Controller
    {
        // GET: Receive
        public void Index()
        {
            string json = null;
            using (var reader = new StreamReader(Request.InputStream))
                json = reader.ReadToEnd();

            // deserialize...
            var wrapper = JsonConvert.DeserializeObject<JsonPostWrapper>(json);

            //Console.WriteLine(info);
            Debug.WriteLine(wrapper);
        }
    }
}