namespace NelderMead
{
    partial class FormAlgorithm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridSimplex = new System.Windows.Forms.DataGridView();
            this.Points = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Results = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxFunction = new System.Windows.Forms.TextBox();
            this.textBoxA = new System.Windows.Forms.TextBox();
            this.textBoxB = new System.Windows.Forms.TextBox();
            this.textBoxC = new System.Windows.Forms.TextBox();
            this.labelQ = new System.Windows.Forms.Label();
            this.labelA = new System.Windows.Forms.Label();
            this.labelB = new System.Windows.Forms.Label();
            this.labelC = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBoxStep = new System.Windows.Forms.TextBox();
            this.labelStep = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSimplex)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridSimplex
            // 
            this.dataGridSimplex.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridSimplex.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Points,
            this.Results});
            this.dataGridSimplex.Location = new System.Drawing.Point(17, 123);
            this.dataGridSimplex.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridSimplex.Name = "dataGridSimplex";
            this.dataGridSimplex.RowHeadersWidth = 51;
            this.dataGridSimplex.RowTemplate.Height = 24;
            this.dataGridSimplex.Size = new System.Drawing.Size(936, 487);
            this.dataGridSimplex.TabIndex = 0;
            // 
            // Points
            // 
            this.Points.HeaderText = "Точка симплекса";
            this.Points.MinimumWidth = 6;
            this.Points.Name = "Points";
            this.Points.Width = 425;
            // 
            // Results
            // 
            this.Results.HeaderText = "Значение функции в точке";
            this.Results.MinimumWidth = 6;
            this.Results.Name = "Results";
            this.Results.Width = 425;
            // 
            // textBoxFunction
            // 
            this.textBoxFunction.Location = new System.Drawing.Point(283, 6);
            this.textBoxFunction.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxFunction.Name = "textBoxFunction";
            this.textBoxFunction.ReadOnly = true;
            this.textBoxFunction.Size = new System.Drawing.Size(670, 26);
            this.textBoxFunction.TabIndex = 1;
            // 
            // textBoxA
            // 
            this.textBoxA.Location = new System.Drawing.Point(223, 44);
            this.textBoxA.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxA.Name = "textBoxA";
            this.textBoxA.ReadOnly = true;
            this.textBoxA.Size = new System.Drawing.Size(106, 26);
            this.textBoxA.TabIndex = 2;
            // 
            // textBoxB
            // 
            this.textBoxB.Location = new System.Drawing.Point(559, 44);
            this.textBoxB.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxB.Name = "textBoxB";
            this.textBoxB.ReadOnly = true;
            this.textBoxB.Size = new System.Drawing.Size(106, 26);
            this.textBoxB.TabIndex = 3;
            // 
            // textBoxC
            // 
            this.textBoxC.Location = new System.Drawing.Point(847, 44);
            this.textBoxC.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxC.Name = "textBoxC";
            this.textBoxC.ReadOnly = true;
            this.textBoxC.Size = new System.Drawing.Size(106, 26);
            this.textBoxC.TabIndex = 4;
            // 
            // labelQ
            // 
            this.labelQ.AutoSize = true;
            this.labelQ.Location = new System.Drawing.Point(13, 9);
            this.labelQ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelQ.Name = "labelQ";
            this.labelQ.Size = new System.Drawing.Size(218, 18);
            this.labelQ.TabIndex = 5;
            this.labelQ.Text = "Целевая функция Q(X)=";
            // 
            // labelA
            // 
            this.labelA.AutoSize = true;
            this.labelA.Location = new System.Drawing.Point(13, 47);
            this.labelA.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelA.Name = "labelA";
            this.labelA.Size = new System.Drawing.Size(168, 18);
            this.labelA.TabIndex = 6;
            this.labelA.Text = "Коэфф. отражения";
            // 
            // labelB
            // 
            this.labelB.AutoSize = true;
            this.labelB.Location = new System.Drawing.Point(337, 47);
            this.labelB.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelB.Name = "labelB";
            this.labelB.Size = new System.Drawing.Size(178, 18);
            this.labelB.TabIndex = 7;
            this.labelB.Text = "Коэфф. растяжения";
            // 
            // labelC
            // 
            this.labelC.AutoSize = true;
            this.labelC.Location = new System.Drawing.Point(673, 47);
            this.labelC.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelC.Name = "labelC";
            this.labelC.Size = new System.Drawing.Size(138, 18);
            this.labelC.TabIndex = 8;
            this.labelC.Text = "Коэфф. сжатия";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(80, 618);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(324, 42);
            this.button1.TabIndex = 9;
            this.button1.Text = "Следующий шаг";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button1_MouseClick);
            // 
            // textBoxStep
            // 
            this.textBoxStep.Location = new System.Drawing.Point(223, 86);
            this.textBoxStep.Name = "textBoxStep";
            this.textBoxStep.ReadOnly = true;
            this.textBoxStep.Size = new System.Drawing.Size(106, 26);
            this.textBoxStep.TabIndex = 10;
            // 
            // labelStep
            // 
            this.labelStep.AutoSize = true;
            this.labelStep.Location = new System.Drawing.Point(169, 89);
            this.labelStep.Name = "labelStep";
            this.labelStep.Size = new System.Drawing.Size(38, 18);
            this.labelStep.TabIndex = 11;
            this.labelStep.Text = "Шаг";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(559, 618);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(324, 42);
            this.button2.TabIndex = 12;
            this.button2.Text = "Получить результат";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button2_MouseClick);
            // 
            // FormAlgorithm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 673);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.labelStep);
            this.Controls.Add(this.textBoxStep);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.labelC);
            this.Controls.Add(this.labelB);
            this.Controls.Add(this.labelA);
            this.Controls.Add(this.labelQ);
            this.Controls.Add(this.textBoxC);
            this.Controls.Add(this.textBoxB);
            this.Controls.Add(this.textBoxA);
            this.Controls.Add(this.textBoxFunction);
            this.Controls.Add(this.dataGridSimplex);
            this.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormAlgorithm";
            this.Text = "Метод в процессе работы...";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridSimplex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridSimplex;
        private System.Windows.Forms.TextBox textBoxFunction;
        private System.Windows.Forms.TextBox textBoxA;
        private System.Windows.Forms.TextBox textBoxB;
        private System.Windows.Forms.TextBox textBoxC;
        private System.Windows.Forms.Label labelQ;
        private System.Windows.Forms.Label labelA;
        private System.Windows.Forms.Label labelB;
        private System.Windows.Forms.Label labelC;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBoxStep;
        private System.Windows.Forms.Label labelStep;
        private System.Windows.Forms.DataGridViewTextBoxColumn Points;
        private Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Results;
    }
}