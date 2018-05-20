using NeuralNetwork.Data;
using System;
using System.Collections.Generic;
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

            Network net = new Network(new int[] { 8, 30, 40,1}, 0.033f);

            for (int i = 0; i < 5000; i++)
            {
                for (int j = 0; j < 400; j++)
                {
                    net.StartFeedForward(input[j].ToArray());
                    net.BackPropagation(target[j].ToArray());
                }
                if(i % 100 == 0)
                {
                    float procent = (((float)i + 1) / 5000) * 100;
                    Console.WriteLine(procent);
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

            Console.ReadLine();
        }
    }
}
