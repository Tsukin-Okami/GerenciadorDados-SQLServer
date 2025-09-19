using MoonUtils;
using IntegracaoBancoDeDados;
using System.Data;
using System.Runtime.InteropServices;

string server = "localhost\\SQLEXPRESS";
string database = "aulabanco";
string user = "administrador";
string password = "adminmaisprati";

Database db;
try
{
    db = new(server, database, user, password);
}
catch(Exception ex)
{
    Console.WriteLine(ex.ToString());
    return;
}

void SelectPeople()
{
    try
    {
        var query = db.ExecuteQuery("select * from Pessoas;");

        Utils.ImprimirTabela(query);

        //foreach (DataRow row in query.Rows)
        //{
        //    for (int i = 0; i < query.Columns.Count; i++)
        //    {
        //        Console.Write($"{query.Columns[i].ColumnName}: {row[i]}\t");
        //    }
        //    Console.WriteLine();
        //}
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }


    Console.WriteLine("\nPressione qualquer tecla para continuar");
    Console.ReadKey();
    Console.Clear();
}

void CreatePeople()
{
    object? eNome = Utils.EntradaUsuario<string>("Digite o nome:", "Nome inválido, digite novamente:");
    object? eIdade = Utils.EntradaUsuario<UInt16>("Digite a idade:", "Idade inválida, digite novamente:");
    object? eEmail = Utils.EntradaUsuario<string>("Digite o email:", "Email inválido, digite novamente:");

    if ( ! (eNome != null && eIdade != null && eEmail != null) )
    {
        Console.WriteLine("Ocorreu um erro desconhecido, um dos campos está inválido");
        return;
    }

    try
    {
        string sqlquery = $"insert into Pessoas(Nome, Idade, Email) values ('{eNome}', {eIdade}, '{eEmail}')";
        var query = db.ExecuteNonQuery(sqlquery);

        if (query != 0)
        {
            Console.WriteLine("Pessoa inserida com sucesso!");
        } 
        else
        {
            Console.WriteLine("Ocorreu um erro ao inserir a pessoa");
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }

    Console.WriteLine("\nPressione qualquer tecla para continuar");
    Console.ReadKey();
}

void DeletePeople()
{
    object? eId = Utils.EntradaUsuario<uint>("Digite o id da pessoa:", "Id inválido, digite novamente:");

    if ( eId == null )
    {
        Console.WriteLine("Ocorreu um erro desconhecido, um dos campos está inválido");
        return;
    }

    try
    {
        var querySelect = db.ExecuteQuery($"select * from Pessoas where Id = {eId}");

        if (querySelect.Rows.Count < 1)
        {
            Console.WriteLine("Nenhum registro encontrado");
        }
        else
        {
            foreach (DataRow row in querySelect.Rows)
            {
                Console.WriteLine("-");
                for (int i = 0; i < querySelect.Columns.Count; i++)
                {
                    Console.WriteLine($"{querySelect.Columns[i].ColumnName}: {row[i]}");
                }
                Console.WriteLine("-");
            }

            void Aceitar()
            {
                try
                {
                    var queryDelete = db.ExecuteNonQuery($"delete from Pessoas where Id = {eId};");

                    if (queryDelete != 0)
                    {
                        Console.WriteLine("Pessoa deletada com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Pessoa não encontrada");
                    }
                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.ToString());
                }
            }

            CMenu selecao = new("Deseja excluir essa pessoa?");
            selecao.SetExitLabel("Recusar");
            selecao.AddOption(1, "Aceitar", Aceitar);
            selecao.Run(true);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }

    Console.WriteLine("\nPressione qualquer tecla para continuar");
    Console.ReadKey();
}

void UpdatePeople()
{
    object? eId = Utils.EntradaUsuario<uint>("Digite o id da pessoa:", "Id inválido, digite novamente:");

    if (eId == null)
    {
        Console.WriteLine("Ocorreu um erro desconhecido, um dos campos está inválido");
        return;
    }

    try
    {
        var querySelect = db.ExecuteQuery($"select * from Pessoas where Id = {eId}");

        if (querySelect.Rows.Count < 1)
        {
            Console.WriteLine("Nenhum registro encontrado");

            Console.WriteLine("\nPressione qualquer tecla para continuar");
            Console.ReadKey();
        }
        else
        {
            Dictionary<string, object> pessoa = [];

            foreach (DataRow row in querySelect.Rows)
            {
                Console.WriteLine("-");
                for (int i = 0; i < querySelect.Columns.Count; i++)
                {
                    Console.WriteLine($"{querySelect.Columns[i].ColumnName}: {row[i]}");
                    pessoa.Add(querySelect.Columns[i].ColumnName, row[i]);
                }
                Console.WriteLine("-");
            }

            void Aceitar()
            {
                try
                {
                    var queryDelete = db.ExecuteNonQuery(
                        $"update Pessoas set " +
                        $"Nome = '{pessoa["Nome"]}', Idade = {pessoa["Idade"]}, Email = '{pessoa["Email"]}' " +
                        $"where Id = {pessoa["Id"]};"
                    );

                    if (queryDelete != 0)
                    {
                        Console.WriteLine("Pessoa atualizada com sucesso!");
                    }
                    else
                    {
                        Console.WriteLine("Pessoa não encontrada");
                    }

                    Console.WriteLine("\nPressione qualquer tecla para continuar");
                    Console.ReadKey();
                }
                catch (Exception ex2)
                {
                    Console.WriteLine(ex2.ToString());

                    Console.WriteLine("\nPressione qualquer tecla para continuar");
                    Console.ReadKey();
                }

                return;
            }

            void EditarNome()
            {
                object? entradaNome = Utils.EntradaUsuario<string>("Digite o nome:", "Nome inválido, digite novamente:");
                if (entradaNome == null)
                {
                    Console.WriteLine("Ocorreu um erro desconhecido, um dos campos está inválido");
                    return;
                }

                pessoa["Nome"] = (string)entradaNome;
            }

            void EditarIdade()
            {
                object? entradaIdade = Utils.EntradaUsuario<uint>("Digite a idade:", "Idade inválida, digite novamente:");
                if (entradaIdade == null)
                {
                    Console.WriteLine("Ocorreu um erro desconhecido, um dos campos está inválido");
                    return;
                }

                pessoa["Idade"] = (uint)entradaIdade;
            }

            void EditarEmail()
            {
                object? entradaEmail = Utils.EntradaUsuario<string>("Digite o email:", "Email inválido, digite novamente:");
                if (entradaEmail == null)
                {
                    Console.WriteLine("Ocorreu um erro desconhecido, um dos campos está inválido");
                    return;
                }

                pessoa["Email"] = (string)entradaEmail;
            }

            void Exibir()
            {
                Console.WriteLine("-");
                foreach (var item in pessoa)
                {
                    Console.WriteLine($"{item.Key.ToString()}: {item.Value.ToString()}");
                }
                Console.WriteLine("-");

                Console.WriteLine("\nPressione qualquer tecla para continuar");
                Console.ReadKey();
            }

            CMenu selecao = new("O que deseja atualizar desta pessoa?");

            selecao.AddOption(1, "Nome", EditarNome);
            selecao.AddOption(2, "Idade", EditarIdade);
            selecao.AddOption(3, "Email", EditarEmail);
            selecao.AddOption(4, "Exibir atual", Exibir);

            selecao.SetExitLabel("Confirmar atualização");
            selecao.OnExit(Aceitar);

            selecao.OnFinished(selecao.Run);
            selecao.Run(true);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());

        Console.WriteLine("\nPressione qualquer tecla para continuar");
        Console.ReadKey();
    }
}

CMenu Menu = new("Manipulador de Banco de Dados");
Menu.OnFinished(Menu.Run);
Menu.SetDescription(db != null ? "conectado" : "desconectado");

Menu.AddOption(1, "Selecionar todas as pessoas", SelectPeople, true);
Menu.AddOption(2, "Adicionar uma pessoa", CreatePeople, true);
Menu.AddOption(3, "Excluir uma pessoa", DeletePeople, true);
Menu.AddOption(4, "Atualizar uma pessoa", UpdatePeople, true);

Menu.Run();

public class CPessoa
{
    public int Id = 0;
    public string Nome = "";
    public int Idade = 0;
    public string Email = "";

    public object this[string key]
    {
        get
        {
            var prop = GetType().GetProperty(key);
            if (prop == null) throw new KeyNotFoundException($"Propriedade '{key}' não existe");
            return prop.GetValue(this);
        }
        set
        {
            var prop = GetType().GetProperty(key);
            if (prop == null) throw new KeyNotFoundException($"Propriedade '{key}' não existe");
            prop.SetValue(this, Convert.ChangeType(value, prop.PropertyType));
        }
    }

    public void Export()
    {
        Console.WriteLine($"Id: {Id}");
        Console.WriteLine($"Nome: {Nome}");
        Console.WriteLine($"Idade: {Idade}");
        Console.WriteLine($"Email: {Email}");
    }
}