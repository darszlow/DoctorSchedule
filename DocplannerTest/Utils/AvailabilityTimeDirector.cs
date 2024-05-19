using DocplannerTest.Model;
using static DocplannerTest.DTO.FreeSlotsDTO;
using static DocplannerTest.DTO.WeeklyAvailabilityDTO;

namespace DocplannerTest.Utils
{
    public class AvailabilityTimeDirector : IAvailabilityTimeDirector
    {
        public AvailabilityTimeDirector()
        {

        }

        public List<Slot> FreeSlotByAvailabilityData(DateOnly day, 
                                               DailyAvailabilityDTO dayDTO, 
                                               int slotDurationMinutes )
        {
            IAvailabilityTimeBuilder Builder = new AvailabilityTimeBuilder();

            Builder.Clear();
            Builder.SetDay(day).SetSlotDuration(slotDurationMinutes);

            if (dayDTO.WorkPeriod == null)
            {
                throw new Exception("Work period is null for day: " + day.ToString());
            }
            Builder.SetBeginHour(dayDTO.WorkPeriod.StartHour);
            Builder.SetEndHour(dayDTO.WorkPeriod.EndHour);

            var lunchBreakStart = new DateTime(day.Year, day.Month, day.Day, dayDTO.WorkPeriod.LunchStartHour, 0, 0);
            var lunchBreakStop = new DateTime(day.Year, day.Month, day.Day, dayDTO.WorkPeriod.LunchEndHour, 0, 0);

            Builder.AddBusyTime(lunchBreakStart, lunchBreakStop);

            if (dayDTO.BusySlots != null)
            {
                foreach (var slot in dayDTO.BusySlots)
                {
                    Builder.AddBusyTime(slot.Start, slot.End);
                }
            }

            return Builder.Build();
        }
    }
}
