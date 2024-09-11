namespace MeetingOrganizer.DTOs
{
    public class MeetingDto
    {
        public string Title { get; set; }          // Toplantı konusu
        public DateTime Date { get; set; }         // Toplantı tarihi
        public TimeSpan StartTime { get; set; }    // Başlangıç saati
        public TimeSpan EndTime { get; set; }      // Bitiş saati
        public List<string> Participants { get; set; }  // Katılımcılar listesi
    }
}
