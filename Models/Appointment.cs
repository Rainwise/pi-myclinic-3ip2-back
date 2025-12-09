using System;
using System.Collections.Generic;

namespace myclinic_back.Models;

public partial class Appointment
{
    public int IdAppointment { get; set; }

    public int? DoctorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime StartsAt { get; set; }

    public virtual Doctor? Doctor { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
