using _4RTools.Model;
using _4RTools.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Presenters;

namespace _4RTools.Forms
{
    public partial class AddServerForm : Form, IAddServerView
    {
        Dictionary<string, int> inputMap = new Dictionary<string, int>();
        private AddServerPresenter presenter;

        public AddServerForm(ClientDTO dto, Subject subject)
        {
            InitializeComponent();
            SetupInputs();

            inputMap.Add("D0", 0);
            inputMap.Add("D1", 1);
            inputMap.Add("D2", 2);
            inputMap.Add("D3", 3);
            inputMap.Add("D4", 4);
            inputMap.Add("D5", 5);
            inputMap.Add("D6", 6);
            inputMap.Add("D7", 7);
            inputMap.Add("D8", 8);
            inputMap.Add("D9", 9);

            inputMap.Add("NumPad0", 0);
            inputMap.Add("NumPad1", 1);
            inputMap.Add("NumPad2", 2);
            inputMap.Add("NumPad3", 3);
            inputMap.Add("NumPad4", 4);
            inputMap.Add("NumPad5", 5);
            inputMap.Add("NumPad6", 6);
            inputMap.Add("NumPad7", 7);
            inputMap.Add("NumPad8", 8);
            inputMap.Add("NumPad9", 9);

            this.presenter = new AddServerPresenter(this, dto, subject);
            
            this.Load += (s, e) => LoadRequested?.Invoke(this, EventArgs.Empty);
            this.btnSave.Click += (s, e) => SaveClicked?.Invoke(this, EventArgs.Empty);
        }

        private void onTextChange(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (inputMap.ContainsKey(textBox.Text))
            {
                textBox.Text = inputMap[textBox.Text].ToString();
            }

            if (!LocalServerManager.IsHex(textBox.Text)) textBox.Clear();
        }

        public void SetupInputs()
        {
            try
            {
                foreach (Control c in FormUtils.GetAll(this, typeof(TextBox)))
                {
                    if (c is TextBox textBox)
                    {
                        textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(FormUtils.OnKeyDown);
                        textBox.KeyPress += new KeyPressEventHandler(FormUtils.OnKeyPress);
                        textBox.TextChanged += new EventHandler(this.onTextChange);
                    }
                }
            }
            catch { }
        }

        // IAddServerView Implementation
        public string HPAddress
        {
            get => string.Concat(txtHP1.Text, txtHP2.Text, txtHP3.Text, txtHP4.Text, txtHP5.Text, txtHP6.Text, txtHP7.Text, txtHP8.Text);
            set
            {
                List<char> chars = value.ToCharArray().ToList();
                txtHP1.Text = chars.ElementAtOrDefault(0).ToString();
                txtHP2.Text = chars.ElementAtOrDefault(1).ToString();
                txtHP3.Text = chars.ElementAtOrDefault(2).ToString();
                txtHP4.Text = chars.ElementAtOrDefault(3).ToString();
                txtHP5.Text = chars.ElementAtOrDefault(4).ToString();
                txtHP6.Text = chars.ElementAtOrDefault(5).ToString();
                txtHP7.Text = chars.ElementAtOrDefault(6).ToString();
                txtHP8.Text = chars.ElementAtOrDefault(7).ToString();
            }
        }

        public string NameAddress
        {
            get => string.Concat(txtName1.Text, txtName2.Text, txtName3.Text, txtName4.Text, txtName5.Text, txtName6.Text, txtName7.Text, txtName8.Text);
            set
            {
                List<char> chars = value.ToCharArray().ToList();
                txtName1.Text = chars.ElementAtOrDefault(0).ToString();
                txtName2.Text = chars.ElementAtOrDefault(1).ToString();
                txtName3.Text = chars.ElementAtOrDefault(2).ToString();
                txtName4.Text = chars.ElementAtOrDefault(3).ToString();
                txtName5.Text = chars.ElementAtOrDefault(4).ToString();
                txtName6.Text = chars.ElementAtOrDefault(5).ToString();
                txtName7.Text = chars.ElementAtOrDefault(6).ToString();
                txtName8.Text = chars.ElementAtOrDefault(7).ToString();
            }
        }

        public string SelectedProcessName { get => processCB.Text; set => processCB.Text = value; }
        public List<string> AvailableProcesses
        {
            set
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    this.processCB.Items.Clear();
                    foreach (var p in value) this.processCB.Items.Add(p);
                });
            }
        }

        public string WindowTitle { set => this.Text = value; }

        public event EventHandler SaveClicked;
        public event EventHandler LoadRequested;

        public void ShowMessage(string message, bool isError)
        {
            MessageBox.Show(message, isError ? "Warning" : "Message", MessageBoxButtons.OK, isError ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        public void CloseView()
        {
            this.Close();
        }
    }
}
