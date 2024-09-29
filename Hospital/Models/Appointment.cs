using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }

        [Required]
        public string Specialization { get; set; }
        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public Patient Patient { get; set; }

        public string Diagnosis { get; set; }   //تشخيص
        public string Treatment { get; set; }
        public DateTime Date { get; set; }
        public float Price { get; set; }

    }
}
