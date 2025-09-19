using GerenciadorEmpresas.Classes;
using IntegracaoBancoDeDados;
using MoonUtils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorEmpresas.Models
{
    public class RelatorioManager
    {
        private readonly Database server;

        public RelatorioManager()
        {
            try
            {
                var db = new DatabaseManager();
                server = db.database;
                Console.WriteLine("conectado ao servidor");
            }
            catch
            {
                throw;
            }
        }

        // Empresas => Nome
        // Cargos => Descricao
        // Funcionarios => Nome
        // Empresa => Funcionarios

        public void EmpresasNome()
        {
            var entradaNome = Utils.EntradaUsuario<string>("Digite o nome da empresa:", "Nome da empresa inválido, digite novamente:");
            Console.Clear();

            var query = server.ExecuteQuery($"select * from Empresas where Nome like '%{entradaNome}%'");

            if (query.Rows.Count <= 0)
            {
                Console.WriteLine("Nenhuma empresa encontrada");
                Utils.KeyWait();
                return;
            }

            Utils.ImprimirTabela(query);

            Utils.KeyWait();
        }

        public void CargosDescricao()
        {
            var entradaNome = Utils.EntradaUsuario<string>("Digite a descrição do cargo:", "Descrição do cargo inválido, digite novamente:");
            Console.Clear();

            var query = server.ExecuteQuery($"select * from Cargos where Descricao like '%{entradaNome}%'");

            if (query.Rows.Count <= 0)
            {
                Console.WriteLine("Nenhum cargo encontrado");
                Utils.KeyWait();
                return;
            }

            Utils.ImprimirTabela(query);

            Utils.KeyWait();
        }

        public void FuncionariosNome()
        {
            var entradaNome = Utils.EntradaUsuario<string>("Digite o nome do funcionário:", "Nome do funcionário inválido, digite novamente:");
            Console.Clear();

            var query = server.ExecuteQuery(
                $@"select f.IdFuncionario,
	                f.Nome, 
	                c.Descricao as 'Cargo',
	                s.Nome as 'Setor',
	                f.Email,
	                e.Nome as 'Empresa'
                from Funcionarios f 
                left join Cargos c on c.IdCargo = f.IdCargo
                left join Setores s on s.IdSetor = f.IdSetor
                left join Empresas e on e.IdEmpresa = s.IdEmpresa
                where f.Nome like '%{entradaNome}%';"
            );

            if (query.Rows.Count <= 0)
            {
                Console.WriteLine("Nenhum funcionário encontrado");
                Utils.KeyWait();
                return;
            }

            Utils.ImprimirTabela(query);

            Utils.KeyWait();
        }

        public void EmpresaFuncionarios()
        {
            var entradaNome = Utils.EntradaUsuario<string>("Digite o nome da empresa:", "Nome da empresa inválido, digite novamente:");
            Console.Clear();

            var query = server.ExecuteQuery($"select * from Empresas where Nome like '%{entradaNome}%'");

            if (query.Rows.Count <= 0)
            {
                Console.WriteLine("Nenhuma empresa encontrada");
                Utils.KeyWait();
                return;
            }

            Empresa empresa = new((Int64)query.Rows[0][0])
            {
                Nome = (string)query.Rows[0][1],
                CNPJ = (string)query.Rows[0][2],
            };

            bool skip = false;

            if (query.Rows.Count > 1)
            {
                // menu de escolha

                CMenu escolha = new("Escolha uma empresa para listar os funcionários");
                
                for (int i = 0; i < query.Rows.Count; i++)
                {
                    var row = query.Rows[i];
                    escolha.AddOption((uint)i+1, (string)row[1], delegate
                    {
                        empresa = new((Int64)row[0])
                        {
                            Nome = (string)row[1],
                            CNPJ = (string)row[2],
                        };
                    });
                }

                escolha.OnExit(delegate
                {
                    skip = true;
                });
                escolha.Run();
            } 

            if (skip == true)
            {
                return;
            }

            var queryFuncionarios = server.ExecuteQuery($@"
                select f.IdFuncionario,
	                f.Nome, 
	                c.Descricao as 'Cargo',
	                s.Nome as 'Setor',
	                f.Email,
	                e.Nome as 'Empresa'
                from Funcionarios f 
                left join Cargos c on c.IdCargo = f.IdCargo
                left join Setores s on s.IdSetor = f.IdSetor
                left join Empresas e on e.IdEmpresa = s.IdEmpresa
                where e.IdEmpresa = {empresa.Id} order by s.Nome;"
            );

            if (queryFuncionarios.Rows.Count <= 0)
            {
                Console.WriteLine("Nenhum funcionário encontrado");
                Utils.KeyWait();
                return;
            }

            Utils.ImprimirTabela(queryFuncionarios);

            Utils.KeyWait();
        }
    }
}
