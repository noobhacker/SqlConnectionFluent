using SqlConnection.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConnection.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = new SqlConnectionFluent("connectionstring")
                .UseStoredProcedure()
                .SetTimeOut(5 * 60)
                .SetParameter("a_id", 123)
                .SetParameter("b_id", "string")
                .ExecuteQuery<QueryResult>();

            foreach(var item in result)
            {
                Console.WriteLine(item.ToString());
            }
        }
    }

    class QueryResult
    {
        public int id { get; set; }
        public string content { get; set; }
        public int other_id { get; set; }
    }
}
