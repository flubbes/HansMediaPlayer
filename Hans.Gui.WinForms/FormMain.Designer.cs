namespace Hans.Gui.WinForms
{
    partial class FormMain
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
            this.lvLibrary = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // lvLibrary
            // 
            this.lvLibrary.Location = new System.Drawing.Point(122, 53);
            this.lvLibrary.Name = "lvLibrary";
            this.lvLibrary.Size = new System.Drawing.Size(442, 309);
            this.lvLibrary.TabIndex = 0;
            this.lvLibrary.UseCompatibleStateImageBehavior = false;
            this.lvLibrary.View = System.Windows.Forms.View.Details;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 406);
            this.Controls.Add(this.lvLibrary);
            this.Name = "FormMain";
            this.Text = "Hans Media Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvLibrary;
    }
}

