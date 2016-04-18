namespace StockVisAg
{
    partial class StockVisualizer
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
            this.btnMakeImage = new System.Windows.Forms.Button();
            this.startTime = new System.Windows.Forms.DateTimePicker();
            this.endTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnGetData = new System.Windows.Forms.Button();
            this.bwGetData = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // btnMakeImage
            // 
            this.btnMakeImage.Enabled = false;
            this.btnMakeImage.Location = new System.Drawing.Point(39, 196);
            this.btnMakeImage.Margin = new System.Windows.Forms.Padding(4);
            this.btnMakeImage.Name = "btnMakeImage";
            this.btnMakeImage.Size = new System.Drawing.Size(267, 59);
            this.btnMakeImage.TabIndex = 2;
            this.btnMakeImage.Text = "Make Image";
            this.btnMakeImage.UseVisualStyleBackColor = true;
            this.btnMakeImage.Click += new System.EventHandler(this.btnMakeImage_Click);
            // 
            // startTime
            // 
            this.startTime.CustomFormat = "MM/dd/yyyy HH mm ss";
            this.startTime.Enabled = false;
            this.startTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startTime.Location = new System.Drawing.Point(62, 134);
            this.startTime.Margin = new System.Windows.Forms.Padding(4);
            this.startTime.Name = "startTime";
            this.startTime.Size = new System.Drawing.Size(265, 22);
            this.startTime.TabIndex = 3;
            // 
            // endTime
            // 
            this.endTime.CustomFormat = "MM/dd/yyyy HH mm ss";
            this.endTime.Enabled = false;
            this.endTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTime.Location = new System.Drawing.Point(62, 166);
            this.endTime.Margin = new System.Windows.Forms.Padding(4);
            this.endTime.Name = "endTime";
            this.endTime.Size = new System.Drawing.Size(265, 22);
            this.endTime.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 134);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Start";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 166);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "End";
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(39, 13);
            this.btnGetData.Margin = new System.Windows.Forms.Padding(4);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(267, 59);
            this.btnGetData.TabIndex = 7;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // bwGetData
            // 
            this.bwGetData.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwGetData_DoWork);
            this.bwGetData.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwGetData_RunWorkerCompleted);
            // 
            // StockVisualizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 272);
            this.Controls.Add(this.btnGetData);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.endTime);
            this.Controls.Add(this.startTime);
            this.Controls.Add(this.btnMakeImage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "StockVisualizer";
            this.ShowIcon = false;
            this.Text = "Stock Visualizer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnMakeImage;
        private System.Windows.Forms.DateTimePicker startTime;
        private System.Windows.Forms.DateTimePicker endTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnGetData;
        private System.ComponentModel.BackgroundWorker bwGetData;
    }
}

