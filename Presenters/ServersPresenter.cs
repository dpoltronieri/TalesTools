using System;
using System.Linq;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public class ServersPresenter
    {
        private IServersView view;
        private Subject subject;

        public ServersPresenter(IServersView view, Subject subject)
        {
            this.view = view;
            this.subject = subject;
            BindEvents();
            LoadServers();
        }

        private void BindEvents()
        {
            this.view.AddServer += (s, e) => this.view.OpenAddServerForm(null);
            this.view.EditServer += (s, e) => this.view.OpenAddServerForm(e.Server);
            this.view.DeleteServer += (s, e) => DeleteServer(e.Server);
        }

        public void LoadServers()
        {
            var servers = LocalServerManager.GetLocalClients();
            this.view.RenderServers(servers);
        }

        private void DeleteServer(ClientDTO server)
        {
            if (this.view.ConfirmDeletion(server.name))
            {
                LocalServerManager.RemoveClient(server);
                this.subject.Notify(new Message(MessageCode.SERVER_LIST_CHANGED, "Server Deleted"));
                this.view.ShowMessage("Server " + server.name + " successfully deleted !!");
                LoadServers();
            }
        }
    }
}
