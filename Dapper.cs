using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Runtime.InteropServices;
using System.Collections;
using System.Net.Http.Headers;
using System.Reflection;

namespace DatabaseManagement
{
    internal class Program
    {
        static async void Main(string[] args)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            SqlConnection queryConnection = new SqlConnection(builder.ConnectionString);
            await queryConnection.OpenAsync();
            List<List<object>> queryResults = await ExecuteQuery(queryConnection, "SELECT * FROM Games;", new List<string>() { "UserID", "SavedData" });
            List<string> types = new List<string>() { "int", "string" };

            for (int i = 0; i < queryResults.Count; i++)
            {
                QueryContainer currentContainer = new QueryContainer((int)queryResults[i][0], (string)queryResults[i][1]);
            }

            List<QueryContainer> containers = await GetDapperObject<QueryContainer>(queryConnection, "SELECT * FROM Games;");
        }

        public async static Task<List<List<object>>> ExecuteQuery(SqlConnection connection, string query, List<string> keys)
        {
            List<List<object>> returnObjects = new List<List<object>>();

            using(SqlCommand command = new SqlCommand(query, connection))
            {
                using(SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while(await reader.ReadAsync())
                    {
                        List<object> currentObjects = new List<object>();

                        for (int i = 0; i < keys.Count; i++)
                        {
                            currentObjects.Add(reader[keys[i]]);
                        }

                        returnObjects.Add(currentObjects);
                    }
                }
            }

            return returnObjects;
        }

        public async static Task<List<T>> GetDapperObject<T>(SqlConnection connection, string query)
        {
            List<T> returnData = new List<T>();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Type currentType = typeof(T);
                        MemberInfo[] members = currentType.GetMembers();
                        /* AddConstructorCapability; */ object target = currentType.InvokeMember(null, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.CreateInstance, null, null, null);
                        //currentType.GetConstructors()[0].GetParameters()[0].

                        for (int i = 0; i < members.Length; i++)
                        {
                            currentType.InvokeMember(members[i].Name, BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetField, null, target, new object[] { reader[members[i].Name] });
                        }

                        returnData.Add((T)target);
                    }
                }
            }

            return returnData;
        }
    }
}
public class QueryContainer
{
    public int? id = null;
    public string data = "";

    public QueryContainer(int ID, string DATA)
    {
        id = ID;
        data = DATA;
    }
}
