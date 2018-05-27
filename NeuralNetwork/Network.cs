using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Network
    {
        int[] layersSize; //ilosc neuronów w kolejnych warstwach
        Layer[] layers; //tablica warstw w sieci
        float learningRate; //współczynnik uczenia teta
        float alpha; //współczynnik alfa dla momentum

        public Network(int[] layersSize, float learningRate, float alpha)
        {
            this.layersSize = new int[layersSize.Length];
            this.alpha = alpha;
            this.learningRate = learningRate;

            for (int i = 0; i < layersSize.Length; i++)
                this.layersSize[i] = layersSize[i];

            layers = new Layer[layersSize.Length - 1];

            for (int i = 0; i < layers.Length; i++)
            {
                layers[i] = new Layer(layersSize[i], layersSize[i + 1], learningRate, alpha); //tutaj wywołuję konstruktor z parametrami które określają ilosć wejsć i wyjść w danej warstwie
            }
        }

        //oblicznie wyjść, dla pierwszej warstwy korzystam z danych wejściowych, dalej z tego co wyrzucą kolejne warstwy
        //dla warstwy wyjściowej nie licze wyjścia
        public float[] StartFeedForward(float[] inputs)
        {
            layers[0].FeedForward(inputs);
            for (int i = 1; i < layers.Length - 1; i++)
            {
                layers[i].FeedForward(layers[i - 1].outputs);
            }
            layers[layers.Length - 1].FeedForward(layers[layers.Length - 2].outputs, true);
            return layers[layers.Length - 1].outputs;
        }

        //algorytm wstecznej propagacji błędu
        public void BackPropagation(float[] target)
        {
            //jak wsteczna propagacja to tzrba zrobić pętlę w druga stronę
            for (int i = layers.Length - 1; i >= 0; i--)
            {
                // jeśli mamy do czynienia z warstwą wyjściową
                if (i == layers.Length - 1)
                {
                    layers[i].BackPropOutput(target);
                }
                //jeśli mamy do czynienia z warstwą ukrytą
                else
                {
                    layers[i].BackPropHidden(layers[i + 1].delta, layers[i + 1].weights);
                }
            }
            //aktualizacja wag
            for (int i = 0; i < layers.Length; i++)
            {
                layers[i].UpdateWeights();
            }
        }

        public void ChangeLearningRate(float learningRate)
        {
            foreach(Layer layer in layers)
            {
                layer.ChangeLearningRate(learningRate);
            }
        }
    }
}
