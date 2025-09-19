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
    public class FuncionarioManager
    {
        private readonly Database server;

        public FuncionarioManager()
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
            var entradaNome = Utils.EntradaUsuario<string>("Digite o nome do funcionário:", "Nome do funcionário inválido, digite novamente:");
            var entradaEmail = Utils.EntradaUsuario<string>("Digite o email do funcionário:", "Email do funcionário inválido, digite novamente:");

            var entradaCargo = Utils.EntradaUsuario<Int64>("Digite o id do cargo do funcionário:", "Id do cargo do funcionário inválido, digite novamente:");
            var entradaSetor = Utils.EntradaUsuario<Int64>("Digite o id do setor do funcionário:", "Id do setor do funcionário inválido, digite novamente:");

            var queryInsert = server.ExecuteNonQuery(
                "insert into Funcionarios(Nome, Email, IdCargo, IdSetor) " +
                $"values ('{entradaNome}', '{entradaEmail}', {entradaCargo}, {entradaSetor})"
            );

            if (queryInsert == 0)
            {
                Console.WriteLine("Ocorreu um problema ao cadastrar o novo funcionário");
                Utils.KeyWait(); return;
            }

            Console.WriteLine("Funcionário cadastrado com sucesso!");
            Utils.KeyWait(); return;
        }

        public void Listar()
        {
            var query = server.ExecuteQuery($@"
                select f.IdFuncionario, 
	                f.Nome, f.Email,
	                f.IdCargo, c.Descricao as 'NomeCargo', 
	                f.IdSetor, s.Nome as 'NomeSetor'
                from Funcionarios f
                left join Cargos c on c.IdCargo = f.IdCargo
                left join Setores s on s.IdSetor = f.IdSetor;"
            );

            Utils.ImprimirTabela(query);

            Utils.KeyWait();
        }

        public void Alterar()
        {
            var entradaId = Utils.EntradaUsuario<Int64>("Digite o id do funcionário para alterar:", "Id do funcionário inválido, digite novamente:");

            var querySelect = server.ExecuteQuery(
                $"select * from Funcionarios where IdFuncionario = {entradaId};"
            );

            if (querySelect == null)
            {
                throw new Exception("SQL Inválido");
            }

            if (querySelect.Rows.Count <= 0)
            {
                Console.WriteLine("Este funcionário não existe, cancelando o alterar");
                Utils.KeyWait(); return;
            }

            Funcionario funcionario = new((Int64)entradaId)
            {
                Nome = (string)querySelect.Rows[0][1],
                IdCargo = (Int64)querySelect.Rows[0][2],
                IdSetor = (Int64)querySelect.Rows[0][3],
                Email = (string)querySelect.Rows[0][4]
            };

            void _Confirmar()
            {
                var queryInsert = server.ExecuteNonQuery($@"
                    update Funcionarios set 
                        Nome = '{funcionario.Nome}', 
                        Email = '{funcionario.Email}', 
                        IdCargo = {funcionario.IdCargo}, 
                        IdSetor = {funcionario.IdSetor} 
                    where IdFuncionario = {funcionario.Id};"
                );

                if (queryInsert == 0)
                {
                    Console.WriteLine("Ocorreu um problema ao alterar o funcionário");
                    Utils.KeyWait(); return;
                }

                Console.WriteLine("Funcionário alterado com sucesso!");
                Utils.KeyWait(); return;
            }

            void _AlterarNome()
            {
                var entradaNome = Utils.EntradaUsuario<string>("Digite o nome do funcionário:", "Nome do funcionário inválido, digite novamente:");

                funcionario.Nome = (string)entradaNome;

                Console.WriteLine("Nome do funcionário alterado");
                Utils.KeyWait();
            }

            void _AlterarEmail()
            {
                var entradaEmail = Utils.EntradaUsuario<string>("Digite o email do funcionário:", "Email do funcionário inválido, digite novamente:");

                funcionario.Email = (string)entradaEmail;

                Console.WriteLine("Email do funcionário alterado");
                Utils.KeyWait();
            }

            void _AlterarCargo()
            {
                var entradaCargo = Utils.EntradaUsuario<Int64>("Digite o id do cargo do funcionário:", "Id do cargo do funcionário inválido, digite novamente:");

                // se o usuario digitar um cargo não existente,
                // o programa morre EXCEPTION,
                //      -> fazer um check no SQL para verificar se o setor existe
                //      e cancelar se não existe

                funcionario.IdCargo = (Int64)entradaCargo;

                Console.WriteLine("Id do cargo do funcionário alterado");
                Utils.KeyWait();
            }

            void _AlterarSetor()
            {
                var entradaSetor = Utils.EntradaUsuario<Int64>("Digite o id do setor do funcionário:", "Id do setor do funcionário inválido, digite novamente:");

                // se o usuario digitar um setor não existente,
                // o programa morre EXCEPTION,
                //      -> fazer um check no SQL para verificar se o setor existe
                //      e cancelar se não existe

                // Verificar o programa inteiro e corrigir este problema!

                funcionario.IdSetor = (Int64)entradaSetor;

                Console.WriteLine("Id do setor do funcionário alterado");
                Utils.KeyWait();
            }

            void _Mostrar()
            {
                Utils.ImprimirTabela(funcionario.GetDataTable());
                Utils.KeyWait();
            }

            CMenu MenuConfirmation = new("Escolha o que deseja alterar");
            MenuConfirmation.SetDescription($"id do funcionário: {entradaId}");

            MenuConfirmation.AddOption(1, "Exibir alterações", _Mostrar, true);
            MenuConfirmation.AddOption(2, "Nome", _AlterarNome, true);
            MenuConfirmation.AddOption(3, "Email", _AlterarEmail, true);
            MenuConfirmation.AddOption(4, "Cargo", _AlterarCargo, true);
            MenuConfirmation.AddOption(5, "Setor", _AlterarSetor, true);

            MenuConfirmation.OnExit(_Confirmar);
            MenuConfirmation.OnFinished(MenuConfirmation.Run);

            MenuConfirmation.SetExitLabel("Confirmar alteração");
            MenuConfirmation.Run();
        }

        public void Excluir()
        {
            var entradaId = Utils.EntradaUsuario<Int64>("Digite o id do funcionário para excluir:", "Id do funcionário inválido, digite novamente:");

            var queryCheck = server.ExecuteQuery(
                $"select * from Funcionarios where IdFuncionario = {entradaId};"
            );

            if (queryCheck == null)
            {
                throw new Exception("SQL Inválido");
            }

            if (queryCheck.Rows.Count <= 0)
            {
                Console.WriteLine("Este funcionário não existe, cancelando o excluir");
                Utils.KeyWait(); return;
            }

            void _excluir()
            {
                var queryInsert = server.ExecuteNonQuery(
                    $"delete from Funcionarios where IdFuncionarios = {entradaId};"
                );

                if (queryInsert == 0)
                {
                    Console.WriteLine("Ocorreu um problema ao excluir o funcionário");
                    Utils.KeyWait(); return;
                }

                Console.WriteLine("Funcionário excluido com sucesso!");
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
