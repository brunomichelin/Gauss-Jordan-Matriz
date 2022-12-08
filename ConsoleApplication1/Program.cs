using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static public int L, C;
        static public decimal[,] Matriz;
        static public bool SPD = false;
        static public bool SPI = false;
        static public bool SI = false;
        static public decimal[] caracteres;

        static void Main(string[] args)
        {
            EscreverTexto();
            BuildMatriz();
            ShowMatriz();
            GaussMake();
            ShowMatriz();
            JordanMake();
            ShowMatriz();

            string resultado = "";

            for (int i = L - 1; i > -1; i--)
                resultado += (Matriz[i, C - 1] / Matriz[i, i]) + "|";

            File.WriteAllText("função polinomial.txt", resultado.Remove(resultado.Length - 1, 1), Encoding.UTF8);

            string[] texto = File.ReadAllLines("função polinomial.txt", Encoding.UTF8);

            double[] valores = new double[texto[0].Split('|').Length];

            for (int i = 0; i < valores.Length; i++)
                valores[i] = Convert.ToDouble(texto[0].Split('|')[i]);

            double resultadoFX = 0;
            string voltar = "";
            File.WriteAllText("resultado.txt", "");

            for (int e = 1; e <= 100; e++)
            {
                resultadoFX = 0;
                for (int i = valores.Length - 1; i > -1; i--)
                    resultadoFX += valores[valores.Length - (i + 1)] * (Math.Pow(e, i));
                File.AppendAllText("resultado.txt", e + " : " + resultadoFX + Environment.NewLine);
            }


            for (int e = 1; e <= valores.Length; e++)
            {
                resultadoFX = 0;
                for (int i = valores.Length - 1; i > -1; i--)
                    resultadoFX += valores[valores.Length - (i + 1)] * (Math.Pow(e, i));

                int numeroASCII = Convert.ToInt32(resultadoFX);
                if (numeroASCII > 0)
                    voltar += Convert.ToChar(numeroASCII).ToString();
                else
                    voltar += Convert.ToChar(numeroASCII * -1).ToString();
            }
            File.WriteAllText("texto retornado.txt", voltar, Encoding.UTF8);
        }

        static public void EscreverTexto()
        {
            int opcao = 0;
            do
            {
                Console.WriteLine("[1] Texto\n[2] Números");
                if (!(int.TryParse(Console.ReadLine().Trim(), out opcao)) || opcao < 1 || opcao > 2)
                    Console.WriteLine("Escreva 1 ou 2");
            } while (opcao < 1 || opcao > 2);

            if (opcao == 1)
            {
                Console.WriteLine("Escreva o Texto: ");
                string texto = Console.ReadLine();
                //aqui é int caracteres é array de int
                caracteres = new decimal[texto.Length];

                for (int i = 0; i < texto.Length; i++)
                    caracteres[i] = Convert.ToInt32(texto[i]);
            }
            else
            {
                Console.WriteLine("Escreva os Números separados por espaço ou por '|': ");
                string[] texto = Console.ReadLine().Trim().Replace(" ", "|").Split('|');
                //aqui é int caracteres é array de int
                caracteres = new decimal[texto.Length];

                for (int i = 0; i < texto.Length; i++)
                    caracteres[i] = Convert.ToDecimal(texto[i]);
            }
        }

        static public void BuildMatriz()
        {
            L = caracteres.Length;
            C = caracteres.Length + 1;
            Matriz = new decimal[L, C];
            for (int i = 0; i < L; i++)
            {
                string linha = "1;";
                for (int f = 1; f < (C - 1); f++)
                    linha += Math.Pow(i + 1, f) + ";";

                linha += caracteres[i];

                for (int j = 0; j < C; j++)
                    if (linha.IndexOf(";") != -1)
                    {
                        Matriz[i, j] = Convert.ToDecimal(Convert.ToDouble(linha.Substring(0, linha.IndexOf(";"))));
                        linha = linha.Substring((linha.IndexOf(";") + 1));
                    }
                    else
                        Matriz[i, j] = Convert.ToDecimal(Convert.ToDouble(linha.Substring(0, linha.Length)));
            }
        }

        static public void ShowMatriz()
        {
            Console.Write("\n");
            for (int i = 0; i < L; i++)
            {
                for (int j = 0; j < C; j++)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.Write("| {0:F3} |", Matriz[i, j]);
                }
                Console.Write("\n");
                Console.ResetColor();
            }
            if (SI == true)
            {
                Console.WriteLine("Sistema Inconsistente!");
                Console.ReadLine();
                Environment.Exit(0);
            }
            Console.ReadLine();
        }

        static public void GaussMake()
        {
            for (int e = 0; e < L; e += (VerLinhasZeros()) ? 2 : 1)
                for (int J = 1; J < (L - e); J++)
                {
                    decimal K = ((Matriz[J + e, e]) / (Matriz[e, e]));
                    for (int i = 0; i < (C - e); i++)
                        Matriz[J + e, i + e] = (Matriz[J + e, i + e] - (K * Matriz[e, i + e]));
                }
        }

        static public void JordanMake()
        {
            for (int EH = 0; EH < C - 2; EH++)
                if (Matriz[L - 1, EH] != 0)
                    SPI = true;

            if (L - 1 > 0)
            {
                int tHelp = (SPI) ? (C - L) - 1 : 0;

                if (SPI)
                    Console.WriteLine("Sistema Possível e Indeterminado!");
                else
                {
                    Console.WriteLine("Sistema Possível e Determinado!");

                    Matriz[L - 1, C - 1] /= Matriz[L - 1, C - 2];
                    //Matriz[L - 1, C - 2] /= Matriz[L - 1, C - 2];
                    Matriz[L - 1, C - 2] = 1;
                }

                for (int e = L - 1, H = 2; e > 0; e--, H++)
                    for (int J = L; J > 1; J--)
                        if (J - H >= 0)
                        {
                            decimal K = ((Matriz[J - H, C - H - tHelp]) / (Matriz[e, e]));
                            for (int i = 0; i < (C - e); i++)
                                Matriz[J - H, i + e] = (Matriz[J - H, i + e] - (K * Matriz[e, i + e]));
                        }
            }
        }

        static public bool VerLinhasZeros()
        {
            bool ch = false;
            int count;
            for (int i = L - 1; i >= 0; i--)
            {
                count = 0;
                for (int col = C - 1; col >= 0; col--)
                    if (Matriz[i, col] == 0)
                        count++;

                if (count == C)
                {
                    if (i < L - 1)
                        for (int YL = i; YL < L; YL++)
                            for (int YC = 0; YC < C; YC++)
                                if (YL < L - 1)
                                    Matriz[YL, YC] = Matriz[YL + 1, YC];
                    L -= 1;
                    ch = true;
                }
            }

            count = 0;
            for (int col = C - 2; col >= 0; col--)
                if (L - 1 >= 0)
                    if (Matriz[L - 1, col] == 0 && Matriz[L - 1, C - 1] != 0)
                        count++;

            if (count == C - 1)
                SI = true;

            return ch;
        }
    }
}
