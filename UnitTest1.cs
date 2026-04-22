using Microsoft.VisualStudio.TestTools.UnitTesting;
using NelderMeadParser;
using NelderMeadCore;
using System;

namespace NelderMeadTests
{
    [TestClass]
    public class NelderMeadTests
    {
        //Тесты парсера (Function)

        [TestMethod]
        public void Function_Evaluate_SimpleAddition_ReturnsCorrect()
        {
            var func = new Function("x+y");
            double result = func.Evaluate(2, 3);
            Assert.AreEqual(5, result, 1e-10);
        }

        [TestMethod]
        public void Function_Evaluate_WithAbs_ReturnsAbsoluteValue()
        {
            var func = new Function("abs(x)");
            double result = func.Evaluate(-5);
            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Function_Evaluate_WithTrigonometry_ReturnsCorrect()
        {
            var func = new Function("sin(0) + cos(0)");
            double result = func.Evaluate();
            Assert.AreEqual(1, result, 1e-10);
        }

        [TestMethod]
        public void Function_Evaluate_WithPower_ReturnsCorrect()
        {
            var func = new Function("x^2 + y^3");
            double result = func.Evaluate(2, 3);
            Assert.AreEqual(4 + 27, result);
        }

        [TestMethod]
        [ExpectedException(typeof(EvaluatingException))]
        public void Function_Evaluate_TooFewArguments_ThrowsEvaluatingException()
        {
            var func = new Function("x+y");
            func.Evaluate(5);
        }

        //Тесты Vector

        [TestMethod]
        public void Vector_Addition_ReturnsCorrectVector()
        {
            var a = new Vector(new double[] { 1, 2 });
            var b = new Vector(new double[] { 3, 4 });
            var result = a + b;
            CollectionAssert.AreEqual(new double[] { 4, 6 }, result.Coordinates);
        }

        [TestMethod]
        public void Vector_Subtraction_ReturnsCorrectVector()
        {
            var a = new Vector(new double[] { 5, 7 });
            var b = new Vector(new double[] { 2, 3 });
            var result = a - b;
            CollectionAssert.AreEqual(new double[] { 3, 4 }, result.Coordinates);
        }

        [TestMethod]
        public void Vector_MultiplicationByScalar_ReturnsCorrectVector()
        {
            var v = new Vector(new double[] { 1, 2, 3 });
            var result = v * 2;
            CollectionAssert.AreEqual(new double[] { 2, 4, 6 }, result.Coordinates);
        }

        [TestMethod]
        public void Vector_DivisionByScalar_ReturnsCorrectVector()
        {
            var v = new Vector(new double[] { 2, 4, 6 });
            var result = v / 2;
            CollectionAssert.AreEqual(new double[] { 1, 2, 3 }, result.Coordinates);
        }

        [TestMethod]
        [ExpectedException(typeof(DivideByZeroException))]
        public void Vector_DivisionByZero_ThrowsException()
        {
            var v = new Vector(new double[] { 1, 2 });
            var result = v / 0;
        }

        [TestMethod]
        public void Vector_Norm_ReturnsEuclideanNorm()
        {
            var v = new Vector(new double[] { 3, 4 });
            Assert.AreEqual(5, v.Norm(), 1e-10);
        }

        [TestMethod]
        public void Vector_Sum_ReturnsCorrectVector()
        {
            var vectors = new Vector[]
            {
                new Vector(new double[] {1, 2}),
                new Vector(new double[] {3, 4}),
                new Vector(new double[] {5, 6})
            };
            var sum = Vector.Sum(vectors);
            CollectionAssert.AreEqual(new double[] { 9, 12 }, sum.Coordinates);
        }

        //Тесты Simplex

        [TestMethod]
        public void Simplex_SortPointsByResult_SortsAscending()
        {
            var func = new Function("x^2 + y^2");
            var points = new Vector[]
            {
                new Vector(new double[] {1, 1}), // value = 2
                new Vector(new double[] {0, 0}), // value = 0
                new Vector(new double[] {2, 2})  // value = 8
            };
            var simplex = new Simplex(points);
            simplex.SortPointsByResult(func);

            // Лучшая точка (минимальное значение) должна быть первой
            double bestValue = func.Evaluate(simplex.Points[0].Coordinates);
            Assert.AreEqual(0, bestValue, 1e-10);
            // Худшая точка должна быть последней
            double worstValue = func.Evaluate(simplex.Points[simplex.Points.Length - 1].Coordinates);
            Assert.AreEqual(8, worstValue, 1e-10);
        }

        [TestMethod]
        public void Simplex_Shrink_ReducesSimplexCorrectly()
        {
            var points = new Vector[]
            {
                new Vector(new double[] {0, 0}),
                new Vector(new double[] {2, 0}),
                new Vector(new double[] {0, 2})
            };
            var simplex = new Simplex(points);
            simplex.Shrink(0.5);

            // Лучшая точка (первая) не меняется
            CollectionAssert.AreEqual(new double[] { 0, 0 }, simplex.Points[0].Coordinates);
            // Остальные точки сжимаются относительно лучшей
            CollectionAssert.AreEqual(new double[] { 1, 0 }, simplex.Points[1].Coordinates);
            CollectionAssert.AreEqual(new double[] { 0, 1 }, simplex.Points[2].Coordinates);
        }

        //Тесты NelderMeadAlgorithm

        [TestMethod]
        public void NelderMeadAlgorithm_OneIteration_ImprovesSimplex()
        {
            var func = new Function("x^2 + y^2");
            var points = new Vector[]
            {
                new Vector(new double[] {1, 1}),  // f=2
                new Vector(new double[] {2, 0}),  // f=4
                new Vector(new double[] {0, 2})   // f=4
            };
            var simplex = new Simplex(points);
            simplex.SortPointsByResult(func);
            var algorithm = new NelderMeadAlgorithm(1.0, 2.0, 0.5, simplex, func);

            // Сохраняем лучшее значение до итерации
            double beforeBest = func.Evaluate(algorithm.CurrentSimplex.Points[0].Coordinates);
            algorithm.NextIteration();
            double afterBest = func.Evaluate(algorithm.CurrentSimplex.Points[0].Coordinates);

            // После итерации значение должно уменьшиться (или остаться таким же, но не стать хуже)
            Assert.IsTrue(afterBest <= beforeBest);
        }

        [TestMethod]
        public void NelderMeadAlgorithm_Convergence_ReachesEndCondition()
        {
            var func = new Function("x^2 + y^2");
            var points = new Vector[]
            {
                new Vector(new double[] {10, 10}),
                new Vector(new double[] {11, 10}),
                new Vector(new double[] {10, 11})
            };
            var simplex = new Simplex(points);
            simplex.SortPointsByResult(func);
            var algorithm = new NelderMeadAlgorithm(1.0, 2.0, 0.5, simplex, func);

            int maxIter = 500;
            for (int i = 0; i < maxIter && !algorithm.EndReached; i++)
            {
                algorithm.NextIteration();
            }

            Assert.IsTrue(algorithm.EndReached);
            // Лучшая точка должна быть близка к (0,0)
            var bestPoint = algorithm.CurrentSimplex.Points[0];
            Assert.IsTrue(Math.Abs(bestPoint.Coordinates[0]) < 1e-5);
            Assert.IsTrue(Math.Abs(bestPoint.Coordinates[1]) < 1e-5);
        }

        [TestMethod]
        public void NelderMeadAlgorithm_HandlesMathematicalErrors_Gracefully()
        {
            // Функция, которая может дать ошибку (ln отрицательного числа)
            var func = new Function("ln(x)");
            var points = new Vector[]
            {
                new Vector(new double[] {1}),   // ln(1)=0
                new Vector(new double[] {2})    // ln(2)=0.693
            };
            // Для симплекса в 1D нужно 2 точки
            var simplex = new Simplex(points);
            simplex.SortPointsByResult(func);
            var algorithm = new NelderMeadAlgorithm(1.0, 2.0, 0.5, simplex, func);

            // Итерация не должна выбросить исключение, а должна корректно обработать ошибку (вернуть +∞)
            try
            {
                algorithm.NextIteration();
                // Если дошли сюда – исключения нет, тест пройден
            }
            catch (Exception)
            {
                Assert.Fail("Алгоритм выбросил исключение при математической ошибке");
            }
        }
    }
}