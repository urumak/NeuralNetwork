using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace NeuralNetwork.Data
{
    class DataHandler
    {
        List<List<float>> input = new List<List<float>>();
        List<List<string>> splittedList = new List<List<string>>();
        List<List<float>> target = new List<List<float>>();
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

        public List<List<float>> GetTarget()
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

        private void ExportToTxt()
        {
            using (StreamWriter writer = new StreamWriter("data.txt"))
            {
                foreach (List<string> list in splittedList)
                {
                    string temp = null;
                    for (int i = 0; i < list.Count; i++)
                    {
                        temp += list[i];
                        if (i != list.Count - 1)
                        {
                            temp += ',';
                        }
                    }
                    writer.WriteLine(temp);
                }
                writer.Close();
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
                List<float> targetTemp = new List<float>();
                targetTemp.Add(float.Parse(splittedList[i][j], CultureInfo.InvariantCulture));
                target.Add(targetTemp);
                input.Add(temp);
            }
        }

        private List<List<string>> DeleteUselessData()
        {
            List<List<string>> newList = new List<List<string>>();
            foreach(List<string> list in splittedList)
            {
                if (list[1] == "0" || list[2] == "0" || list[3] == "0" || list[4] == "0" || list[5] == "0" || list[5] == "0.0" || list[6] == "0" || list[7] == "0")
                {
                    continue;
                }
                newList.Add(list);
            }
            return newList;
        }
        private void CountNum()
        {
            int one = 0;
            int zero = 0;
            int onet = 0;
            int zerot = 0;
            int i = 0;
            int all = 0;
            int allt = 0;
            List<List<string>> newList = new List<List<string>>();
            foreach (List<float> list in target)
            {
                if (i % 3 != 0)
                {
                    if (list[0] == 1)
                    {
                        one++;
                    }
                    else
                    {
                        zero++;
                    }
                    all++;
                }
                else
                {
                    if (list[0] == 1)
                    {
                        onet++;
                    }
                    else
                    {
                        zerot++;
                    }
                    allt++;
                }
                i++;
            }
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}", one, zero, onet, zerot, all, allt);
            Console.ReadLine();
        }
    }
}
