﻿namespace MIVClientGUI
{
    partial class ServerBrowser
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.clientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openTutorialToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutMIVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resolutionSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearServerListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteConsoleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverMonitorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runGameWithoutClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.conectManuallyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listView1 = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 585);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(922, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(922, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // clientToolStripMenuItem
            // 
            this.clientToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runGameWithoutClientToolStripMenuItem,
            this.conectManuallyToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.clientToolStripMenuItem.Name = "clientToolStripMenuItem";
            this.clientToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.clientToolStripMenuItem.Text = "Client";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.remoteConsoleToolStripMenuItem,
            this.serverMonitorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resolutionSettingsToolStripMenuItem,
            this.clearServerListToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openTutorialToolStripMenuItem,
            this.aboutMIVToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // openTutorialToolStripMenuItem
            // 
            this.openTutorialToolStripMenuItem.Name = "openTutorialToolStripMenuItem";
            this.openTutorialToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.openTutorialToolStripMenuItem.Text = "Open tutorial";
            // 
            // aboutMIVToolStripMenuItem
            // 
            this.aboutMIVToolStripMenuItem.Name = "aboutMIVToolStripMenuItem";
            this.aboutMIVToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.aboutMIVToolStripMenuItem.Text = "About MIV";
            // 
            // resolutionSettingsToolStripMenuItem
            // 
            this.resolutionSettingsToolStripMenuItem.Name = "resolutionSettingsToolStripMenuItem";
            this.resolutionSettingsToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.resolutionSettingsToolStripMenuItem.Text = "Resolution settings";
            // 
            // clearServerListToolStripMenuItem
            // 
            this.clearServerListToolStripMenuItem.Name = "clearServerListToolStripMenuItem";
            this.clearServerListToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.clearServerListToolStripMenuItem.Text = "Clear server list";
            // 
            // remoteConsoleToolStripMenuItem
            // 
            this.remoteConsoleToolStripMenuItem.Name = "remoteConsoleToolStripMenuItem";
            this.remoteConsoleToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.remoteConsoleToolStripMenuItem.Text = "Remote console";
            // 
            // serverMonitorToolStripMenuItem
            // 
            this.serverMonitorToolStripMenuItem.Name = "serverMonitorToolStripMenuItem";
            this.serverMonitorToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.serverMonitorToolStripMenuItem.Text = "Server monitor";
            // 
            // runGameWithoutClientToolStripMenuItem
            // 
            this.runGameWithoutClientToolStripMenuItem.Name = "runGameWithoutClientToolStripMenuItem";
            this.runGameWithoutClientToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.runGameWithoutClientToolStripMenuItem.Text = "Run game without client";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // conectManuallyToolStripMenuItem
            // 
            this.conectManuallyToolStripMenuItem.Name = "conectManuallyToolStripMenuItem";
            this.conectManuallyToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.conectManuallyToolStripMenuItem.Text = "Connect manually";
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(12, 56);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(898, 511);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(12, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(93, 27);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(174, 27);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Remove";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(318, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(592, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Server info (not selected)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ServerBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 607);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ServerBrowser";
            this.Text = "MIV Server Browser";
            this.Load += new System.EventHandler(this.ServerBrowser_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runGameWithoutClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem remoteConsoleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serverMonitorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resolutionSettingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearServerListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openTutorialToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMIVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem conectManuallyToolStripMenuItem;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
    }
}
