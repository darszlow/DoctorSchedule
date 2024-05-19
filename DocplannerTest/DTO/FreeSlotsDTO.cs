namespace DocplannerTest.DTO
{
    public class FreeSlotsDTO
    {
        public class SlotDTO
        {
            public DateTime Start { get; set; }
            public DateTime Stop { get; set; }
        }

        public Dictionary<string, SlotDTO> Slots { get; set; }
    }
}
