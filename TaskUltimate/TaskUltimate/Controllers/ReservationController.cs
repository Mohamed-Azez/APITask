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
        public ReservationController(IReservationService reservation)
        {
            _reservation = reservation;
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
        public IActionResult AddReservationDetails([Required][FromBody] ReservationDetailsDto reservationDetails)
        {
            bool result = _reservation.AddReservationDetails(reservationDetails);
            if (result == true) return Ok("Success");
            else return BadRequest();
        }
    }
}
