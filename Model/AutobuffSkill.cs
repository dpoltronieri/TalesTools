﻿using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using _4RTools.Utils;

namespace _4RTools.Model
{

    public class AutoBuffSkill : Action
    {
        public static string ACTION_NAME_AUTOBUFFSKILL = "AutobuffSkill";
        public string actionName { get; set; }
        private _4RThread thread;
        public int delay { get; set; } = 100;
        [JsonIgnore]
        public List<String> listCities { get; set; }

        public Dictionary<EffectStatusIDs, Key> buffMapping = new Dictionary<EffectStatusIDs, Key>();

        public AutoBuffSkill(string actionName)
        {
            this.actionName = actionName;
        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                Stop();
                if (this.listCities == null || this.listCities.Count == 0) this.listCities = LocalServerManager.GetListCities();
                this.thread = AutoBuffThread(roClient);
                _4RThread.Start(this.thread);
            }
        }

        public _4RThread AutoBuffThread(Client c)
        {
            _4RThread autobuffItemThread = new _4RThread(_ =>
            {
                if (KeyboardHookHelper.HandlePriorityKey())
                    return 0;

                bool foundQuag = false;
                bool foundDecreaseAgi = false;
                string currentMap = c.ReadCurrentMap();
                bool hasAntiBot = hasBuff(c, EffectStatusIDs.ANTI_BOT);
                bool stopBuffsCity = ProfileSingleton.GetCurrent().UserPreferences.stopBuffsCity;
                bool isInCityList = this.listCities.Contains(currentMap);
                bool hasOpenChat = c.ReadOpenChat();
                bool stopOpenChat = ProfileSingleton.GetCurrent().UserPreferences.stopWithChat;

                bool canAutobuff = !hasAntiBot
                    && !(hasOpenChat && stopOpenChat)
                    && !(stopBuffsCity && isInCityList);

                if (canAutobuff)
                {
                    List<EffectStatusIDs> buffs = new List<EffectStatusIDs>();
                    Dictionary<EffectStatusIDs, Key> bmClone = new Dictionary<EffectStatusIDs, Key>(this.buffMapping);
                    for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
                    {
                        uint currentStatus = c.CurrentBuffStatusCode(i);

                        if (currentStatus == uint.MaxValue) { continue; }

                        buffs.Add((EffectStatusIDs)currentStatus);
                        EffectStatusIDs status = (EffectStatusIDs)currentStatus;

                        if (status == EffectStatusIDs.OVERTHRUSTMAX)
                        {
                            if (buffMapping.ContainsKey(EffectStatusIDs.OVERTHRUST))
                            {
                                bmClone.Remove(EffectStatusIDs.OVERTHRUST);
                            }
                        }

                        if (buffMapping.ContainsKey(status)) //CHECK IF STATUS EXISTS IN STATUS LIST AND DO ACTION
                        {
                            bmClone.Remove(status);
                        }

                        if (status == EffectStatusIDs.QUAGMIRE) foundQuag = true;
                        if (status == EffectStatusIDs.DECREASE_AGI) foundDecreaseAgi = true;
                    }
                    if (!buffs.Contains(EffectStatusIDs.ANTI_BOT) && (!c.ReadOpenChat() || !ProfileSingleton.GetCurrent().UserPreferences.stopWithChat))
                    {
                        if (!buffs.Contains(EffectStatusIDs.RIDDING) || !ProfileSingleton.GetCurrent().UserPreferences.stopBuffsRein)
                        {
                            foreach (var item in bmClone)
                            {
                                if (foundQuag && (item.Key == EffectStatusIDs.CONCENTRATION || item.Key == EffectStatusIDs.INC_AGI || item.Key == EffectStatusIDs.TRUESIGHT || item.Key == EffectStatusIDs.ADRENALINE || item.Key == EffectStatusIDs.SPEARQUICKEN || item.Key == EffectStatusIDs.ONEHANDQUICKEN || item.Key == EffectStatusIDs.WINDWALK || item.Key == EffectStatusIDs.TWOHANDQUICKEN))
                                {
                                    break;
                                }
                                else if (foundDecreaseAgi && (item.Key == EffectStatusIDs.TWOHANDQUICKEN || item.Key == EffectStatusIDs.ADRENALINE || item.Key == EffectStatusIDs.ADRENALINE2 || item.Key == EffectStatusIDs.ONEHANDQUICKEN || item.Key == EffectStatusIDs.SPEARQUICKEN))
                                {
                                    break;
                                }
                                else if (c.ReadCurrentHp() >= Constants.MINIMUM_HP_TO_RECOVER)
                                {
                                    this.useAutobuff(item.Value);
                                    Thread.Sleep(delay);
                                }
                            }
                        }
                    }
                    buffs.Clear();
                }
                Thread.Sleep(500);
                return 0;
            });

            return autobuffItemThread;
        }
        private bool hasBuff(Client c, EffectStatusIDs buff)
        {
            for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
            {
                uint currentStatus = c.CurrentBuffStatusCode(i);
                if (currentStatus == (int)buff) { return true; }
            }
            return false;
        }
        public void AddKeyToBuff(EffectStatusIDs status, Key key)
        {
            if (buffMapping.ContainsKey(status))
            {
                buffMapping.Remove(status);
            }

            if (FormUtils.IsValidKey(key))
            {
                buffMapping.Add(status, key);
            }
        }

        public void setBuffMapping(Dictionary<EffectStatusIDs, Key> buffs)
        {
            this.buffMapping = new Dictionary<EffectStatusIDs, Key>(buffs);
        }
        public void ClearKeyMapping()
        {
            buffMapping.Clear();
        }

        public void Stop()
        {
            if (this.thread != null)
            {
                _4RThread.Stop(this.thread);
            }
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName()
        {
            return this.actionName;
        }

        private void useAutobuff(Key key)
        {
            if ((key != Key.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
                Interop.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), key.ToString()), 0);
        }
    }
}
