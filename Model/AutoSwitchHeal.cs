using System;
using System.ComponentModel;
using _4RTools.Utils;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace _4RTools.Model
{

    public class AutoSwitchHeal : Action
    {

        public static string ACTION_NAME_AUTOPOT = "AutoSwitchHeal";
        public Key lessHpKey { get; set; }
        public Key moreHpKey { get; set; }
        public int lessHpPercent { get; set; }
        public int moreHpPercent { get; set; }

        public Key lessSpKey { get; set; }
        public Key moreSpKey { get; set; }
        public int lessSpPercent { get; set; }
        public int moreSpPercent { get; set; }
        public int equipDelay { get; set; } = 3;


        public Key itemKey { get; set; }
        public Key skillKey { get; set; }
        public Key nextItemKey { get; set; }
        public int spPercent { get; set; }
        public int qtdSkill { get; set; }
        public int switchDelay { get; set; } = 500;

        public int hpPercent { get; set; }
        public Key hpKey { get; set; }
        public Key spKey { get; set; }

        public int delayYgg { get; set; } = 500;

        public string actionName { get; set; }
        private _4RThread thread;

        public List<String> listCities { get; set; } = GlobalVariablesHelper.CityList;

        public AutoSwitchHeal() { }
        public AutoSwitchHeal(string actionName)
        {
            this.actionName = actionName;
        }

        public AutoSwitchHeal(Key hpKey, int hpPercent, int delay, Key spKey, int spPercent, Key tiKey)
        {

            // HP
            this.hpKey = hpKey;
            this.hpPercent = hpPercent;

            // SP
            this.spKey = spKey;
            this.spPercent = spPercent;

        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                if (this.thread != null)
                {
                    _4RThread.Stop(this.thread);
                }
                int hpPotCount = 0;
                if (this.listCities == null || this.listCities.Count == 0) this.listCities = GlobalVariablesHelper.CityList;
                this.thread = new _4RThread(_ => AutoSwitchHealThreadExecution(roClient, hpPotCount));
                _4RThread.Start(this.thread);
            }
        }


        private int AutoSwitchHealThreadExecution(Client roClient, int hpPotCount)
        {
            if (KeyboardHookHelper.HandlePriorityKey())
                return 0;

            string currentMap = roClient.ReadCurrentMap();
            bool hasAntiBot = hasBuff(roClient, EffectStatusIDs.ANTI_BOT);
            bool stopSpammersBot = ProfileSingleton.GetCurrent().UserPreferences.stopSpammersBot;
            bool hasBerserk = hasBuff(roClient, EffectStatusIDs.BERSERK);
            bool isCompetitive = hasBuff(roClient, EffectStatusIDs.COMPETITIVA);
            bool stopHealCity = ProfileSingleton.GetCurrent().UserPreferences.stopHealCity;
            bool isInCityList = this.listCities.Contains(currentMap);

            bool canHeal = !(hasAntiBot && stopSpammersBot)
                && !hasBerserk
                && !(stopHealCity && isInCityList);

            if (canHeal)
            {
                bool hasCriticalWound = hasBuff(roClient, EffectStatusIDs.CRITICALWOUND);

                    healHPFirst(roClient, hpPotCount, hasCriticalWound);

                    healSPFirst(roClient, hpPotCount, hasCriticalWound);
            }

            
            Thread.Sleep(this.delayYgg);
            return 0;
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

        private void healSPFirst(Client roClient, int hpPotCount, bool hasCriticalWound)
        {
            if (roClient.IsSpBelow(spPercent))
            {
                pot(this.spKey);
                hpPotCount++;

                if (hpPotCount == 3 && roClient.IsHpBelow(hpPercent))
                {
                    hpPotCount = 0;

                        pot(this.hpKey);


                }
            }
            // check hp
            if (roClient.IsHpBelow(hpPercent))
            {
                pot(this.hpKey);
            }
        }

        private void healHPFirst(Client roClient, int hpPotCount, bool hasCriticalWound)
        {
            if (roClient.IsHpBelow(hpPercent))
            {
                    pot(this.hpKey);
                    hpPotCount++;
            }
            // check sp
            if (roClient.IsSpBelow(spPercent))
            {
                pot(this.spKey);
            }
        }

        private void pot(Key key)
        {
            Keys k = (Keys)Enum.Parse(typeof(Keys), key.ToString());
            if ((k != Keys.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
            {
                Interop.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, k, 0); // keydown
                Interop.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, Constants.WM_KEYUP_MSG_ID, k, 0); // keyup
            }
        }

        public void Stop()
        {
            _4RThread.Stop(this.thread);
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName()
        {
            return this.actionName != null ? this.actionName : ACTION_NAME_AUTOPOT;
        }

    }
}
