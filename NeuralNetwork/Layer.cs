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
        public float[] inputs; //wejścia dla bieżącej warstwy
        public float[,] weights; //wagi dla bieżącej warstwy
        public float[,] corrections; //poprawki dla wag w bieżącej warstwie
        public float[] delta; //delta dla bieżącej warstwy
        public float[] error; //błąd, tylko dla warstwy wyjściowej
        public float[,] previousWeights; //wagi z poprzedniego kroku

        public static Random random = new Random(); //instancja klasy generującej pseudolosowe liczby

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
            delta = new float[outputsNum];
            error = new float[outputsNum];
            previousWeights = new float[outputsNum, inputsNum];
            InitilizeWeights(); //zainicjowanie wag
        }

        //inicjalizacja wag
        public void InitilizeWeights()
        {
            for (int i = 0; i < outputsNum; i++)
            {
                for (int j = 0; j < inputsNum; j++)
                {
                    weights[i, j] = (float)random.NextDouble() - 0.5f; //losowe wagi początkowe
                    previousWeights[i, j] = weights[i, j]; //różnica miedzy wagami w pierwszej a poprzedniej iteracji będzie zerowa, nie mam wag z poprzedniej iteracji, bo jej nie było 
                }
            }
        }

        //obliczenie wyjść dla bieżącej warstwy
        public float[] FeedForward(float[] inputs)
        {
            this.inputs = inputs;

            for (int i = 0; i < outputsNum; i++)
            {
                outputs[i] = 0;
                //pętla licząca sumę wag, pomnożonych przez wagi
                for (int j = 0; j < inputsNum; j++)
                {
                    outputs[i] += inputs[j] * weights[i, j];
                }

                outputs[i] = (float)Math.Tanh(outputs[i]); //zastosowanie funkcji aktywacji(tangens hiperboliczny)
            }
            return outputs; //zwrócenie tablicy wyjść, które będą wejsciami dla kolejnej warstwy
        }

        //funkcja obliczająca wartość pochodznej tangensa hiperbolicznego
        public float TanhDerivative(float value)
        {
            return 1 - (value * value);
        }

        //algorytm wsteczej propagacji błędu dla warstwy wyjściowej
        public void BackPropOutput(float[] target)
        {
            //obliczenie wartości błędu dla każdego z wyjść
            for (int i = 0; i < outputsNum; i++)
                error[i] = outputs[i] - target[i]; //błąd obliczam jako różnicę między tym co powinno byc na wyjściu a co jest

            //obliczenie parametru delta dla każdego z wyjść
            for (int i = 0; i < outputsNum; i++)
                delta[i] = error[i] * TanhDerivative(outputs[i]);

            //obliczenie poprawek wag dla wejść
            for (int i = 0; i < outputsNum; i++)
            {
                for (int j = 0; j < inputsNum; j++)
                {
                    corrections[i, j] = delta[i] * inputs[j];
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
            for (int i = 0; i < outputsNum; i++)
            {
                for (int j = 0; j < inputsNum; j++)
                {
                    weights[i, j] -= corrections[i, j] *learningRate + alpha * (weights[i, j] - previousWeights[i, j]); // 0.033f to będzie learningRate
                }
            }
            for (int i = 0; i < outputsNum; i++)
            {
                for (int j = 0; j < inputsNum; j++)
                {
                    previousWeights[i, j] = weights[i, j]; //zapamiętuję poprzednie wagi dla warstwy
                }
            }
        }
        
    }
}
