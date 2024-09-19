using Sell_Console_System.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Console_System.UI
{
    internal static class Helper
    {
        public static void DisplayMessage(IResponse result, string successMessage, Action? action = null)
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
