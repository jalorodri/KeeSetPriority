
namespace KeeSetPriority
{
    partial class ProcessPicker
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.removeButton = new System.Windows.Forms.Button();
            this.addButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.availableProcessesBox = new System.Windows.Forms.ListBox();
            this.selectedProcessesBox = new System.Windows.Forms.ListBox();
            this.getAvailableProcessesBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.updateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // removeButton
            // 
            this.removeButton.Location = new System.Drawing.Point(256, 144);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(49, 31);
            this.removeButton.TabIndex = 19;
            this.removeButton.Text = "<<";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(256, 107);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(49, 31);
            this.addButton.TabIndex = 18;
            this.addButton.Text = ">>";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Available programs";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(430, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 17);
            this.label1.TabIndex = 20;
            this.label1.Text = "Selected programs";
            // 
            // availableProcessesBox
            // 
            this.availableProcessesBox.FormattingEnabled = true;
            this.availableProcessesBox.ItemHeight = 16;
            this.availableProcessesBox.Location = new System.Drawing.Point(4, 21);
            this.availableProcessesBox.Name = "availableProcessesBox";
            this.availableProcessesBox.Size = new System.Drawing.Size(246, 276);
            this.availableProcessesBox.TabIndex = 21;
            // 
            // selectedProcessesBox
            // 
            this.selectedProcessesBox.FormattingEnabled = true;
            this.selectedProcessesBox.ItemHeight = 16;
            this.selectedProcessesBox.Location = new System.Drawing.Point(311, 21);
            this.selectedProcessesBox.Name = "selectedProcessesBox";
            this.selectedProcessesBox.Size = new System.Drawing.Size(246, 276);
            this.selectedProcessesBox.TabIndex = 22;
            // 
            // getAvailableProcessesBackgroundWorker
            // 
            this.getAvailableProcessesBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.GetAvailableProcessesBackgroundWorker_DoWork);
            this.getAvailableProcessesBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.GetAvailableProcessesBackgroundWorker_RunWorkerCompleted);
            // 
            // updateButton
            // 
            this.updateButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.updateButton.Location = new System.Drawing.Point(257, 252);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(48, 44);
            this.updateButton.TabIndex = 23;
            this.updateButton.Text = " ⟳";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // ProcessPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.availableProcessesBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.selectedProcessesBox);
            this.Name = "ProcessPicker";
            this.Size = new System.Drawing.Size(560, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox availableProcessesBox;
        private System.Windows.Forms.ListBox selectedProcessesBox;
        private System.ComponentModel.BackgroundWorker getAvailableProcessesBackgroundWorker;
        private System.Windows.Forms.Button updateButton;
    }
}
