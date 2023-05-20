namespace TaskUltimate.ViewModel
{
    public class ReservationDetailsDto
    {
        public int PersonCount { get; set; }
        public DateTime ReservationTime { get; set; }
        public DateTime CreationTime { get; set; }
        public string Description { get; set; }
        public int ReservationId { get; set; }
    }
}
