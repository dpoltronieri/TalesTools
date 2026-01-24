using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class ServersForm : Form, IObserver, IServersView
    {
        private Subject subject;
        private ServersPresenter presenter;

        public ServersForm(Subject subject)
        {
            InitializeComponent();
            this.subject = subject;
            this.presenter = new ServersPresenter(this, subject);
            
            this.btnAddServer.Click += (s, e) => AddServer?.Invoke(this, EventArgs.Empty);
            this.datagridServers.AllowUserToAddRows = false;
            
            subject.Attach(this);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.SERVER_LIST_CHANGED:
                    this.presenter.LoadServers();
                    break;
            }
        }

        private void datagridServers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //Update
            if (e.RowIndex < 0) return;

            ClientDTO current = (ClientDTO)clientDTOBindingSource[e.RowIndex];
            current.index = e.RowIndex;

            if (this.datagridServers.Columns[e.ColumnIndex].Name == "Delete")
            {
                DeleteServer?.Invoke(this, new ServerEventArgs { Server = current });
            }
            else
            {
                EditServer?.Invoke(this, new ServerEventArgs { Server = current });
            }
        }

        // IServersView Implementation
        public event EventHandler AddServer;
        public event EventHandler<ServerEventArgs> EditServer;
        public event EventHandler<ServerEventArgs> DeleteServer;

        public void RenderServers(List<ClientDTO> servers)
        {
            clientDTOBindingSource.Clear();
            servers.ForEach(c => clientDTOBindingSource.Add(c));
        }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public bool ConfirmDeletion(string serverName)
        {
            return MessageBox.Show("Are you sure want to delete this Server?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        public void OpenAddServerForm(ClientDTO server)
        {
            new AddServerForm(server, this.subject).Show();
        }
    }
}
