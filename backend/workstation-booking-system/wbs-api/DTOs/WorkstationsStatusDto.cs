namespace wbs_api.DTOs
{
    public class WorkstationStatusDto
    {
        public int Id { get; set; }
        public int? BookingId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Wing { get; set; } = string.Empty;
        public double XPosition { get; set; }
        public double YPosition { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}