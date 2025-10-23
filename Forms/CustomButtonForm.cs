using System;
using System.Windows.Forms;
using _4RTools.Utils;
using _4RTools.Model;
using System.Windows.Input;

namespace _4RTools.Forms
{
    public partial class CustomButtonForm : Form, IObserver
    {

        private Custom custom;
        public CustomButtonForm(Subject subject)
        {
            InitializeComponent();
            toolTip1.SetToolTip(label1, "Simula alt+botão direito do mouse para transferencia rapida de itens entre armazem e inventario");
            subject.Attach(this);
        }

        public void Update(ISubject subject)
        {

            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    InitializeApplicationForm();
                    break;
                case MessageCode.TURN_OFF:
                    if(this.custom != null) this.custom.Stop();
                    break;
                case MessageCode.TURN_ON:
                    if(this.custom != null) this.custom.Start();
                    break;
            }
        }

        private void InitializeApplicationForm()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                KeyboardHookHelper.PriorityKey = ProfileSingleton.GetCurrent().Custom.priorityKey;
                KeyboardHookHelper.GameWindowHandle = roClient.process.MainWindowHandle;
                KeyboardHookHelper.PriorityDelay = ProfileSingleton.GetCurrent().Custom.priorityDelay;  
            }
            this.custom = ProfileSingleton.GetCurrent().Custom; 
            this.txtTransferKey.Text = custom.tiMode.ToString();
            this.txtPriorityKey.Text = custom.priorityKey.ToString();
            this.txtPriorityDelay.Text = this.custom.priorityDelay.ToString();

            this.txtTransferKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.txtTransferKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtTransferKey.TextChanged += new EventHandler(onTransferKeyChange);
            this.txtPriorityKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.txtPriorityKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtPriorityKey.TextChanged += new EventHandler(onPriorityKeyChange);
            this.ActiveControl = null;
        }
        private void txtPriorityDelay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                ProfileSingleton.GetCurrent().Custom.priorityDelay = Convert.ToInt16(this.txtPriorityDelay.Value);
                ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().Custom);
            }
            catch { }
        }

        private void onTransferKeyChange(object sender, EventArgs e)
        {
            Key key = (Key)Enum.Parse(typeof(Key), this.txtTransferKey.Text.ToString());
            try
            {
                this.custom.tiMode = key;
                ProfileSingleton.SetConfiguration(this.custom);
            }
            catch { }
            this.ActiveControl = null;
        }

        private void onPriorityKeyChange(object sender, EventArgs e)
        {
            Key key = (Key)Enum.Parse(typeof(Key), this.txtPriorityKey.Text.ToString());
            try
            {
                this.custom.priorityKey = key;
                ProfileSingleton.SetConfiguration(this.custom);
            }
            catch { }
            this.ActiveControl = null;
        }
    }
}
