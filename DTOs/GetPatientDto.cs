namespace myclinic_back.DTOs
{
    public class GetPatientDto
    {
        public int IdPatient { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public bool IsActive { get; set; }

        public int HealthRecordId { get; set; }
    }
}
