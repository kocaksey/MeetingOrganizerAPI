using Dapper;
using MeetingOrganizer.DTOs;
using MeetingOrganizer.Models;
using MeetingOrganizer.Repository.Abstract;
using MySql.Data.MySqlClient;

namespace MeetingOrganizer.Repositories.Concrete
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly string _connectionString;

        public MeetingRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void CreateMeeting(MeetingDto meetingDto)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // DTO'dan verileri alıyoruz
                var startTimeSpan = TimeSpan.Parse(meetingDto.StartTime); // String to TimeSpan dönüşümü
                var endTimeSpan = TimeSpan.Parse(meetingDto.EndTime);     // String to TimeSpan dönüşümü

                // Veritabanına kaydetme işlemi
                string insertMeetingSql = "INSERT INTO Meetings (title, date, start_time, end_time) VALUES (@Title, @Date, @StartTime, @EndTime)";
                connection.Execute(insertMeetingSql, new { meetingDto.Title, meetingDto.Date, StartTime = startTimeSpan, EndTime = endTimeSpan });

                int meetingId = connection.QuerySingle<int>("SELECT LAST_INSERT_ID()");

                // Katılımcıları ekleme
                foreach (var participant in meetingDto.Participants)
                {
                    int participantId = connection.QuerySingleOrDefault<int>("SELECT participant_id FROM Participants WHERE name = @Name", new { Name = participant });

                    if (participantId == 0)
                    {
                        string insertParticipantSql = "INSERT INTO Participants (name) VALUES (@Name)";
                        connection.Execute(insertParticipantSql, new { Name = participant });
                        participantId = connection.QuerySingle<int>("SELECT LAST_INSERT_ID()");
                    }

                    string insertMeetingParticipantSql = "INSERT INTO MeetingParticipants (meeting_id, participant_id) VALUES (@MeetingId, @ParticipantId)";
                    connection.Execute(insertMeetingParticipantSql, new { MeetingId = meetingId, ParticipantId = participantId });
                }
            }
        }



        public IEnumerable<Meeting> GetAllMeetings()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                return connection.Query<Meeting>("SELECT * FROM Meetings");
            }
        }

        public Meeting GetMeetingById(int meetingId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                return connection.QuerySingleOrDefault<Meeting>("SELECT * FROM Meetings WHERE meeting_id = @MeetingId", new { MeetingId = meetingId });
            }
        }

        public void UpdateMeeting(int meetingId, MeetingDto meetingDto)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string updateMeetingSql = "UPDATE Meetings SET title = @Title, date = @Date, start_time = @StartTime, end_time = @EndTime WHERE meeting_id = @MeetingId";
                connection.Execute(updateMeetingSql, new { meetingDto.Title, meetingDto.Date, meetingDto.StartTime, meetingDto.EndTime, MeetingId = meetingId });

                string deleteParticipantsSql = "DELETE FROM MeetingParticipants WHERE meeting_id = @MeetingId";
                connection.Execute(deleteParticipantsSql, new { MeetingId = meetingId });

                foreach (var participant in meetingDto.Participants)
                {
                    int participantId = connection.QuerySingleOrDefault<int>("SELECT participant_id FROM Participants WHERE name = @Name", new { Name = participant });

                    if (participantId == 0)
                    {
                        string insertParticipantSql = "INSERT INTO Participants (name) VALUES (@Name)";
                        connection.Execute(insertParticipantSql, new { Name = participant });
                        participantId = connection.QuerySingle<int>("SELECT LAST_INSERT_ID()");
                    }

                    string insertMeetingParticipantSql = "INSERT INTO MeetingParticipants (meeting_id, participant_id) VALUES (@MeetingId, @ParticipantId)";
                    connection.Execute(insertMeetingParticipantSql, new { MeetingId = meetingId, ParticipantId = participantId });
                }
            }
        }

        public void DeleteMeeting(int meetingId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                string deleteParticipantsSql = "DELETE FROM MeetingParticipants WHERE meeting_id = @MeetingId";
                connection.Execute(deleteParticipantsSql, new { MeetingId = meetingId });

                string deleteMeetingSql = "DELETE FROM Meetings WHERE meeting_id = @MeetingId";
                connection.Execute(deleteMeetingSql, new { MeetingId = meetingId });
            }
        }
    }
}
