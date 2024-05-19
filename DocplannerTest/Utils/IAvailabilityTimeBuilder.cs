using DocplannerTest.Model;

namespace DocplannerTest.Utils
{
    public interface IAvailabilityTimeBuilder
    {
        IAvailabilityTimeBuilder Clear();
        IAvailabilityTimeBuilder SetDay(DateOnly _day);
        IAvailabilityTimeBuilder SetBeginHour(int _startHour);
        IAvailabilityTimeBuilder SetEndHour(int _endHour);
        IAvailabilityTimeBuilder SetSlotDuration(int _slotDurationMinutes);
        IAvailabilityTimeBuilder AddBusyTime(DateTime busyStart, DateTime busyStop);
        List<Slot> Build();
    }
}
