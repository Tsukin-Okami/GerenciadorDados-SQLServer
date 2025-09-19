using GerenciadorEmpresas;
using MoonUtils;

CMenu MenuLobby = new("Gerenciador de Banco");
MenuLobby.SetDescription("Escolha a categoria desejada:");

MenuLobby.AddOption(1, "Cargo", Interface.Cargo, true);
MenuLobby.AddOption(2, "Empresa", Interface.Empresa, true);
MenuLobby.AddOption(3, "Setor", Interface.Setor, true);
MenuLobby.AddOption(4, "Funcionário", Interface.Funcionario, true);
MenuLobby.AddOption(5, "Relatório", Interface.Relatorio, true);

MenuLobby.OnFinished(MenuLobby.Run);
MenuLobby.Run();