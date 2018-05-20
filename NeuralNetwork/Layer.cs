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


        public float[] outputs; //wyjscia bieżącej warstwy
        public float[] inputs; //wejścia dla bieżącej warstwy
        public float[,] weights; //wagi dla bieżącej warstwy
        public float[,] correction; //poprawki dla wag w bieżącej warstwie
        public float[] delta; //delta dla bieżącej warstwy
        public float[] error; //błąd, tylko dla warstwy wyjściowej

        public static Random random = new Random();

        public Layer(int inputsNum, int outputsNum) //konstruktor przyjmuje liczbę wejść i wyjść dla bieżącej warstwy
        {
            this.inputsNum = inputsNum;
            this.outputsNum = outputsNum;

            //ustawienie odpowiedniach rozmiarów tablic
            outputs = new float[outputsNum];
            inputs = new float[inputsNum];
            weights = new float[outputsNum, inputsNum];
            correction = new float[outputsNum, inputsNum];
            delta = new float[outputsNum];
            error = new float[outputsNum];

            InitilizeWeights(); //zainicjowanie wag
        }


        public void InitilizeWeights()
        {
            for (int i = 0; i < outputsNum; i++)
            {
                for (int j = 0; j < inputsNum; j++)
                {
                    weights[i, j] = (float)random.NextDouble() - 0.5f; //losowe wagi poczatkowe
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
                    correction[i, j] = delta[i] * inputs[j];
                }
            }
        }

        //algorytm wsteczej propagacji błędu dla warstw ukrytych
        public void BackPropHidden(float[] gammaForward, float[,] weightsFoward)
        {
            //obliczenie parametró delta
            for (int i = 0; i < outputsNum; i++)
            {
                delta[i] = 0;

                for (int j = 0; j < gammaForward.Length; j++)
                {
                    delta[i] += gammaForward[j] * weightsFoward[j, i];
                }

                delta[i] *= TanhDerivative(outputs[i]);
            }

            //obliczenie poprawek dla wag
            for (int i = 0; i < outputsNum; i++)
            {
                for (int j = 0; j < inputsNum; j++)
                {
                    correction[i, j] = delta[i] * inputs[j];
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
                    weights[i, j] -= correction[i, j] *2* 0.033f; // 0.033f to bedzie learningRate
                }
            }
        }
        
    }
}
