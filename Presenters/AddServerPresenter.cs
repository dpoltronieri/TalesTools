using System;
using System.Collections.Generic;
using System.Diagnostics;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public class AddServerPresenter
    {
        private IAddServerView view;
        private ClientDTO dto;
        private Subject subject;

        public AddServerPresenter(IAddServerView view, ClientDTO dto, Subject subject)
        {
            this.view = view;
            this.dto = dto;
            this.subject = subject;
            BindEvents();
            Initialize();
        }

        private void BindEvents()
        {
            this.view.LoadRequested += (s, e) => RefreshProcessList();
            this.view.SaveClicked += (s, e) => Save();
        }

        private void Initialize()
        {
            if (this.dto != null)
            {
                this.view.HPAddress = this.dto.hpAddress.Replace("0x", "");
                this.view.NameAddress = this.dto.nameAddress.Replace("0x", "");
                this.view.SelectedProcessName = this.dto.name;
                this.view.WindowTitle = "Edit Server " + this.dto.name;
            }
        }

        private void RefreshProcessList()
        {
            List<string> processes = new List<string>();
            foreach (Process p in Process.GetProcesses())
            {
                if (!string.IsNullOrEmpty(p.MainWindowTitle))
                {
                    processes.Add(p.ProcessName);
                }
            }
            this.view.AvailableProcesses = processes;
        }

        private void Save()
        {
            try
            {
                string hp = this.view.HPAddress;
                string name = this.view.NameAddress;
                string process = this.view.SelectedProcessName;

                if (this.dto == null)
                {
                    LocalServerManager.AddServer(hp, name, process);
                    this.view.ShowMessage("Server " + process + " successfully added !!", false);
                }
                else
                {
                    LocalServerManager.RemoveClient(this.dto);
                    LocalServerManager.AddServer(hp, name, process);
                    this.view.ShowMessage("Server " + process + " successfully saved !!", false);
                }

                this.subject.Notify(new Message(MessageCode.SERVER_LIST_CHANGED, "Server Added"));
                this.view.CloseView();
            }
            catch (Exception ex)
            {
                this.view.ShowMessage(ex.Message, true);
            }
        }
    }
}
