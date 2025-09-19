using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorEmpresas.Classes
{
    public class Empresa(Int64 id)
    {
        public readonly Int64 Id = id;
        public string? Nome { get; set; }
        public string? CNPJ { get; set; }
        
        public DataTable GetDataTable()
        {
            DataTable dt = new();

            dt.Columns.Add("Id");
            dt.Columns.Add("Nome");
            dt.Columns.Add("CNPJ");

            dt.Rows.Add([ Id, Nome, CNPJ ]);

            return dt;
        }
    }
}
