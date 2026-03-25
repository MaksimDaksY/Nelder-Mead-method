using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NelderMead
{
    public partial class FormAlgorithm : Form
    {
        int CurrentStep;
        NelderMeadAlgorithm Parameters;
        public FormAlgorithm()
        {
            InitializeComponent();
        }
        public FormAlgorithm(NelderMeadAlgorithm parameters)
        {
            InitializeComponent();
            CurrentStep = 0;
            Parameters = parameters;
            textBoxFunction.Text = parameters.GoalFunction.FunctionText;
            textBoxA.Text = parameters.ReflectionCoefficient.ToString();
            textBoxB.Text = parameters.ExpansionCoefficient.ToString();
            textBoxC.Text = parameters.ContractCoefficient.ToString();
            TableFill();
        }
        private void TableFill()
        {
            for (int i = 0; i < Parameters.CurrentSimplex.Points.Length; i++)
            {
                string vectorToText = "(";
                for (int j = 0; j < Parameters.CurrentSimplex.Points[i].Coordinates.Length-1; j++)
                {
                    vectorToText += Parameters.CurrentSimplex.Points[i].Coordinates[j].ToString();
                    vectorToText += "; ";
                }
                vectorToText += Parameters.CurrentSimplex.Points[i].Coordinates.Last().ToString();
                vectorToText += ")";
                dataGridSimplex.Rows.Add(vectorToText, Parameters.GoalFunction.Value(Parameters.CurrentSimplex.Points[i]));
            }
            textBoxStep.Text = CurrentStep.ToString();
            CurrentStep++;
        }
        private void ButtonDeactivate()
        {
            button1.Text = "Условие остановки достигнуто"; button1.Enabled = false;
        }
        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            Parameters.NextIteration();
            dataGridSimplex.Rows.Clear();
            TableFill();
            if (Parameters.EndReached) ButtonDeactivate();
        }
    }
}
