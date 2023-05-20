namespace TaskUltimate.Models
{
    public class ReservationDetails
    {
        public int Id { get; set; }
        public int PersonCount { get; set; }
        public DateTime ReservationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string Description { get; set; }
        public int ReservationId { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}
