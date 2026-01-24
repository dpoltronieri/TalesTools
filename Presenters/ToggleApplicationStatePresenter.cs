using System;
using System.Drawing;
using System.Windows.Forms;
using _4RTools.Model;
using _4RTools.Utils;
using System.Media;
using _4RTools.Properties;

namespace _4RTools.Presenters
{
    public class ToggleApplicationStatePresenter
    {
        private IToggleApplicationStateView view;
        private Subject subject;
        private Keys lastKey;
        private Keys healLastKey;
        
        // Internal state tracking
        private bool _isStatusOn = false; 
        private bool _isHealStatusOn = false;

        public ToggleApplicationStatePresenter(IToggleApplicationStateView view, Subject subject)
        {
            this.view = view;
            this.subject = subject;
            Initialize();
            BindEvents();
        }

        private void Initialize()
        {
            KeyboardHook.Enable();
            this.view.ToggleKey = ProfileSingleton.GetCurrent().UserPreferences.toggleStateKey;
            this.view.ToggleHealKey = ProfileSingleton.GetCurrent().UserPreferences.toggleStateHealKey;
        }

        private void BindEvents()
        {
            this.view.ToggleKeyChanged += (s, e) => {
                try {
                    Keys currentToggleKey = (Keys)Enum.Parse(typeof(Keys), this.view.ToggleKey);
                    KeyboardHook.RemoveDown(lastKey);
                    KeyboardHook.AddKeyDown(currentToggleKey, new KeyboardHook.KeyPressed(ToggleStatus));
                    ProfileSingleton.GetCurrent().UserPreferences.toggleStateKey = currentToggleKey.ToString();
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                    lastKey = currentToggleKey;
                } catch {}
            };

            this.view.ToggleHealKeyChanged += (s, e) => {
                try {
                    Keys currentHealToggleKey = (Keys)Enum.Parse(typeof(Keys), this.view.ToggleHealKey);
                    KeyboardHook.RemoveUp(healLastKey);
                    KeyboardHook.AddKeyUp(currentHealToggleKey, new KeyboardHook.KeyPressed(ToggleStatusHeal));
                    ProfileSingleton.GetCurrent().UserPreferences.toggleStateHealKey = currentHealToggleKey.ToString();
                    ProfileSingleton.SetConfiguration(ProfileSingleton.GetCurrent().UserPreferences);
                    healLastKey = currentHealToggleKey;
                } catch {}
            };

            this.view.ToggleStatus += (s, e) => ToggleStatus();
            this.view.ToggleHealStatus += (s, e) => ToggleStatusHeal();
            this.view.ShutdownApplication += (s, e) => this.subject.Notify(new Utils.Message(MessageCode.SHUTDOWN_APPLICATION, null));
        }

        public void OnProfileChanged()
        {
            // Re-bind keys based on new profile
            Keys currentToggleKey = (Keys)Enum.Parse(typeof(Keys), ProfileSingleton.GetCurrent().UserPreferences.toggleStateKey);
            KeyboardHook.RemoveDown(lastKey);
            this.view.ToggleKey = currentToggleKey.ToString();
            KeyboardHook.AddKeyDown(currentToggleKey, new KeyboardHook.KeyPressed(ToggleStatus));
            lastKey = currentToggleKey;

            Keys currentHealToggleKey = (Keys)Enum.Parse(typeof(Keys), ProfileSingleton.GetCurrent().UserPreferences.toggleStateHealKey);
            KeyboardHook.RemoveUp(healLastKey);
            this.view.ToggleHealKey = currentHealToggleKey.ToString();
            KeyboardHook.AddKeyUp(currentHealToggleKey, new KeyboardHook.KeyPressed(ToggleStatusHeal));
            healLastKey = currentHealToggleKey;
        }

        public bool ToggleStatus()
        {
            return ExecuteToggleStatus(_isStatusOn);
        }

        public bool ToggleStatusHeal()
        {
            return ExecuteToggleHealStatus(_isHealStatusOn);
        }

        public bool ExecuteToggleStatus(bool isCurrentlyOn)
        {
            if (isCurrentlyOn)
            {
                this.view.ToggleButtonBackColor = Color.Red;
                this.view.ToggleButtonText = "OFF";
                this.view.SetIcon(Resources._4RTools.ETCResource.TalesIcon_off);
                this.subject.Notify(new Utils.Message(MessageCode.TURN_OFF, null));
                this.view.StatusText = "Press the key to start!";
                this.view.StatusColor = Color.FromArgb(148, 155, 164);
                this.view.PlaySound(Resources._4RTools.ETCResource.Speech_Off);
                _isStatusOn = false;
            }
            else
            {
                Client client = ClientSingleton.GetClient();
                if (client == null)
                {
                    System.Collections.Generic.List<string> processes = ClientObserver.Instance.GetProcessList();
                    if (processes.Count == 1)
                    {
                        ClientObserver.Instance.SelectProcess(processes[0]);
                        client = ClientSingleton.GetClient();
                    }
                }

                if (client != null)
                {
                    this.view.ToggleButtonBackColor = Color.Green;
                    this.view.ToggleButtonText = "ON";
                    this.view.SetIcon(Resources._4RTools.ETCResource.TalesIcon_on);
                    this.subject.Notify(new Utils.Message(MessageCode.TURN_ON, null));
                    this.view.StatusText = "Press the key to stop!";
                    this.view.StatusColor = Color.FromArgb(148, 155, 164);
                    this.view.PlaySound(Resources._4RTools.ETCResource.Speech_On);
                    _isStatusOn = true;
                }
                else
                {
                    this.view.StatusText = "Please select a valid Ragnarok Client!";
                    this.view.StatusColor = Color.Red;
                }
            }
            return true;
        }

        public bool ExecuteToggleHealStatus(bool isCurrentlyOn)
        {
            if (isCurrentlyOn)
            {
                this.view.ToggleHealButtonBackColor = Color.Red;
                this.view.ToggleHealButtonText = "OFF";
                this.subject.Notify(new Utils.Message(MessageCode.TURN_HEAL_OFF, null));
                this.view.HealStatusText = "Press the key to start healing!";
                this.view.HealStatusColor = Color.FromArgb(148, 155, 164);
                this.view.PlaySound(Resources._4RTools.ETCResource.Healing_Off);
                _isHealStatusOn = false;
            }
            else
            {
                Client client = ClientSingleton.GetClient();
                if (client != null)
                {
                    this.view.ToggleHealButtonBackColor = Color.Green;
                    this.view.ToggleHealButtonText = "ON";
                    this.subject.Notify(new Utils.Message(MessageCode.TURN_HEAL_ON, null));
                    this.view.HealStatusText = "Press the key to stop healing!";
                    this.view.HealStatusColor = Color.FromArgb(148, 155, 164);
                    this.view.PlaySound(Resources._4RTools.ETCResource.Healing_On);
                    _isHealStatusOn = true;
                }
                else
                {
                    this.view.HealStatusText = "Please select a valid Ragnarok Client!";
                    this.view.HealStatusColor = Color.Red;
                }
            }
            return true;
        }
    }
}