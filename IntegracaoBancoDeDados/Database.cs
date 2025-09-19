using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegracaoBancoDeDados
{
    public class Database
    {
        private readonly string _connectionString;

        public Database(string server, string database, string user, string password)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                UserID = user,
                Password = password,
                IntegratedSecurity = false,
                TrustServerCertificate = true,
                ConnectTimeout = 30
            };

            _connectionString = builder.ConnectionString;
        }

        // Executa uma consulta SELECT e retorna um DataTable com os resultados
        public DataTable ExecuteQuery(string sql)
        {
            var table = new DataTable();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            table.Load(reader);
            return table;
        }

        // Executa um INSERT/UPDATE/DELETE e retorna o número de linhas afetadas
        public int ExecuteNonQuery(string sql)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        // Executa uma consulta escalar (por exemplo, SELECT COUNT(*) ...)
        public object? ExecuteScalar(string sql)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            return cmd.ExecuteScalar();
        }
    }
}