#if DEBUG
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
            this.dgvBrisaLeveBuffs = new System.Windows.Forms.DataGridView();
            this.colBuffName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEnumName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBuffStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRelatedKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblBrisaLeve = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.mainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBrisaLeveBuffs)).BeginInit();
            this.SuspendLayout();
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.dgvBrisaLeveBuffs);
            this.mainPanel.Controls.Add(this.lblBrisaLeve);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(800, 450);
            this.mainPanel.TabIndex = 0;
            // 
            // dgvBrisaLeveBuffs
            // 
            this.dgvBrisaLeveBuffs.AllowUserToAddRows = false;
            this.dgvBrisaLeveBuffs.AllowUserToDeleteRows = false;
            this.dgvBrisaLeveBuffs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBrisaLeveBuffs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBrisaLeveBuffs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBrisaLeveBuffs.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBuffName,
            this.colEnumName,
            this.colId,
            this.colBuffStatus,
            this.colRelatedKey});
            this.dgvBrisaLeveBuffs.Location = new System.Drawing.Point(12, 26);
            this.dgvBrisaLeveBuffs.Name = "dgvBrisaLeveBuffs";
            this.dgvBrisaLeveBuffs.ReadOnly = true;
            this.dgvBrisaLeveBuffs.Size = new System.Drawing.Size(776, 412);
            this.dgvBrisaLeveBuffs.TabIndex = 1;
            // 
            // colBuffName
            // 
            this.colBuffName.HeaderText = "Buff Name";
            this.colBuffName.Name = "colBuffName";
            this.colBuffName.ReadOnly = true;
            // 
            // colEnumName
            // 
            this.colEnumName.HeaderText = "Enum Name";
            this.colEnumName.Name = "colEnumName";
            this.colEnumName.ReadOnly = true;
            // 
            // colId
            // 
            this.colId.HeaderText = "ID";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            // 
            // colBuffStatus
            // 
            this.colBuffStatus.HeaderText = "Status";
            this.colBuffStatus.Name = "colBuffStatus";
            this.colBuffStatus.ReadOnly = true;
            // 
            // colRelatedKey
            // 
            this.colRelatedKey.HeaderText = "Related Key";
            this.colRelatedKey.Name = "colRelatedKey";
            this.colRelatedKey.ReadOnly = true;
            // 
            // lblBrisaLeve
            // 
            this.lblBrisaLeve.AutoSize = true;
            this.lblBrisaLeve.Location = new System.Drawing.Point(12, 10);
            this.lblBrisaLeve.Name = "lblBrisaLeve";
            this.lblBrisaLeve.Size = new System.Drawing.Size(132, 13);
            this.lblBrisaLeve.TabIndex = 0;
            this.lblBrisaLeve.Text = "Brisa Leve Buff Information";
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvBrisaLeveBuffs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.Label lblBrisaLeve;
        private System.Windows.Forms.DataGridView dgvBrisaLeveBuffs;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBuffName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEnumName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBuffStatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRelatedKey;
    }
}
#endif

