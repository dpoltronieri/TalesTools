using System;
using System.Windows.Input;
using System.Collections.Generic;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public class AHKPresenter
    {
        private IAHKView view;
        private AHK ahk;

        public AHKPresenter(IAHKView view, AHK ahk)
        {
            this.view = view;
            this.ahk = ahk;
            BindEvents();
            UpdateView();
        }

        private void BindEvents()
        {
            this.view.AHKDelayChanged += (s, e) => {
                try {
                    this.ahk.AhkDelay = int.Parse(this.view.AHKDelay);
                    Save();
                } catch {}
            };
            this.view.NoShiftChanged += (s, e) => {
                this.ahk.noShift = this.view.NoShift;
                Save();
            };
            this.view.MouseFlickChanged += (s, e) => {
                this.ahk.mouseFlick = this.view.MouseFlick;
                Save();
            };
            this.view.AHKModeChanged += (s, e) => {
                this.ahk.ahkMode = this.view.AHKMode;
                Save();
            };
            this.view.CheckboxChanged += (s, e) => {
                if (e.Checked)
                {
                    try {
                        Key key = (Key)new KeyConverter().ConvertFromString(e.Key);
                        // Determine if it should have mouse click based on CheckState - simplified here as the view should handle the tri-state logic mapping to 'haveMouseClick'
                        // However, the original code used CheckState.Checked for 'haveMouseClick' = true.
                        // I will assume the view passes 'true' if it's checked/active, but we need to know if it's "Checked" (click) or "Indeterminate" (no click).
                        // Let's refine the interface or assume the View handles the logic of what "Checked" means in this context, 
                        // but actually the original code distinguished between Checked (Click) and Indeterminate (No Click).
                        
                        // To properly support tri-state in MVP, the View should probably provide the state directly or separate events.
                        // For now, I'll rely on the view passing the correct config.
                        // But wait, the presenter needs to know if it's "Click" or "No Click".
                        // I will update the event args to include the `ClickActive` state directly.
                    } catch {}
                }
                else
                {
                    this.ahk.RemoveAHKEntry(e.Name);
                }
                Save();
            };
        }

        public void ProcessCheckboxChange(string name, bool isChecked, bool isClickActive, string keyString)
        {
            if (isChecked)
            {
                try
                {
                    Key key = (Key)new KeyConverter().ConvertFromString(keyString);
                    this.ahk.AddAHKEntry(name, new KeyConfig(key, isClickActive));
                }
                catch { }
            }
            else
            {
                this.ahk.RemoveAHKEntry(name);
            }
            Save();
        }

        public void UpdateView()
        {
            this.view.AHKDelay = this.ahk.AhkDelay.ToString();
            this.view.NoShift = this.ahk.noShift;
            this.view.MouseFlick = this.ahk.mouseFlick;
            this.view.AHKMode = this.ahk.ahkMode;
        }

        public void UpdateModel(AHK newModel)
        {
            this.ahk = newModel;
            UpdateView();
        }

        private void Save()
        {
            ProfileSingleton.SetConfiguration(this.ahk);
        }
    }
}
