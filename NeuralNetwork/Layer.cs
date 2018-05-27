using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    class Layer
    {
        int inputsNum; //liczba neuronów w poprzedniej warstwie
        int outputsNum; //liczba neuronów w bieżącej warstwie
        float learningRate; //współczynnik uczenia teta
        float alpha; //współczynnik alfa dla momentum

        public float[] outputs; //wyjscia bieżącej warstwy
        float[] inputs; //wejścia dla bieżącej warstwy
        public float[,] weights; //wagi dla bieżącej warstwy
        float[] bias; //bias dla bieżącej warstwy
        float[,] corrections; //poprawki dla wag w bieżącej warstwie
        public float[] delta; //delta dla bieżącej warstwy
        float[] error; //błąd, tylko dla warstwy wyjściowej
        float[,] previousWeights; //wagi z poprzedniego kroku
        float[] previousBias;

        static Random random = new Random(); //instancja klasy generującej pseudolosowe liczby

        public Layer(int inputsNum, int outputsNum, float learningRate, float alpha) //konstruktor przyjmuje liczbę wejść i wyjść dla bieżącej warstwy
        {
            this.inputsNum = inputsNum;
            this.outputsNum = outputsNum;
            this.alpha = alpha;
            this.learningRate = learningRate;

            //ustawienie odpowiedniach rozmiarów tablic
            outputs = new float[outputsNum];
            inputs = new float[inputsNum];
            weights = new float[outputsNum, inputsNum];
            corrections = new float[outputsNum, inputsNum];
            bias = new float[outputsNum];
            delta = new float[outputsNum];
            error = new float[outputsNum];
            previousWeights = new float[outputsNum, inputsNum];
            previousBias = new float[outputsNum];
            InitilizeWeights(); //zainicjowanie wag
        }

        //inicjalizacja wag
        void InitilizeWeights()
        {
            for (int i = 0; i < outputsNum; i++)
            {
                for (int j = 0; j < inputsNum; j++)
                {
                    weights[i, j] = (float)random.NextDouble() - 0.5f; //losowe wagi początkowe
                    previousWeights[i, j] = weights[i, j]; //różnica miedzy wagami w pierwszej a poprzedniej iteracji będzie zerowa, nie mam wag z poprzedniej iteracji, bo jej nie było 
                }
                bias[i] = (float)random.NextDouble() - 0.5f;
                previousBias[i] = bias[i];
            }
        }

        //obliczenie wyjść dla bieżącej warstwy
        public float[] FeedForward(float[] inputs, bool last = false)
        {
            this.inputs = inputs;

            for (int i = 0; i < outputsNum; i++)
            {
                outputs[i] = 0;
                //pętla licząca sumę wejść, pomnożonych przez wagi
                for (int j = 0; j < inputsNum; j++)
                {
                    outputs[i] += inputs[j] * weights[i, j];
                }
                outputs[i] += bias[i];
                if(!last)
                {
                    outputs[i] = (float)Math.Tanh(outputs[i]); //zastosowanie funkcji aktywacji(tangens hiperboliczny)
                    // nie robie tego, jeśli mam do czynienia z ostatnia warstwą, gdzie przyjmuję liniową funkcję aktywacji y = x
                }
            }
            return outputs; //zwrócenie tablicy wyjść
        }

        //funkcja obliczająca wartość pochodznej tangensa hiperbolicznego
        float TanhDerivative(float value)
        {
            return 1 - (value * value);
        }

        //algorytm wsteczej propagacji błędu dla warstwy wyjściowej
        public void BackPropOutput(float[] target)
        {
            //obliczenie wartości błędu dla każdego z wyjść
            for (int i = 0; i < outputsNum; i++)
                error[i] = outputs[i] - target[i]; //błąd obliczam jako różnicę między tym co powinno byc na wyjściu a co jest
            //neuron wyjściowy jest neuronem liniowym, nie mnożę go przez pochodną funkcji aktywacji, ponieważ wynosi ona 1

            //obliczenie poprawek wag dla wejść
            for (int i = 0; i < outputsNum; i++)
            {
                for (int j = 0; j < inputsNum; j++)
                {
                    corrections[i, j] = error[i] * inputs[j];
                }
            }
        }

        //algorytm wsteczej propagacji błędu dla warstw ukrytych
        public void BackPropHidden(float[] deltaForward, float[,] weightsFoward)
        {
            //obliczenie parametrów delta
            for (int i = 0; i < outputsNum; i++)
            {
                delta[i] = 0;

                for (int j = 0; j < deltaForward.Length; j++)
                {
                    delta[i] += deltaForward[j] * weightsFoward[j, i];
                }

                delta[i] *= TanhDerivative(outputs[i]);
            }

            //obliczenie poprawek dla wag
            for (int i = 0; i < outputsNum; i++)
            {
                for (int j = 0; j < inputsNum; j++)
                {
                    corrections[i, j] = delta[i] * inputs[j];
                }
            }
        }


        //aktualizacja wag
        public void UpdateWeights()
        {
            float[,] temp = new float[outputsNum, inputsNum];
            float[] biasTemp = new float[outputsNum];

            for (int i = 0; i < outputsNum; i++)
            {
                for (int j = 0; j < inputsNum; j++)
                {
                    temp[i, j] = weights[i, j]; //zapamiętuję poprzednie wagi dla warstwy
                    weights[i, j] -= corrections[i, j] *learningRate + alpha * (weights[i, j] - previousWeights[i, j]); // 0.033f to będzie learningRate
                    previousWeights[i, j] = temp[i, j]; //zapamiętuję poprzednie wagi dla warstwy
                }
                biasTemp[i] = bias[i]; //zapamiętuję poprzednie wagi dla warstwy
                bias[i] -= delta[i] * learningRate + alpha * (bias[i] - previousBias[i]); // 0.033f to będzie learningRate
                previousBias[i] = biasTemp[i]; //zapamiętuję poprzednie wagi dla warstwy
            }
        }
        

        public void ChangeLearningRate(float learningRate)
        {
            this.learningRate = learningRate;
        }
    }
}
