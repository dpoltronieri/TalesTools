using System;
using System.ComponentModel;
using _4RTools.Utils;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace _4RTools.Model
{

    public class AutoSwitchHeal : Action
    {

        public static string ACTION_NAME_AUTOSWITCHHEAL = "AutoSwitchHeal";
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
        private _4RThread threadEquips;
        private _4RThread threadPet;

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
                if (this.threadEquips != null)
                {
                    _4RThread.Stop(this.threadEquips);
                }
                if (this.threadPet != null)
                {
                    _4RThread.Stop(this.threadPet);
                }
                if (this.listCities == null || this.listCities.Count == 0) this.listCities = GlobalVariablesHelper.CityList;
                this.threadEquips = new _4RThread(_ => AutoSwitchHealThreadExecution(roClient));
                _4RThread.Start(this.threadEquips);
                this.threadPet = new _4RThread(_ => AutoSwitchHealPetThreadExecution(roClient));
                _4RThread.Start(this.threadPet);
            }
        }
        private int AutoSwitchHealPetThreadExecution(Client roClient)
        {
            if (KeyboardHookHelper.HandlePriorityKey())
                return 0;
            if (this.itemKey == Key.None)
                return 0;

            string currentMap = roClient.ReadCurrentMap();
            bool hasAntiBot = hasBuff(roClient, EffectStatusIDs.ANTI_BOT);
            bool stopSpammersBot = ProfileSingleton.GetCurrent().UserPreferences.stopSpammersBot;
            bool stopHealCity = ProfileSingleton.GetCurrent().UserPreferences.stopHealCity;
            bool isInCityList = this.listCities.Contains(currentMap);

            bool canEquipPet = !(hasAntiBot && stopSpammersBot)
                && !(stopHealCity && isInCityList);

            if (canEquipPet)
            {
                changePet(roClient);
            }
            Thread.Sleep(this.switchDelay);
            return 0;
        }

        private void changePet(Client roClient)
        {
            if (roClient.IsSpBelow(spPercent) && roClient.IsHpAbove(10))
            {
                switchPet();
            }
        }
        private void switchPet()
        {
            pressKey(itemKey);
            Thread.Sleep(1000);
            foreach(var i in Enumerable.Range(0, this.qtdSkill))
            {
                pressKey(skillKey);
                Thread.Sleep(this.switchDelay);
            }
            pressKey(nextItemKey);
        }

        private int AutoSwitchHealThreadExecution(Client roClient)
        {
            if (KeyboardHookHelper.HandlePriorityKey())
                return 0;

            string currentMap = roClient.ReadCurrentMap();
            bool hasAntiBot = hasBuff(roClient, EffectStatusIDs.ANTI_BOT);
            bool stopSpammersBot = ProfileSingleton.GetCurrent().UserPreferences.stopSpammersBot;
            bool stopHealCity = ProfileSingleton.GetCurrent().UserPreferences.stopHealCity;
            bool isInCityList = this.listCities.Contains(currentMap);

            bool canEquip = !(hasAntiBot && stopSpammersBot)
                && !(stopHealCity && isInCityList);

            if (canEquip)
            {
                changeEquip(roClient);
            }

            Thread.Sleep(this.equipDelay * 1000);
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

        private void changeEquip(Client roClient)
        {
            if (roClient.IsHpBelow(lessHpPercent))
            {
                pressKey(this.lessHpKey);
            }
            else if (roClient.IsHpAbove(moreHpPercent))
            {
                pressKey(this.moreHpKey);
            }

            if (roClient.IsSpBelow(lessSpPercent))
            {
                pressKey(this.lessSpKey);
            }
            else if (roClient.IsSpAbove(moreSpPercent))
            {
                pressKey(this.moreSpKey);
            }
        }

        private void pressKey(Key key)
        {
            if ((key != Key.None) && !Keyboard.IsKeyDown(Key.LeftAlt) && !Keyboard.IsKeyDown(Key.RightAlt))
            {
                Interop.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), key.ToString()), 0);
            }
        }

        public void Stop()
        {
            _4RThread.Stop(this.threadEquips);
            _4RThread.Stop(this.threadPet);
        }

        public string GetConfiguration()
        {
            return JsonConvert.SerializeObject(this);
        }

        public string GetActionName()
        {
            return this.actionName != null ? this.actionName : ACTION_NAME_AUTOSWITCHHEAL;
        }

    }
}
