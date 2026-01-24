using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _4RTools.Model;
using _4RTools.Utils;
using Newtonsoft.Json;

namespace _4RTools.Presenters
{
    public class ClientUpdaterPresenter
    {
        private IClientUpdaterView view;

        public ClientUpdaterPresenter(IClientUpdaterView view)
        {
            this.view = view;
        }

        public async Task StartUpdate()
        {
            List<ClientDTO> clients = new List<ClientDTO>();

            try
            {
                clients.AddRange(LocalServerManager.GetLocalClients());
                this.view.MaxProgressBarValue = clients.Count > 0 ? clients.Count : 100;
                LoadServers(clients);
                await Task.Delay(100);
            }
            catch (Exception)
            {
                this.view.ShowMessage("Cannot load supported_servers file. Loading resource instead....");
                string resourceContent = Resources._4RTools.ETCResource.supported_servers;
                clients = JsonConvert.DeserializeObject<List<ClientDTO>>(resourceContent);
                this.view.MaxProgressBarValue = clients.Count;
                LoadServers(clients);
            }
            finally
            {
                this.view.StartContainer();
                this.view.CloseView();
            }
        }

        private void LoadServers(List<ClientDTO> clients)
        {
            foreach (ClientDTO clientDTO in clients)
            {
                try
                {
                    ClientListSingleton.AddClient(new Client(clientDTO));
                    this.view.ProgressBarValue += 1;
                }
                catch { }
            }
        }
    }
}
