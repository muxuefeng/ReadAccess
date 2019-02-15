namespace TTStatistics
{
    partial class TTStatistics
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TTStatistics));
            this.gb_datacenter = new System.Windows.Forms.GroupBox();
            this.tb_datacenter = new System.Windows.Forms.TextBox();
            this.btn_datacenter = new System.Windows.Forms.Button();
            this.gb_statistics = new System.Windows.Forms.GroupBox();
            this.dataGrid_stroke = new System.Windows.Forms.DataGridView();
            this.dataGrid_players = new System.Windows.Forms.DataGridView();
            this.gb_match = new System.Windows.Forms.GroupBox();
            this.tb_resultpath = new System.Windows.Forms.TextBox();
            this.btn_path = new System.Windows.Forms.Button();
            this.btn_result = new System.Windows.Forms.Button();
            this.gb_datacenter.SuspendLayout();
            this.gb_statistics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_stroke)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_players)).BeginInit();
            this.gb_match.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_datacenter
            // 
            this.gb_datacenter.Controls.Add(this.tb_datacenter);
            this.gb_datacenter.Controls.Add(this.btn_datacenter);
            this.gb_datacenter.Location = new System.Drawing.Point(4, 3);
            this.gb_datacenter.Name = "gb_datacenter";
            this.gb_datacenter.Size = new System.Drawing.Size(517, 70);
            this.gb_datacenter.TabIndex = 0;
            this.gb_datacenter.TabStop = false;
            this.gb_datacenter.Text = "数据中心设置";
            // 
            // tb_datacenter
            // 
            this.tb_datacenter.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_datacenter.Location = new System.Drawing.Point(117, 26);
            this.tb_datacenter.Name = "tb_datacenter";
            this.tb_datacenter.Size = new System.Drawing.Size(380, 29);
            this.tb_datacenter.TabIndex = 1;
            // 
            // btn_datacenter
            // 
            this.btn_datacenter.Location = new System.Drawing.Point(8, 20);
            this.btn_datacenter.Name = "btn_datacenter";
            this.btn_datacenter.Size = new System.Drawing.Size(103, 42);
            this.btn_datacenter.TabIndex = 0;
            this.btn_datacenter.Text = "加载数据中心";
            this.btn_datacenter.UseVisualStyleBackColor = true;
            this.btn_datacenter.Click += new System.EventHandler(this.btn_datacenter_Click);
            // 
            // gb_statistics
            // 
            this.gb_statistics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gb_statistics.Controls.Add(this.dataGrid_stroke);
            this.gb_statistics.Controls.Add(this.dataGrid_players);
            this.gb_statistics.Location = new System.Drawing.Point(4, 88);
            this.gb_statistics.Name = "gb_statistics";
            this.gb_statistics.Size = new System.Drawing.Size(1323, 396);
            this.gb_statistics.TabIndex = 1;
            this.gb_statistics.TabStop = false;
            this.gb_statistics.Text = "技术统计";
            // 
            // dataGrid_stroke
            // 
            this.dataGrid_stroke.AllowUserToAddRows = false;
            this.dataGrid_stroke.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGrid_stroke.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid_stroke.Location = new System.Drawing.Point(663, 20);
            this.dataGrid_stroke.MultiSelect = false;
            this.dataGrid_stroke.Name = "dataGrid_stroke";
            this.dataGrid_stroke.RowTemplate.Height = 23;
            this.dataGrid_stroke.Size = new System.Drawing.Size(648, 370);
            this.dataGrid_stroke.TabIndex = 39;
            // 
            // dataGrid_players
            // 
            this.dataGrid_players.AllowUserToAddRows = false;
            this.dataGrid_players.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGrid_players.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid_players.Location = new System.Drawing.Point(8, 20);
            this.dataGrid_players.MultiSelect = false;
            this.dataGrid_players.Name = "dataGrid_players";
            this.dataGrid_players.RowTemplate.Height = 23;
            this.dataGrid_players.Size = new System.Drawing.Size(648, 370);
            this.dataGrid_players.TabIndex = 39;
            // 
            // gb_match
            // 
            this.gb_match.Controls.Add(this.tb_resultpath);
            this.gb_match.Controls.Add(this.btn_path);
            this.gb_match.Controls.Add(this.btn_result);
            this.gb_match.Location = new System.Drawing.Point(544, 3);
            this.gb_match.Name = "gb_match";
            this.gb_match.Size = new System.Drawing.Size(783, 70);
            this.gb_match.TabIndex = 2;
            this.gb_match.TabStop = false;
            this.gb_match.Text = "比赛信息";
            // 
            // tb_resultpath
            // 
            this.tb_resultpath.Font = new System.Drawing.Font("SimSun", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tb_resultpath.Location = new System.Drawing.Point(136, 26);
            this.tb_resultpath.Name = "tb_resultpath";
            this.tb_resultpath.Size = new System.Drawing.Size(551, 29);
            this.tb_resultpath.TabIndex = 1;
            // 
            // btn_path
            // 
            this.btn_path.Location = new System.Drawing.Point(11, 19);
            this.btn_path.Name = "btn_path";
            this.btn_path.Size = new System.Drawing.Size(119, 43);
            this.btn_path.TabIndex = 42;
            this.btn_path.Text = "选择文件（.xlsx）";
            this.btn_path.UseVisualStyleBackColor = true;
            this.btn_path.Click += new System.EventHandler(this.btn_path_Click);
            // 
            // btn_result
            // 
            this.btn_result.Location = new System.Drawing.Point(693, 19);
            this.btn_result.Name = "btn_result";
            this.btn_result.Size = new System.Drawing.Size(84, 42);
            this.btn_result.TabIndex = 0;
            this.btn_result.Text = "获取成绩";
            this.btn_result.UseVisualStyleBackColor = true;
            this.btn_result.Click += new System.EventHandler(this.btn_result_Click);
            // 
            // TTStatistics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(217)))), ((int)(((byte)(247)))));
            this.ClientSize = new System.Drawing.Size(1326, 481);
            this.Controls.Add(this.gb_match);
            this.Controls.Add(this.gb_statistics);
            this.Controls.Add(this.gb_datacenter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TTStatistics";
            this.Text = "TTStatistics";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TTStatistics_FormClosing);
            this.gb_datacenter.ResumeLayout(false);
            this.gb_datacenter.PerformLayout();
            this.gb_statistics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_stroke)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid_players)).EndInit();
            this.gb_match.ResumeLayout(false);
            this.gb_match.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_datacenter;
        private System.Windows.Forms.TextBox tb_datacenter;
        private System.Windows.Forms.Button btn_datacenter;
        private System.Windows.Forms.GroupBox gb_statistics;
        private System.Windows.Forms.GroupBox gb_match;
        private System.Windows.Forms.DataGridView dataGrid_players;
        private System.Windows.Forms.Button btn_result;
        private System.Windows.Forms.Button btn_path;
        private System.Windows.Forms.TextBox tb_resultpath;
        private System.Windows.Forms.DataGridView dataGrid_stroke;
    }
}

