using Sell_Console_System.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sell_Console_System.Models
{
    abstract class QueryBuilder
    {
        protected string GenerateInsertQueryStatement(IIdentifier entity, Dictionary<string, string> data)
        {
            string fields = "";
            string values = "";
            string query = "";

            foreach (KeyValuePair<string, string> item in data)
            {
                fields += $"{item.Key}, ";
                values += $"'{item.Value}', ";
            }

            if (!string.IsNullOrEmpty(fields))
            {
                fields = fields.Substring(0, fields.Length - 2); //removing the last space and comma (,)
            }

            if (!string.IsNullOrEmpty(values))
            {
                values = values.Substring(0, values.Length - 2); //removing the last space and comma (,)
            }

            if (!string.IsNullOrEmpty(fields) && !string.IsNullOrEmpty(values))
            {
                query = $"INSERT INTO {entity.Table} ({fields}) VALUES ({values})";
            }

            return query;
        }

        protected string GenerateUpdateQueryStatement(IIdentifier entity, string id, Dictionary<string, string> data)
        {
            string setStatement = "";
            string query = "";

            foreach (KeyValuePair<string, string> item in data)
            {
                setStatement += $"{item.Key} = '{item.Value}', ";
            }

            if (!string.IsNullOrEmpty(setStatement))
            {
                setStatement = setStatement.Substring(0, setStatement.Length - 2); //removing the last comma (,)
            }

            if (!string.IsNullOrEmpty(setStatement))
            {
                query = $"UPDATE {entity.Table} SET {setStatement} WHERE {entity.Id} = '{id}'";
            }

            return query;
        }

        protected string GenerateDeleteQueryStatement(IIdentifier entity, string id)
        {
            return $"DELETE FROM {entity.Table} WHERE {entity.Id} = '{id}'";
        }

        protected string GenerateSelectStatement(IIdentifier entity)
        {
            return $"SELECT * FROM {entity.Table}";
        }

        protected string GenerateSelectStatement(IIdentifier entity, string[] fields)
        {
            string selectStatement = "";
            string selectFields = "";

            foreach (string field in fields)
            {
                selectFields += $"{field}, ";
            }

            if (!string.IsNullOrEmpty(selectFields))
            {
                selectFields = selectFields.Substring(0, selectFields.Length - 2);
                selectStatement = $"SELECT {selectFields} FROM {entity.Table}";
            }

            return selectStatement;
        }

        protected string GenerateSelectStatement(IIdentifier entity, string id)
        {
            return $"SELECT * FROM {entity.Table} WHERE {entity.Id} = '{id}'";
        }

        protected string GenerateSelectStatement(IIdentifier entity, string id, string[] fields)
        {
            string selectStatement = "";
            string selectFields = "";

            foreach (string field in fields)
            {
                selectFields += $"{field}, ";
            }

            if (!string.IsNullOrEmpty(selectFields))
            {
                selectFields = selectFields.Substring(0, selectFields.Length - 2);
                selectStatement = $"SELECT {selectFields} FROM {entity.Table} WHERE {entity.Id} = '{id}'";
            }

            return selectStatement;
        }
    }
}
