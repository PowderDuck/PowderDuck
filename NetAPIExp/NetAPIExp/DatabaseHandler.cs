using Microsoft.Data.SqlClient;

namespace NetAPIExp.Database
{
    public class DatabaseHandler
    {
        public static DatabaseHandler Handler { get; set; }

        private SqlConnection con;

        public DatabaseHandler(string connectionString)
        {
            Handler = this;

            con = new SqlConnection(connectionString);
        }

        public async Task<List<List<object>>> ExecuteQuery(string query, List<string> keys)
        {
            await con.OpenAsync();
            List<List<object>> queryObjects = new List<List<object>>();

            using(SqlCommand command = new SqlCommand(query, con))
            {
                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        List<object> currentObjects = new List<object>();
                        
                        for (int i = 0; i < keys.Count; i++)
                        {
                            currentObjects.Add(reader[keys[i]]);
                        }

                        queryObjects.Add(currentObjects);
                    }
                }
            }

            await con.CloseAsync();

            return queryObjects;
        }
    }
}
