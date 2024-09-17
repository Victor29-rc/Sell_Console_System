using Sell_Console_System.Contracts;
using Sell_Console_System.Entities;
using Sell_Console_System.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sell_Console_System.UI
{
    internal class ProductUI : IUinterface
    {
        private readonly Repository<Product> _repository;
        public ProductUI() 
        {
            _repository = new Repository<Product>(new Product());
        }

        public bool Init()
        {
            string input;
            int optionSelected;
            
            do
            {
                Console.Clear();
                Console.WriteLine("┌--------------------------------------------------------------┐");
                Console.WriteLine("|                         Products Menu                        |");
                Console.WriteLine("└--------------------------------------------------------------┘");
                Console.WriteLine("\n");

                Console.WriteLine("Available options.\n");

                Console.WriteLine("1) List products. ");
                Console.WriteLine("2) Add product. ");
                Console.WriteLine("3) Edit product. ");
                Console.WriteLine("4) Delete product. ");

                Console.WriteLine("\n5) Go back. ");

                Console.Write("\n- Please choose an option: ");

                input = Console.ReadLine();
                int.TryParse(input, out optionSelected);

                while (optionSelected != 1 && optionSelected != 2 && optionSelected != 3 && optionSelected != 4)
                {
                    if (optionSelected == 5)
                    {
                        return false;
                    }

                    Console.Write("\n- Please Choose a valid option (1, 2, 3, 4, 5): ");

                    input = Console.ReadLine();

                    int.TryParse(input, out optionSelected);
                }

            } while (GetOption(optionSelected));

            return true;
        }

        public bool GetOption(int option) 
        {
            switch(option)
            {
                case 1:
                    ListProducts();
                    break;
                case 2:
                    AddProduct();
                    break;
                case 3:
                    UpdateProduct();
                    break;
                case 4:
                    DeleteProduct();
                    break;
            }

            return true;
        }

        private void ListProducts()
        {
            Console.Clear();
            Console.WriteLine("┌--------------------------------------------------------------┐");
            Console.WriteLine("|                        Products List                         |");
            Console.WriteLine("└--------------------------------------------------------------┘");

            IResponse result = _repository.GetAll(["id", "name", "price", "description"]);
            int resultsCounter = 1;

            Console.WriteLine(result.ErrorMessage);

            foreach (var row in result.Results)
            {
                string productRow = "";

                foreach (var item in (IDictionary<string, object>)row)
                {
                    productRow += $"{item.Key}: {item.Value}, ";
                }

                productRow = productRow.Substring(0, productRow.Length - 2) + ".";

                Console.WriteLine(productRow);

                resultsCounter++;
            }

            Console.WriteLine("\nPress a key to go back.");
            Console.ReadKey();
        }

        private void AddProduct()
        {
            Console.Clear();
            Console.WriteLine("┌--------------------------------------------------------------┐");
            Console.WriteLine("|                          Add Product                         |");
            Console.WriteLine("└--------------------------------------------------------------┘");
            Console.WriteLine("\n");

            Dictionary<string, string> product = GetProductInput();

            IResponse result =  _repository.Add(product);

            DisplayMessage(result, "Product Added successfully", AddProduct);
        }

        private void UpdateProduct()
        {
            int id;
            bool productExists;

            Console.Clear();
            Console.WriteLine("┌--------------------------------------------------------------┐");
            Console.WriteLine("|                         Update Product                       |");
            Console.WriteLine("└--------------------------------------------------------------┘");
            Console.WriteLine("\n");

            do
            {
                Console.Write("- Enter the product ID: ");
                bool isIdValid = int.TryParse(Console.ReadLine(), out id);

                while (!isIdValid)
                {
                    Console.Write("\n- Enter a valid product ID: ");
                    isIdValid = int.TryParse(Console.ReadLine(), out id);
                }

                IResponse product = _repository.Get(id.ToString());

                if (product.HasError)
                {
                    DisplayMessage(product, "", UpdateProduct);
                }

                if(product.Results.Count == 0)
                {
                    productExists = false;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($" - There is no product with the ID: {id}, please try again");

                    Thread.Sleep(2000);

                    Console.ResetColor();
                    Console.Clear();
                }
                else
                {
                    productExists = true;
                }
            } 
            while (!productExists);

            Console.Clear();
            Dictionary<string, string> productUpdated = GetProductInput();

            IResponse result = _repository.Update(id.ToString(), productUpdated);

            DisplayMessage(result, "Product Updated successfully", UpdateProduct);
        }

        private void DeleteProduct()
        {
            int id;
            bool productExists;

            Console.Clear();
            Console.WriteLine("┌--------------------------------------------------------------┐");
            Console.WriteLine("|                         Delete Product                       |");
            Console.WriteLine("└--------------------------------------------------------------┘");
            Console.WriteLine("\n");

            do
            {
                Console.Write("- Enter the product ID: ");
                bool isIdValid = int.TryParse(Console.ReadLine(), out id);

                while (!isIdValid)
                {
                    Console.Write("\n- Enter a valid product ID: ");
                    isIdValid = int.TryParse(Console.ReadLine(), out id);
                }

                IResponse product = _repository.Get(id.ToString());

                if (product.HasError)
                {
                    DisplayMessage(product, "", DeleteProduct);
                }

                if (product.Results.Count == 0)
                {
                    productExists = false;

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" - There is no product with the ID: {id}, please try again");

                    Thread.Sleep(2000);

                    Console.ResetColor();
                    Console.Clear();
                } 
                else
                {
                    productExists = true;
                }
            }
            while (!productExists);

            IResponse result = _repository.Delete(id.ToString());

            DisplayMessage(result, "Product Deleted successfully", DeleteProduct);
        }

        private Dictionary<string, string> GetProductInput()
        {
            string name = "";
            string description = "";
            double price = 0;
            bool priceIsValid = false;

            Console.Write("Enter the product name: ");
            name = Console.ReadLine();

            Console.Write("Enter the product price: ");
            priceIsValid = double.TryParse(Console.ReadLine(), out price);

            while (!priceIsValid)
            {
                Console.Write("Please enter a valid price: ");
                priceIsValid = double.TryParse(Console.ReadLine(), out price);
            }

            Console.Write("Enter a product description (optional): ");
            description = Console.ReadLine();

            //TODO: ADD VALIDATION LOGIC

            Dictionary<string, string> product = new Dictionary<string, string>()
            {
                {"name", name},
                {"price", price.ToString()},
                {"description", description},
            };

            return product;
        }

        private void DisplayMessage(IResponse result, string successMessage, Action? action = null)
        {
            if (result.HasError)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.ErrorMessage);

                Console.ResetColor();
                Thread.Sleep(2000);

                action?.Invoke();
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(successMessage);

                Console.ResetColor();
                Thread.Sleep(2000);
            }
        }
    }


}
