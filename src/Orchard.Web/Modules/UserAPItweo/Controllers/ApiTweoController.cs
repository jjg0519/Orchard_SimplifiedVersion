using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UserAPItweo.Controllers
{
    public class ApiTweoController : ApiController
    {
        // GET: api/ApiTweo
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ApiTweo/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/ApiTweo
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/ApiTweo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ApiTweo/5
        public void Delete(int id)
        {
        }
    }
}
