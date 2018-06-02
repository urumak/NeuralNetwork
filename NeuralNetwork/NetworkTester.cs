using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class NetworkTester
    {
        float learningRate = 0.01f;
        double previousSSE = 0.0;
        float er = 1.04f;
        float erInc = 0.01f;
        float erMax = 1.055f;
        float lrInc = 1.05f;
        float lrIncInc = 0.01f;
        float lrIncMax = 1.091f;
        float lrDec = 0.7f;
        float lrDecInc = 0.1f;
        float lrDecMax = 0.91f;
        float momentum = 0.2f;
        float momentumInc = 0.1f;
        float momentumMax = 0.3f;
        int neurons1 = 30;
        int n1Inc = 4;
        int n1Max = 42;
        int neurons2 = 20;
        int n2Inc = 4;
        int n2Max = 26;
        int maxEpoch = 100000;
        int epoch = 0;
        double sse = 0;
        float result = 0;
        float errorGoal = 40f;
        int loopInd = 0;
        float zerosResult = 0;
        float onesResult = 0;
        int modulo = 0;
        TimeSpan interval = new TimeSpan();
        List<List<float>> input = new List<List<float>>();
        List<List<float>> target = new List<List<float>>();

        public NetworkTester(List<List<float>> input, List<List<float>> target)
        {
            this.input = input;
            this.target = target;
        }

        public void Experiment()
        {
            for (neurons1 = 35; neurons1 < 36; neurons1 += n1Inc)
            {
                for (neurons2 = 20; neurons2 < 21; neurons2 += n2Inc)
                {
                    for (lrInc = 1.03f; lrInc < 1.035; lrInc += lrIncInc)
                    {
                        for (lrDec = 0.8f; lrDec < 0.85; lrDec += lrDecInc)
                        {
                            for (er = 1.05f; er < 1.055; er += erInc)
                            {
                                for (momentum = 0.2f; momentum < 0.25; momentum += 0.1f)
                                {
                                    //for (errorGoal = 60f; errorGoal > 35; errorGoal -= 10f)
                                    //{
                                    for (modulo = 6; modulo < 10; modulo++)
                                    {
                                        for (int i = 0; i < 3; i++)
                                        {
                                            learningRate = 0.1f;
                                            DateTime startTime = DateTime.Now;
                                            Network net = new Network(new int[] { input[0].Count, neurons1, neurons2,target[0].Count }, learningRate, momentum);
                                            Train(net);
                                            DateTime stopTime = DateTime.Now;
                                            interval = stopTime - startTime;
                                            Test(net);
                                            Save();
                                            //loopInd++;
                                            //Console.WriteLine(loopInd / 35 * 100);
                                        }
                                    }
                                   // }
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
                    if (j % 10 != modulo)
                    {
                        float output = net.StartFeedForward(input[j].ToArray())[0];
                        sse += Math.Pow(output - target[j][0], 2);
                        net.BackPropagation(target[j].ToArray());
                    }
                }
                if (sse < errorGoal || sse > 999999)
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
                    float procent = (((float)epoch + 1) / maxEpoch) * 100;
                    Console.WriteLine("epoch {0} / {1}, sse {2}, lr {3}", epoch, maxEpoch, sse, learningRate);
                }
            }
        }

        public void Test(Network net)
        {
            float good = 0;
            float wrong = 0;
            float ones = 0;
            float zeros = 0;
            float onesGood = 0;
            float zerosGood = 0;

            for (int i = 0; i < input.Count; i++)
            {
                if (i % 10 == modulo)
                {
                    if (target[i][0] == 1)
                    {
                        ones++;
                    }
                    else
                    {
                        zeros++;
                    }
                    float output = net.StartFeedForward(input[i].ToArray())[0];
                    Console.WriteLine("{0}\t{1}", output, target[i][0]);
                    if (Math.Abs(output - target[i][0]) < 0.5f)
                    {
                        good++;
                        if (target[i][0] == 1)
                        {
                            onesGood++;
                        }
                        else
                        {
                            zerosGood++;
                        }
                    }
                    else
                    {
                        wrong++;
                    }
                }
            }
            result = (good / (good + wrong)) * 100;
            onesResult = (onesGood / (ones) * 100);
            zerosResult = (zerosGood / (zeros) * 100);
            Console.WriteLine("{0}, {1}, {2}, {3}, {4}", result, onesResult, zerosResult, ones, zeros);
        }

        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(@"C:\Users\13cru\source\repos\krzyzbez.txt", true))
            {
                writer.WriteLine(neurons1 + "\t" + neurons2 + "\t" + lrInc + "\t" + lrDec + "\t" + momentum
                    + "\t" + er + "\t" + result + "\t" + onesResult + "\t" + zerosResult + "\t" + epoch + "\t" + sse + "\t" + interval.TotalMinutes + "\t" + modulo);
                writer.Close();
            }
        }
    }
}
