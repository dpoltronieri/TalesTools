using System;
using System.Collections.Generic;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public interface IServersView
    {
        event EventHandler AddServer;
        event EventHandler<ServerEventArgs> EditServer;
        event EventHandler<ServerEventArgs> DeleteServer;

        void RenderServers(List<ClientDTO> servers);
        void ShowMessage(string message);
        bool ConfirmDeletion(string serverName);
        void OpenAddServerForm(ClientDTO server);
    }

    public class ServerEventArgs : EventArgs
    {
        public ClientDTO Server { get; set; }
    }
}
