using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetworks
{
    public class Neuron
    {
        private double Sigmoid(double xValue)
        {
            return 1 / (1 + Math.Exp(-xValue));
        }

        public double dSigmoid(double xValue)
        {
            return Sigmoid(xValue) * (1 - Sigmoid(xValue));
        }

        public int globalIndex = -1;
        public int localIndex = -1;

        public double lastIn;
        public double result;

        public int layerIndex;

        public Neuron(int globalInd, int layerInd, int locInd)
        {
            this.globalIndex = globalInd;
            this.localIndex = locInd;
        }

        public void Sinaps(double value)
        {
            this.lastIn += value;
        }

        public void Activate()
        {
            this.result = this.Sigmoid(lastIn);
        }
    }

    public class NeuronLayer
    {
        public Neuron[] neurons;
        public int neuronsCount;

        public int layerIndex;

        public NeuronLayer(int neuCount, int lIndex)
        {
            this.neuronsCount = neuCount;
            neurons = new Neuron[this.neuronsCount];

            this.layerIndex = lIndex;
        }

        public void AddNeuron(int index, Neuron n)
        {
            this.neurons[index] = n;
        }

        public void SetInputLayer(double[] input)
        {
            for (int i = 0; i < this.neurons.Length; i++)
            {
                this.neurons[i].result = input[i];
            }
        }

        public void ProcessInput(double[,] weights, Neuron[] allNeurons)
        {
            if (this.layerIndex != 0)
            {
                this.ActivateLayer();
            }
            for (int i = 0; i < this.neurons.Length; i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    if (weights[this.neurons[i].globalIndex, j] != double.NegativeInfinity)
                    {
                        allNeurons[j].Sinaps(weights[this.neurons[i].globalIndex, j] * this.neurons[i].result);
                    }
                }
            }
        }

        public void ActivateLayer()
        {
            for (int i = 0; i < this.neurons.Length; i++)
            {
                neurons[i].Activate();
            }
        }

        public void ConnectLayer(NeuronLayer next, double[,] weights)
        {
            for (int i = 0; i < this.neurons.Length; i++)
            {
                for (int j = 0; j < next.neurons.Length; j++)
                {
                    weights[this.neurons[i].globalIndex, next.neurons[j].globalIndex] = 1;
                }
            }
        }

        public double[] GetResultsFromNeurons()
        {
            var result = new double[this.neurons.Length];

            for (int i = 0; i < this.neurons.Length; i++)
            {
                result[i] = this.neurons[i].result;
            }

            return result;
        }

        public void ClearLastIn()
        {
            for (int i = 0; i < this.neurons.Length; i++)
            {
                this.neurons[i].lastIn = 0;
            }
        }
    }

    public class NeuralNetwork
    {
        private static Random randomizer = new Random();

        public Neuron[] neurons;
        public NeuronLayer[] layers;
        public double[,] weights;

        public int[] topology;

        public int layersCount;

        public double error = double.PositiveInfinity;

        public NeuralNetwork(int[] _topology)
        {
            topology = _topology;

            layersCount = topology.Length;
            layers = new NeuronLayer[layersCount];
            int count = 0;
            for (int i = 0; i < topology.Length; i++)
            {
                count += topology[i];
                layers[i] = new NeuronLayer(topology[i], i);
            }

            neurons = new Neuron[count];

            weights = new double[count, count];

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    weights[i, j] = double.NegativeInfinity;
                }
            }

            int index = 0;
            for (int i = 0; i < layers.Length; i++)
            {
                for (int j = 0; j < topology[i]; j++)
                {
                    neurons[index] = new Neuron(index, i, j);
                    layers[i].AddNeuron(j, neurons[index]);
                    index++;

                }
            }
            for (int i = 0; i < layers.Length - 1; i++)
            {
                layers[i].ConnectLayer(layers[i + 1], weights);
            }

            this.RandomizeWeights();
        }

        public void RandomizeWeights()
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    if (weights[i, j] != double.NegativeInfinity)
                    {
                        weights[i, j] = (randomizer.NextDouble() - 0.5d);
                    }
                }
            }
        }

        public double[] ProcessInput(double[] input)
        {
            this.layers[0].SetInputLayer(input);
            for (int i = 0; i < this.layers.Length; i++)
            {
                this.layers[i].ClearLastIn();
            }
            for (int i = 0; i < this.layers.Length; i++)
            {
                this.layers[i].ProcessInput(weights, neurons);
            }

            return this.layers[this.layers.Length - 1].GetResultsFromNeurons();
        }

        public double[,] GetCopy()
        {
            var nWeights = new double[weights.GetLength(0), weights.GetLength(1)];
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    nWeights[i, j] = weights[i, j];
                }
            }

            return nWeights;
        }

        public void SetFromCopy(double[,] w)
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    this.weights[i, j] = w[i, j];
                }
            }
        }
    }

    public class TrainSet2
    {
        public static List<List<double[]>> Data = new List<List<double[]>>()
        {
            new List<double[]>() { new double[] {0,0}, new double[] {0} },
            new List<double[]>() { new double[] {0,1}, new double[] {1} },
            new List<double[]>() { new double[] {1,0}, new double[] {1} },
            new List<double[]>() { new double[] {1,1}, new double[] {0} },
        };
    }

    public class TrainSet
    {
        public List<List<double[]>> Data = new List<List<double[]>>()
        {
        };
    }

    class EvolutionController
    {
        private static Random randomizer = new Random();

        public static double[,] Cross(double[,] w1, double[,] w2)
        {
            var result = new double[w1.GetLength(0), w2.GetLength(1)];

            for (int i = 0; i < w1.GetLength(0); i++)
            {
                for (int j = 0; j < w1.GetLength(1); j++)
                {
                    result[i, j] = w1[i, j] + w2[i, j];
                    result[i, j] /= 2;
                }
            }

            return result;
        }

        public static double[,] Mutate(double[,] w1, double error)
        {
            var result = new double[w1.GetLength(0), w1.GetLength(1)];

            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = w1[i, j] + (randomizer.NextDouble() - 0.5d) * 0.1d * error;
                }
            }

            return result;
        }
    }

    public class NeuralNetworkData
    {
        public double[,] weights;

        public TrainSet Data;

        public NeuralNetworkData()
        {
            Data = new TrainSet();
        }

        public void AddToData(double[] input, double[] output)
        {
            var data = Data.Data;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i][0] == input)
                {
                    //UnityEngine.Debug.Log(string.Format("{0} is bad!", input));
                    return;
                }
            }

            Data.Data.Add(new List<double[]>() { input, output });
        }
    }
}