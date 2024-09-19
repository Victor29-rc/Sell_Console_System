using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Sell_Console_System.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sell_Console_System.Models
{
    internal class DBModel : QueryBuilder
    {   
        private const string connectionString = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=sell_system;Integrated Security=True;TrustServerCertificate=True;";
        private readonly SqlConnection connection;
        private SqlCommand cmd;
        public DBModel()
        {
            connection = new SqlConnection(connectionString);
        }

        public IResponse SelectAll(IIdentifier entity)
        {
            string query = GenerateSelectStatement(entity);

            return ExecuteSelectQuery(query);
        }

        public IResponse SelectAll(IIdentifier entity, string[] fields)
        {
            string query = GenerateSelectStatement(entity, fields);

            return ExecuteSelectQuery(query);
        }

        public IResponse Select(IIdentifier entity, string id)
        {
            string query = GenerateSelectStatement(entity, id);

            return ExecuteSelectQuery(query);
        }

        public IResponse Select(IIdentifier entity, string id, string[] fields)
        {
            string query = GenerateSelectStatement(entity, id, fields);

            return ExecuteSelectQuery(query);
        }

        public IResponse Select(string query)
        {
            return ExecuteSelectQuery(query);
        }

        public IResponse Insert(IIdentifier entity, Dictionary<string, string> data)
        {
            string query = GenerateInsertQueryStatement(entity, data);
           
            return ExecuteQuery(query);
        }

        public IResponse Update(IIdentifier entity, string id, Dictionary<string, string> data)
        {
            string query = GenerateUpdateQueryStatement(entity, id, data);

            return ExecuteQuery(query);
        }

        public IResponse Delete(IIdentifier entity, string id)
        {
            string query = GenerateDeleteQueryStatement(entity, id);

            return ExecuteQuery(query);
        }

        public int GetLastInsertedId(IIdentifier entity)
        {
            string query = $"SELECT TOP 1 {entity.Id} FROM {entity.Table} ORDER BY {entity.Id} DESC";

            IResponse response = ExecuteSelectQuery(query);

            if (response.HasError || response.Results.Count == 0)
            {
                return 0;
            }
            else 
            { 

                IDictionary<string, object> result = (IDictionary<string, object>)response.Results[0];

                if ((result).ContainsKey(entity.Id))
                {
                    return int.Parse(result[entity.Id].ToString()); 
                }

                return 0;
            }
        }

        //if the get all parameter its true, it sets the IResponse.Results variable instead of the IResponse.Result variable
        private IResponse ExecuteSelectQuery(string selectQuery)
        {
            Response response;
            List<object> rows = new List<object>();
     
            try
            {
                List<string> keys = new List<string>();
                List<string> values = new List<string>();

                connection.Open();
                cmd = new SqlCommand(selectQuery, connection);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    dynamic exp = new ExpandoObject();
                    var expDict = (IDictionary<string, object>)exp;

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        expDict[reader.GetName(i)] = reader.GetValue(i);
                    }

                    rows.Add(expDict);
                }
                
                if(rows.Count > 0) 
                { 
                    response = new Response(false, rows);
                }
                else
                {
                    response = new Response(true, "0 records found");
                }  
            }
            catch (Exception ex)
            {
                response = new Response(true, ex.Message);   
            }
            finally
            {
                connection.Close();
            }

            return response;
        }

        private IResponse ExecuteQuery(string query)
        {
            IResponse result;

            try
            {
                connection.Open();
                cmd = new SqlCommand(query, connection);
                cmd.ExecuteNonQuery();

                result = new Response(false);
            }
            catch (Exception ex)
            {
                result = new Response(true, ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return result;
        }
    }
}
