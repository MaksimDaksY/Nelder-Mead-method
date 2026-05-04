using System;

namespace NelderMeadCore
{
    public class Vector
    {
        private double[] _coordinates;

        public int Length => _coordinates.Length;

        public double this[int index]
        {
            get => _coordinates[index];
            set => _coordinates[index] = value;
        }

        public Vector(int size)
        {
            _coordinates = new double[size];
        }

        public Vector(double[] coordinates)
        {
            _coordinates = new double[coordinates.Length];
            Array.Copy(coordinates, _coordinates, coordinates.Length);
        }

        public double[] ToArray() => (double[])_coordinates.Clone();

        public static Vector Sum(Vector[] vectors)
        {
            if (vectors == null || vectors.Length == 0)
                throw new ArgumentException("At least one vector required");
            int size = vectors[0].Length;
            Vector sum = new Vector(size);
            for (int i = 0; i < vectors.Length; i++)
            {
                if (vectors[i].Length != size)
                    throw new ArgumentException("All vectors must have the same length");
                for (int j = 0; j < size; j++)
                    sum[j] += vectors[i][j];
            }
            return sum;
        }

        public double Norm()
        {
            double norm = 0;
            for (int i = 0; i < _coordinates.Length; i++)
                norm += Math.Pow(_coordinates[i], 2);
            return Math.Sqrt(norm);
        }

        public static Vector operator +(Vector a, Vector b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Vector lengths must match");
            Vector result = new Vector(a.Length);
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i] + b[i];
            return result;
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Vector lengths must match");
            Vector result = new Vector(a.Length);
            for (int i = 0; i < a.Length; i++)
                result[i] = a[i] - b[i];
            return result;
        }

        public static Vector operator *(Vector v, double scalar)
        {
            Vector result = new Vector(v.Length);
            for (int i = 0; i < v.Length; i++)
                result[i] = v[i] * scalar;
            return result;
        }

        public static Vector operator *(double scalar, Vector v) => v * scalar;

        public static Vector operator /(Vector v, double scalar)
        {
            if (scalar == 0)
                throw new DivideByZeroException("Division by zero");
            Vector result = new Vector(v.Length);
            for (int i = 0; i < v.Length; i++)
                result[i] = v[i] / scalar;
            return result;
        }

        public static Vector operator -(Vector v) => v * -1;
    }
}