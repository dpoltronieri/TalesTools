using System;
using System.Collections.Generic;
using System.Windows.Forms;
using _4RTools.Model;
using _4RTools.Utils;
using System.Windows.Input;
using System.Drawing;

namespace _4RTools.Forms
{
    public partial class StuffAutoBuffForm : Form, IObserver
    {
        private List<BuffContainer> stuffContainers = new List<BuffContainer>();

        public StuffAutoBuffForm(Subject subject)
        {
            InitializeComponent();

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
                    BuffRenderer.doUpdate(new Dictionary<EffectStatusIDs, Key>(ProfileSingleton.GetCurrent().AutobuffStuff.buffMapping), this);
                    this.numericDelay.Value = ProfileSingleton.GetCurrent().AutobuffStuff.delay;
                    break;
                case MessageCode.TURN_OFF:
                    ProfileSingleton.GetCurrent().AutobuffStuff.Stop();
                    break;
                case MessageCode.TURN_ON:
                    ProfileSingleton.GetCurrent().AutobuffStuff.Start();
                    break;
            }
        }

        private void btnResetAutobuff_Click(object sender, EventArgs e)
        {
            ProfileSingleton.GetCurrent().AutobuffStuff.ClearKeyMapping();
            ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutobuffStuff);
            BuffRenderer.doUpdate(new Dictionary<EffectStatusIDs, Key>(ProfileSingleton.GetCurrent().AutobuffStuff.buffMapping), this);
            this.numericDelay.Value = 100;
        }

        private void numericDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProfileSingleton.GetCurrent().AutobuffStuff.delay = Convert.ToInt16(this.numericDelay.Value);
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().AutobuffStuff);
                this.ActiveControl = null;
            }
            catch { }
        }
    }
}
