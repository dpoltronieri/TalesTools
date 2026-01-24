using System;
using System.Windows.Forms;
using _4RTools.Utils;
using _4RTools.Model;
using System.Windows.Input;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class CustomButtonForm : Form, IObserver, ICustomButtonView
    {
        private Custom custom;
        private CustomButtonPresenter presenter;

        public CustomButtonForm(Subject subject)
        {
            InitializeComponent();
            toolTip1.SetToolTip(label1, "Simula alt+botão direito do mouse para transferencia rapida de itens entre armazem e inventario");
            
            this.custom = ProfileSingleton.GetCurrent().Custom;
            this.presenter = new CustomButtonPresenter(this, this.custom);

            WireUpInputHandlers();
            subject.Attach(this);
        }

        private void WireUpInputHandlers()
        {
            this.txtTransferKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.txtTransferKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtTransferKey.TextChanged += (s, e) => TransferKeyChanged?.Invoke(this, EventArgs.Empty);

            this.txtPriorityKey.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
            this.txtPriorityKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtPriorityKey.TextChanged += (s, e) => PriorityKeyChanged?.Invoke(this, EventArgs.Empty);

            this.txtPriorityDelay.TextChanged += (s, e) => PriorityDelayChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.custom = ProfileSingleton.GetCurrent().Custom;
                    this.presenter.UpdateModel(this.custom);
                    break;
                case MessageCode.TURN_OFF:
                    if(this.custom != null) this.custom.Stop();
                    break;
                case MessageCode.TURN_ON:
                    if(this.custom != null) this.custom.Start();
                    break;
            }
        }

        // ICustomButtonView Implementation
        public string TransferKey { get => txtTransferKey.Text; set => txtTransferKey.Text = value; }
        public string PriorityKey { get => txtPriorityKey.Text; set => txtPriorityKey.Text = value; }
        public string PriorityDelay { get => txtPriorityDelay.Value.ToString(); set { try { txtPriorityDelay.Value = decimal.Parse(value); } catch {} } }

        public event EventHandler TransferKeyChanged;
        public event EventHandler PriorityKeyChanged;
        public event EventHandler PriorityDelayChanged;
    }
}
