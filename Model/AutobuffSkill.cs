using System;
using System.Threading;
using System.Windows.Input;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using _4RTools.Utils;
using System.Linq;

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
        [JsonIgnore]
        private Dictionary<EffectStatusIDs, Key> buffsToCast = new Dictionary<EffectStatusIDs, Key>();
        [JsonIgnore]
        private HashSet<EffectStatusIDs> currentBuffsSet = new HashSet<EffectStatusIDs>();

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

                // Reuse HashSet instead of creating a new one
                this.currentBuffsSet.Clear();
                for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
                {
                    uint currentStatus = c.CurrentBuffStatusCode(i);
                    if (currentStatus != uint.MaxValue)
                    {
                        this.currentBuffsSet.Add((EffectStatusIDs)currentStatus);
                    }
                }

                string currentMap = c.ReadCurrentMap();
                Profile currentProfile = ProfileSingleton.GetCurrent();
                bool stopHealCity = currentProfile != null && currentProfile.UserPreferences.stopHealCity;
                bool hasOpenChat = c.ReadOpenChat();
                bool stopOpenChat = currentProfile != null && currentProfile.UserPreferences.stopWithChat;

                bool hasAntiBot = this.currentBuffsSet.Contains(EffectStatusIDs.ANTI_BOT);
                bool isInCityList = this.listCities != null && this.listCities.Contains(currentMap);
                bool stopBuffsCity = ProfileSingleton.GetCurrent().UserPreferences.stopBuffsCity;
                bool canAutobuff = !hasAntiBot
                    && !(hasOpenChat && stopOpenChat)
                    && !(stopBuffsCity && isInCityList);

                if (canAutobuff)
                {
                    // Reuse Dictionary instead of creating a new one
                    this.buffsToCast.Clear();
                    if (this.buffMapping != null)
                    {
                        foreach (var item in this.buffMapping)
                        {
                            this.buffsToCast.Add(item.Key, item.Value);
                        }
                    }

                    // Process existing buffs to remove them from the "to-do" list (buffsToCast)
                    foreach (EffectStatusIDs status in this.currentBuffsSet)
                    {
                        if (status == EffectStatusIDs.OVERTHRUSTMAX)
                        {                            this.buffsToCast.Remove(EffectStatusIDs.OVERTHRUST);
                        }

                        // Remove the buff if it's active
                        this.buffsToCast.Remove(status);
                    }

                    if (this.currentBuffsSet.Any())
                    {
                        this.buffsToCast.Remove(EffectStatusIDs.EDEN);
                    }

                    bool foundQuag = this.currentBuffsSet.Contains(EffectStatusIDs.QUAGMIRE);
                    bool foundDecreaseAgi = this.currentBuffsSet.Contains(EffectStatusIDs.DECREASE_AGI);

                    if (!this.currentBuffsSet.Contains(EffectStatusIDs.RIDDING) || (currentProfile != null && !currentProfile.UserPreferences.stopBuffsRein))
                    {
                        foreach (var item in this.buffsToCast)
                        {
                            if (foundQuag && (item.Key == EffectStatusIDs.CONCENTRATION || item.Key == EffectStatusIDs.INC_AGI || item.Key == EffectStatusIDs.TRUESIGHT || item.Key == EffectStatusIDs.ADRENALINE || item.Key == EffectStatusIDs.SPEARQUICKEN || item.Key == EffectStatusIDs.ONEHANDQUICKEN || item.Key == EffectStatusIDs.WINDWALK || item.Key == EffectStatusIDs.TWOHANDQUICKEN))
                            {
                                continue; // Use continue instead of break to check other buffs
                            }

                            if (foundDecreaseAgi && (item.Key == EffectStatusIDs.TWOHANDQUICKEN || item.Key == EffectStatusIDs.ADRENALINE || item.Key == EffectStatusIDs.ADRENALINE2 || item.Key == EffectStatusIDs.ONEHANDQUICKEN || item.Key == EffectStatusIDs.SPEARQUICKEN))
                            {                                continue; // Use continue instead of break
                            }

                            if (c.ReadCurrentHp() >= Constants.MINIMUM_HP_TO_RECOVER)
                            {
                                this.useAutobuff(item.Value);
                                Thread.Sleep(delay);
                            }
                        }
                    }
                }
                Thread.Sleep(500);
                return 0;
            });

            return autobuffItemThread;
        }

        /// <summary>
        /// OPTIMIZED: Reads all buffs from memory once and stores them in a HashSet for fast lookups.
        /// </summary>
        /// <param name="c">The game client</param>
        /// <returns>A HashSet containing all active buff IDs.</returns>
        public HashSet<EffectStatusIDs> GetCurrentBuffsAsSet(Client c)
        {
            var activeBuffs = new HashSet<EffectStatusIDs>();
            for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
            {
                uint currentStatus = c.CurrentBuffStatusCode(i);
                if (currentStatus != uint.MaxValue)
                {
                    activeBuffs.Add((EffectStatusIDs)currentStatus);
                }
            }
            return activeBuffs;
        }

        /// <summary>
        /// OPTIMIZED: Performs a fast check against a pre-fetched set of current buffs.
        /// </summary>
        /// <param name="currentBuffs">A HashSet of currently active buffs.</param>
        /// <param name="buff">The buff to check for.</param>
        /// <returns>True if the buff is active, otherwise false.</returns>
        public bool hasBuff(HashSet<EffectStatusIDs> currentBuffs, EffectStatusIDs buff)
        {
            return currentBuffs.Contains(buff);
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


