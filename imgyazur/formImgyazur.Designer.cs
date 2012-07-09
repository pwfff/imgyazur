namespace imgyazur
{
    partial class formImgyazur
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
            this.components = new System.ComponentModel.Container();
            this.SuspendLayout();
            // 
            // formImgyazur
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Cursor = System.Windows.Forms.Cursors.Cross;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "formImgyazur";
            this.Opacity = 0.01D;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "imgyazur";
            this.Deactivate += new System.EventHandler(this.formImgyazur_Deactivate);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.formImgyazur_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.formImgyazur_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.formImgyazur_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.formImgyazur_MouseUp);
            this.ResumeLayout(false);
        }

        #endregion
    }
}

