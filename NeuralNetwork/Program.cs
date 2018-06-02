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
            DataHandler handler = new DataHandler("pima-indians-diabetes.data");
            NetworkTester tester = new NetworkTester(handler.GetInput(), handler.GetTarget());
            tester.Experiment();
        }
    }
}
