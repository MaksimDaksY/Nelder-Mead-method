using System;

namespace NelderMeadCore
{
    public class Vector
    {
        public double[] Coordinates;

        public Vector(int size)
        {
            Coordinates = new double[size];
            for (int i = 0; i < size; i++)
            {
                Coordinates[i] = 0;
            }
        }

        public Vector(double[] coordinates)
        {
            Coordinates = new double[coordinates.Length];
            for (int i = 0; i < coordinates.Length; i++)
            {
                Coordinates[i] = coordinates[i];
            }
        }

        public static Vector Sum(Vector[] vectors)
        {
            Vector sum = new Vector(vectors[0].Coordinates.Length);
            for (int i = 0; i < vectors.Length; i++)
            {
                sum += vectors[i];
            }
            return sum;
        }

        public double Norm()
        {
            double norm = 0;
            for (int i = 0; i < Coordinates.Length; i++)
            {
                norm += Math.Pow(Coordinates[i], 2);
            }
            return Math.Sqrt(norm);
        }


        public static Vector operator +(Vector a, Vector b)
        {
            if (a.Coordinates.Length != b.Coordinates.Length)
                throw new ArgumentException("Vector lengths must match");

            Vector result = new Vector(a.Coordinates.Length);
            for (int i = 0; i < a.Coordinates.Length; i++)
            {
                result.Coordinates[i] = a.Coordinates[i] + b.Coordinates[i];
            }
            return result;
        }

        public static Vector operator -(Vector a, Vector b)
        {
            if (a.Coordinates.Length != b.Coordinates.Length)
                throw new ArgumentException("Vector lengths must match");

            Vector result = new Vector(a.Coordinates.Length);
            for (int i = 0; i < a.Coordinates.Length; i++)
            {
                result.Coordinates[i] = a.Coordinates[i] - b.Coordinates[i];
            }
            return result;
        }

        public static Vector operator *(Vector v, double scalar)
        {
            Vector result = new Vector(v.Coordinates);
            for (int i = 0; i < v.Coordinates.Length; i++)
            {
                result.Coordinates[i] *= scalar;
            }
            return result;
        }

        public static Vector operator *(double scalar, Vector v)
        {
            return v * scalar;
        }

        public static Vector operator /(Vector v, double scalar)
        {
            if (scalar == 0)
                throw new DivideByZeroException("Division by zero");

            Vector result = new Vector(v.Coordinates);
            for (int i = 0; i < v.Coordinates.Length; i++)
            {
                result.Coordinates[i] /= scalar;
            }
            return result;
        }

        public static Vector operator -(Vector v)
        {
            return v * -1;
        }
    }
}