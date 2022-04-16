namespace SPG_Fachtheorie.Aufgabe1.Model
{
    public class Article : EntityBase
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public float Price { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
    }
}
