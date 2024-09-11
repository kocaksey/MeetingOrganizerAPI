namespace MeetingOrganizer.Models
{
    public class Meeting
    {
        public int MeetingId { get; set; }        // Toplantı ID'si (Primary Key)
        public string Title { get; set; }         // Toplantı konusu
        public DateTime Date { get; set; }        // Toplantı tarihi
        public TimeSpan StartTime { get; set; }   // Başlangıç saati
        public TimeSpan EndTime { get; set; }     // Bitiş saati
        public List<Participant> Participants { get; set; }  // Katılımcılar listesi
    }
}
