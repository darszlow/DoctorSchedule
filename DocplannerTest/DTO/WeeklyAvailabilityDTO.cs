namespace DocplannerTest.DTO
{
    public class WeeklyAvailabilityDTO
    {
        public class FacilityDTO
        {
            public string Name { get; set; } = string.Empty;
            public string Address { get; set; } = string.Empty;
        }

        public class DailyAvailabilityDTO
        {
            public class WorkPeriodDTO
            {
                public int StartHour { get; set; }
                public int EndHour { get; set; }
                public int LunchStartHour { get; set; }
                public int LunchEndHour { get; set; }
            }

            public class BusySlot
            {
                public DateTime Start { get; set; }
                public DateTime End { get; set; }
            }

            public WorkPeriodDTO? WorkPeriod { get; set; }
            public List<BusySlot>? BusySlots { get; set; }
        }

        public FacilityDTO? Facility { get; set; }
        public int SlotDurationMinutes { get; set; } 
        public DailyAvailabilityDTO? Monday { get; set; }
        public DailyAvailabilityDTO? Tuesday { get; set; }
        public DailyAvailabilityDTO? Wednesday { get; set; }
        public DailyAvailabilityDTO? Thursday { get; set; }
        public DailyAvailabilityDTO? Friday { get; set; }
        public DailyAvailabilityDTO? Saturday { get; set; }
        public DailyAvailabilityDTO? Sunday { get; set; }
    }
}
