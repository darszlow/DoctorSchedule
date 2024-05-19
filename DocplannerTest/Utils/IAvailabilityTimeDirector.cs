using DocplannerTest.Model;
using static DocplannerTest.DTO.WeeklyAvailabilityDTO;

namespace DocplannerTest.Utils
{
    public interface IAvailabilityTimeDirector
    {
        List<Slot> FreeSlotByAvailabilityData(DateOnly day, DailyAvailabilityDTO dayDTO, int slotDurationMinutes);
    }
}
