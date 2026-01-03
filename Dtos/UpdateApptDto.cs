namespace myclinic_back.Dtos
{
    public class UpdateApptDto
    {
        public int IdAppointment { get; set; }

        public int? DoctorId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime StartsAt { get; set; }
    }
}
