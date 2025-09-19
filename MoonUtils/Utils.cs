using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace MoonUtils
{
    public class Utils
    {
        public static void KeyWait()
        {
            Console.WriteLine("\nDigite qualquer tecla para continuar");
            Console.ReadKey();
        }

        public static void Print<T>(T valor)
        {
            Console.WriteLine(valor == null ? "null" : valor.ToString());
        }

        public static void ImprimirTabela(DataTable tabela)
        {
            // Descobrir o tamanho máximo de cada coluna para alinhar
            int[] larguras = new int[tabela.Columns.Count];
            for (int i = 0; i < tabela.Columns.Count; i++)
            {
                larguras[i] = tabela.Columns[i].ColumnName.Length; // começa pelo tamanho do cabeçalho
                foreach (DataRow row in tabela.Rows)
                {
                    int comprimento = row[i]?.ToString()?.Length ?? 0;
                    if (comprimento > larguras[i])
                        larguras[i] = comprimento;
                }
            }

            // Imprimir cabeçalho
            for (int i = 0; i < tabela.Columns.Count; i++)
            {
                Console.Write($"| {tabela.Columns[i].ColumnName.PadRight(larguras[i])} ");
            }
            Console.WriteLine("|");

            // Linha separadora
            for (int i = 0; i < tabela.Columns.Count; i++)
            {
                Console.Write($"+-{new string('-', larguras[i])}-");
            }
            Console.WriteLine("+");

            // Imprimir dados
            foreach (DataRow row in tabela.Rows)
            {
                for (int i = 0; i < tabela.Columns.Count; i++)
                {
                    string valor = row[i]?.ToString() ?? "";
                    Console.Write($"| {valor.PadRight(larguras[i])} ");
                }
                Console.WriteLine("|");
            }
        }

        public static object EntradaUsuario<T>(string titulo, string textoInvalido)
        {
            Console.WriteLine(titulo);
            while (true)
            {
                string? entrada = Console.ReadLine();

                if (typeof(T) == typeof(string))
                {
                    if (!string.IsNullOrWhiteSpace(entrada))
                        return (T)(object)entrada!;
                }
                else
                {
                    // Conversão automática para qualquer tipo numérico
                    try
                    {
                        object valor = Convert.ChangeType(entrada, typeof(T), CultureInfo.InvariantCulture);
                        return (T)valor;
                    } 
                    catch
                    {
                        Console.WriteLine(textoInvalido);
                    }
                }

            }
        }
    }
}
