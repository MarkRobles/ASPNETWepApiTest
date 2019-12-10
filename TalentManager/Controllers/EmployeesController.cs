using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using TalentManager.CustomActionFilters.Etags;
using TalentManager.Etags;
using TalentManager.Models;

namespace TalentManager.Controllers
{
    public class EmployeesController : ApiController
    {
        [EnableETag]
        public Employee Get(int id)
        {
            return new Employee()
            {
                Id = id,
                Name = "John Q Law",
                Department = "Enforcement"
            };
        }
        //public IEnumerable<Employee> GetAllEmployees()
        //{
        //    return new Employee[]
        //    {
        //            new Employee()
        //            {
        //            Id = 12345,
        //            Name = "John Q Law",
        //            Department = "Enforcement"
        //            },
        //            new Employee()
        //            {
        //            Id = 45678,
        //            Name = "Jane Q Taxpayer",
        //            Department = "Revenue"
        //            }
        //                        };
        //}


        public HttpResponseMessage GetAllEmployees()
        {
            var employees = new Employee[]
            {
                    new Employee()
                    {
                    Id = 12345,
                    Name = "John Q Law",
                    Department = "Enforcement"
                    },

                    new Employee()
                    {
                    Id = 45678,
                    Name = "Jane Q Taxpayer",
                    Department = "Revenue"
                    }
                    };
            var response = Request.CreateResponse<IEnumerable<Employee>>
            (HttpStatusCode.OK, employees);
            response.Headers.CacheControl = new CacheControlHeaderValue()
            {
                MaxAge = TimeSpan.FromSeconds(6),
                MustRevalidate = true,
                Private = true
            };
            return response;
        }

        [ConcurrencyChecker]
        public void Put(Employee employee) { }



    }
}
