using Xunit;
using Microsoft.EntityFrameworkCore;
using SPG_Fachtheorie.Aufgabe1.Model;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SPG_Fachtheorie.Aufgabe1.Test;

/// <summary>
/// Unittests für den DBContext.
/// Die Datenbank wird im Ordner SPG_Fachtheorie\SPG_Fachtheorie.Aufgabe1.Test\bin\Debug\net6.0\Invoice.db
/// erzeugt und kann mit SQLite Management Studio oder DBeaver betrachtet werden
/// </summary>
public class InvoiceContextTests
{
    /// <summary>
    /// Prüft, ob die Datenbank mit dem Model im InvoiceContext angelegt werden kann.
    /// </summary>
    [Fact]
    public void CreateDatabaseTest()
    {
        var options = new DbContextOptionsBuilder()
            .UseSqlite("Data Source=Invoice.db")
            .Options;

        using var db = new InvoiceContext(options);
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        Employee employee = new Employee()
        {
            VorName = "Matthias",
            Nachname = "Holzer"
        };
        db.Employees.Add(employee);

        Company company = new Company()
        {
            Name = "IVH",
            Address = "Spengergasse 5",
            EMail = "hol18849@spengergasse.at",
            PhoneNumber = "+43 6803337833"
        };
        db.Companys.Add(company);

        Article article1 = new Article()
        {
            Name = "4er-Schüler",
            Price = 0.5f
        };
        Article article2 = new Article()
        {
            Name = "3er-Schüler",
            Price = 50f
        };
        Article article3 = new Article()
        {
            Name = "2er-Schüler",
            Price = 500f
        };
        db.Articles.AddRange(article1, article2, article3);

        Customer customer = new Customer()
        {
            FirstName = "Martin",
            LastName = "Schrutek"
        };
        db.Customers.Add(customer);

        Invoice invoice = new Invoice()
        {
            Number = 001,
            Customer = customer,
            Clerk = employee,
            Discount = 0.05f,
            Date = System.DateTime.Now
        };
        db.Invoices.Add(invoice);

        List<InvoiceItem> invoiceItems = new List<InvoiceItem>()
        {
            new InvoiceItem()
            {
                Article = article1,
                Amount = 20,
                ArticlePrice = article1.Price,
                Invoice = invoice
            },
            new InvoiceItem(){
                Article = article2,
                Amount = 2,
                ArticlePrice = article2.Price,
                Invoice = invoice
            },
            new InvoiceItem()
            {
                Article = article3,
                Amount = 1,
                ArticlePrice = article3.Price,
                Invoice = invoice
            }
        };
        db.InvoiceItems.AddRange(invoiceItems);
        db.SaveChanges();
    }
}