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
            var query = new SqlConnectionFluent("connectionstring")
                .UseStoredProcedure()
                .SetTimeOut(5 * 60)
                .SetParameter("a_id", 123)
                .SetParameter("b_id", "string")
                .ExecuteQuery<QueryResult>();

            foreach(var item in query)
            {
                Console.WriteLine(item.ToString());
            }

            var queryWithoutReflection = new SqlConnectionFluent("connectionstring")
                .UseStoredProcedure()
                .SetTimeOut(5 * 60)
                .SetParameter("a_id", 123)
                .ExecuteQuery();

            var resultWithoutReflection = queryWithoutReflection.Select(r => new QueryResult
            {
                id = r.Get<int>("id"),
                content = r.Get<string>("content"),
                other_id = r.Get<int>("other_id")
            });
        }
    }

    class QueryResult
    {
        public int id { get; set; }
        public string content { get; set; }
        public int other_id { get; set; }
    }
}
