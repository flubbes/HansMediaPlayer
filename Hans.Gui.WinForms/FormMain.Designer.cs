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
            this.components = new System.ComponentModel.Container();
            this.olvLibrary = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsslSongsInLibrary = new System.Windows.Forms.ToolStripStatusLabel();
            this.olvPlaylist = new BrightIdeasSoftware.ObjectListView();
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.tc = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.olvSearch = new BrightIdeasSoftware.ObjectListView();
            this.tbSearch = new System.Windows.Forms.TextBox();
            this.cmsSearch = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.olvLibrary)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvPlaylist)).BeginInit();
            this.tc.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvSearch)).BeginInit();
            this.cmsSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvLibrary
            // 
            this.olvLibrary.AllColumns.Add(this.olvColumn1);
            this.olvLibrary.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1});
            this.olvLibrary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvLibrary.FullRowSelect = true;
            this.olvLibrary.GridLines = true;
            this.olvLibrary.Location = new System.Drawing.Point(3, 3);
            this.olvLibrary.Name = "olvLibrary";
            this.olvLibrary.Size = new System.Drawing.Size(482, 352);
            this.olvLibrary.TabIndex = 0;
            this.olvLibrary.UseCompatibleStateImageBehavior = false;
            this.olvLibrary.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "Title";
            this.olvColumn1.CellPadding = null;
            this.olvColumn1.Text = "Title";
            this.olvColumn1.Width = 205;
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
            // olvPlaylist
            // 
            this.olvPlaylist.AllColumns.Add(this.olvColumn2);
            this.olvPlaylist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn2});
            this.olvPlaylist.Dock = System.Windows.Forms.DockStyle.Right;
            this.olvPlaylist.FullRowSelect = true;
            this.olvPlaylist.GridLines = true;
            this.olvPlaylist.Location = new System.Drawing.Point(502, 0);
            this.olvPlaylist.Name = "olvPlaylist";
            this.olvPlaylist.Size = new System.Drawing.Size(226, 384);
            this.olvPlaylist.TabIndex = 2;
            this.olvPlaylist.UseCompatibleStateImageBehavior = false;
            this.olvPlaylist.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "Title";
            this.olvColumn2.CellPadding = null;
            this.olvColumn2.Text = "Title";
            this.olvColumn2.Width = 114;
            // 
            // tc
            // 
            this.tc.Controls.Add(this.tabPage1);
            this.tc.Controls.Add(this.tabPage2);
            this.tc.Dock = System.Windows.Forms.DockStyle.Left;
            this.tc.Location = new System.Drawing.Point(0, 0);
            this.tc.Name = "tc";
            this.tc.SelectedIndex = 0;
            this.tc.Size = new System.Drawing.Size(496, 384);
            this.tc.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.olvLibrary);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(488, 358);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Library";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.olvSearch);
            this.tabPage2.Controls.Add(this.tbSearch);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(488, 358);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Online search";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // olvSearch
            // 
            this.olvSearch.ContextMenuStrip = this.cmsSearch;
            this.olvSearch.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.olvSearch.FullRowSelect = true;
            this.olvSearch.GridLines = true;
            this.olvSearch.Location = new System.Drawing.Point(3, 32);
            this.olvSearch.Name = "olvSearch";
            this.olvSearch.Size = new System.Drawing.Size(482, 323);
            this.olvSearch.TabIndex = 1;
            this.olvSearch.UseCompatibleStateImageBehavior = false;
            this.olvSearch.View = System.Windows.Forms.View.Details;
            // 
            // tbSearch
            // 
            this.tbSearch.Location = new System.Drawing.Point(6, 6);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(241, 20);
            this.tbSearch.TabIndex = 0;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // cmsSearch
            // 
            this.cmsSearch.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadToolStripMenuItem});
            this.cmsSearch.Name = "cmsSearch";
            this.cmsSearch.Size = new System.Drawing.Size(129, 26);
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.downloadToolStripMenuItem.Text = "Download";
            this.downloadToolStripMenuItem.Click += new System.EventHandler(this.downloadToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 406);
            this.Controls.Add(this.tc);
            this.Controls.Add(this.olvPlaylist);
            this.Controls.Add(this.statusStrip1);
            this.Name = "FormMain";
            this.Text = "Hans Media Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.olvLibrary)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvPlaylist)).EndInit();
            this.tc.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvSearch)).EndInit();
            this.cmsSearch.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvLibrary;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tsslSongsInLibrary;
        private BrightIdeasSoftware.ObjectListView olvPlaylist;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private System.Windows.Forms.TabControl tc;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox tbSearch;
        private System.Windows.Forms.ContextMenuStrip cmsSearch;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
        private BrightIdeasSoftware.ObjectListView olvSearch;

    }
}

