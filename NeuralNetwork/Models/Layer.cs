using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Models
{
    class Layer
    {
        int inputsNum;//liczba neuronów w poprzedniej warstwie
        int outputsNum;//liczba neuronów w bieżącej warstwie
        public Layer(int inputsNum, int outputsNum)
        {
            this.inputsNum = inputsNum;
            this.outputsNum = outputsNum;
        }
    }
}