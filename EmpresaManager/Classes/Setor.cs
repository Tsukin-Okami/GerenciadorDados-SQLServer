using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorEmpresas.Classes
{
    public class Setor(Int64 id)
    {
        public readonly Int64 Id = id;
        public string? Nome { get; set; }
        public Int64? IdEmpresa { get; set; }
        
        public DataTable GetDataTable()
        {
            DataTable dt = new();

            dt.Columns.Add("Id");
            dt.Columns.Add("Nome");
            dt.Columns.Add("IdEmpresa");

            dt.Rows.Add([ Id, Nome, IdEmpresa ]);

            return dt;
        }
    }
}
