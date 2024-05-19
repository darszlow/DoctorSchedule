using DocplannerTest.DTO;

namespace DocplannerTest.Service
{
    public interface IAvailabilityService
    {
        Task<WeeklyAvailabilityDTO> GetWeekly(string dateStr);
        Task TakeSlot(TakeSlotDTO slotData);
    }
}
