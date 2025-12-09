namespace myclinic_back.Dtos
{
    public class CreateAccountDto
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public int? RoleId { get; set; }

        public string EmailAddres { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int SpecializationId { get; set; }

    }
}
