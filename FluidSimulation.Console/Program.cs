using FluidSimulation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FluidSimulation.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            int width = 10;
            int height = 10;

            //Perlin
            int octaveCount = 5;

            float[][] perlinNoise = PerlinNoise.PerlinNoise.GeneratePerlinNoise(width, height, octaveCount);

            //======================
            var map = new Cell[width, height];

            for (int line = 0; line < height; line++)
            {
                for (int col = 0; col < width; col++)
                {
                    map[line, col] = new Cell()
                    {
                        Altitude = perlinNoise[line][col],
                        X = col,
                        Y = line
                    };
                }
            }

            var fluidPlugin = new FluidSimulation(map);

            Program.ShowMap(fluidPlugin.GetMap());

            fluidPlugin.AddWater(5, 5, 10f);

            int cpt = 0;
            while(cpt < 100)
            {
                fluidPlugin.Update(1);

                Console.WriteLine("Iteration : " + cpt);
                Program.ShowMap(fluidPlugin.GetMap());

                //Thread.Sleep(1000);
                cpt++;
            }


            Console.ReadKey();
        }

        public static void ShowMap(Cell[,] map)
        {
            Console.WriteLine("===============================================");
            for (int line = 0; line < map.GetLength(1); line++)
            {
                for (int col = 0; col < map.GetLength(0); col++)
                {
                    Console.Write(map[line, col].Water.ToString("0.00"));
                    Console.Write("|");
                }
                Console.WriteLine("");
                for (int col = 0; col < map.GetLength(0)*2; col++)
                {
                    Console.Write("-");
                }
                Console.WriteLine("");
            }
            Console.WriteLine("===============================================");

        }
    }
}
