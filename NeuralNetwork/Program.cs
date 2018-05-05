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
            DataHandler handler = new DataHandler();
            handler.PrepareData();
            input = handler.GetInput();
            int i = 1;
            foreach (List<float> list in input)
            {
                Console.Write("{0}. ", i);
                foreach (float number in list)
                {
                    Console.Write("{0} ", number);
                }
                Console.WriteLine();
                i++;
            }
            Console.ReadLine();
        }
    }
}
