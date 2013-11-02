using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BrainBowTestWebAPI.Models;
using Newtonsoft.Json.Linq;

namespace BrainBowTestWebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        
        // GET api/values/5
        public JObject Get(string typeofsearch, string filtercriteria)
        {
            if (typeofsearch == "movie")
            {
                RoviAPI roviAPI = new RoviAPI();
                return roviAPI.GetMovieByKeyword(filtercriteria);
            }
            else
            {
                RoviAPI roviAPI = new RoviAPI();
                return roviAPI.GetCelebByName(filtercriteria);
            }
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}