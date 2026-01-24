using System;
using System.Collections.Generic;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public class DevPresenter
    {
        private IDevView view;

        public DevPresenter(IDevView view)
        {
            this.view = view;
            BindEvents();
        }

        private void BindEvents()
        {
            this.view.MonitorTick += (s, e) => MonitorBuffs();
            this.view.SaveBuffName += (s, e) => SaveBuffName();
            this.view.Reload += (s, e) => this.view.ClearBuffList();
        }

        private void MonitorBuffs()
        {
            Client client = ClientSingleton.GetClient();
            if (client == null)
            {
                this.view.ClearBuffList();
                return;
            }

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

            this.view.UpdateBuffList(activeBuffs);
        }

        private void SaveBuffName()
        {
            if (int.TryParse(this.view.BuffId, out int id) && !string.IsNullOrWhiteSpace(this.view.BuffName))
            {
                BuffData.AddOrUpdateName(id, this.view.BuffName);
                this.view.ShowMessage($"Buff Name Saved: ID {id} = {this.view.BuffName}");
                this.view.BuffId = "";
                this.view.BuffName = "";
            }
            else
            {
                this.view.ShowMessage("Invalid ID or Name.");
            }
        }
    }
}
