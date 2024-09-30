using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{

    [Flags]
    public enum WeekDays : byte
    {
        Saturday = 1, Sunday = 2, Monday = 4, Tuesday = 8, Wednesday = 16, Thursday = 32, Friday = 64
    }

    [Table("Doctors")]
    public class Doctor : Person
    {
        public WeekDays WorkingDays { get; set; }

        public TimeOnly StartTime { get; set; }

        public TimeOnly EndTime { get; set; }


        public int SpecializationId { get; set; }

        [ForeignKey("SpecializationId")]
        public Specialization Specialization { get; set; }

        public float Salary { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}
