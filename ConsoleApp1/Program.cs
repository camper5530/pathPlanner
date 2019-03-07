using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static int i, j, n, p, xn, xk;
        static bool[] flag = new bool[11];
        static uint[,] c = new uint[11, 11];
        static uint[] l = new uint[11];
        //static string s = new string(new char[80]);
        //static sbyte[,] path = new sbyte[11,80];
        static int min(int n)
        {
            int i, result = 0;
            for (i = 0; i < n; i++)
                if (!(flag[i])) result = i;
            for (i = 0; i < n; i++)
                if ((l[result] > l[i]) && (!flag[i])) result = i;
            return result;
        }
        static uint minim(uint x, uint y)
        {
            if (x < y) return x;
            return y;
        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string s = "";
            string path = "";
            Console.Write("Напишите число точек: ");


            //n = Convert.ToInt32(Console.ReadLine());

            n = 10;

            for (i = 0; i < n; i++)
                for (j = 0; j < n; j++) c[i, j] = 0;
            for (i = 0; i < n; i++)
                for (j = i + 1; j < n; j++)
                {
                    Console.Write(" задайте длины рёбер  x");
                    Console.Write(i + 1);
                    Console.Write(" do x");
                    Console.Write(j + 1);
                    Console.Write(": ");
                    //c[i, j] = Convert.ToUInt32(Console.ReadLine());
                    c[i, j] = 3;
                }
            Console.Write("   ");
            for (i = 0; i < n; i++)
            {
                Console.Write("    X");
                Console.Write(i + 1);
            }
            Console.Write("\n");
            Console.Write("\n");

            for (i = 0; i < n; i++)
            {
                Console.Write("X{0:D}", i + 1);

                for (j = 0; j < n; j++)
                {
                    Console.Write("{0,6:D}", c[i, j]);
                    c[j, i] = c[i, j];
                }
                Console.Write("\n\n");

            }
            for (i = 0; i < n; i++)
                for (j = 0; j < n; j++)
                    if (c[i, j] == 0) c[i, j] = 65535; //nekonecno
            Console.Write(" задайте начальную точку: ");
            //xn = Convert.ToInt32(Console.ReadLine());
            xn = 3;
            path = Convert.ToString(xn + " ");
            Console.Write(" задайте конечную точку: ");
            //xk = Convert.ToInt32(Console.ReadLine());
            xk = 1;
            int rem = xk;
            xk--;
            xn--;
            if (xn == xk)
            {

                Console.WriteLine("Начальная и конечные точки совпадают");
                Console.ReadLine();
                return;
            }

            for (i = 0; i < n; i++)
            {
                flag[i] = false;
                l[i] = 65535;
            }
            l[xn] = 0;
            flag[xn] = true;
            p = xn;

            s = Convert.ToString(xn + 1);
            for (i = 1; i <= n; i++)
            {

                //path[i] = Convert.ToSByte("X");

                //path[i] +=Convert.ToSByte(s);
            }
            do
            {
                for (i = 0; i < n; i++)
                    if ((c[p, i] != 65535) && (!flag[i]) && (i != p))
                    {
                        if (l[i] > l[p] + c[p, i])
                        {

                            s = Convert.ToString(i + 1);

                            //path[i + 1,0] = path[p + 1,0];
                            //path[i + 1] += Convert.ToSByte("-X");

                            //path[i + 1,0] += Convert.ToSByte(s);

                        }
                        l[i] = minim(l[i], l[p] + c[p, i]);
                    }
                p = min(n);
                path += n;
                flag[p] = true;
            }
            while (p != xk);

            if (l[p] != 65535)
            {
                path += Convert.ToString(rem);
                Console.Write("Put: ");
                Console.Write(path);
                Console.Write("\n");
                Console.Write("Dlina puti: ");
                Console.Write(l[p]);
                Console.Write("\n");

            }
            else
                Console.Write("Путь не существует!");
            Console.Write("\n");

            sw.Stop();
            Console.WriteLine("Executed time:");
            Console.WriteLine((sw.Elapsed.TotalMilliseconds).ToString());
            Console.ReadLine();
        }
    }
}
