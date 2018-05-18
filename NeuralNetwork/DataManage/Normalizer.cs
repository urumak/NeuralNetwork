using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Data
{
    class Normalizer
    {
        List<List<float>> transposedList = new List<List<float>>();

        public List<List<float>> Normalize(List<List<float>> input)
        {
            List<List<float>> normalizedList = new List<List<float>>();
            input = Transpose(input);
            foreach (List<float> list in input)
            {
                List<float> temp = new List<float>();
                foreach (float number in list)
                {
                    //normalizacja -1 1
                    float norm = ((number - list.Min()) / (list.Max() - list.Min())) * 2 - 1;
                    //normalizacja 0 1
                    //float norm = ((number - list.Min()) / (list.Max() - list.Min()))
                    temp.Add(norm);
                }
                Console.WriteLine("{0}, {1}", list.Max(), list.Min());
                normalizedList.Add(temp);
            }
            for (int i = 0; i< normalizedList.Count; i++)
            {
                Console.Write(normalizedList[i].Min());
                Console.WriteLine(normalizedList[i].Max());
            }
            normalizedList = Transpose(normalizedList);
            return normalizedList;
        }


        List<List<float>> Transpose(List<List<float>> input)
        {
            List<List<float>> helpList = new List<List<float>>();
            for (int i = 0; i < input[0].Count; i++)
            {
                List<float> temp = new List<float>();
                for (int j = 0; j < input.Count; j++)
                {
                    temp.Add(input[j][i]);
                }
                helpList.Add(temp);
            }
            return helpList;
        }
    }
}
