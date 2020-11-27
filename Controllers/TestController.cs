using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using ItEnterpriseTest.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ItEnterpriseTest.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    [Produces("application/json")]
    public class TestController : ControllerBase
    {
        ApplicationContext _applicationContext;

        public TestController(ApplicationContext application)
        {
            _applicationContext = application;
        }

        
        [Route("measure")]
        public void Measure()                             //      measures response time
        {
            int period = 5;                                 //      measurement period in seconds
            string url = "https://www.google.com/";         //      URL for testing
            var watch = new Stopwatch();                    //      watch for measuring time

            while (true)
            {
                WebRequest webRequest = WebRequest.Create(url);             //      Create a 'WebRequest' object with the specified url
                watch.Start();
                WebResponse webResponse = webRequest.GetResponse();         //      Send the 'WebRequest' and wait for response.
                webResponse.Close();                                        //      Release resources of response object.
                watch.Stop();
                Thread.Sleep(period * 1000);
                string result = watch.ElapsedMilliseconds.ToString();
                watch.Reset();
                Result res = new Result { ResponsingTime = result };
                _applicationContext.Results.Add(res);
                _applicationContext.SaveChanges();
            }
        }

        [HttpGet]
        [Route("results")]
        public IEnumerable<Result> GetAll()                     //      returns all notes from db
        {
            return _applicationContext.Results.ToList();
        }
    }
}
