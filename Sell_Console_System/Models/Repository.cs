using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sell_Console_System.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sell_Console_System.Models
{
    internal class Repository<TEntity> : IAction  where TEntity : IIdentifier
    {
        private TEntity _entity;
        private DBModel _dbModel;
        public Repository(TEntity entity)
        {
            _entity = entity;
            _dbModel = new DBModel();
        }
        public IResponse Add(Dictionary<string, string> data)
        {
            IResponse result = _dbModel.Insert(_entity, data);

            return result;
        }

        public IResponse Update(string id, Dictionary<string, string> data)
        {
            IResponse result = _dbModel.Update(_entity, id, data);

            return result;
        }

        public IResponse Delete(string id)
        {
            IResponse result = _dbModel.Delete(_entity, id);

            return result;
        }

        public IResponse Select(string query)
        {
            return _dbModel.Select(query);
        }

        public IResponse Get(string id)
        {
            return _dbModel.Select(_entity, id);
        }

        public IResponse Get(string id, string[] fields)
        {
            return _dbModel.Select(_entity, id, fields);
        }

        public IResponse GetAll()
        {
           return _dbModel.SelectAll(_entity);
        }

        public IResponse GetAll(string[] fields)
        {
            return _dbModel.SelectAll(_entity, fields);
        }

        public int GetLastInsertedId()
        {
            return _dbModel.GetLastInsertedId(_entity);
        }
    }
}
