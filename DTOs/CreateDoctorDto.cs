namespace myclinic_back.DTOs
{
    public class CreateDoctorDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public int SpecializationId { get; set; }

        public string LicenseNumber { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
