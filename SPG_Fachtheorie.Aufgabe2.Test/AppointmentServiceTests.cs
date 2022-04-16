using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using SPG_Fachtheorie.Aufgabe2;
using Microsoft.EntityFrameworkCore;

namespace SPG_Fachtheorie.Aufgabe2.Test
{
    [Collection("Sequential")]
    public class AppointmentServiceTests
    {
        /// <summary>
        /// Legt die Datenbank an und befüllt sie mit Musterdaten. Die Datenbank ist
        /// nach Ausführen des Tests ServiceClassSuccessTest in
        /// SPG_Fachtheorie\SPG_Fachtheorie.Aufgabe2.Test\bin\Debug\net6.0\Appointment.db
        /// und kann mit SQLite Manager, DBeaver, ... betrachtet werden.
        /// </summary>
        private AppointmentContext GetAppointmentContext()
        {
            var options = new DbContextOptionsBuilder()
                .UseSqlite("Data Source=Appointment.db")
                .Options;

            var db = new AppointmentContext(options);
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
            db.Seed();
            return db;
        }
        [Fact]
        public void ServiceClassSuccessTest()
        {
            using var db = GetAppointmentContext();
            Assert.True(db.Students.Count() > 0);
            var service = new AppointmentService(db);
        }
        [Fact]
        public void AskForAppointmentSuccessTest()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var offer = db.Offers.FirstOrDefault();
            var student = db.Students.FirstOrDefault();
            var datetime = offer.From;

            Assert.True(service.AskForAppointment(offer.Id, student.Id, datetime.AddSeconds((offer.To - offer.From).TotalSeconds/2)));
        }
        [Fact]
        public void AskForAppointmentReturnsFalseIfNoOfferExists()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var student = db.Students.FirstOrDefault();

            Assert.False(service.AskForAppointment(Guid.NewGuid(), student.Id, DateTime.Now));
        }
        [Fact]
        public void AskForAppointmentReturnsFalseIfOutOfDate()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var student = db.Students.FirstOrDefault();
            var offer = db.Offers.FirstOrDefault();

            Assert.False(service.AskForAppointment(offer.Id, student.Id, offer.To.AddSeconds(1)));
            Assert.False(service.AskForAppointment(offer.Id, student.Id, offer.From.AddSeconds(-1)));
        }
        [Fact]
        public void ConfirmAppointmentSuccessTest()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.FirstOrDefault(x => x.State == Model.AppointmentState.AskedFor);

            Assert.True(service.ConfirmAppointment(appointment.Id));
        }
        [Fact]
        public void ConfirmAppointmentReturnsFalseIfStateIsInvalid()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.FirstOrDefault(x => x.State == Model.AppointmentState.TookPlace);

            Assert.False(service.ConfirmAppointment(appointment.Id));
        }
        [Fact]
        public void CancelAppointmentStudentSuccessTest()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.FirstOrDefault(x => x.State == Model.AppointmentState.AskedFor);

            Assert.True(service.CancelAppointment(appointment.Id, appointment.Offer.Teacher.Id));
        }
        [Fact]
        public void CancelAppointmentCoachSuccessTest()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.FirstOrDefault(x => x.State == Model.AppointmentState.AskedFor);

            Assert.True(service.CancelAppointment(appointment.Id, appointment.Offer.Teacher.Id));
        }
        [Fact]
        public void ConfirmAppointmentStudentReturnsFalseIfStateIsInvalid()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.FirstOrDefault(x => x.State == Model.AppointmentState.Confirmed);

            Assert.False(service.CancelAppointment(appointment.Id, appointment.StudentId));
        }
        [Fact]
        public void ConfirmAppointmentCoachReturnsFalseIfStateIsInvalid()
        {
            using var db = GetAppointmentContext();
            var service = new AppointmentService(db);
            var appointment = db.Appointments.FirstOrDefault(x => x.State == Model.AppointmentState.TookPlace);

            Assert.False(service.CancelAppointment(appointment.Id, appointment.Offer.Id));
        }
    }
}
