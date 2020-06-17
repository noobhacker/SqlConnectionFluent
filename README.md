# SqlConnectionFluent
To use it, chain the fluent API as follows,
```
var result = new SqlConnectionFluent("connectionstring")
                .UseStoredProcedure()
                .SetTimeOut(5 * 60)
                .SetParameter("a_id", 123)
                .SetParameter("b_id", "string")
                .ExecuteQuery<QueryResult>();
```
The approach above uses reflection to retrieve property and set one by one from QueryResult, performance is unknown with huge number of rows. .ExecuteQuery() can be added that manually assign ordinals and map to user-specificed result object.
