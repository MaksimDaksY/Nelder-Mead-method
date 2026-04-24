using System;
using System.Linq;          // добавлено для ToArray()
using NelderMeadParser;

namespace NelderMeadCore
{
    public class Simplex
    {
        private Vector[] _points;

        public IReadOnlyList<Vector> Points => Array.AsReadOnly(_points);
        public int Size => _points.Length;

        public Simplex(Vector[] points)
        {
            _points = new Vector[points.Length];
            for (int i = 0; i < points.Length; i++)
                _points[i] = new Vector(points[i].ToArray());
        }

        public void SetPoint(int index, Vector newPoint)
        {
            if (index < 0 || index >= _points.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
            _points[index] = new Vector(newPoint.ToArray());
        }

        public Vector GetPointCopy(int index) => new Vector(_points[index].ToArray());

        public void SortPointsByResult(Function goalFunction)
        {
            double[] functionResults = new double[_points.Length];
            for (int i = 0; i < _points.Length; i++)
                functionResults[i] = goalFunction.Evaluate(_points[i].ToArray());

            Array.Sort(functionResults, _points);
        }

        public void Shrink(double shrinkCoefficient)
        {
            for (int i = 1; i < _points.Length; i++)
                _points[i] = _points[0] + (_points[i] - _points[0]) * shrinkCoefficient;
        }
    }

    public class NelderMeadAlgorithm
    {
        public double ReflectionCoefficient;
        public double ExpansionCoefficient;
        public double ContractCoefficient;
        public Simplex CurrentSimplex;
        public Function GoalFunction;
        public bool EndReached;

        public NelderMeadAlgorithm(double alpha, double beta, double gamma, Simplex simplex, Function goalFunction)
        {
            ReflectionCoefficient = alpha;
            ExpansionCoefficient = beta;
            ContractCoefficient = gamma;
            CurrentSimplex = simplex;
            GoalFunction = goalFunction;
            EndReached = false;
        }

        private double SafeEvaluate(Vector v)
        {
            try
            {
                return GoalFunction.Evaluate(v.ToArray());
            }
            catch
            {
                return double.PositiveInfinity;
            }
        }

        public void NextIteration()
        {
            int n = CurrentSimplex.Size;                     // количество точек симплекса
            Vector bestpoint = CurrentSimplex.Points[0];
            Vector okpoint = CurrentSimplex.Points[n - 2];
            Vector badpoint = CurrentSimplex.Points[n - 1];

            // Преобразуем IReadOnlyList в массив для Vector.Sum
            Vector[] pointsArray = CurrentSimplex.Points.ToArray();
            Vector sum = Vector.Sum(pointsArray);
            Vector midpoint = (sum - badpoint) / (n - 1);

            Vector reflected = midpoint + (midpoint - badpoint) * ReflectionCoefficient;

            double f_reflected = SafeEvaluate(reflected);
            double f_best = SafeEvaluate(bestpoint);
            double f_ok = SafeEvaluate(okpoint);
            double f_bad = SafeEvaluate(badpoint);

            if (f_reflected < f_best)
            {
                Vector expanded = midpoint + (reflected - midpoint) * ExpansionCoefficient;
                double f_expanded = SafeEvaluate(expanded);
                if (f_expanded < f_best)
                    badpoint = expanded;
                else
                    badpoint = reflected;
            }
            else if (f_reflected < f_ok)
            {
                badpoint = reflected;
            }
            else
            {
                if (f_reflected < f_bad)
                    badpoint = reflected;

                Vector contracted = midpoint + (badpoint - midpoint) * ContractCoefficient;
                double f_contracted = SafeEvaluate(contracted);
                if (f_contracted < f_bad)
                {
                    badpoint = contracted;
                }
                else
                {
                    // Ветвь сжатия: заменяем худшую точку, затем сжимаем весь симплекс
                    CurrentSimplex.SetPoint(n - 1, badpoint);
                    CurrentSimplex.Shrink(0.5);
                    CurrentSimplex.SortPointsByResult(GoalFunction);

                    // Проверка условия остановки
                    Vector[] newPointsArray = CurrentSimplex.Points.ToArray();
                    Vector avg = Vector.Sum(newPointsArray) / n;
                    double dispersion = (avg - CurrentSimplex.Points[0]).Norm();
                    if (dispersion < 0.000001)
                        EndReached = true;
                    return;
                }
            }

            // Замена худшей точки на новую и сортировка
            CurrentSimplex.SetPoint(n - 1, badpoint);
            CurrentSimplex.SortPointsByResult(GoalFunction);

            // Проверка условия остановки
            Vector[] finalPointsArray = CurrentSimplex.Points.ToArray();
            Vector avgFinal = Vector.Sum(finalPointsArray) / n;
            double dispersionFinal = (avgFinal - CurrentSimplex.Points[0]).Norm();
            if (dispersionFinal < 0.000001)
                EndReached = true;
        }
    }
}