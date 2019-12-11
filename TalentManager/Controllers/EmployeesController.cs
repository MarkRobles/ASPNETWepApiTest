using System;
using System.Collections.Generic;
using System.IdentityModel.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;
using TalentManager.CustomActionFilters.Etags;
using TalentManager.Etags;
using TalentManager.Models;

namespace TalentManager.Controllers
{

    public static class PrincipalHelper
    {
        public static bool CheckAccess(this IPrincipal principal, string resource, string action,   IList<Claim> resourceClaims)
        {
            var context = new AuthorizationContext(principal as ClaimsPrincipal,
            resource, action);
            resourceClaims.ToList().ForEach(c => context.Resource.Add(c));
            var config = new IdentityConfiguration();
            return config.ClaimsAuthorizationManager.CheckAccess(context);
        }
    }
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

            //Agregar CORS
            response.Headers.Add("Access-Control-Allow-Origin", "*");

            //Habilitar cache 
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


        public HttpResponseMessage Delete(int id)
        {
            // Based on ID, retrieve employee details and create the list of resource claims
            var employeeClaims = new List<Claim>()
                {
                new Claim(ClaimTypes.Country, "US"),
                new Claim("http://badri/claims/department", "Engineering")
                };

            if (User.CheckAccess("Employee", "Delete", employeeClaims))
            {
                //repository.Remove(id);
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            else
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }



    }
}
