using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using _4RTools.Utils;
using System.ComponentModel;
using System.Reflection;

namespace _4RTools.Model
{
    public static class BuffData
    {
        private static readonly string FilePath = "buff_names.json";
        private static Dictionary<int, string> _customNames = new Dictionary<int, string>();

        static BuffData()
        {
            Load();
        }

        public static void Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    string json = File.ReadAllText(FilePath);
                    _customNames = JsonConvert.DeserializeObject<Dictionary<int, string>>(json) ?? new Dictionary<int, string>();
                }
            }
            catch { }
        }

        public static void Save()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_customNames, Formatting.Indented);
                File.WriteAllText(FilePath, json);
            }
            catch { }
        }

        public static void AddOrUpdateName(int id, string name)
        {
            _customNames[id] = name;
            Save();
        }

        public static string GetBuffName(int id)
        {
            // 1. Check Custom Names
            if (_customNames.TryGetValue(id, out string customName))
            {
                return customName;
            }

            // 2. Check Enum
            if (Enum.IsDefined(typeof(EffectStatusIDs), (uint)id))
            {
                EffectStatusIDs status = (EffectStatusIDs)id;
                return GetEnumDescription(status);
            }

            return "Unknown";
        }

        private static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }
    }
}
