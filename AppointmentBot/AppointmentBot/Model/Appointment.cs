using System;

namespace AppointmentBot.Model
{
    [Serializable]
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public DateTime Datetime { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public TimeSpan Time { get; set; }
    }
}