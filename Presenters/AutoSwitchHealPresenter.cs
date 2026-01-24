using System;
using System.Reflection;
using System.Windows.Input;
using _4RTools.Model;

namespace _4RTools.Presenters
{
    public class AutoSwitchHealPresenter
    {
        private IAutoSwitchHealView view;
        private AutoSwitchHeal model;

        public AutoSwitchHealPresenter(IAutoSwitchHealView view, AutoSwitchHeal model)
        {
            this.view = view;
            this.model = model;
            BindEvents();
            UpdateView();
        }

        private void BindEvents()
        {
            this.view.PropertyChanged += (s, e) => {
                try {
                    PropertyInfo property = typeof(AutoSwitchHeal).GetProperty(e.PropertyName);
                    if (property != null)
                    {
                        object value = null;
                        if (property.PropertyType == typeof(Key))
                        {
                            value = (Key)Enum.Parse(typeof(Key), e.Value);
                        }
                        else if (property.PropertyType == typeof(int))
                        {
                            value = int.Parse(e.Value);
                        }

                        if (value != null)
                        {
                            property.SetValue(this.model, value);
                            Save();
                        }
                    }
                } catch {}
            };
        }

        public void UpdateView()
        {
            PropertyInfo[] properties = typeof(AutoSwitchHeal).GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == typeof(Key) || property.PropertyType == typeof(int))
                {
                    object val = property.GetValue(this.model);
                    if (val != null)
                    {
                        this.view.SetControlValue(property.Name, val.ToString());
                    }
                }
            }
        }

        public void UpdateModel(AutoSwitchHeal newModel)
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
