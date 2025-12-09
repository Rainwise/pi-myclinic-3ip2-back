namespace myclinic_back.Dtos
{
    public class GetAccountDto
    {
        public int IdAccount { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string EmailAddres { get; set; } = null!;

        public string Specialization { get; set; }
    }
}
