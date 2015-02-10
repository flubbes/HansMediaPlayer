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
            this.olvLibrary = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslSongsInLibrary = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.olvLibrary)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvLibrary
            // 
            this.olvLibrary.AllColumns.Add(this.olvColumn1);
            this.olvLibrary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.olvLibrary.FullRowSelect = true;
            this.olvLibrary.GridLines = true;
            this.olvLibrary.Location = new System.Drawing.Point(12, 12);
            this.olvLibrary.Name = "olvLibrary";
            this.olvLibrary.Size = new System.Drawing.Size(447, 347);
            this.olvLibrary.TabIndex = 0;
            this.olvLibrary.UseCompatibleStateImageBehavior = false;
            this.olvLibrary.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Title";
            this.olvColumn1.CellPadding = null;
            this.olvColumn1.Text = "Title";
            this.olvColumn1.Width = 114;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslSongsInLibrary});
            this.statusStrip1.Location = new System.Drawing.Point(0, 384);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(728, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsslSongsInLibrary
            // 
            this.tsslSongsInLibrary.Name = "tsslSongsInLibrary";
            this.tsslSongsInLibrary.Size = new System.Drawing.Size(97, 17);
            this.tsslSongsInLibrary.Text = "Songs in Library: ";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 406);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.olvLibrary);
            this.Name = "FormMain";
            this.Text = "Hans Media Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.olvLibrary)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvLibrary;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslSongsInLibrary;

    }
}

