namespace wbs_api.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public int WorkstationId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public DateTime? BookingDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsPermanent { get; set; }
    }
}

