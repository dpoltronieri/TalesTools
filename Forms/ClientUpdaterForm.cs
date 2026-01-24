using _4RTools.Utils;
using _4RTools.Model;
using System.Windows.Forms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class ClientUpdaterForm : Form, IClientUpdaterView
    {
        private ClientUpdaterPresenter presenter;

        public ClientUpdaterForm()
        {
            InitializeComponent();
            this.presenter = new ClientUpdaterPresenter(this);
            this.Load += async (s, e) => await this.presenter.StartUpdate();
        }

        // IClientUpdaterView Implementation
        public int ProgressBarValue { get => pbSupportedServer.Value; set => pbSupportedServer.Value = value; }
        public int MaxProgressBarValue { get => pbSupportedServer.Maximum; set => pbSupportedServer.Maximum = value; }

        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void StartContainer()
        {
            new Container().Show();
        }

        public void CloseView()
        {
            this.Hide();
        }
    }
}
