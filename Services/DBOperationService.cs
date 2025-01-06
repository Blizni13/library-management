using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace library_management.Services
{
    public class DBOperationService
    {
        public static bool CheckRowExistence(string tableName, params (string columnName, string columnValue)[] pairs)
        {
            var connection = DBConnectionService.GetConnection();

            try
            {
                string query = $"SELECT * FROM {tableName} WHERE ";
                int n = pairs.Length;
                for (int i = 0; i < n; i++)
                {
                    query +=
                        i == n - 1
                        ?
                        $"{pairs[i].columnName} = '{pairs[i].columnValue}'"
                        :
                        $"{pairs[i].columnName} = '{pairs[i].columnValue}' and ";
                }

                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);

                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds.Tables[0].Rows.Count > 0;
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowErrorBox($"An error occured: {ex.Message}");
                return false;
            }
            finally
            {
                DBConnectionService.CloseConnection();
            }
        }
    }
}
