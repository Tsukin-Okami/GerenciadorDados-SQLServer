using GerenciadorEmpresas.Models;
using MoonUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GerenciadorEmpresas
{
    public static class Interface
    {
        public static void NotImplemented()
        {
            Console.WriteLine("Não implementado");
            Utils.KeyWait();
        }

        public static void Cargo()
        {
            // cadastrar / listar / alterar / excluir

            CargoManager manager = new();

            CMenu Menu = new("Gerenciador de Cargos");
            Menu.SetDescription("Escolha o que deseja fazer:");

            Menu.AddOption(1, "Cadastrar", manager.Cadastrar, true);
            Menu.AddOption(2, "Listar", manager.Listar, true);
            Menu.AddOption(3, "Alterar", manager.Alterar, true);
            Menu.AddOption(4, "Excluir", manager.Excluir, true);

            Menu.OnFinished(Menu.Run);
            Menu.Run();
        }

        public static void Empresa()
        {
            // cadastrar / listar / alterar / excluir

            EmpresaManager manager = new();

            CMenu Menu = new("Gerenciador de Empresas");
            Menu.SetDescription("Escolha o que deseja fazer:");

            Menu.AddOption(1, "Cadastrar", manager.Cadastrar, true);
            Menu.AddOption(2, "Listar", manager.Listar, true);
            Menu.AddOption(3, "Alterar", manager.Alterar, true);
            Menu.AddOption(4, "Excluir", manager.Excluir, true);

            Menu.OnFinished(Menu.Run);
            Menu.Run();
        }

        public static void Setor()
        {
            SetorManager manager = new();

            CMenu Menu = new("Gerenciador de Setores");
            Menu.SetDescription("Escolha o que deseja fazer:");

            Menu.AddOption(1, "Cadastrar", manager.Cadastrar, true);
            Menu.AddOption(2, "Listar", manager.Listar, true);
            Menu.AddOption(3, "Alterar", manager.Alterar, true);
            Menu.AddOption(4, "Excluir", manager.Excluir, true);

            Menu.OnFinished(Menu.Run);
            Menu.Run();
        }

        public static void Funcionario()
        {
            FuncionarioManager manager = new();

            CMenu Menu = new("Gerenciador de Funcionários");
            Menu.SetDescription("Escolha o que deseja fazer:");

            Menu.AddOption(1, "Cadastrar", manager.Cadastrar, true);
            Menu.AddOption(2, "Listar", manager.Listar, true);
            Menu.AddOption(3, "Alterar", manager.Alterar, true);
            Menu.AddOption(4, "Excluir", manager.Excluir, true);

            Menu.OnFinished(Menu.Run);
            Menu.Run();
        }

        public static void Relatorio()
        {
            // Empresas => Nome
            // Cargos => Descricao
            // Funcionarios => Nome
            // Empresa => Funcionarios

            RelatorioManager manager = new();

            CMenu Menu = new("Gerador de Relatórios");
            Menu.SetDescription("Escolha o que deseja fazer:");

            Menu.AddOption(1, "Pesquisar empresa por nome", manager.EmpresasNome, true);
            Menu.AddOption(2, "Pesquisar cargos por descrição", manager.CargosDescricao, true);
            Menu.AddOption(3, "Pesquisar funcionários por nome", manager.FuncionariosNome, true);
            Menu.AddOption(4, "Listar funcionários de um empresa", manager.EmpresaFuncionarios, true);

            Menu.OnFinished(Menu.Run);
            Menu.Run();
        }
    }
}
