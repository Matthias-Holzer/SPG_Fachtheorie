using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public enum Salutation { Mr, Mrs}
    public class Customer : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public Salutation Salutation { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}
