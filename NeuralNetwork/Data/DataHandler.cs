using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Data
{
    class DataHandler
    {
        List<List<float>> input = new List<List<float>>();
        List<List<string>> splittedList = new List<List<string>>();
        List<float> target = new List<float>();
        List<string> helpList = new List<string>();
        string filename;

        public DataHandler (string filename)
        {
            this.filename = filename;
            Read();
            RemoveSeparator();
            ConvertToFloat();
            Normalizer norm = new Normalizer();
            input = norm.Normalize(input);
        }


        public List<List<float>> GetInput()
        {
            return input;
        }

        public List<float> GetTarget()
        {
            return target;
        }

        private void Read()
        {
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        helpList.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        private void RemoveSeparator()
        {
            foreach (string line in helpList)
            {
                List<string> temp = new List<string>();
                char separator = ',';
                temp = line.Split(separator).ToList();
                splittedList.Add(temp);
            }
        }

        private void ConvertToFloat()
        {
            for (int i = 0; i < splittedList.Count; i++)
            {
                List<float> temp = new List<float>();
                int j;
                for (j = 0; j < splittedList[i].Count - 1; j++)
                {
                    float number;
                    number = float.Parse(splittedList[i][j], CultureInfo.InvariantCulture);
                    temp.Add(number);
                }
                target.Add(float.Parse(splittedList[i][j], CultureInfo.InvariantCulture));
                input.Add(temp);
            }
        }
    }
}
