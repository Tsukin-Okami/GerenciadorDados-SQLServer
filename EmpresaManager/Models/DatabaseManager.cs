using IntegracaoBancoDeDados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorEmpresas.Models
{
    public class DatabaseManager
    {
        private readonly string server = "localhost\\SQLEXPRESS";
        private readonly string databaseName = "aulabanco";
        private readonly string user = "administrador";
        private readonly string password = "adminmaisprati";

        public readonly Database database;

        public DatabaseManager()
        {
            database = new(server, databaseName, user, password);
        }
    }
}
