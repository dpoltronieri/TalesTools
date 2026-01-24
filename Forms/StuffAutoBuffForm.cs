using System;
using System.Collections.Generic;
using System.Windows.Forms;
using _4RTools.Model;
using _4RTools.Utils;
using System.Windows.Input;
using System.Drawing;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class StuffAutoBuffForm : Form, IObserver, IStuffAutoBuffView
    {
        private List<BuffContainer> stuffContainers = new List<BuffContainer>();
        private StuffAutoBuffPresenter presenter;
        private AutoBuffStuff model;

        public StuffAutoBuffForm(Subject subject)
        {
            InitializeComponent();

            this.model = ProfileSingleton.GetCurrent().AutobuffStuff;
            this.presenter = new StuffAutoBuffPresenter(this, this.model);

            // Create a panel to hold the config buttons
            Panel configPanel = new Panel { Width = 520, Height = 60 };

            // Remove controls from the main form and add them to the panel
            this.Controls.Remove(this.label5);
            this.Controls.Remove(this.numericDelay);
            this.Controls.Remove(this.btnResetAutobuff);

            // Set locations relative to the panel
            this.label5.Location = new Point(10, 5);
            this.numericDelay.Location = new Point(10, 25);
            this.btnResetAutobuff.Location = new Point(80, 22);

            configPanel.Controls.Add(this.label5);
            configPanel.Controls.Add(this.numericDelay);
            configPanel.Controls.Add(this.btnResetAutobuff);

            // Add the panel to the top of the flow layout
            this.flowLayoutPanel1.Controls.Add(configPanel);

            stuffContainers.Add(new BuffContainer(this.PotionsGP, Buff.GetPotionsBuffs()));
            stuffContainers.Add(new BuffContainer(this.ElementalsGP, Buff.GetElementalsBuffs()));
            stuffContainers.Add(new BuffContainer(this.BoxesGP, Buff.GetBoxesBuffs()));
            stuffContainers.Add(new BuffContainer(this.FoodsGP, Buff.GetFoodBuffs()));
            stuffContainers.Add(new BuffContainer(this.ScrollBuffsGP, Buff.GetScrollBuffs()));
            stuffContainers.Add(new BuffContainer(this.EtcGP, Buff.GetETCBuffs()));

            foreach (var container in stuffContainers)
            {
                this.Controls.Remove(container.container);
                this.flowLayoutPanel1.Controls.Add(container.container);
            }

            new BuffRenderer(stuffContainers, toolTip1, ProfileSingleton.GetCurrent().AutobuffStuff.actionName, subject).doRender();
            subject.Attach(this);
            WireUpEvents();
        }

        private void WireUpEvents()
        {
            this.numericDelay.ValueChanged += (s, e) => DelayChanged?.Invoke(this, EventArgs.Empty);
            this.btnResetAutobuff.Click += (s, e) => ResetRequested?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RenderStuffGroups();
        }

        private void RenderStuffGroups()
        {
            this.Invoke((MethodInvoker)delegate {
                UserPreferences prefs = ProfileSingleton.GetCurrent().UserPreferences;

                var groupMapping = new Dictionary<string, GroupBox> {
                    { "FoodsGP", FoodsGP },
                    { "PotionsGP", PotionsGP },
                    { "BoxesGP", BoxesGP },
                    { "ElementalsGP", ElementalsGP },
                    { "ScrollBuffsGP", ScrollBuffsGP },
                    { "EtcGP", EtcGP },
                };

                foreach (var entry in groupMapping)
                {
                    if (prefs.AutobuffStuffVisibility.TryGetValue(entry.Key, out bool isVisible))
                    {
                        entry.Value.Visible = isVisible;
                    }
                    else
                    {
                        entry.Value.Visible = true; // Default to visible
                    }
                }
            });
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    RenderStuffGroups();
                    this.model = ProfileSingleton.GetCurrent().AutobuffStuff;
                    this.presenter.UpdateModel(this.model);
                    break;
                case MessageCode.TURN_OFF:
                    this.model.Stop();
                    break;
                case MessageCode.TURN_ON:
                    this.model.Start();
                    break;
            }
        }

        // IStuffAutoBuffView Implementation
        public int Delay { get => decimal.ToInt32(numericDelay.Value); set { try { numericDelay.Value = value; } catch {} } }

        public event EventHandler DelayChanged;
        public event EventHandler ResetRequested;

        public void UpdateUI(Dictionary<EffectStatusIDs, System.Windows.Input.Key> buffMapping)
        {
            BuffRenderer.doUpdate(buffMapping, this);
        }
    }
}