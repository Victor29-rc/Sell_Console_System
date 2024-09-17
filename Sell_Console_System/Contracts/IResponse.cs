using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Console_System.Contracts
{
    internal interface IResponse
    {
        public bool HasError { get; }
        public string ErrorMessage { get; }
        public List<object> Results { get; }
    }
}
