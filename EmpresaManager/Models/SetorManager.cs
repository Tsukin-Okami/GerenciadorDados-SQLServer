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
    public class SetorManager
    {
        private readonly Database server;

        public SetorManager()
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

        // cadastrar / listar / alterar / excluir

        public void Cadastrar()
        {
            var entradaNome = Utils.EntradaUsuario<string>("Digite o nome do setor:", "Nome do setor inválido, digite novamente:");
            var entradaIdEmpresa = Utils.EntradaUsuario<Int64>("Digite o id da empresa:", "Id da empresa inválido, digite novamente:");

            var queryCheckExists = server.ExecuteScalar(
                $"select count(*) from Empresas where IdEmpresa = {entradaIdEmpresa};"
            );

            if ( queryCheckExists == null )
            {
                throw new Exception("SQL Inválido");
            }

            if ((int)queryCheckExists <= 0)
            {
                Console.WriteLine("Esta empresa não existe, cancelando o cadastrar");
                Utils.KeyWait(); return;
            }

            var queryInsert = server.ExecuteNonQuery(
                $"insert into Setores(Nome, IdEmpresa) values ('{entradaNome}', {entradaIdEmpresa});"
            );

            if (queryInsert == 0)
            {
                Console.WriteLine("Ocorreu um problema ao cadastrar o novo setor");
                Utils.KeyWait(); return;
            }

            Console.WriteLine("Setor cadastrado com sucesso!");
            Utils.KeyWait(); return;
        }

        public void Listar()
        {
            var query = server.ExecuteQuery("select s.*, e.Nome as 'Empresa' from Setores s left join Empresas e on e.IdEmpresa = s.IdEmpresa");

            Utils.ImprimirTabela(query);

            Utils.KeyWait();
        }

        public void Alterar()
        {
            var entradaId = Utils.EntradaUsuario<Int64>("Digite o id do setor para alterar:", "Id do setor inválido, digite novamente:");

            var querySelect = server.ExecuteQuery(
                $"select * from Setores where IdSetor = {entradaId};"
            );

            if (querySelect == null)
            {
                throw new Exception("SQL Inválido");
            }

            if (querySelect.Rows.Count <= 0)
            {
                Console.WriteLine("Este setor não existe, cancelando o alterar");
                Utils.KeyWait(); return;
            }

            Setor setor = new((Int64)entradaId)
            {
                Nome = (string)querySelect.Rows[0][1],
                IdEmpresa = (Int64)querySelect.Rows[0][2]
            };

            void _Confirmar()
            {
                var queryInsert = server.ExecuteNonQuery(
                    $"update Setores set Nome = '{setor.Nome}', IdEmpresa = '{setor.IdEmpresa}' where IdSetor = {setor.Id};"
                );

                if (queryInsert == 0)
                {
                    Console.WriteLine("Ocorreu um problema ao alterar o setor");
                    Utils.KeyWait(); return;
                }

                Console.WriteLine("Setor alterado com sucesso!");
                Utils.KeyWait(); return;
            }

            void _AlterarNome()
            {
                var entradaNome = Utils.EntradaUsuario<string>("Digite o nome do setor:", "Nome do setor inválido, digite novamente:");

                setor.Nome = (string)entradaNome;

                Console.WriteLine("Nome do setor alterado");
                Utils.KeyWait();
            }

            void _AlterarEmpresa()
            {
                var entradaEmpresa = Utils.EntradaUsuario<Int64>("Digite o id da empresa:", "Id da empresa inválido, digite novamente:");

                setor.IdEmpresa = (Int64)entradaEmpresa;

                Console.WriteLine("Id da empresa do setor alterado");
                Utils.KeyWait();
            }

            void _Mostrar()
            {
                Utils.ImprimirTabela(setor.GetDataTable());
                Utils.KeyWait();
            }

            CMenu MenuConfirmation = new("Escolha o que deseja alterar");
            MenuConfirmation.SetDescription($"id do setor: {entradaId}");

            MenuConfirmation.AddOption(1, "Exibir alterações", _Mostrar, true);
            MenuConfirmation.AddOption(2, "Nome", _AlterarNome, true);
            MenuConfirmation.AddOption(3, "Empresa", _AlterarEmpresa, true);

            MenuConfirmation.OnExit(_Confirmar);
            MenuConfirmation.OnFinished(MenuConfirmation.Run);

            MenuConfirmation.SetExitLabel("Confirmar alteração");
            MenuConfirmation.Run();
        }

        public void Excluir()
        {
            var entradaId = Utils.EntradaUsuario<Int64>("Digite o id do setor para excluir:", "Id do setor inválido, digite novamente:");

            var queryCheck = server.ExecuteQuery(
                $"select * from Setores where IdSetor = {entradaId};"
            );

            if (queryCheck == null)
            {
                throw new Exception("SQL Inválido");
            }

            if (queryCheck.Rows.Count <= 0)
            {
                Console.WriteLine("Este setor não existe, cancelando o excluir");
                Utils.KeyWait(); return;
            }

            void _excluir()
            {
                var queryInsert = server.ExecuteNonQuery(
                    $"delete from Setores where IdSetor = {entradaId};"
                );

                if (queryInsert == 0)
                {
                    Console.WriteLine("Ocorreu um problema ao excluir o setor");
                    Utils.KeyWait(); return;
                }

                Console.WriteLine("Setor excluido com sucesso!");
                Utils.KeyWait(); return;
            }

            CMenu MenuConfirmation = new("Confirmar exclusão");
            MenuConfirmation.AddOption(1, "Excluir", _excluir);
            MenuConfirmation.SetExitLabel("Cancelar");

            Console.WriteLine();
            Utils.ImprimirTabela(queryCheck);
            Console.WriteLine();

            MenuConfirmation.Run(true);
        }
    }
}
