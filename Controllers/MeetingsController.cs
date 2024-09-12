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
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _meetingRepository.CreateMeeting(meetingDto);
                return Ok(new { message = "Toplantı başarıyla kaydedildi." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Toplantı eklenirken bir hata oluştu.", error = ex.Message });
            }
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
            var meeting = _meetingRepository.GetMeetingById(id);
            if (meeting == null)
            {
                return NotFound(new { message = "Toplantı bulunamadı." });
            }

            // Veritabanında güncelleme işlemi
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
