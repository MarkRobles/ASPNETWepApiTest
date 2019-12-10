using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TalentManager.Controllers.MessageHandlers
{
    public class MyHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
        {

            if (request.Method == HttpMethod.Post && request.Headers.Contains("X-HTTP-Method-Override"))
            {
                var method = request.Headers.GetValues("X-HTTP-Method-Override").FirstOrDefault();
                bool isPut = String.Equals(method, "PUT", StringComparison.OrdinalIgnoreCase);
                bool isDelete = String.Equals(method, "DELETE", StringComparison.OrdinalIgnoreCase);
                if (isPut || isDelete)
                {
                    request.Method = new HttpMethod(method);
                }
            }else
            {
                // Inspect and do your stuff with request here
                // If you are not happy for any reason,
                // you can reject the request right here like this
                bool isBadRequest = false;
                if (isBadRequest)
                    return request.CreateResponse(HttpStatusCode.BadRequest);
                var response = await base.SendAsync(request, cancellationToken);
                // Inspect and do your stuff with response here
                return response;
            }
            return await base.SendAsync(request, cancellationToken);
          
        }
    }
}