namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Employee : EntityBase
    {
        public string VorName { get; set; }
        public string Nachname { get; set; }
        public List<Invoice> Invoices { get; set; }
    }
}
