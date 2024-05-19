﻿namespace DocplannerTest.DTO
{
    public class TakeSlotDTO
    {
        public string Start {  get; set; }
        public string End { get; set; } 
        public string Comments {  get; set; }
        public PatientDTO Patient { get; set; }

        public class PatientDTO
        {
            public string Name { get; set; }
            public string SecondName { get; set; }
            public string Email { get; set; }
            public string Phone {  get; set; }
        }
    }
}
