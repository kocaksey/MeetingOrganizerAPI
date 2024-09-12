namespace MeetingOrganizer.Models
{
    public class Meeting
    {
        public int? MeetingId { get; set; }      
        public string Title { get; set; }      
        public DateTime Date { get; set; }       
        public TimeSpan StartTime { get; set; }  
        public TimeSpan EndTime { get; set; }    
        public List<string> Participants { get; set; }  
    }
}
