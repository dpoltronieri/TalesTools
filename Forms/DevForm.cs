using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;
using System.Linq;

namespace _4RTools.Forms
{
    public partial class DevForm : Form
    {
        public DevForm()
        {
            InitializeComponent();
        }

        private void DevForm_Load(object sender, EventArgs e)
        {
            // Initial load of buff names happens in static constructor of BuffData
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Client client = ClientSingleton.GetClient();
            if (client == null)
            {
                // Clear grid if no client
                this.dgvAllBuffs.Rows.Clear();
                return;
            }

            // Read all active buffs
            Dictionary<int, string> activeBuffs = new Dictionary<int, string>();

            for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
            {
                uint currentStatusId = client.CurrentBuffStatusCode(i);
                if (currentStatusId != uint.MaxValue && currentStatusId > 0)
                {
                    int id = (int)currentStatusId;
                    if (!activeBuffs.ContainsKey(id))
                    {
                        activeBuffs.Add(id, BuffData.GetBuffName(id));
                    }
                }
            }

            // Update GridView
            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();
            List<int> existingIds = new List<int>();

            foreach (DataGridViewRow row in this.dgvAllBuffs.Rows)
            {
                if (row.Cells["colId"].Value != null)
                {
                    int rowId = (int)row.Cells["colId"].Value;
                    existingIds.Add(rowId);

                    if (!activeBuffs.ContainsKey(rowId))
                    {
                        rowsToRemove.Add(row);
                    }
                    else
                    {
                        // Update name if changed (e.g. user just saved a new name)
                        row.Cells["colName"].Value = activeBuffs[rowId];
                    }
                }
            }

            // Remove rows for buffs that are gone
            foreach (var row in rowsToRemove)
            {
                this.dgvAllBuffs.Rows.Remove(row);
            }

            // Add new rows
            foreach (var buff in activeBuffs)
            {
                if (!existingIds.Contains(buff.Key))
                {
                    this.dgvAllBuffs.Rows.Add(buff.Key, buff.Value, "N/A");
                }
            }
        }

        private void btnSaveBuff_Click(object sender, EventArgs e)
        {
            if (int.TryParse(this.txtBuffId.Text, out int id) && !string.IsNullOrWhiteSpace(this.txtBuffName.Text))
            {
                BuffData.AddOrUpdateName(id, this.txtBuffName.Text);
                MessageBox.Show($"Buff Name Saved: ID {id} = {this.txtBuffName.Text}");
                this.txtBuffId.Text = "";
                this.txtBuffName.Text = "";
            }
            else
            {
                MessageBox.Show("Invalid ID or Name.");
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            this.dgvAllBuffs.Rows.Clear();
        }
    }
}