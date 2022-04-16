using SPG_Fachtheorie.Aufgabe2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPG_Fachtheorie.Aufgabe2
{
    public class AppointmentService
    {
        private readonly AppointmentContext _db;

        public AppointmentService(AppointmentContext db)
        {
            _db = db;
        }

        public bool AskForAppointment(Guid offerId, Guid studentId, DateTime date)
        {
            var offer = _db.Offers.FirstOrDefault(x => x.Id == offerId);    
            if (offer == null) return false;
            if (offer.From > date || offer.To < date) return false;

            _db.Appointments.Add(new Appointment()
            {
                Id = Guid.NewGuid(),
                Offer = offer,
                Student = _db.Students.FirstOrDefault(x => x.Id == studentId),
                Date = date,
                Location = "Pausenraum B1",
                State =AppointmentState.AskedFor
            });
            return _db.SaveChanges() == 1;
        }

        public bool ConfirmAppointment(Guid appointmentId)
        {
            var appointment = _db.Appointments.FirstOrDefault(x => x.Id == appointmentId);
            if (appointment == null) return false;
            if (appointment.State != AppointmentState.AskedFor) return false;

            appointment.State=AppointmentState.AskedFor;
            _db.Appointments.Update(appointment);
            return _db.SaveChanges() == 1;
        }

        public bool CancelAppointment(Guid appointmentId, Guid studentId)
        {
            Appointment appointment = _db.Appointments.FirstOrDefault(x => x.Id == appointmentId);
            if (appointment == null) return false;

            Student student = _db.Students.FirstOrDefault(x => x.Id == studentId);
            if (student == null) return false;

            if (appointment.State == AppointmentState.AskedFor || appointment.State == AppointmentState.Confirmed && student.Id == appointment.OfferId)
            {
                appointment.State = AppointmentState.Cancelled;
                _db.Appointments.Update(appointment);
                return _db.SaveChanges() == 1;
            }
            return false;
        }
    }
}
