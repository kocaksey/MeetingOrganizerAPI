using MeetingOrganizer.DTOs;
using MeetingOrganizer.Models;

namespace MeetingOrganizer.Repository.Abstract
{
    public interface IMeetingRepository
    {
        void CreateMeeting(MeetingDto meetingDto); // MeetingDto kullanmaya devam ediyoruz
        
        Meeting GetMeetingById(int meetingId);
        void UpdateMeeting(int meetingId, MeetingDto meetingDto);
        void DeleteMeeting(int meetingId);
        IEnumerable<MeetingDto> GetAllMeetings();
    }

}
