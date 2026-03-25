namespace NelderMead
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBoxInputFunct = new System.Windows.Forms.TextBox();
            this.labelMakeInput = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.labelQ = new System.Windows.Forms.Label();
            this.textBoxInputA = new System.Windows.Forms.TextBox();
            this.textBoxInputB = new System.Windows.Forms.TextBox();
            this.textBoxInputC = new System.Windows.Forms.TextBox();
            this.labelA = new System.Windows.Forms.Label();
            this.labelB = new System.Windows.Forms.Label();
            this.labelC = new System.Windows.Forms.Label();
            this.labelFunct = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxInputFunct
            // 
            this.textBoxInputFunct.Location = new System.Drawing.Point(82, 70);
            this.textBoxInputFunct.Margin = new System.Windows.Forms.Padding(4);
            this.textBoxInputFunct.Name = "textBoxInputFunct";
            this.textBoxInputFunct.Size = new System.Drawing.Size(275, 30);
            this.textBoxInputFunct.TabIndex = 0;
            this.textBoxInputFunct.TextChanged += new System.EventHandler(this.textBoxInputFunct_TextChanged);
            // 
            // labelMakeInput
            // 
            this.labelMakeInput.AutoSize = true;
            this.labelMakeInput.Location = new System.Drawing.Point(78, 9);
            this.labelMakeInput.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelMakeInput.Name = "labelMakeInput";
            this.labelMakeInput.Size = new System.Drawing.Size(226, 23);
            this.labelMakeInput.TabIndex = 1;
            this.labelMakeInput.Text = "Введите параметры:";
            // 
            // buttonStart
            // 
            this.buttonStart.Enabled = false;
            this.buttonStart.Location = new System.Drawing.Point(82, 246);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(137, 33);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "Запуск";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonStart_MouseClick);
            // 
            // labelQ
            // 
            this.labelQ.AutoSize = true;
            this.labelQ.Location = new System.Drawing.Point(5, 77);
            this.labelQ.Name = "labelQ";
            this.labelQ.Size = new System.Drawing.Size(70, 23);
            this.labelQ.TabIndex = 3;
            this.labelQ.Text = "Q(X)=";
            // 
            // textBoxInputA
            // 
            this.textBoxInputA.Location = new System.Drawing.Point(225, 107);
            this.textBoxInputA.Name = "textBoxInputA";
            this.textBoxInputA.Size = new System.Drawing.Size(132, 30);
            this.textBoxInputA.TabIndex = 4;
            this.textBoxInputA.Text = "1";
            this.textBoxInputA.TextChanged += new System.EventHandler(this.textBoxInputA_TextChanged);
            this.textBoxInputA.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxInputA_KeyPress);
            // 
            // textBoxInputB
            // 
            this.textBoxInputB.Location = new System.Drawing.Point(225, 143);
            this.textBoxInputB.Name = "textBoxInputB";
            this.textBoxInputB.Size = new System.Drawing.Size(132, 30);
            this.textBoxInputB.TabIndex = 5;
            this.textBoxInputB.Text = "2";
            this.textBoxInputB.TextChanged += new System.EventHandler(this.textBoxInputB_TextChanged);
            this.textBoxInputB.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxInputB_KeyPress);
            // 
            // textBoxInputC
            // 
            this.textBoxInputC.Location = new System.Drawing.Point(225, 179);
            this.textBoxInputC.Name = "textBoxInputC";
            this.textBoxInputC.Size = new System.Drawing.Size(132, 30);
            this.textBoxInputC.TabIndex = 6;
            this.textBoxInputC.Text = "0,5";
            this.textBoxInputC.TextChanged += new System.EventHandler(this.textBoxInputC_TextChanged);
            this.textBoxInputC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxInputC_KeyPress);
            // 
            // labelA
            // 
            this.labelA.AutoSize = true;
            this.labelA.Location = new System.Drawing.Point(5, 110);
            this.labelA.Name = "labelA";
            this.labelA.Size = new System.Drawing.Size(202, 23);
            this.labelA.TabIndex = 7;
            this.labelA.Text = "Коэфф. отражения";
            // 
            // labelB
            // 
            this.labelB.AutoSize = true;
            this.labelB.Location = new System.Drawing.Point(5, 146);
            this.labelB.Name = "labelB";
            this.labelB.Size = new System.Drawing.Size(214, 23);
            this.labelB.TabIndex = 8;
            this.labelB.Text = "Коэфф. растяжения";
            // 
            // labelC
            // 
            this.labelC.AutoSize = true;
            this.labelC.Location = new System.Drawing.Point(5, 182);
            this.labelC.Name = "labelC";
            this.labelC.Size = new System.Drawing.Size(166, 23);
            this.labelC.TabIndex = 9;
            this.labelC.Text = "Коэфф. сжатия";
            // 
            // labelFunct
            // 
            this.labelFunct.AutoSize = true;
            this.labelFunct.Location = new System.Drawing.Point(78, 43);
            this.labelFunct.Name = "labelFunct";
            this.labelFunct.Size = new System.Drawing.Size(190, 23);
            this.labelFunct.TabIndex = 10;
            this.labelFunct.Text = "Целевая функция";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 313);
            this.Controls.Add(this.labelFunct);
            this.Controls.Add(this.labelC);
            this.Controls.Add(this.labelB);
            this.Controls.Add(this.labelA);
            this.Controls.Add(this.textBoxInputC);
            this.Controls.Add(this.textBoxInputB);
            this.Controls.Add(this.textBoxInputA);
            this.Controls.Add(this.labelQ);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.labelMakeInput);
            this.Controls.Add(this.textBoxInputFunct);
            this.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Введите параметры";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxInputFunct;
        private System.Windows.Forms.Label labelMakeInput;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label labelQ;
        private System.Windows.Forms.TextBox textBoxInputA;
        private System.Windows.Forms.TextBox textBoxInputB;
        private System.Windows.Forms.TextBox textBoxInputC;
        private System.Windows.Forms.Label labelA;
        private System.Windows.Forms.Label labelB;
        private System.Windows.Forms.Label labelC;
        private System.Windows.Forms.Label labelFunct;
    }
}

