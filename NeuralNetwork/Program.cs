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
            float learningRate = 0.001f;
            double previousSSE = 0.0;
            float er = 1.04f;
            float erInc = 1.05f;
            float erDec = 0.7f;
            DataHandler handler = new DataHandler("pima-indians-diabetes.data");
            input = handler.GetInput();
            target = handler.GetTarget();


            Network net = new Network(new int[] { 8, 45, 38, 1 }, learningRate, 0.2f);

            bool stop = false;
            for (int i = 0; i < 20000; i++)
            {
                double sse = 0;
                for (int j = 0; j < input.Count; j++)
                {
                    net.ChangeLearningRate(learningRate);
                    float output = net.StartFeedForward(input[j].ToArray())[0];
                    sse += 0.5f * Math.Pow(output - target[j][0], 2);
                    net.BackPropagation(target[j].ToArray());
                }
                if (sse < 0.25)
                {
                    break;
                }
                if(i != 0)
                {
                    if(sse > er * previousSSE)
                    {
                        learningRate *= erDec;
                    }
                    if(sse < previousSSE)
                    {
                        learningRate *= erInc;
                    }
                }
                previousSSE = sse;
                if (i % 100 == 0)
                {
                    float procent = (((float)i* + 1) / 20000) * 100;
                    Console.WriteLine("epoch {0} / {1}, sse {2}, lr{3}", i, 20000, sse, learningRate);
                }
            }

            float good = 0;
            float wrong = 0;

            for (int i = 0; i < input.Count; i++)
            {
                float output = net.StartFeedForward(input[i].ToArray())[0];
                Console.WriteLine("{0}\t{1}", output, target[i][0]);
                if ((Math.Abs(output - target[i][0]) < 0.3f && target[i][0] == 0) || (Math.Abs(output - target[i][0]) < 0.3f && target[i][0] == 1))
                {
                    good++;
                }
                else
                {
                    wrong++;
                }
            }
            float result = (good / (good + wrong)) * 100;
            Console.WriteLine(result);
            /*using (StreamWriter writer = new StreamWriter(@"C:\Users\13cru\source\repos\wyniki5.txt", true))
            {
                writer.WriteLine(n + " : " + n2 + " : " + result);
                writer.Close();
            }*/

            Console.ReadLine();
        }
    }
}
