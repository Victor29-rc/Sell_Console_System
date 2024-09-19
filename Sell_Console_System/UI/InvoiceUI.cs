using Sell_Console_System.Contracts;
using Sell_Console_System.Entities;
using Sell_Console_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Console_System.UI
{
    internal class InvoiceUI
    {
        private readonly Repository<Invoice> _invoiceRepository;
        private readonly Repository<InvoiceProduct> _invoiceProductRepository;
        public InvoiceUI()
        {
            _invoiceRepository = new Repository<Invoice>(new Invoice());
            _invoiceProductRepository = new Repository<InvoiceProduct>(new InvoiceProduct());
        }

        public bool Init()
        {
            string input;
            int optionSelected;

            do
            {
                Console.Clear();
                Console.WriteLine("┌--------------------------------------------------------------┐");
                Console.WriteLine("|                          Invoice Menu                        |");
                Console.WriteLine("└--------------------------------------------------------------┘");
                Console.WriteLine("\n");

                Console.WriteLine("Available options.\n");

                Console.WriteLine("1) List invoices. ");
                Console.WriteLine("2) Add invoice. ");
                Console.WriteLine("3) Delete invoice. ");

                Console.WriteLine("\n5) Go back. ");

                Console.Write("\n- Please choose an option: ");

                input = Console.ReadLine();
                int.TryParse(input, out optionSelected);

                while (optionSelected != 1 && optionSelected != 2 && optionSelected != 3)
                {
                    if (optionSelected == 5)
                    {
                        return false;
                    }

                    Console.Write("\n- Please Choose a valid option (1, 2, 3, 5): ");

                    input = Console.ReadLine();

                    int.TryParse(input, out optionSelected);
                }

            } while (GetOption(optionSelected));

            return true;
        }

        public bool GetOption(int option)
        {
            switch (option)
            {
                case 1:
                    ListInvoices();
                    break;
                case 2:
                    AddInvoice();
                    break;
                case 3:
                    DeleteInvoice();
                    break;
            }

            return true;
        }

        public void ListInvoices()
        {
            Console.Clear();
            Console.WriteLine("┌--------------------------------------------------------------┐");
            Console.WriteLine("|                         Invoices List                        |");
            Console.WriteLine("└--------------------------------------------------------------┘");
            Console.WriteLine("\n");

            IResponse response = _invoiceRepository.GetAll(["id AS 'Invoice ID', customer AS Customer, id, total AS Total"]);
            List<object> invoices = response.Results;

            foreach (object invoice in invoices)
            {
                foreach (KeyValuePair<string, object> invoiceData in (IDictionary<string, object>)invoice)
                {
                    if(invoiceData.Key != "id") {

                        if(invoiceData.Key == "Invoice ID")
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine($"{invoiceData.Key}: {invoiceData.Value.ToString()}\n");
                            Console.ResetColor();
                        }
                        else if(invoiceData.Key == "Total")
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"\n\n{invoiceData.Key}: {invoiceData.Value.ToString()}\n");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"{invoiceData.Key}: {invoiceData.Value.ToString()}\n");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Product details:\n");
                        DisplayItemsByInvoice(invoiceData.Value.ToString());
                    }
                }

                Console.WriteLine("____________________________________________________________________________\n");
            }

            Console.WriteLine("\nPress a key to go back.");
            Console.ReadKey();
        }

        public bool AddInvoice()
        {
            string customerName;
            int optionSelected;
            bool selectedOptionIsNumeric;
            bool userIsEditingInvoice = true;

            List<ProductRow> productsAdded = new List<ProductRow>();

            Console.Clear();
            Console.WriteLine("┌--------------------------------------------------------------┐");
            Console.WriteLine("|                          Add Invoice                         |");
            Console.WriteLine("└--------------------------------------------------------------┘");
            Console.WriteLine("\n");

            Console.Write("- Enter the customer name (DEFAULT => Consumer customer): ");
            customerName = Console.ReadLine();

            if (String.IsNullOrEmpty(customerName))
            {
                customerName = "Consumer customer.";
            }

            do
            {
                Console.Clear();
                Console.WriteLine("┌--------------------------------------------------------------┐");
                Console.WriteLine("|                          Add Invoice                         |");
                Console.WriteLine("└--------------------------------------------------------------┘");

                Console.Write($"\nCustomer: ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"{customerName}");

                Console.ResetColor();
                Console.Write($"\t\t Date: ");

                Console.ForegroundColor= ConsoleColor.Yellow;
                Console.WriteLine(DateTime.Now.ToShortDateString());
                Console.ResetColor();

                double totalInvoice = 0;

                if (productsAdded.Count > 0)
                {
                    Console.WriteLine("\n\nProducts added to the invoice:\n");
                     
                    foreach (ProductRow productAdded in productsAdded)
                    {
                        totalInvoice += productAdded.CreateProductAddedRow();
                    }

                    Console.Write("\t\t\t\tTotal invoice amount: ");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("$"+totalInvoice);
                    Console.ResetColor();
                }

                Console.WriteLine("\nOptions: \n");

                Console.WriteLine("1) Add product to the invoice.");
                Console.WriteLine("2) Save invoice.");
                Console.WriteLine("3) Exit without save.");

                Console.Write("\n Choose an option: ");
                selectedOptionIsNumeric = int.TryParse(Console.ReadLine(), out optionSelected);

                while (!selectedOptionIsNumeric || (optionSelected != 1 && optionSelected != 2))
                {
                    if (optionSelected == 3) //this means to exit
                    {
                        return true;
                    }

                    Console.Write("- Please choose a valid option: ");
                    selectedOptionIsNumeric = int.TryParse(Console.ReadLine(), out optionSelected);
                }

                Repository<Product> productRepository = new Repository<Product>(new Product());

                if (optionSelected == 1) //if the user wants to add a new product
                {
                    int productId, quantity;
                    bool productIdEnteredIsNumber;
                    bool quantityIsValid;

                    Console.Clear();
                    Console.WriteLine("┌--------------------------------------------------------------┐");
                    Console.WriteLine("|                     Add Product to Invoice                   |");
                    Console.WriteLine("└--------------------------------------------------------------┘");
                    Console.WriteLine("\n");


                    Console.Write("- Enter the product ID: ");
                    productIdEnteredIsNumber = int.TryParse(Console.ReadLine(), out productId);

                    while (!productIdEnteredIsNumber)
                    {
                        Console.Write("- Enter a valid product ID: ");
                        productIdEnteredIsNumber = int.TryParse(Console.ReadLine(), out productId);
                    }

                    IResponse response = productRepository.Get(productId.ToString(), ["id", "name", "price"]);

                    while (!productIdEnteredIsNumber || response.HasError)
                    {

                        if (!productIdEnteredIsNumber)
                        {
                            Console.WriteLine("- Enter a valid product ID: ");
                            productIdEnteredIsNumber = int.TryParse(Console.ReadLine(), out productId);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(response.ErrorMessage + ". Please try again. \n");
                            Console.ResetColor();

                            Console.Write("- Enter the product ID: ");
                            productIdEnteredIsNumber = int.TryParse(Console.ReadLine(), out productId);
                        }

                        if(productIdEnteredIsNumber)
                        {
                            response = productRepository.Get(productId.ToString(), ["id", "name", "price"]);
                        }
                    }

                    Console.Write("- Enter the quantity: ");
                    quantityIsValid = int.TryParse(Console.ReadLine(), out quantity);

                    while (!quantityIsValid)
                    {
                        Console.Write("- Enter a valid product ID: ");
                        quantityIsValid = int.TryParse(Console.ReadLine(), out quantity);
                    }

                    IDictionary<string, object> product = (IDictionary<string, object>)response.Results[0];

                    productsAdded.Add(new ProductRow(product, quantity));
                }

                if(optionSelected == 2)
                {
                    if(productsAdded.Count == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n - Error: You can not save an invoice without items");
                        Console.ResetColor();
                        Thread.Sleep(1500);
                    }
                    else
                    {
                        if(totalInvoice <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n - Error: The invoice total can not be 0 or less");
                            Console.ResetColor();
                            Thread.Sleep(1500);
                        }
                        else
                        {
                            Dictionary<string, string> invoice = new Dictionary<string, string>()
                            {
                                {"customer", customerName},
                                {"subtotal", totalInvoice.ToString()},
                                {"total", totalInvoice.ToString()},
                            };

                            IResponse insertInvoiceResult = _invoiceRepository.Add(invoice);

                            if(!insertInvoiceResult.HasError)
                            {
                                int lasInsertedInvoiceId = _invoiceRepository.GetLastInsertedId();
                                
                                if(lasInsertedInvoiceId != 0)
                                {
                                    foreach (ProductRow product in productsAdded)
                                    {
                                        _invoiceProductRepository.Add(new Dictionary<string, string>()
                                        {
                                            {"invoiceId", lasInsertedInvoiceId.ToString()},
                                            {"productId", product.Id.ToString()},
                                            {"quantity", product.Quantity.ToString()},
                                            {"total", product.Total.ToString()},
                                        });
                                    }

                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("\nInvoice inserted successfully");
                                    Console.ResetColor();
                                    Thread.Sleep(2000);

                                    userIsEditingInvoice = false;
                                }
                            }
                        }
                    }
                }

            } while(userIsEditingInvoice);
             
            return true;
        }

        public void DeleteInvoice()
        {
            int id;
            bool invoiceExists;

            Console.Clear();
            Console.WriteLine("┌--------------------------------------------------------------┐");
            Console.WriteLine("|                         Delete Invoice                       |");
            Console.WriteLine("└--------------------------------------------------------------┘");
            Console.WriteLine("\n");

            do
            {
                Console.Write("- Enter the invoice ID: ");
                bool isIdValid = int.TryParse(Console.ReadLine(), out id);

                while (!isIdValid)
                {
                    Console.Write("\n- Enter a valid invoice ID: ");
                    isIdValid = int.TryParse(Console.ReadLine(), out id);
                }

                IResponse invoice = _invoiceRepository.Get(id.ToString());

                if (invoice.HasError)
                {
                    invoiceExists = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(invoice.ErrorMessage);

                    if (invoice.Results.Count == 0)
                    {
                        Console.WriteLine($" - There is no invoice with the ID: {id}, please try again");
                    }

                    Thread.Sleep(2000);

                    Console.ResetColor();
                    Console.Clear();
                }
                else
                {
                    invoiceExists = true;
                }
            }
            while (!invoiceExists);

            IResponse result = _invoiceRepository.Delete(id.ToString());

            Helper.DisplayMessage(result, "Product Deleted successfully", DeleteInvoice);
        }

        public void DisplayItemsByInvoice(string invoiceId)
        {
            string itemDetailsRow;

            IResponse details = _invoiceProductRepository.Select(InvoiceProduct.GetInvoiceDetailsQuery(invoiceId));
            List<object> items = details.Results;

            foreach (var item in items)
            {
                itemDetailsRow = "\t";

                foreach (KeyValuePair<string, object> product in (IDictionary<string, object>)item)
                {
                    itemDetailsRow += $"{product.Key}: {product.Value}, ";
                }

                itemDetailsRow = itemDetailsRow.Substring(0, itemDetailsRow.Length - 2) + ".";

                Console.WriteLine(itemDetailsRow);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.ResetColor();
            }
        }
    }

    internal struct ProductRow
    {
        IDictionary<string, object> Product { get; set;  }
        public int Quantity { get; set; }
        public readonly int Id
        {
            get
            {
                foreach (KeyValuePair<string, object> item in Product)
                {
                    if (item.Key == new Product().Id)
                    {
                        return int.Parse(item.Value.ToString());
                    }
                }

                return 0;
            }
        }
        public readonly double Total {  
            get   
            {
                foreach (KeyValuePair<string, object> item in Product)
                {
                    if (item.Key == "price")
                    {
                        string value = item.Value.ToString();

                        return double.Parse(value) * Quantity;
                    }
                }

                return 0;
            }
        }

        public ProductRow(IDictionary<string, object> product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public double CreateProductAddedRow()
        {
            double total = 0;
            string productAddedRow = "";

            foreach (KeyValuePair<string, object> item in Product)
            {
                productAddedRow += $"{UCFirst(item.Key.ToString())}: {item.Value}, ";

                if (item.Key == "price")
                {
                    string value = item.Value.ToString();

                    total = double.Parse(value) * Quantity;

                    productAddedRow += $"Quantity: {Quantity}, ";
                    productAddedRow += $"Total: ${total}, ";
                }
            }

            productAddedRow = productAddedRow.Substring(0, productAddedRow.Length - 2) + ".";
            Console.WriteLine(productAddedRow);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("__________________________________________________________\n");
            Console.ResetColor();
            return total;
        }

        private string UCFirst(string str)
        {
            return str[0].ToString().ToUpper() + str.Substring(1, str.Length - 1);
        }
    }
}
