using System;
using System.Windows.Forms;
using NelderMeadParser;
using NelderMeadCore;

namespace NelderMead
{
    public partial class FormAlgorithm : Form
    {
        private int CurrentStep;
        private NelderMeadAlgorithm Parameters;

        public FormAlgorithm()
        {
            InitializeComponent();
        }

        public FormAlgorithm(NelderMeadAlgorithm parameters)
        {
            InitializeComponent();
            CurrentStep = 1;
            Parameters = parameters;

            textBoxFunction.Text = parameters.GoalFunction.FunctionText;
            textBoxA.Text = parameters.ReflectionCoefficient.ToString();
            textBoxB.Text = parameters.ExpansionCoefficient.ToString();
            textBoxC.Text = parameters.ContractCoefficient.ToString();

            UpdateTable();
            textBoxStep.Text = "0";
        }

        private void UpdateTable()
        {
            try
            {
                dataGridSimplex.Rows.Clear();
                for (int i = 0; i < Parameters.CurrentSimplex.Points.Length; i++)
                {
                    Vector point = Parameters.CurrentSimplex.Points[i];
                    string vectorToText = "(" + string.Join("; ", point.Coordinates) + ")";
                    double value = Parameters.GoalFunction.Evaluate(point.Coordinates);
                    dataGridSimplex.Rows.Add(vectorToText, value);
                }
            }
            catch (EvaluatingException ex)
            {
                MessageBox.Show($"Ошибка вычисления функции в одной из точек:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ButtonDeactivate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неожиданная ошибка при заполнении таблицы: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ButtonDeactivate();
            }
        }

        private void TableFill()
        {
            UpdateTable();
            textBoxStep.Text = CurrentStep.ToString();
            CurrentStep++;
        }

        private void ButtonDeactivate()
        {
            button1.Text = "Условие остановки достигнуто";
            button1.Enabled = false;
            button2.Enabled = false;
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                Parameters.NextIteration();
                TableFill();
                if (Parameters.EndReached)
                    ButtonDeactivate();
            }
            catch (EvaluatingException ex)
            {
                MessageBox.Show($"Ошибка вычисления функции на шаге {CurrentStep}:\n{ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ButtonDeactivate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неожиданная ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ButtonDeactivate();
            }
        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;

            int stepsAlreadyDone = CurrentStep - 1;
            int additionalIterations = 0;

            try
            {
                while (!Parameters.EndReached)
                {
                    Parameters.NextIteration();
                    additionalIterations++;
                }

                UpdateTable();

                int totalSteps = stepsAlreadyDone + additionalIterations;
                textBoxStep.Text = totalSteps.ToString();

                CurrentStep = totalSteps + 1;

                ButtonDeactivate();

                MessageBox.Show($"Алгоритм завершён за {additionalIterations} дополнительных итераций.\nВсего выполнено шагов: {totalSteps}.",
                                "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (EvaluatingException ex)
            {
                MessageBox.Show($"Ошибка вычисления функции в процессе итераций:\n{ex.Message}",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ButtonDeactivate();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неожиданная ошибка: {ex.Message}",
                                "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ButtonDeactivate();
            }
            finally
            {
                if (!Parameters.EndReached)
                {
                    button1.Enabled = true;
                    button2.Enabled = true;
                }
            }
        }
    }
}