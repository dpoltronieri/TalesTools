using System;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Presenters
{
    public class CustomButtonPresenter
    {
        private ICustomButtonView view;
        private Custom model;

        public CustomButtonPresenter(ICustomButtonView view, Custom model)
        {
            this.view = view;
            this.model = model;
            BindEvents();
            UpdateView();
        }

        private void BindEvents()
        {
            this.view.TransferKeyChanged += (s, e) => {
                try {
                    this.model.tiMode = (Key)Enum.Parse(typeof(Key), this.view.TransferKey);
                    Save();
                } catch {}
            };

            this.view.PriorityKeyChanged += (s, e) => {
                try {
                    Key key = (Key)Enum.Parse(typeof(Key), this.view.PriorityKey);
                    this.model.priorityKey = key;
                    KeyboardHookHelper.PriorityKey = key;
                    Save();
                } catch {}
            };

            this.view.PriorityDelayChanged += (s, e) => {
                try {
                    int delay = int.Parse(this.view.PriorityDelay);
                    this.model.priorityDelay = delay;
                    KeyboardHookHelper.PriorityDelay = delay;
                    Save();
                } catch {}
            };
        }

        public void UpdateView()
        {
            this.view.TransferKey = this.model.tiMode.ToString();
            this.view.PriorityKey = this.model.priorityKey.ToString();
            this.view.PriorityDelay = this.model.priorityDelay.ToString();

            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                KeyboardHookHelper.PriorityKey = this.model.priorityKey;
                KeyboardHookHelper.GameWindowHandle = roClient.process.MainWindowHandle;
                KeyboardHookHelper.PriorityDelay = this.model.priorityDelay;
            }
        }

        public void UpdateModel(Custom newModel)
        {
            this.model = newModel;
            UpdateView();
        }

        private void Save()
        {
            ProfileSingleton.SetConfiguration(this.model);
        }
    }
}
