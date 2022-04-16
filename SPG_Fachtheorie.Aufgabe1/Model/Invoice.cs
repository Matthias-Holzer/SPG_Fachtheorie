using System.ComponentModel.DataAnnotations.Schema;

namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Invoice : EntityBase
    {
        public Customer Customer { get; set; }
        public Guid CustomerId { get; set; }
        public Employee Clerk { get; set; }
        public Guid ClerkId { get; set; }
        public float Discount { get; set; }
        public DateTime Date { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}
