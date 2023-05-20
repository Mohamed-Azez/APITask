using TaskUltimate.ViewModel;

namespace TaskUltimate.Interfaces
{
    public interface IReservationService
    {
        public int AddReservation(ReservationDto reservation);
        public bool AddReservationDetails(ReservationDetailsDto reservationdetails);
    }
}
