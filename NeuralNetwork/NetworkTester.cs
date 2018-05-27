using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class NetworkTester
    {
        float learningRate = 0.01f;
        double previousSSE = 0.0;
        float er = 1.04f;
        float erInc = 0.01f;
        float erMax = 1.05f;
        float lrInc = 1.05f;
        float lrIncInc = 0.01f;
        float lrIncMax = 1.06f;
        float lrDec = 0.7f;
        float lrDecInc = 0.1f;
        float lrDecMax = 0.8f;
        float momentum = 0.2f;
        float momentumInc = 0.1f;
        float momentumMax = 0.3f;
        int neurons1 = 45;
        int n1Inc = 5;
        int n1Max = 50;
        int neurons2 = 38;
        int n2Inc = 2;
        int n2Max = 40;
        int maxEpoch = 10000;
        int epoch = 0;
        double sse = 0;
        float result = 0;
        float errorGoal = 0.25f;
        List<List<float>> input = new List<List<float>>();
        List<List<float>> target = new List<List<float>>();

        public NetworkTester(List<List<float>> input, List<List<float>> target)
        {
            this.input = input;
            this.target = target;
        }

        public void Experiment()
        {
            for (neurons1 = 25; neurons1 < n1Max; neurons1 += n1Inc)
            {
                for (neurons2 = 32; neurons2 < n2Max; neurons2 += n2Inc)
                {
                    for (lrInc = 1.05f; lrInc < lrIncMax; lrInc += lrIncInc)
                    {
                        for (lrDec = 0.7f; lrDec < lrDecMax; lrDec += lrDecInc)
                        {
                            for (er = 1.04f; er < erMax; er += erInc)
                            {
                                for (momentum = 0.2f; momentum < 0.4; momentum += 0.5f)
                                {
                                    sse = 0;
                                    Network net = new Network(new int[] { input[0].Count, neurons1, neurons2, target[0].Count }, learningRate, momentum);
                                    Train(net);
                                    Test(net);
                                    Save();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Train(Network net)
        {
            for (epoch = 0; epoch < maxEpoch; epoch++)
            {
                net.ChangeLearningRate(learningRate);
                sse = 0;
                for (int j = 0; j < input.Count; j++)
                {
                    float output = net.StartFeedForward(input[j].ToArray())[0];
                    sse += Math.Pow(output - target[j][0], 2);
                    net.BackPropagation(target[j].ToArray());
                }
                if (sse < errorGoal || sse == double.NaN)
                {
                    break;
                }
                if (epoch != 0)
                {
                    if (sse > er * previousSSE)
                    {
                        learningRate *= lrDec;
                    }
                    if (sse < previousSSE)
                    {
                        learningRate *= lrInc;
                    }
                }
                previousSSE = sse;
                if (epoch % 100 == 0)
                {
                    float procent = (((float)epoch  + 1) / maxEpoch) * 100;
                    Console.WriteLine("epoch {0} / {1}, sse {2}, lr {3}", epoch, maxEpoch, sse, learningRate);
                }
            }
        }

        public void Test(Network net)
        {
            float good = 0;
            float wrong = 0;

            for (int i = 0; i < input.Count; i++)
            {
                float output = net.StartFeedForward(input[i].ToArray())[0];
                //Console.WriteLine("{0}\t{1}", output, target[i][0]);
                if (Math.Abs(output - target[i][0]) < 0.5f)
                {
                    good++;
                }
                else
                {
                    wrong++;
                }
            }
            result = (good / (good + wrong)) * 100;
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(@"C:\Users\13cru\source\repos\wyniki7.txt", true))
            {
                writer.WriteLine(neurons1 + " : " + neurons2 + " : " + lrInc + " : " + lrDec + " : " + momentum 
                    + " : " + er + " : " + result + " : " + epoch + " : " + sse);
                writer.Close();
            }
        }
    }
}
