using System;
using System.Windows.Forms;

namespace NelderMead
{
    public partial class Form1 : Form
    {
        Function GoalFunction;
        public Form1()
        {
            InitializeComponent();
        }
        private void CheckCoefficientInput(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }
        private void CheckEmpty()
        {
            if (textBoxInputFunct.Text != "" && textBoxInputA.Text != "" && textBoxInputB.Text != "" && textBoxInputC.Text != "")
            { buttonStart.Enabled = true; }
            else { buttonStart.Enabled = false; }
        }
        private void textBoxInputFunct_TextChanged(object sender, EventArgs e)
        {
            CheckEmpty();
        }
        private void buttonStart_MouseClick(object sender, MouseEventArgs e)
        {
            GoalFunction = new Function(textBoxInputFunct.Text);
            int size = GoalFunction.VariablesCount;
            Vector[] simplexPoints = new Vector[size+1];
            simplexPoints[0] = new Vector(size);
            for (int i = 1; i < size+1; i++)
            {
                double[] coordinates = new double[size];
                for (int j = 0; j < size; j++)
                {
                    if (j == i - 1) coordinates[j] = 1; else coordinates[j] = 0;
                }
                simplexPoints[i] = new Vector(coordinates);
            }
            Simplex simplex = new Simplex(simplexPoints);
            simplex.SortPointsByResult(GoalFunction);
            NelderMeadAlgorithm nelderMead = new NelderMeadAlgorithm(Convert.ToDouble(textBoxInputA.Text), Convert.ToDouble(textBoxInputB.Text), Convert.ToDouble(textBoxInputC.Text), simplex, GoalFunction);
            FormAlgorithm formAlgorithm = new FormAlgorithm(nelderMead);
            formAlgorithm.ShowDialog();
        }
        private void textBoxInputC_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckCoefficientInput(sender, e);
        }
        private void textBoxInputB_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckCoefficientInput(sender, e);
        }
        private void textBoxInputA_KeyPress(object sender, KeyPressEventArgs e)
        {
            CheckCoefficientInput(sender, e);
        }
        private void textBoxInputA_TextChanged(object sender, EventArgs e)
        {
            CheckEmpty();
        }
        private void textBoxInputB_TextChanged(object sender, EventArgs e)
        {
            CheckEmpty();
        }
        private void textBoxInputC_TextChanged(object sender, EventArgs e)
        {
            CheckEmpty();
        }
    }
}
