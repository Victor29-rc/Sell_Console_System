using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Console_System.UI
{
    internal class App
    {
        private readonly ProductUI productUI;
        private readonly InvoiceUI invoiceUI;

        public App() 
        {
            productUI = new ProductUI();
            invoiceUI = new InvoiceUI();
        }
        public void Init()
        {
            bool stopApp = false;

            do
            {
                string input;
                int option;

                Console.Clear();

                Console.WriteLine("┌--------------------------------------------------------------┐");
                Console.WriteLine("|                          SELL SYSTEM                         |");
                Console.WriteLine("└--------------------------------------------------------------┘");
                Console.WriteLine("\n");

                Console.WriteLine("Available options.\n");

                Console.WriteLine("1) Products. ");
                Console.WriteLine("2) Invoices. ");
                Console.WriteLine("3) Exit. ");

                Console.Write("\n- Please choose an option: ");
                input = Console.ReadLine();

                int.TryParse(input, out option);

                while (option != 1 && option != 2 && option != 3)
                {
                    Console.Write("\n- Please Choose a valid option (1, 2, 3): ");

                    input = Console.ReadLine();

                    int.TryParse(input, out option);
                }

                Console.Clear();

                switch (option)
                {
                    case 1:
                        stopApp = productUI.Init();
                        break;
                    case 2:
                        stopApp = invoiceUI.Init();
                        break;
                    case 3:
                        stopApp = true;
                        break;
                    default:
                        break;

                }
            } while (!stopApp);
        }

     
    }
}
