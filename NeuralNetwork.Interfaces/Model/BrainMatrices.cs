using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuralNetwork.Interfaces.Model
{
    public class Matrix
    {
        private readonly float[] _matrix;
        private readonly int _rowNumber;
        private readonly int _columnNumber;

        public Matrix(int rows, int columns)
        {
            _matrix = new float[rows * columns];
            _rowNumber = rows;
            _columnNumber = columns;
        }

        public void SetElement(int i, int j, float value)
        {
            _matrix[i * _columnNumber + j] = value;
        }

        public override string ToString()
        {
            var result = new StringBuilder($"{_matrix[0]}");

            for (int i = 1; i < _matrix.Length; i++)
                result.Append($";{_matrix[i]}");

            return result.ToString();
        }
    }

    public class BrainMatrices
    {
        public Matrix NeutralMatrix { get; set; }

        public Matrix OutputMatrix { get; set; }

        public BrainMatrices(NetworkCaracteristics networkCaracteristics)
        {
            NeutralMatrix = new Matrix(networkCaracteristics.InputNumber, networkCaracteristics.NeutralNumber);
            OutputMatrix = new Matrix(networkCaracteristics.InputNumber + networkCaracteristics.NeutralNumber, networkCaracteristics.OutputNumber);
        }
    }
}
