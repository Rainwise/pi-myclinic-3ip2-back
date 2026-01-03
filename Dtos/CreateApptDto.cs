namespace myclinic_back.Dtos
{
    public class CreateApptDto
    {
        public int? DoctorId { get; set; }

        public DateTime StartsAt { get; set; }
    }
}
