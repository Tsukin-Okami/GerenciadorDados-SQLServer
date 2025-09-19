using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorEmpresas.Classes
{
    public class Funcionario(Int64 id)
    {
        public readonly Int64 Id = id;
        public string? Nome { get; set; }
        public Int64? IdCargo { get; set; }
        public Int64? IdSetor { get; set; }
        public string? Email { get; set; }

        public DataTable GetDataTable()
        {
            DataTable dt = new();

            dt.Columns.Add("Id");
            dt.Columns.Add("Nome");
            dt.Columns.Add("IdCargo");
            dt.Columns.Add("IdSetor");
            dt.Columns.Add("Email");

            dt.Rows.Add([ Id, Nome, IdCargo, IdSetor, Email ]);

            return dt;
        }
    }
}
