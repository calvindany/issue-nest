﻿using backend_issue_nest.Controllers.Helper;
using backend_issue_nest.Models;
using backend_issue_nest.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [Route("")]
        [HttpGet]
        public IActionResult GetTickets()
        {
            List<Ticket> tickets = _ticketRepository.GetTicket();

            Response response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status200OK, tickets, null);

            return JSONResponse(response);
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> PostTicket([FromBody] Ticket ticket)
        {
            Response response = null;

            try
            {
                Ticket res = await _ticketRepository.CreateTicket(ticket);

                response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status200OK, ticket, null);

                return JSONResponse(response);
            } 
            catch (Exception ex)
            {
                response = ResponseHelper.GenerateResponseData("Internal Server Error", StatusCodes.Status500InternalServerError, null, ex);

                return JSONResponse(response);
            }
        }

        [Route("")]
        [HttpPut]
        public async Task<IActionResult> PutTicket([FromBody] Ticket ticket)
        {
            Response response = null;

            try
            {
                Ticket res = await _ticketRepository.UpdateTicket(ticket);

                response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status200OK, res, null);

                return JSONResponse(response);
            }
            catch (Exception ex)
            {
                response = ResponseHelper.GenerateResponseData("Success", StatusCodes.Status500InternalServerError, null, ex);
                return JSONResponse(response);
            }
        }

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
