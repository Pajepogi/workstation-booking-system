namespace wbs_api.DTOs
{
    public class CreateBookingDTO
    {
        public int WorkstationId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public DateTime? BookingDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsPermanent { get; set; }

    }
}
