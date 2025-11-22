namespace ImageAnnotationApp.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.menuUser = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProjects = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAdmin = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAdminProjects = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAdminQueues = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAdminUsers = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAdminExport = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAccount = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblUser = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelWelcome = new System.Windows.Forms.Panel();
            this.btnAdminEntry = new System.Windows.Forms.Button();
            this.btnUserEntry = new System.Windows.Forms.Button();
            this.lblWelcome = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelWelcome.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip
            //
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuUser,
            this.menuAdmin,
            this.menuAccount});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1000, 25);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            //
            // menuUser
            //
            this.menuUser.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProjects});
            this.menuUser.Name = "menuUser";
            this.menuUser.Size = new System.Drawing.Size(68, 21);
            this.menuUser.Text = "用户功能";
            //
            // menuProjects
            //
            this.menuProjects.Name = "menuProjects";
            this.menuProjects.Size = new System.Drawing.Size(124, 22);
            this.menuProjects.Text = "项目列表";
            this.menuProjects.Click += new System.EventHandler(this.menuProjects_Click);
            //
            // menuAdmin
            //
            this.menuAdmin.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAdminProjects,
            this.menuAdminQueues,
            this.menuAdminUsers,
            this.menuAdminExport});
            this.menuAdmin.Name = "menuAdmin";
            this.menuAdmin.Size = new System.Drawing.Size(80, 21);
            this.menuAdmin.Text = "管理员功能";
            this.menuAdmin.Visible = false;
            //
            // menuAdminProjects
            //
            this.menuAdminProjects.Name = "menuAdminProjects";
            this.menuAdminProjects.Size = new System.Drawing.Size(124, 22);
            this.menuAdminProjects.Text = "项目管理";
            this.menuAdminProjects.Click += new System.EventHandler(this.menuAdminProjects_Click);
            //
            // menuAdminQueues
            //
            this.menuAdminQueues.Name = "menuAdminQueues";
            this.menuAdminQueues.Size = new System.Drawing.Size(124, 22);
            this.menuAdminQueues.Text = "队列管理";
            this.menuAdminQueues.Click += new System.EventHandler(this.menuAdminQueues_Click);
            //
            // menuAdminUsers
            //
            this.menuAdminUsers.Name = "menuAdminUsers";
            this.menuAdminUsers.Size = new System.Drawing.Size(124, 22);
            this.menuAdminUsers.Text = "用户管理";
            this.menuAdminUsers.Click += new System.EventHandler(this.menuAdminUsers_Click);
            //
            // menuAdminExport
            //
            this.menuAdminExport.Name = "menuAdminExport";
            this.menuAdminExport.Size = new System.Drawing.Size(124, 22);
            this.menuAdminExport.Text = "数据导出";
            this.menuAdminExport.Click += new System.EventHandler(this.menuAdminExport_Click);
            //
            // menuAccount
            //
            this.menuAccount.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.menuAccount.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuLogout});
            this.menuAccount.Name = "menuAccount";
            this.menuAccount.Size = new System.Drawing.Size(44, 21);
            this.menuAccount.Text = "账户";
            //
            // menuLogout
            //
            this.menuLogout.Name = "menuLogout";
            this.menuLogout.Size = new System.Drawing.Size(124, 22);
            this.menuLogout.Text = "退出登录";
            this.menuLogout.Click += new System.EventHandler(this.menuLogout_Click);
            //
            // statusStrip
            //
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.lblUser});
            this.statusStrip.Location = new System.Drawing.Point(0, 728);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1000, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            //
            // lblStatus
            //
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(32, 17);
            this.lblStatus.Text = "就绪";
            //
            // lblUser
            //
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(953, 17);
            this.lblUser.Spring = true;
            this.lblUser.Text = "用户";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // panelMain
            //
            this.panelMain.Controls.Add(this.panelWelcome);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 25);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1000, 703);
            this.panelMain.TabIndex = 2;
            //
            // panelWelcome
            //
            this.panelWelcome.Controls.Add(this.btnAdminEntry);
            this.panelWelcome.Controls.Add(this.btnUserEntry);
            this.panelWelcome.Controls.Add(this.lblWelcome);
            this.panelWelcome.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWelcome.Location = new System.Drawing.Point(0, 0);
            this.panelWelcome.Name = "panelWelcome";
            this.panelWelcome.Size = new System.Drawing.Size(1000, 703);
            this.panelWelcome.TabIndex = 1;
            //
            // btnAdminEntry
            //
            this.btnAdminEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnAdminEntry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            this.btnAdminEntry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdminEntry.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnAdminEntry.ForeColor = System.Drawing.Color.White;
            this.btnAdminEntry.Location = new System.Drawing.Point(520, 390);
            this.btnAdminEntry.Name = "btnAdminEntry";
            this.btnAdminEntry.Size = new System.Drawing.Size(200, 60);
            this.btnAdminEntry.TabIndex = 2;
            this.btnAdminEntry.Text = "进入管理员功能";
            this.btnAdminEntry.UseVisualStyleBackColor = false;
            this.btnAdminEntry.Visible = false;
            this.btnAdminEntry.Click += new System.EventHandler(this.btnAdminEntry_Click);
            //
            // btnUserEntry
            //
            this.btnUserEntry.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnUserEntry.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            this.btnUserEntry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserEntry.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnUserEntry.ForeColor = System.Drawing.Color.White;
            this.btnUserEntry.Location = new System.Drawing.Point(280, 390);
            this.btnUserEntry.Name = "btnUserEntry";
            this.btnUserEntry.Size = new System.Drawing.Size(200, 60);
            this.btnUserEntry.TabIndex = 1;
            this.btnUserEntry.Text = "进入用户功能";
            this.btnUserEntry.UseVisualStyleBackColor = false;
            this.btnUserEntry.Click += new System.EventHandler(this.btnUserEntry_Click);
            //
            // lblWelcome
            //
            this.lblWelcome.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblWelcome.Font = new System.Drawing.Font("Microsoft YaHei UI", 24F, System.Drawing.FontStyle.Bold);
            this.lblWelcome.Location = new System.Drawing.Point(0, 0);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(1000, 300);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "欢迎使用图片标注系统";
            this.lblWelcome.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // MainForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 750);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "图片标注系统";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelWelcome.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuUser;
        private System.Windows.Forms.ToolStripMenuItem menuProjects;
        private System.Windows.Forms.ToolStripMenuItem menuAdmin;
        private System.Windows.Forms.ToolStripMenuItem menuAdminProjects;
        private System.Windows.Forms.ToolStripMenuItem menuAdminQueues;
        private System.Windows.Forms.ToolStripMenuItem menuAdminUsers;
        private System.Windows.Forms.ToolStripMenuItem menuAdminExport;
        private System.Windows.Forms.ToolStripMenuItem menuAccount;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblUser;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelWelcome;
        private System.Windows.Forms.Button btnAdminEntry;
        private System.Windows.Forms.Button btnUserEntry;
        private System.Windows.Forms.Label lblWelcome;
    }
}
