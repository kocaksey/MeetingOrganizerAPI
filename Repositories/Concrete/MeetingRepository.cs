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
        public IEnumerable<MeetingDto> GetAllMeetings()
        {
            var query = "SELECT m.meeting_id AS MeetingId, m.title, m.date, m.start_time AS StartTime, m.end_time AS EndTime, p.name " +
                        "FROM Meetings m " +
                        "JOIN MeetingParticipants mp ON m.meeting_id = mp.meeting_id " +
                        "JOIN Participants p ON mp.participant_id = p.participant_id";

            using (var connection = new MySqlConnection(_connectionString))
            {
                var meetingDict = new Dictionary<int, MeetingDto>();

                connection.Query<MeetingDto, string, MeetingDto>(
                    query,
                    (meeting, participant) =>
                    {
                        if (!meetingDict.TryGetValue(meeting.MeetingId, out var currentMeeting))
                        {
                            currentMeeting = meeting;
                            currentMeeting.Participants = new List<string>();
                            meetingDict[meeting.MeetingId] = currentMeeting;
                        }

                        currentMeeting.Participants.Add(participant);
                        return currentMeeting;
                    },
                    splitOn: "name"  // Katılımcıların name sütununa göre ayırıyoruz
                );

                return meetingDict.Values.ToList();
            }
        }





        public void CreateMeeting(MeetingDto meetingDto)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                // DTO'dan verileri alıyoruz, TimeSpan'e dönüştürmeye gerek yok
                TimeSpan startTimeSpan = meetingDto.StartTime;
                TimeSpan endTimeSpan = meetingDto.EndTime;

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
