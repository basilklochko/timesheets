using System.Net.Http;
using System.Net.Http.Headers;

namespace Timesheet.Api.Models
{
    public class AuthAttribute : System.Web.Http.AuthorizeAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            HttpRequestMessage request = actionContext.Request;
            HttpRequestHeaders requestHeaders = request.Headers;

            if (requestHeaders.GetCookies().Count == 0 || requestHeaders.GetCookies()[0][Constant.AuthCookie] == null || requestHeaders.GetCookies()[0][Constant.AuthCookie].Value.ToLower() == "false")
            {
                base.OnAuthorization(actionContext);
            }
        }
    }
}