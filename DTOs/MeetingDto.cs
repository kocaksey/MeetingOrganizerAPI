namespace MeetingOrganizer.DTOs
{
    public class MeetingDto
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }  // TimeSpan yerine string
        public string EndTime { get; set; }    // TimeSpan yerine string
        public List<string> Participants { get; set; }
    }
}
