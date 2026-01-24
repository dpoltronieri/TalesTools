using System;
using System.Drawing;
using System.Windows.Forms;
using _4RTools.Utils;
using _4RTools.Model;
using System.Media;
using _4RTools.Properties;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class ToggleApplicationStateForm : Form, IObserver, IToggleApplicationStateView
    {
        private Subject subject;
        private ContextMenu contextMenu;
        private MenuItem menuItem;
        private ToggleApplicationStatePresenter presenter;

        public ToggleApplicationStateForm() { }

        public ToggleApplicationStateForm(Subject subject)
        {
            InitializeComponent();

            subject.Attach(this);
            this.subject = subject;
            this.presenter = new ToggleApplicationStatePresenter(this, subject);

            WireUpInputHandlers();
            InitializeContextualMenu();
        }

        private void WireUpInputHandlers()
        {
            this.txtStatusToggleKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            this.txtStatusToggleKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtStatusToggleKey.TextChanged += (s, e) => ToggleKeyChanged?.Invoke(this, EventArgs.Empty);

            this.txtStatusHealToggleKey.KeyDown += new KeyEventHandler(FormUtils.OnKeyDown);
            this.txtStatusHealToggleKey.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
            this.txtStatusHealToggleKey.TextChanged += (s, e) => ToggleHealKeyChanged?.Invoke(this, EventArgs.Empty);

            this.btnStatusToggle.Click += (s, e) => {
                bool isOn = this.btnStatusToggle.Text == "ON";
                this.presenter.ExecuteToggleStatus(isOn);
            };

            this.btnStatusHealToggle.Click += (s, e) => {
                bool isOn = this.btnStatusHealToggle.Text == "ON";
                this.presenter.ExecuteToggleHealStatus(isOn);
            };
        }

        private void InitializeContextualMenu()
        {
            this.contextMenu = new ContextMenu();
            this.menuItem = new MenuItem();

            this.contextMenu.MenuItems.AddRange(
                    new MenuItem[] { this.menuItem });

            this.menuItem.Index = 0;
            this.menuItem.Text = "Close";
            this.menuItem.Click += (s, e) => ShutdownApplication?.Invoke(this, EventArgs.Empty);

            this.notifyIconTray.ContextMenu = this.contextMenu;
        }

        public void Update(ISubject subject)
        {
            switch ((subject as Subject).Message.code)
            {
                case MessageCode.PROFILE_CHANGED:
                    this.presenter.OnProfileChanged();
                    break;
            }
        }

        public bool TurnOFF()
        {
            bool isOn = this.btnStatusToggle.Text == "ON";
            if (isOn)
            {
                this.presenter.ExecuteToggleStatus(true);
            }

            bool isOnheal = this.btnStatusHealToggle.Text == "ON";
            if (isOnheal)
            {
                this.presenter.ExecuteToggleHealStatus(true);
            }
            return true;
        }

        private void notifyIconDoubleClick(object sender, MouseEventArgs e)
        {
            this.subject.Notify(new Utils.Message(MessageCode.CLICK_ICON_TRAY, null));
        }

        // IToggleApplicationStateView Implementation
        public string ToggleKey { get => txtStatusToggleKey.Text; set => txtStatusToggleKey.Text = value; }
        public string ToggleHealKey { get => txtStatusHealToggleKey.Text; set => txtStatusHealToggleKey.Text = value; }
        public string StatusText { set => lblStatusToggle.Text = value; }
        public Color StatusColor { set => lblStatusToggle.ForeColor = value; }
        public string ToggleButtonText { set => btnStatusToggle.Text = value; }
        public Color ToggleButtonBackColor { set => btnStatusToggle.BackColor = value; }
        public string HealStatusText { set => lblStatusHealToggle.Text = value; }
        public Color HealStatusColor { set => lblStatusHealToggle.ForeColor = value; }
        public string ToggleHealButtonText { set => btnStatusHealToggle.Text = value; }
        public Color ToggleHealButtonBackColor { set => btnStatusHealToggle.BackColor = value; }

        public event EventHandler ToggleKeyChanged;
        public event EventHandler ToggleHealKeyChanged;
        public event EventHandler ToggleStatus;
        public event EventHandler ToggleHealStatus;
        public event EventHandler ShutdownApplication;

        public void SetIcon(Icon icon)
        {
            this.notifyIconTray.Icon = icon;
        }

        public void PlaySound(System.IO.Stream sound)
        {
            new SoundPlayer(sound).Play();
        }
    }
}
