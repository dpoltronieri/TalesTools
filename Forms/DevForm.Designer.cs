namespace _4RTools.Forms
{
    partial class DevForm
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
                    this.mainPanel = new System.Windows.Forms.Panel();
                    this.dgvAllBuffs = new System.Windows.Forms.DataGridView();
                    this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    this.colCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
                    this.lblTitle = new System.Windows.Forms.Label();
                    this.grpAddBuff = new System.Windows.Forms.GroupBox();
                    this.lblBuffId = new System.Windows.Forms.Label();
                    this.txtBuffId = new System.Windows.Forms.TextBox();
                    this.lblBuffName = new System.Windows.Forms.Label();
                    this.txtBuffName = new System.Windows.Forms.TextBox();
                    this.btnSaveBuff = new System.Windows.Forms.Button();
                    this.btnReload = new System.Windows.Forms.Button();
                    this.timer = new System.Windows.Forms.Timer(this.components);
                    this.mainPanel.SuspendLayout();
                    ((System.ComponentModel.ISupportInitialize)(this.dgvAllBuffs)).BeginInit();
                    this.grpAddBuff.SuspendLayout();
                    this.SuspendLayout();
                    // 
                    // mainPanel
                    // 
                    this.mainPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(31)))), ((int)(((byte)(34)))));
                    this.mainPanel.Controls.Add(this.grpAddBuff);
                    this.mainPanel.Controls.Add(this.dgvAllBuffs);
                    this.mainPanel.Controls.Add(this.lblTitle);
                    this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.mainPanel.Location = new System.Drawing.Point(0, 0);
                    this.mainPanel.Name = "mainPanel";
                    this.mainPanel.Size = new System.Drawing.Size(800, 450);
                    this.mainPanel.TabIndex = 0;
                    // 
                    // dgvAllBuffs
                    // 
                    this.dgvAllBuffs.AllowUserToAddRows = false;
                    this.dgvAllBuffs.AllowUserToDeleteRows = false;
                    this.dgvAllBuffs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
                    | System.Windows.Forms.AnchorStyles.Left) 
                    | System.Windows.Forms.AnchorStyles.Right)));
                    this.dgvAllBuffs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
                    this.dgvAllBuffs.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(45)))), ((int)(((byte)(49)))));
                    this.dgvAllBuffs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                                this.dgvAllBuffs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                                this.colId,
                                this.colName,
                                this.colCount});
                                this.dgvAllBuffs.Location = new System.Drawing.Point(12, 100);
                                this.dgvAllBuffs.Name = "dgvAllBuffs";
                                this.dgvAllBuffs.ReadOnly = true;
                                this.dgvAllBuffs.Size = new System.Drawing.Size(776, 338);
                                this.dgvAllBuffs.TabIndex = 1;
                                // 
                                // colId
                                // 
                                this.colId.HeaderText = "ID";
                                this.colId.Name = "colId";
                                this.colId.ReadOnly = true;
                                // 
                                // colName
                                // 
                                this.colName.HeaderText = "Name";
                                this.colName.Name = "colName";
                                this.colName.ReadOnly = true;
                                // 
                                // colCount
                                // 
                                this.colCount.HeaderText = "Value/Count";
                                this.colCount.Name = "colCount";
                                this.colCount.ReadOnly = true;
                                // 
                                // lblTitle
                                // 
                                this.lblTitle.AutoSize = true;
                                this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                                this.lblTitle.ForeColor = System.Drawing.Color.White;
                                this.lblTitle.Location = new System.Drawing.Point(12, 10);
                                this.lblTitle.Name = "lblTitle";
                                this.lblTitle.Size = new System.Drawing.Size(176, 17);
                                this.lblTitle.TabIndex = 0;
                                this.lblTitle.Text = "Active Buffs Monitoring";
                                // 
                                // grpAddBuff
                                // 
                                this.grpAddBuff.Controls.Add(this.btnReload);
                                this.grpAddBuff.Controls.Add(this.btnSaveBuff);
                                this.grpAddBuff.Controls.Add(this.txtBuffName);
                                this.grpAddBuff.Controls.Add(this.lblBuffName);
                                this.grpAddBuff.Controls.Add(this.txtBuffId);
                                this.grpAddBuff.Controls.Add(this.lblBuffId);
                                this.grpAddBuff.ForeColor = System.Drawing.Color.White;
                                this.grpAddBuff.Location = new System.Drawing.Point(12, 35);
                                this.grpAddBuff.Name = "grpAddBuff";
                                this.grpAddBuff.Size = new System.Drawing.Size(776, 59);
                                this.grpAddBuff.TabIndex = 2;
                                this.grpAddBuff.TabStop = false;
                                this.grpAddBuff.Text = "Name Unknown Buff";
                                // 
                                // lblBuffId
                                // 
                                this.lblBuffId.AutoSize = true;
                                this.lblBuffId.Location = new System.Drawing.Point(15, 25);
                                this.lblBuffId.Name = "lblBuffId";
                                this.lblBuffId.Size = new System.Drawing.Size(43, 13);
                                this.lblBuffId.TabIndex = 0;
                                this.lblBuffId.Text = "Buff ID:";
                                // 
                                // txtBuffId
                                // 
                                this.txtBuffId.Location = new System.Drawing.Point(64, 22);
                                this.txtBuffId.Name = "txtBuffId";
                                this.txtBuffId.Size = new System.Drawing.Size(100, 20);
                                this.txtBuffId.TabIndex = 1;
                                // 
                                // lblBuffName
                                // 
                                this.lblBuffName.AutoSize = true;
                                this.lblBuffName.Location = new System.Drawing.Point(180, 25);
                                this.lblBuffName.Name = "lblBuffName";
                                this.lblBuffName.Size = new System.Drawing.Size(60, 13);
                                this.lblBuffName.TabIndex = 2;
                                this.lblBuffName.Text = "Buff Name:";
                                // 
                                // txtBuffName
                                // 
                                this.txtBuffName.Location = new System.Drawing.Point(246, 22);
                                this.txtBuffName.Name = "txtBuffName";
                                this.txtBuffName.Size = new System.Drawing.Size(200, 20);
                                this.txtBuffName.TabIndex = 3;
                                // 
                                // btnSaveBuff
                                // 
                                this.btnSaveBuff.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
                                this.btnSaveBuff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                                this.btnSaveBuff.Location = new System.Drawing.Point(460, 20);
                                this.btnSaveBuff.Name = "btnSaveBuff";
                                this.btnSaveBuff.Size = new System.Drawing.Size(75, 23);
                                this.btnSaveBuff.TabIndex = 4;
                                this.btnSaveBuff.Text = "Save";
                                this.btnSaveBuff.UseVisualStyleBackColor = false;
                                this.btnSaveBuff.Click += new System.EventHandler(this.btnSaveBuff_Click);
                                // 
                                // btnReload
                                // 
                                this.btnReload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(51)))), ((int)(((byte)(56)))));
                                this.btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                                this.btnReload.Location = new System.Drawing.Point(541, 20);
                                this.btnReload.Name = "btnReload";
                                this.btnReload.Size = new System.Drawing.Size(95, 23);
                                this.btnReload.TabIndex = 5;
                                this.btnReload.Text = "Clear & Reload";
                                this.btnReload.UseVisualStyleBackColor = false;
                                this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
                                // 
                                // timer
                                // 
                                this.timer.Enabled = true;
                                this.timer.Interval = 500;
                                this.timer.Tick += new System.EventHandler(this.timer_Tick);
                                // 
                                // DevForm
                                // 
                                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                                this.ClientSize = new System.Drawing.Size(800, 450);
                                this.Controls.Add(this.mainPanel);
                                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                                this.Name = "DevForm";
                                this.Text = "DevForm";
                                this.Load += new System.EventHandler(this.DevForm_Load);
                                this.mainPanel.ResumeLayout(false);
                                this.mainPanel.PerformLayout();
                                ((System.ComponentModel.ISupportInitialize)(this.dgvAllBuffs)).EndInit();
                                this.grpAddBuff.ResumeLayout(false);
                                this.grpAddBuff.PerformLayout();
                                this.ResumeLayout(false);
                    
                            }
                    
                            #endregion
                    
                            private System.Windows.Forms.Panel mainPanel;
                            private System.Windows.Forms.Label lblTitle;
                            private System.Windows.Forms.DataGridView dgvAllBuffs;
                            private System.Windows.Forms.GroupBox grpAddBuff;
                            private System.Windows.Forms.Label lblBuffId;
                            private System.Windows.Forms.TextBox txtBuffId;
                            private System.Windows.Forms.Label lblBuffName;
                            private System.Windows.Forms.TextBox txtBuffName;
                            private System.Windows.Forms.Button btnSaveBuff;
                            private System.Windows.Forms.Button btnReload;
                            private System.Windows.Forms.Timer timer;
                            private System.Windows.Forms.DataGridViewTextBoxColumn colId;
                            private System.Windows.Forms.DataGridViewTextBoxColumn colName;
                            private System.Windows.Forms.DataGridViewTextBoxColumn colCount;
                        }
                    }        