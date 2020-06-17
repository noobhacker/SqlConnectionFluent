using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnection.Fluent
{
    public class SqlConnectionFluent
    {
        private string _connectionString;
        private CommandType _commandType;
        private int _timeOut;
        private List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();

        public SqlConnectionFluent(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnectionFluent UseStoredProcedure()
        {
            _commandType = CommandType.StoredProcedure;
            return this;
        }

        public SqlConnectionFluent SetTimeOut(int timeout)
        {
            _timeOut = timeout;
            return this;
        }

        public SqlConnectionFluent SetParameter(string parameter, object value)
        {
            parameters.Add(new KeyValuePair<string, object>(parameter, value));
            return this;
        }

        public List<T> ExecuteQuery<T>()
            where T : new()
        {
            var ts = new List<T>();
            using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandType = _commandType;
                command.CommandTimeout = _timeOut;
                foreach(var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                var properties = typeof(T).GetProperties().ToList();
                connection.Open();
                using(var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        var t = new T(); 
                        foreach (var property in properties)
                        {
                            var row = reader.GetValue(reader.GetOrdinal(property.Name));
                            var propertyInfo = t.GetType().GetProperty(property.Name);
                            propertyInfo.SetValue(propertyInfo, row);
                        }
                        ts.Add(t);
                    }
                }
            }
            return ts;
        }

        public List<SqlDataRow> ExecuteQuery()
        {
            var rows = new List<SqlDataRow>();
            using (var connection = new System.Data.SqlClient.SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandType = _commandType;
                command.CommandTimeout = _timeOut;
                foreach (var parameter in parameters)
                {
                    command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                }

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        var t = new SqlDataRow();
                        for(int i = 0; i < reader.FieldCount; i++)
                        {
                            t.Columns.Add(new SqlDataColumn
                            {
                                 ColumnName = reader.GetName(i),
                                 Data = reader[i]
                            });
                        }
                        rows.Add(t);
                    }
                }
            }
            return rows;
        }

    }

    public class SqlDataRow
    {
        public List<SqlDataColumn> Columns { get; set; }
        public T Get<T>(string columnName)
        {
            return (T)Columns.First(x => x.ColumnName == columnName).Data;
        }
    }

    public class SqlDataColumn
    {
        public string ColumnName { get; set; }
        public object Data { get; set; }       
    }

}
