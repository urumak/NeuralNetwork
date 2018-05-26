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
        List<List<float>> target = new List<List<float>>();
        List<string> helpList = new List<string>();
        string filename;

        public DataHandler (string filename)
        {
            this.filename = filename;
            Read();
            RemoveSeparator();
            splittedList = DeleteUselessData();
            ConvertToFloat();
            Normalizer norm = new Normalizer();
            input = norm.Normalize(input);
            //target = norm.Normalize(target);
            //ExportToTxt();
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
            using (StreamWriter writer = new StreamWriter(@"C:\Users\13cru\source\repos\data.txt"))
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
                if (list[2] == "0" || list[3] == "0" || list[5] == "0" || list[5] == "0.0" || list[7] == "0")
                {
                    continue;
                }
                newList.Add(list);
            }
            return newList;
        }
    }
}
