using DocplannerTest.DTO;
using DocplannerTest.Model;
using DocplannerTest.Service;
using DocplannerTest.Utils;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Text.RegularExpressions;
using static DocplannerTest.DTO.WeeklyAvailabilityDTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DocplannerTest.Handlers
{
    public class WeekRequestHandler
    {
        IAvailabilityService SlotService { get; }
        IAvailabilityTimeDirector AvailabilityDirector { get; }

        Dictionary<string, List<Slot>> freeSlots = new();
        WeeklyAvailabilityDTO slotDTO;

        public WeekRequestHandler(IAvailabilityService slotService,
                                  IAvailabilityTimeDirector availabilityDirector) 
        {
            SlotService = slotService;
            AvailabilityDirector = availabilityDirector;
        }

        public async Task<Dictionary<string, List<Slot>>> Handle(string mondayDate)
        {
            freeSlots = new();

            if (!Regex.IsMatch(mondayDate, @"^\d{8}$"))
            {
                throw new ArgumentException("mondatDate format does not match the yyyyMMdd format");
            }

            DateOnly currentDay = DateOnly.ParseExact(mondayDate, "yyyyMMdd");

            if (currentDay.DayOfWeek != DayOfWeek.Monday)
            {
                throw new Exception("Request date is not Monady");
            }

            slotDTO = await SlotService.GetWeekly(mondayDate);

            if (slotDTO.Monday != null)
                ProcessDay(currentDay, slotDTO.Monday);

            currentDay = currentDay.AddDays(1);
            if (slotDTO.Tuesday != null)
                ProcessDay(currentDay, slotDTO.Tuesday);

            currentDay = currentDay.AddDays(1);
            if (slotDTO.Wednesday != null)
                ProcessDay(currentDay, slotDTO.Wednesday);

            currentDay = currentDay.AddDays(1);
            if (slotDTO.Thursday != null)
                ProcessDay(currentDay, slotDTO.Thursday);

            currentDay = currentDay.AddDays(1);
            if (slotDTO.Friday != null)
                ProcessDay(currentDay,slotDTO.Friday);

            currentDay = currentDay.AddDays(1);
            if (slotDTO.Saturday != null)
                ProcessDay(currentDay,slotDTO.Saturday);

            currentDay = currentDay.AddDays(1);
            if (slotDTO.Sunday != null)
                ProcessDay(currentDay,slotDTO.Sunday);

            return freeSlots;
        }

        private void ProcessDay(DateOnly day, DailyAvailabilityDTO dayDTO)
        {
            freeSlots[day.DayOfWeek.ToString()] = AvailabilityDirector.FreeSlotByAvailabilityData(day, dayDTO, slotDTO.SlotDurationMinutes);
        }
    }
}
