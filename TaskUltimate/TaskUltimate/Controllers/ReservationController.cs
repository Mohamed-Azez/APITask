using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using TaskUltimate.Interfaces;
using TaskUltimate.Models;
using TaskUltimate.ViewModel;

namespace TaskUltimate.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservation;
        private readonly ISendMessageService _sendMessage;
        public ReservationController(IReservationService reservation, ISendMessageService sendMessage)
        {
            _reservation = reservation;
            _sendMessage = sendMessage;
        }
        [HttpPost]
        [Route(nameof(AddReservation))]
        public IActionResult AddReservation([Required][FromBody] ReservationDto reservation)
        {
            int result = _reservation.AddReservation(reservation);
            if (result > 0) return Ok(result);
            else return BadRequest();
        }
        [HttpPost]
        [Route(nameof(AddReservationDetails))]
        public async Task<IActionResult> AddReservationDetails([Required][FromBody] ReservationDetailsDto reservationDetails)
        {
            bool result = _reservation.AddReservationDetails(reservationDetails);
            string Messagebody = "PersonCount : " + reservationDetails.PersonCount +
                "<br> Description : " + reservationDetails.Description + 
                "<br> ReservationTime : " + reservationDetails.ReservationTime+
                "<br> ReservationID : "+reservationDetails.ReservationId;
            if (result == true)
            {
                await _sendMessage.SendEmailMessage(new MailRequest
                {
                    ToEmail = "mohamedazez238@gmail.com",
                    Subject = "Message",
                    Body = Messagebody,
                });

                _sendMessage.SendWhatsAppMessage("+201144472548", Messagebody);
                return Ok("Success");
            }
            else return BadRequest();
        }
    }
}
