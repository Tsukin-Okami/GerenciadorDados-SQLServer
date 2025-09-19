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
    public class CargoManager
    {
        private readonly Database server;

        public CargoManager()
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
            var entradaNome = Utils.EntradaUsuario<string>("Digite a descrição do cargo:", "Descrição de cargo inválida, digite novamente:");

            var queryCheckExists = server.ExecuteScalar(
                $"select count(*) from Cargos where Descricao like '{entradaNome}';"
            );

            if ( queryCheckExists == null )
            {
                throw new Exception("SQL Inválido");
            }

            if ((int)queryCheckExists > 0)
            {
                Console.WriteLine("Este cargo já existe, cancelando o cadastrar");
                Utils.KeyWait(); return;
            }

            var queryInsert = server.ExecuteNonQuery(
                $"insert into Cargos(Descricao) values ('{entradaNome}');"
            );

            if (queryInsert == 0)
            {
                Console.WriteLine("Ocorreu um problema ao cadastrar o novo cargo");
                Utils.KeyWait(); return;
            }

            Console.WriteLine("Cargo cadastrado com sucesso!");
            Utils.KeyWait(); return;
        }

        public void Listar()
        {
            var query = server.ExecuteQuery("select * from Cargos");

            Utils.ImprimirTabela(query);

            Utils.KeyWait();
        }

        public void Alterar()
        {
            var entradaId = Utils.EntradaUsuario<Int64>("Digite o id do cargo para alterar:", "Id do cargo inválido, digite novamente:");

            var queryCargo = server.ExecuteQuery(
                $"select * from Cargos where IdCargo = {entradaId};"
            );

            if (queryCargo == null)
            {
                throw new Exception("SQL Inválido");
            }

            if (queryCargo.Rows.Count <= 0)
            {
                Console.WriteLine("Este cargo não existe, cancelando o alterar");
                Utils.KeyWait(); return;
            }

            Cargo cargo = new((Int64)entradaId)
            {
                Descricao = (string)queryCargo.Rows[0][1]
            };

            void _Confirmar()
            {
                var queryInsert = server.ExecuteNonQuery(
                    $"update Cargos set Descricao = '{cargo.Descricao}' where IdCargo = {cargo.Id};"
                );

                if (queryInsert == 0)
                {
                    Console.WriteLine("Ocorreu um problema ao alterar o cargo");
                    Utils.KeyWait(); return;
                }

                Console.WriteLine("Cargo alterado com sucesso!");
                Utils.KeyWait(); return;
            }

            void _Alterar()
            {
                var entradaDescricao = Utils.EntradaUsuario<string>("Digite o nome/descrição do cargo:", "Nome/descrição do cargo inválida, digite novamente:");

                cargo.Descricao = (string)entradaDescricao;

                Console.WriteLine("Nome/descrição do cargo alterado");
                Utils.KeyWait();
            }

            void _Mostrar()
            {
                Utils.ImprimirTabela(cargo.GetDataTable());
                Utils.KeyWait();
            }

            CMenu MenuConfirmation = new("Escolha o que deseja alterar");
            MenuConfirmation.SetDescription($"id do cargo: {entradaId}");

            MenuConfirmation.AddOption(1, "Exibir alterações", _Mostrar, true);
            MenuConfirmation.AddOption(2, "Nome/Descrição", _Alterar, true);

            MenuConfirmation.OnExit(_Confirmar);
            MenuConfirmation.OnFinished(MenuConfirmation.Run);

            MenuConfirmation.SetExitLabel("Confirmar alteração");
            MenuConfirmation.Run();
        }

        public void Excluir()
        {
            var entradaId = Utils.EntradaUsuario<Int64>("Digite o id do cargo para excluir:", "Id do cargo inválido, digite novamente:");

            var queryCargo = server.ExecuteQuery(
                $"select * from Cargos where IdCargo = {entradaId};"
            );

            if (queryCargo == null)
            {
                throw new Exception("SQL Inválido");
            }

            if (queryCargo.Rows.Count <= 0)
            {
                Console.WriteLine("Este cargo não existe, cancelando o excluir");
                Utils.KeyWait(); return;
            }

            void _excluir()
            {
                var queryInsert = server.ExecuteNonQuery(
                    $"delete from Cargos where IdCargo = {entradaId};"
                );

                if (queryInsert == 0)
                {
                    Console.WriteLine("Ocorreu um problema ao excluir o cargo");
                    Utils.KeyWait(); return;
                }

                Console.WriteLine("Cargo excluido com sucesso!");
                Utils.KeyWait(); return;
            }

            CMenu MenuConfirmation = new("Confirmar exclusão");
            MenuConfirmation.AddOption(1, "Excluir", _excluir);
            MenuConfirmation.SetExitLabel("Cancelar");

            Console.WriteLine();
            Utils.ImprimirTabela(queryCargo);
            Console.WriteLine();

            MenuConfirmation.Run(true);
        }
    }
}
