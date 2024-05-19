using DocplannerTest.Model;
using System;
using System.Security.Cryptography;

namespace DocplannerTest.Utils
{
    public class AvailabilityTimeBuilder : IAvailabilityTimeBuilder
    {
        private DateOnly day;
        private bool isDaySet; 

        private int startHour; 
        private int endHour;
        private int slotDurationMinutes;
        List<Slot> busySlots = new List<Slot>();

        public AvailabilityTimeBuilder()
        {
            Clear();
        }

        
        public IAvailabilityTimeBuilder Clear()
        {
            isDaySet = false;
            startHour = -1;
            endHour = -1;
            slotDurationMinutes = -1;
            busySlots = new List<Slot>();

            return this;
        }

        public IAvailabilityTimeBuilder SetDay(DateOnly _day)
        {
            day = _day;
            isDaySet = true;

            return this;
        }

        public IAvailabilityTimeBuilder SetBeginHour(int _startHour)
        {
            startHour = _startHour;
            return this;
        }

        public IAvailabilityTimeBuilder SetEndHour(int _endHour)
        {
            endHour = _endHour;
            return this;
        }

        public IAvailabilityTimeBuilder SetSlotDuration(int _slotDurationMinutes)
        {
            slotDurationMinutes = _slotDurationMinutes;
            return this;
        }

        public IAvailabilityTimeBuilder AddBusyTime(DateTime busyStart, DateTime busyStop)
        {
            Slot span = new()
            {
                Start = busyStart,
                Stop = busyStop
            };

            busySlots.Add(span);
            return this;
        }

        public List<Slot> Build()
        {
            CheckFields();

            List<Slot> freeSlots = new List<Slot>();

            DateTime beginSlot = new DateTime(day.Year, day.Month, day.Day, startHour, 0, 0);
            DateTime endSlot = beginSlot.AddMinutes(slotDurationMinutes);

            DateTime endWorkPeriod = new DateTime(day.Year, day.Month, day.Day, endHour, 0, 0);

            while(endSlot <= endWorkPeriod)
            {
                Slot slot = new()
                {
                    Start = beginSlot,
                    Stop = endSlot
                };
                
                if(IsNotBusySlot(slot))
                {
                    freeSlots.Add(slot);
                }

                beginSlot = beginSlot.AddMinutes(slotDurationMinutes);
                endSlot = endSlot.AddMinutes(slotDurationMinutes);
            }

            Clear();
            return freeSlots;
        }

        private void CheckFields()
        {
            if (!isDaySet || startHour < 0 || endHour < 0 || slotDurationMinutes < 0)
            {
                throw new Exception("Not all required fields are filled in");
            }
        }

        private bool IsNotBusySlot(Slot newSlot)
        {
            foreach(var element in busySlots)
            {
                if(IsStartInRange(newSlot.Start, element) || IsStopInRange(newSlot.Stop, element))
                {
                    return false;
                }
            }

            return true;
        }

        private bool IsStartInRange(DateTime timestamp, Slot element)
        {
            return timestamp < element.Stop && timestamp >= element.Start;
        }

        private bool IsStopInRange(DateTime timestamp, Slot element)
        {
            return timestamp <= element.Stop && timestamp > element.Start;
        }
    }
}
