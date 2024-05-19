using DocplannerTest.DTO;
using DocplannerTest.Handlers;
using DocplannerTest.Service;
using DocplannerTest.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DocplannerTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeeklyFreeSlotsController : ControllerBase
    {
        IAvailabilityService SlotService { get; }
        private readonly ILogger<WeeklyFreeSlotsController> _logger;

        public WeeklyFreeSlotsController(ILogger<WeeklyFreeSlotsController> logger, IAvailabilityService slotService)
        {
            _logger = logger;
            SlotService = slotService;
        }

        [HttpGet(Name = "GetWeeklyFreeSlot")]
        public async Task<IActionResult> GetWeekly(string mondayDate)
        {
            try
            {
                IAvailabilityTimeDirector director = new AvailabilityTimeDirector();
                WeekRequestHandler handler = new WeekRequestHandler(SlotService, director);
                var freeSlotsModel = await handler.Handle(mondayDate);

                return Ok(freeSlotsModel);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool IsDateTimeInFormat(string dateString, string format)
        {
            DateTime dateTime;
            return DateTime.TryParseExact(
                dateString,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dateTime);
        }

        //POST
        [HttpPost(Name = "TakeSlot")]
        public async Task<IActionResult> TakeSlot([FromBody] TakeSlotDTO slotData)
        {
            try
            {
                string format = "yyyy-MM-dd HH:mm:ss";

                if (!IsDateTimeInFormat(slotData.Start, format))
                {
                    throw new Exception("Start time has invalid format. Required: " + format);
                }

                if (!IsDateTimeInFormat(slotData.End, format))
                {
                    throw new Exception("End time has invalid format. Required: " + format);
                }

                await SlotService.TakeSlot(slotData);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
