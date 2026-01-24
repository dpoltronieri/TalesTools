using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;
using System.Linq;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class DevForm : Form, IDevView
    {
        private DevPresenter presenter;

        public DevForm()
        {
            InitializeComponent();
            this.presenter = new DevPresenter(this);
            
            this.timer.Tick += (s, e) => MonitorTick?.Invoke(this, EventArgs.Empty);
            this.btnSaveBuff.Click += (s, e) => SaveBuffName?.Invoke(this, EventArgs.Empty);
            this.btnReload.Click += (s, e) => Reload?.Invoke(this, EventArgs.Empty);
        }

        // IDevView Implementation
        public string BuffId { get => txtBuffId.Text; set => txtBuffId.Text = value; }
        public string BuffName { get => txtBuffName.Text; set => txtBuffName.Text = value; }

        public event EventHandler SaveBuffName;
        public event EventHandler Reload;
        public event EventHandler MonitorTick;

        public void UpdateBuffList(Dictionary<int, string> activeBuffs)
        {
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
                        row.Cells["colName"].Value = activeBuffs[rowId];
                    }
                }
            }

            foreach (var row in rowsToRemove)
            {
                this.dgvAllBuffs.Rows.Remove(row);
            }

            foreach (var buff in activeBuffs)
            {
                if (!existingIds.Contains(buff.Key))
                {
                    this.dgvAllBuffs.Rows.Add(buff.Key, buff.Value, "N/A");
                }
            }
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ClearBuffList()
        {
            this.dgvAllBuffs.Rows.Clear();
        }
    }
}