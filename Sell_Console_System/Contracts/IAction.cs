using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Console_System.Contracts
{
    internal interface IAction
    {
        IResponse Get(string id);
        IResponse GetAll();
        IResponse Add(Dictionary<string, string> data);
        IResponse Update(string id, Dictionary<string, string> data);
        IResponse Delete(string data);
    }
}
