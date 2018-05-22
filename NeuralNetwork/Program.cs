using NeuralNetwork.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            List<List<float>> input = new List<List<float>>();
            List<List<float>> target = new List<List<float>>();
            DataHandler handler = new DataHandler("pima-indians-diabetes.data");
            input = handler.GetInput();
            target = handler.GetTarget();


            Network net = new Network(new int[] { 8, 45, 38, 1 }, 0.066f, 0.2f);

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    net.StartFeedForward(input[j].ToArray());
                    net.BackPropagation(target[j].ToArray());
                }
                if (i % 100 == 0)
                {
                    float procent = (((float)i* + 1) / 1000) * 100;
                    Console.WriteLine("procent {0}", procent);
                }
            }

            float good = 0;
            float wrong = 0;

            for (int i = 400; i < input.Count; i++)
            {
                if (Math.Abs(net.StartFeedForward(input[i].ToArray())[0] - target[i][0]) < 0.1f)
                {
                    good++;
                }
                else
                {
                    wrong++;
                }
            }
            float procentnauczenia = (good / (good + wrong)) * 100;
            Console.WriteLine(procentnauczenia);
            /*using (StreamWriter writer = new StreamWriter(@"C:\Users\13cru\source\repos\wyniki5.txt", true))
            {
                writer.WriteLine(n + " : " + n2 + " : " + procentnauczenia);
                writer.Close();
            }*/

            Console.ReadLine();
        }
    }
}
