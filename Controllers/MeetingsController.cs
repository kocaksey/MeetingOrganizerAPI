using MeetingOrganizer.DTOs;
using MeetingOrganizer.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace MeetingOrganizer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeetingsController : Controller
    {
        private readonly IMeetingRepository _meetingRepository;

        public MeetingsController(IMeetingRepository meetingRepository)
        {
            _meetingRepository = meetingRepository;
        }

        [HttpPost("create")]
        public IActionResult CreateMeeting([FromBody] MeetingDto meetingDto)
        {
            _meetingRepository.CreateMeeting(meetingDto);
            return Ok(new { message = "Toplantı başarıyla kaydedildi." });
        }

        [HttpGet("list")]
        public IActionResult GetMeetings()
        {
            var meetings = _meetingRepository.GetAllMeetings();
            return Ok(meetings);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetMeetingById(int id)
        {
            var meeting = _meetingRepository.GetMeetingById(id);
            if (meeting == null)
            {
                return NotFound();
            }
            return Ok(meeting);
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdateMeeting(int id, [FromBody] MeetingDto meetingDto)
        {
            _meetingRepository.UpdateMeeting(id, meetingDto);
            return Ok(new { message = "Toplantı başarıyla güncellendi." });
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteMeeting(int id)
        {
            _meetingRepository.DeleteMeeting(id);
            return Ok(new { message = "Toplantı başarıyla silindi." });
        }

    }
}
