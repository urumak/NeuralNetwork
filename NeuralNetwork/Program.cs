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
            int i = 0;
            foreach (List<float> list in input)
            {
                Console.Write("{0}. ", i + 1);
                foreach (float number in list)
                {
                    Console.Write("{0} ", number);
                }
                Console.Write("  {0}", target[i][0]);
                Console.WriteLine();
                i++;
            }
            Console.ReadLine();
        }
    }
}
