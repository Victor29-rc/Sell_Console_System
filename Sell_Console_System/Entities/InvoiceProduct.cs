using Sell_Console_System.Contracts;
using Sell_Console_System.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Console_System.Entities
{
    internal class InvoiceProduct : IIdentifier
    {
        public string Table { get; }
        public string Id { get; }
        public InvoiceProduct() 
        {
            Id = "id";
            Table = "product_invoice";
        }

        public static string GetInvoiceDetailsQuery(string invoiceId)
        {
            return 
                $@" SELECT b.name AS Product, b.price AS Price, a.quantity AS Quantity, a.total AS Total
                    FROM product_invoice AS a 
                    JOIN product AS b ON b.id = a.productId 
                    WHERE a.invoiceId = {invoiceId}
                ";
        }
    }
}
