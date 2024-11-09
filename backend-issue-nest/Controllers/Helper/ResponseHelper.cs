using backend_issue_nest.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend_issue_nest.Controllers.Helper
{
    public class ResponseHelper
    {
        public static Response GenerateResponseData(string message, int statusCode, object result, Exception err)
        {
            Response response = new Response()
            {
                status_code = statusCode,
                message = message,
                result = result,
                err = err != null ? new { err.Message, err.StackTrace } : null,
            };

            return response;
        }

    }
}
