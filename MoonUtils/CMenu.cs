using System.Numerics;
using System.Reflection.Metadata;

namespace MoonUtils
{
    public class CMenu
    {
        private const uint ExitNumber = 0;

        private class OptionContent(uint id, string label, Action action, bool clearOnRun)
        {
            public uint Id = id;
            public string Label = label;
            public Action Action = action;
            public bool ClearOnRun = clearOnRun;

            public void Run()
            {
                if (ClearOnRun == true)
                {
                    Console.Clear();
                }
                Action();
            }
        }

        private Dictionary<uint, OptionContent> MenuItems = [];

        private string Title;
        private string? Description;
        private uint Input;

        private string ExitLabel = "Sair";

        private Action ExitAction;
        private Action FinishedAction;

        // constructor
        public CMenu()
        {
            Title = "Menu";
            ExitAction = delegate { return; };
            FinishedAction = ExitAction;
        }

        public CMenu(string title)
        {
            Title = title;
            ExitAction = delegate { return; };
            FinishedAction = ExitAction;
        }

        public CMenu(string title, Action exitAction)
        {
            Title = title;
            ExitAction = exitAction;
            FinishedAction = exitAction;
        }

        public CMenu(string title, Action? exitAction, bool finishedActionRecursive)
        {
            Title = title;

            if (exitAction != null)
            {
                ExitAction = exitAction;
            }
            else
            {
                ExitAction = delegate { return; };
            }


            if (finishedActionRecursive == true)
            {
                FinishedAction = this.Run;
            }
            else
            {
                FinishedAction = ExitAction;
            }

        }

        public CMenu(string title, Action exitAction, Action finishedAction)
        {
            Title = title;
            ExitAction = exitAction;
            FinishedAction = finishedAction;
        }

        // public

        /// <summary>
        /// Executa o menu, mostra na tela as opções e recebe valores de entrada
        /// </summary>
        public void Run()
        {
            Console.Clear();

            ShowMenu();
            ReadInput();

            Int16 inputReturn = SelectInput();

            switch (inputReturn)
            {
                case 1:
                    FinishedAction();
                    break;
                case 0:
                    ExitAction();
                    break;
                case -1:
                    Run();
                    break;
            }
        }

        /// <summary>
        /// Executa o menu, mostra na tela as opções e recebe valores de entrada
        /// </summary>
        /// <param name="dontClearConsole">Não limpar o console ao executar</param>
        public void Run(bool dontClearConsole = false)
        {
            if (dontClearConsole != true)
            {
                // ... perform screen clearing (as before)
                // In compatible terminals, also clear the scrollback buffer.
                // cronic error in unix-terminals
                // Clears the screen and the scrollback buffer in xterm-compatible terminals.
                Console.Clear(); Console.WriteLine("\x1b[3J");
            }

            ShowMenu();
            ReadInput();

            Int16 inputReturn = SelectInput();

            switch (inputReturn)
            {
                case 1:
                    FinishedAction();
                    break;
                case 0:
                    ExitAction();
                    break;
                case -1:
                    Run();
                    break;
            }
        }

        /// <summary>
        /// Evento de saída, executa quando opção <c>0</c> ser chamada.
        /// </summary>
        /// <param name="action"></param>
        public void OnExit(Action action)
        {
            ExitAction = action;
        }

        /// <summary>
        /// Evento de término de opção, executa após alguma opção ser terminada.
        /// Ignora a opção de saída: <c>0</c>
        /// </summary>
        /// <param name="action"></param>
        public void OnFinished(Action action)
        {
            FinishedAction = action;
        }

        /// <summary>
        /// Adiciona uma nova opção para o menu, utilizando um número, título e função específica,
        /// ao executar a opção, ela dispara um evento de <c>OnFinished</c>
        /// </summary>
        /// <param name="id">Indentificador númerico único para a opção</param>
        /// <param name="name">Título da opção</param>
        /// <param name="action">Função para executar ao selecionar a opção</param>
        public void AddOption(uint id, string name, Action action)
        {
            MenuItems.Add(id, new OptionContent(id, name, action, false));
        }

        /// <summary>
        /// Adiciona uma nova opção para o menu, utilizando um número, título, função específica e a limpeza do console,
        /// ao executar a opção, ela dispara um evento de <c>OnFinished</c>
        /// </summary>
        /// <param name="id">Indentificador númerico único para a opção</param>
        /// <param name="name">Título da opção</param>
        /// <param name="action">Função para executar ao selecionar a opção</param>
        /// <param name="onClear">Limpar o console ao executar a opção</param>
        public void AddOption(uint id, string name, Action action, bool onClear)
        {
            MenuItems.Add(id, new OptionContent(id, name, action, onClear));
            //MenuContents.Add(id, name);
            //MenuActions.Add(id, action);
        }

        /// <summary>
        /// Remove uma opção, recomenda-se não utilizar com frequência, caso precise utilizar,
        /// verifique seu código e lembre-se que a opção precisa existir para ser excluído ou irá
        /// acionar uma <c>Exception</c>
        /// </summary>
        /// <param name="id">Identificador númerico da opção adiconada</param>
        public void RemoveOption(uint id)
        {
            MenuItems.Remove(id);
            //bool success_content = MenuContents.Remove(id);
            //bool success_action = MenuActions.Remove(id);
        }

        public void SetNewTitle(string title)
        {
            Title = title;
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        /// <summary>
        /// Definir um título para a opção de saída: <c>0</c>
        /// </summary>
        /// <param name="label">Título</param>
        public void SetExitLabel(string label)
        {
            ExitLabel = label;
        }

        // private

        private void ShowMenu()
        {
            Colorib.Run(ConsoleColor.Blue, delegate
            {
                Console.WriteLine(
                    $"{Title}:\n-"
                );
            });

            if ( Description != null )
            {
                Colorib.Run(ConsoleColor.Yellow, delegate
                {
                    Console.WriteLine($"{Description}\n-");
                });
            }

            Colorib.Run(ConsoleColor.Green, delegate
            {
                foreach (var item in MenuItems)
                {
                    Console.WriteLine($"{item.Value.Id} - {item.Value.Label}");
                }
            });

            Colorib.Run(ConsoleColor.Red, delegate
            {
                Console.WriteLine($"{ExitNumber} - {ExitLabel}");
            });
        }

        private void ReadInput()
        {
            Colorib.Run(ConsoleColor.Yellow, delegate
            {
                Console.WriteLine("-\nDigite a opção escolhida:");
            });
            try
            {
                Input = Convert.ToUInt32(Console.ReadLine());
            }
            catch
            {
                Console.WriteLine("Opção inválida, digite novamente!");
                ReadInput();
                return;
            }
        }

        /// <summary>
        /// Retorna o sucesso do input em Int16.
        /// <example>
        ///  <c>0</c> => Saida voluntária;
        ///  <c>1</c> => Saida por finalização de uma opção(ação);
        ///  <c>-1</c> => Saida por opção inválida;
        /// </example>
        /// </summary>
        /// <returns>
        /// <c>Int16</c>
        /// </returns>
        private Int16 SelectInput()
        {
            if (Input == ExitNumber)
            {
                return 0;
            }

            if (MenuItems.TryGetValue(Input, out OptionContent? value))
            {
                value.Run();
                return 1;
            }

            return -1;
        }

    }
}