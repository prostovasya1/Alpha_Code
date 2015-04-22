using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace AlphaCode
{
    class Program
    {
        static string start = "ВОРК";
        static string end = "РТРП";
        public static int c = 2;
        static List<string> slovar = new List<string>();//начальный словарь
        static List<string> potential = new List<string>();//один из вариантов цепочки
        static List<List<string>> pot = new List<List<string>>();//массив вариантов цепочки
        static void Main(string[] args)
        {
            FileStream fs = new FileStream("2.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            start = sr.ReadLine();
            end = sr.ReadLine();
            fs.Close(); sr.Close();
            FileStream fs1 = new FileStream("1.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr1 = new StreamReader(fs1, Encoding.Default);
            while (!sr1.EndOfStream)
            {
                slovar.Add(sr1.ReadLine());
            }
            fs1.Close(); sr1.Close();
            potential.Add(start);
            slovar.Remove(start);
            loop(start);
            potential.Clear();
            int n = pot.Min(p => p.Count());
            foreach (var item in pot)
            {
                if (item.Count == n)
                    potential.AddRange(item);
            }
            foreach (var item in potential)
                Console.WriteLine(item);
            Console.ReadKey();

        }

        
        static void loop(string st)
        {
            bool flag = true;
            if (slovar.Count > 1)
            {
                for (int i = 0; i < slovar.Count(); i++)
                {
                    if (LevD(slovar[i], st) == 1)
                    {
                        flag = false;
                        if (LevD(slovar[i], end) == 1)
                        {
                            potential.Add(slovar[i]);
                            potential.Add(end);
                            if (pot.Count > 0)
                            {
                                if (!pot.Contains(potential))
                                {
                                    List<string> final = new List<string>();
                                    final.AddRange(potential);
                                    pot.Add(final);
                                    slovar.Remove(potential.Last());
                                    slovar.Add(potential.Last());
                                    c = potential.Count() - 1;
                                    potential.Clear();
                                    potential.Add(start);
                                    loop(start);
                                }
                                else
                                {
                                    slovar.Remove(potential[potential.Count() - c]);
                                    slovar.Add(potential[potential.Count() - c]);
                                    c--;
                                    potential.Clear();
                                    potential.Add(start);
                                    loop(start);
                                }
                            }
                            else
                            {
                                List<string> final = new List<string>();
                                final.AddRange(potential);
                                pot.Add(final);
                                slovar.Remove(potential.Last());
                                slovar.Add(potential.Last());
                                potential.Clear();
                                potential.Add(start);
                                loop(start);
                            }
                        }
                        else
                        {
                            potential.Add(slovar[i]);
                            slovar.Remove(slovar[i]);
                            loop(potential.Last());
                        }
                    }
                }
                if (flag)
                {
                    if (st != start)
                    {
                        if (potential.Contains(st))
                        {
                            potential.Remove(st);
                            if (potential.Count() > 0)
                                loop(potential.Last());
                            else
                                loop(start);
                        }
                        else
                            loop(start);
                    }
                }
            }
        }
        public static int LevD(string string1, string string2)//Алгоритм поиска расстояния Дамерау — Левенштейна
        {
            if (string1 == null) throw new ArgumentNullException("string1");
            if (string2 == null) throw new ArgumentNullException("string2");
            int diff;
            int[,] m = new int[string1.Length + 1, string2.Length + 1];

            for (int i = 0; i <= string1.Length; i++) m[i, 0] = i;
            for (int j = 0; j <= string2.Length; j++) m[0, j] = j;

            for (int i = 1; i <= string1.Length; i++)
                for (int j = 1; j <= string2.Length; j++)
                {
                    diff = (string1[i - 1] == string2[j - 1]) ? 0 : 1;

                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
                                             m[i, j - 1] + 1),
                                             m[i - 1, j - 1] + diff);
                }

            return m[string1.Length, string2.Length];
        }
    }
}
