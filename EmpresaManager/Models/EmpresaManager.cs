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
    public class EmpresaManager
    {
        private readonly Database server;

        public EmpresaManager()
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
            var entradaNome = Utils.EntradaUsuario<string>("Digite o nome da empresa:", "Nome da empresa inválido, digite novamente:");
            var entradaCNPJ = Utils.EntradaUsuario<string>("\nExemplo: 88.888.888/0001-10\nDigite o CNPJ da empresa:", "CNPJ da empresa inválido, digite novamente:");

            var queryCheckExists = server.ExecuteScalar(
                $"select count(*) from Empresas where CNPJ like '{entradaCNPJ}';"
            );

            if ( queryCheckExists == null )
            {
                throw new Exception("SQL Inválido");
            }

            if ((int)queryCheckExists > 0)
            {
                Console.WriteLine("Este CNPJ já existe, cancelando o cadastrar");
                Utils.KeyWait(); return;
            }

            var queryInsert = server.ExecuteNonQuery(
                $"insert into Empresas(Nome, CNPJ) values ('{entradaNome}', '{entradaCNPJ}');"
            );

            if (queryInsert == 0)
            {
                Console.WriteLine("Ocorreu um problema ao cadastrar a nova empresa");
                Utils.KeyWait(); return;
            }

            Console.WriteLine("Empresa cadastrado com sucesso!");
            Utils.KeyWait(); return;
        }

        public void Listar()
        {
            var query = server.ExecuteQuery("select * from Empresas");

            Utils.ImprimirTabela(query);

            Utils.KeyWait();
        }

        public void Alterar()
        {
            var entradaId = Utils.EntradaUsuario<uint>("Digite o id da empresa para alterar:", "Id da empresa inválido, digite novamente:");

            var querySelect = server.ExecuteQuery(
                $"select * from Empresas where IdEmpresa = {entradaId};"
            );

            if (querySelect == null)
            {
                throw new Exception("SQL Inválido");
            }

            if (querySelect.Rows.Count <= 0)
            {
                Console.WriteLine("Esta empresa não existe, cancelando o alterar");
                Utils.KeyWait(); return;
            }

            Empresa empresa = new((uint)entradaId)
            {
                Nome = (string)querySelect.Rows[0][1],
                CNPJ = (string)querySelect.Rows[0][2]
            };

            void _Confirmar()
            {
                var queryInsert = server.ExecuteNonQuery(
                    $"update Empresas set Nome = '{empresa.Nome}', CNPJ = '{empresa.CNPJ}' where IdEmpresa = {empresa.Id};"
                );

                if (queryInsert == 0)
                {
                    Console.WriteLine("Ocorreu um problema ao alterar a empresa");
                    Utils.KeyWait(); return;
                }

                Console.WriteLine("Empresa alterada com sucesso!");
                Utils.KeyWait(); return;
            }

            void _AlterarNome()
            {
                var entradaNome = Utils.EntradaUsuario<string>("Digite o nome da empresa:", "Nome da empresa inválida, digite novamente:");

                empresa.Nome = (string)entradaNome;

                Console.WriteLine("Nome da empresa alterado");
                Utils.KeyWait();
            }

            void _AlterarCNPJ()
            {
                var entradaCNPJ = Utils.EntradaUsuario<string>("Digite o CNPJ da empresa:", "CNPJ da empresa inválido, digite novamente:");

                empresa.CNPJ = (string)entradaCNPJ;

                Console.WriteLine("CNPJ da empresa alterado");
                Utils.KeyWait();
            }

            void _Mostrar()
            {
                Utils.ImprimirTabela(empresa.GetDataTable());
                Utils.KeyWait();
            }

            CMenu MenuConfirmation = new("Escolha o que deseja alterar");
            MenuConfirmation.SetDescription($"id da empresa: {entradaId}");

            MenuConfirmation.AddOption(1, "Exibir alterações", _Mostrar, true);
            MenuConfirmation.AddOption(2, "Nome", _AlterarNome, true);
            MenuConfirmation.AddOption(3, "CNPJ", _AlterarCNPJ, true);

            MenuConfirmation.OnExit(_Confirmar);
            MenuConfirmation.OnFinished(MenuConfirmation.Run);

            MenuConfirmation.SetExitLabel("Confirmar alteração");
            MenuConfirmation.Run();
        }

        public void Excluir()
        {
            var entradaId = Utils.EntradaUsuario<uint>("Digite o id da empresa para excluir:", "Id da empresa inválido, digite novamente:");

            var queryCheck = server.ExecuteQuery(
                $"select * from Empresas where IdEmpresa = {entradaId};"
            );

            if (queryCheck == null)
            {
                throw new Exception("SQL Inválido");
            }

            if (queryCheck.Rows.Count <= 0)
            {
                Console.WriteLine("Esta empresa não existe, cancelando o excluir");
                Utils.KeyWait(); return;
            }

            void _excluir()
            {
                var queryInsert = server.ExecuteNonQuery(
                    $"delete from Empresas where IdEmpresa = {entradaId};"
                );

                if (queryInsert == 0)
                {
                    Console.WriteLine("Ocorreu um problema ao excluir a empresa");
                    Utils.KeyWait(); return;
                }

                Console.WriteLine("Empresa excluida com sucesso!");
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
