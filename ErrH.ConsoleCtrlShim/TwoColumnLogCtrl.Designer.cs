namespace ErrH.ConsoleCtrlShim
{
    partial class TwoColumnLogCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cons = new ConsoleControl.ConsoleControl();
            this.SuspendLayout();
            // 
            // cons
            // 
            this.cons.IsInputEnabled = true;
            this.cons.Location = new System.Drawing.Point(19, 35);
            this.cons.Name = "cons";
            this.cons.SendKeyboardCommandsToProcess = false;
            this.cons.ShowDiagnostics = false;
            this.cons.Size = new System.Drawing.Size(115, 95);
            this.cons.TabIndex = 0;
            // 
            // TwoColumnLogCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cons);
            this.Name = "TwoColumnLogCtrl";
            this.ResumeLayout(false);

        }

        #endregion

        private ConsoleControl.ConsoleControl cons;
    }
}
