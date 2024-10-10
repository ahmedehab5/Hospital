using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        [ForeignKey("Doctor")]
        public string DoctorId { get; set; }
        public Doctor? Doctor { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public string PatientId { get; set; }
        public Patient? Patient { get; set; }

        [Required]
        public string Specialization { get; set; }

        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }

        public DateTime Date { get; set; }

    }
}
