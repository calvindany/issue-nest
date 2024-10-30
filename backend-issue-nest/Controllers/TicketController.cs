using backend_issue_nest.Controllers.Helper;
using backend_issue_nest.Models;
using backend_issue_nest.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace backend_issue_nest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketRepository _ticketRepository;

        public TicketController(TicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        [Authorize(Policy = "NormalAuthentication")]
        [Route("")]
        [HttpGet]
        public IActionResult GetTickets()
        {
            Response response = null;
            string user_id = "";
            string role = "";
            ClaimsIdentity ? identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null && identity.FindFirst("id") != null)
            {
                user_id = identity.FindFirst("id").Value;
            }
            
            if (identity != null && identity.FindFirst(ClaimTypes.Role) != null)
            {
                role = identity.FindFirst(ClaimTypes.Role).Value;
            }

            try
            {
                List<Ticket> tickets = _ticketRepository.GetTicket(user_id, role);

                response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status200OK, tickets, null);

                return JSONResponse(response);
            }
            catch (Exception ex)
            {
                response = ResponseHelper.GenerateResponseData("Internal Server Error", StatusCodes.Status500InternalServerError, null, ex);

                return JSONResponse(response);
            }
        }

        [Authorize(Policy = "UserAuthentication")]
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> PostTicket([FromBody] Ticket ticket)
        {
            Response response = null;
            string user_id = "";

            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null && identity.FindFirst("id") != null)
            {
                user_id = identity.FindFirst("id").Value;
            }
            try
            {
                Ticket res = await _ticketRepository.CreateTicket(ticket, user_id);

                if(res == null)
                {
                    response = ResponseHelper.GenerateResponseData("You can't update other clint ticket data", StatusCodes.Status401Unauthorized, null, null);

                    return JSONResponse(response);
                }

                response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status200OK, ticket, null);

                return JSONResponse(response);
            } 
            catch (Exception ex)
            {
                response = ResponseHelper.GenerateResponseData("Internal Server Error", StatusCodes.Status500InternalServerError, null, ex);

                return JSONResponse(response);
            }
        }

        [Authorize(Policy = "NormalAuthentication")]
        [Route("")]
        [HttpPut]
        public async Task<IActionResult> PutTicket([FromBody] Ticket ticket)
        {
            Response response = null;
            ClaimsIdentity? identity = HttpContext.User.Identity as ClaimsIdentity;

            string user_id = "";
            string role = "";
            if (identity != null && identity.FindFirst("id") != null)
            {
                user_id = identity.FindFirst("id").Value;
            }

            if (identity != null && identity.FindFirst(ClaimTypes.Role) != null)
            {
                role = identity.FindFirst(ClaimTypes.Role).Value;
            }

            try
            {
                Ticket res = await _ticketRepository.UpdateTicket(ticket, user_id, role);

                response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status200OK, res, null);

                return JSONResponse(response);
            }
            catch (Exception ex)
            {
                response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status500InternalServerError, null, ex);
                return JSONResponse(response);
            }
        }

        [Authorize(Policy = "NormalAuthentication")]
        [Route("")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTicket(int ticket_id)
        {
            Response response = null;
            try
            {
                await _ticketRepository.DeleteTicket(ticket_id);

                response = ResponseHelper.GenerateResponseData("Success delete ticket with id: " + ticket_id, StatusCodes.Status200OK, null, null);

                return JSONResponse(response);
            }
            catch (Exception ex)
            {
                response = ResponseHelper.GenerateResponseData("Internal Server Error", StatusCodes.Status500InternalServerError, null, ex);

                return JSONResponse(response);
            }
        }

        private IActionResult JSONResponse(Response responseData)
        {
            if (responseData.status_code != 200)
            {
                return StatusCode(responseData.status_code, responseData);
            }

            return Ok(responseData);
        }
    }
}
