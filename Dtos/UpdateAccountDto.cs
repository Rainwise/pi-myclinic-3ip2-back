namespace myclinic_back.Dtos
{
    public class UpdateAccountDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string EmailAddres { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string Specialization { get; set; }
    }
}
