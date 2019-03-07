using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp17
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Stopwatch.IsHighResolution)
            {
                Console.WriteLine("Operations timed using the system's high-resolution performance counter.");
            }
            else
            {
                Console.WriteLine("Operations timed using the DateTime class.");
            }

            long frequency = Stopwatch.Frequency;
            Console.WriteLine("  Timer frequency in ticks per second = {0}",
                frequency);
            long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
            Console.WriteLine("  Timer is accurate within {0} nanoseconds",
                nanosecPerTick);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            
            var cur = new double[][] {
                new double[]{-0.402,0,0.267},
                new double[]{ 0.492,0,0.274},
                new double[]{ 0.225,0,-0.218},
                new double[]{-0.0961,0,0.545},
                new double[]{-0.0979,0,0.272},
                
                new double[]{ 0.174,0,0.0654},
                new double[]{ 0.0192,0,-0.153},
                new double[]{-0.356,0,0.0567},
                new double[]{-0.0548,0,-0.342},
                new double[]{ 0.354,0,0.21},

                new double[]{-0.264,0,-0.206},
                new double[]{-0.281,0,0.48},
                new double[]{ 0.151,0,0.295},
                new double[]{-0.135,0,-0.0344},
                new double[]{-0.445, 0,-0.107},
                new double[]{-0.245,0,0.0927},
                new double[]{0.0551,0,0.393},
                new double[]{ 0.155,0,0.493},
                new double[]{-0.57,0,0.0406},
                new double[]{-0.269,0,0.317},
                new double[]{0.0122,0,0.54},
                new double[]{-0.0931,0,0.378},
                new double[]{-0.145,0,0.156},
                new double[]{0.0339,0,-0.000845},
                new double[]{-0.569,0,-0.0862},
                
                new double[]{0.0464,0,0.185},
                new double[]{  0.33,0,0.39},
                new double[]{-0.302,0,-0.0827},
                new double[]{0.0959,0,-0.319},
                new double[]{ 0.308,0,0.0146},
                new double[]{ 0.622, 0,0.0692},
                new double[]{ 0.708, 0,0.415},
                new double[]{ 0.359, 0,0.586},
                new double[]{ 0.177, 0,0.615},
                new double[]{ 0.507, 0,-0.0755},
                new double[]{0.0452,0,0.702},
                new double[]{-0.0698,0,0.789},
                new double[]{ 0.437, 0,-0.201},
                new double[]{ 0.526, 0,0.596},
                new double[]{ 0.328, 0,0.356},
                new double[]{-0.125, 0,0.679},
                new double[]{ 0.594, 0,0.199},
                new double[]{ 0.243, 0,-0.566},
                new double[]{0.0692,0,-0.646},
                new double[]{-0.178, 0,0.846},
                new double[]{-0.108,0,-0.559},
                new double[]{-0.223,0,-0.4},
                new double[]{0.0753,0,-0.457},
                new double[]{-0.402,0,-0.262},
                new double[]{-0.566,0,-0.263}
                
            };

            var robot = new Module[50];
            Console.WriteLine("Инициализация группы роботов");
            for (int i = 0; i < cur.Length; i++)
            {
                Console.WriteLine("робот"+i);
                robot[i] = new Module(cur[i]);
                showData(robot[i].data, 2);
            }
            Console.WriteLine("ЗАПУСК ЦИКЛА КОММУНИКАЦИИ");
            int x = 0;
            while(x < 2){
                Console.WriteLine("--- Итерация "+ x);
                int k = 0;
                foreach (Module m in robot)
                {
                    Console.WriteLine("Module " + k + ":");
                    m.FindNeubourghs(robot);
                    
                    var state = 1;
                    foreach (Module neub in m.neubourghs)
                    {
                        m.communicateModules(neub.data);
                        if (compareData(m.data, neub.data) == false)
                        {
                            state = 0;
                        }
                    }
                    m.state = state;
                    
                    Console.WriteLine(m.state);
                    showData(m.data, 4);
                    k++;
                }
                x++;
            }
            sw.Stop();
            Console.WriteLine("Executed time:");
            Console.WriteLine((sw.Elapsed.TotalMilliseconds).ToString());
            //Console.WriteLine((sw.fre).ToString());
            Console.ReadKey();
        }

        static bool compareData(double[] x, double[] y)
        {
            var res = false;
            if(
                x[0] == y[0] && 
                x[1] == y[1] &&
                x[2] == y[2] &&
                x[3] == y[3]
            )
            {
                res = true;
            }
            else if(
                x[0] == y[2] &&
                x[1] == y[3] &&
                x[2] == y[0] &&
                x[3] == y[1]
            )
            {
                res = true;
            }
            return res;
        }

        static void showData(double[] x, int k)
        {
            for(int y = 0; y<k; y++)
            {
                Console.Write(x[y]+" ");
            }
            Console.WriteLine();
        }
    }

    public class Module
    {
        private double[][] config = new double[][] {
            new double[]{-0.121839,0,0.329979},
            new double[]{-0.0609721,0,0.299589},
            new double[]{-0.0840197,0,0.236992},
            new double[]{-0.021657,0,0.21775},
            new double[]{-0.0480357,0,0.151874}
        };

        public double[] coords;
        public int state;
        public double[] data; 
        private double channelLength = 1.2;
        public Module[] neubourghs;

        public Module(double[] c)
        {
            this.coords = c;
            data = new double[]{coords[0], coords[2], 0, 0};
        }

        public void FindNeubourghs(Module[] robot)
        {
            var n = new Module[0];
            foreach(Module m in robot)
            {
                if ((coords[0] == m.coords[0]) && (coords[2] == m.coords[2]))
                {
                    continue;
                }
                else
                {
                    double d = getDistance(coords[0], coords[2], m.coords[0], m.coords[2]);
                    if (d < channelLength)
                    {
                        Array.Resize(ref n, n.Length+1);
                        n[n.Length-1] = m;
                    }
                }
            }
            neubourghs = n;
        }

        public double getDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(
                (x1-x2)*(x1 - x2) + (y1 - y2)*(y1-y2)
            );
        }

        private double[] getConfigurationCenter(double[] d)
        {
            var res = new double[2];
            res[0] = (d[0] + d[2]) / 2;
            res[1] = (d[1] + d[3]) / 2;
            return res;
        }
        
        public void communicateModules(double[] r)
        {
            if(state == 0)
            {
                if (data[2] == 0 && data[3] == 0)
                {//Console.WriteLine("firstOption");
                    data[2] = r[0];
                    data[3] = r[1];
                }
                var distanceToNeub = getDistance(
                    data[0],
                    data[1],
                    r[0],
                    r[1]
                );//Console.WriteLine("distanceToNeub:"+ distanceToNeub);
                var distanceToFarNeub = getDistance(
                    data[0],
                    data[1],
                    r[2],
                    r[3]
                );//Console.WriteLine("distanceToFarNeub:" + distanceToFarNeub);
                if (distanceToFarNeub > distanceToNeub)
                {//Console.WriteLine("distanceToFarNeub > distanceToNeub");
                    for (int z = 0; z < 4; z++)
                    {
                        data[z] = r[z];
                    }
                }
                else
                {//Console.WriteLine("else");
                    var distanceToPrevNeub = getDistance(data[0], data[1], data[2], data[3]);
                    if (distanceToNeub > distanceToPrevNeub)
                    {
                        data[2] = r[0];
                        data[3] = r[1];
                    }
                }
            }
            else
            {
                //формируем новый пакет данных для передачи
                var o = getConfigurationCenter(data);
                var position = getPosition();
                data[0] = coords[0];
                data[1] = coords[2];
                data[2] = position[0];
                data[3] = position[1];
            }
        }

        private double[] getPosition()
        {
            var res = new double[] { config[0][0], config[0][2] };
            var xDis = getDistance(coords[0], config[0][0], coords[2], config[0][2]);
            int i = 0;
            foreach (var c in config)
            {
                if (i == 0) continue;
                var yDis = getDistance(coords[0], c[0], coords[2], c[2]);
                if (yDis < xDis)
                {
                    res[0] = c[0];
                    res[1] = c[2];
                }
                i++;
            }
            return res;
        }
    }
}
