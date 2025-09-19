using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorEmpresas.Classes
{
    public class Cargo(Int64 id)
    {
        public readonly Int64 Id = id;
        public string? Descricao { get; set; }

        public DataTable GetDataTable()
        {
            DataTable dt = new();

            dt.Columns.Add("Id");
            dt.Columns.Add("Descricao");

            dt.Rows.Add([Id, Descricao]);

            return dt;
        }
    }
}
