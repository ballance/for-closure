using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ballance.it.for_closure.common.repositories;
using ballance.it.for_closure.common.models;

namespace ballance.it.for_closure.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        RepositoryBase _propertyPersistenceManager = new RepositoryBase(
                "Server=re-db.chtlgfr8b1iu.us-east-1.rds.amazonaws.com;Port=7306;Database=re_db;Uid=re_user;Pwd=vXani82LdScu;");

        [HttpGet]
        public IEnumerable<PropertyModel> Get()
        {
            var properties = _propertyPersistenceManager.RetrieveProperties();
            return properties;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public PropertyModel Get(string id)
        {
            var property = _propertyPersistenceManager.GetPropertyById(id);
            return property;
        }

        // // POST api/values
        // [HttpPost]
        // public void Post([FromBody] string value)
        // {
        // }

        // // PUT api/values/5
        // [HttpPut("{id}")]
        // public void Put(int id, [FromBody] string value)
        // {
        // }

        // // DELETE api/values/5
        // [HttpDelete("{id}")]
        // public void Delete(int id)
        // {
        // }
    }
}
