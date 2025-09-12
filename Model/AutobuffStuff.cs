using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Utils;
using Newtonsoft.Json;

namespace _4RTools.Model
{
    public class AutoBuffStuff : Action
    {
        public static string ACTION_NAME_AUTOBUFF_STUFF = "AutoBuffStuff";
        public string actionName { get; set; }
        private _4RThread thread;
        public int delay { get; set; } = 100;
        public Dictionary<EffectStatusIDs, Key> buffMapping { get; set; } = new Dictionary<EffectStatusIDs, Key>();

        public AutoBuffStuff(string actionName)
        {
            this.actionName = actionName;
        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                Stop();
                this.thread = AutoBuffThread(roClient);
                _4RThread.Start(this.thread);
            }
        }

        public _4RThread AutoBuffThread(Client c)
        {
            _4RThread autobuffItemThread = new _4RThread(_ =>
            {
                if (KeyboardHookHelper.HandlePriorityKey()) return 0;

                // OTIMIZAÇÃO: Lê todos os buffs da memória uma única vez por ciclo.
                HashSet<EffectStatusIDs> currentBuffs = GetCurrentBuffsAsSet(c);

                bool hasAntiBot = hasBuff(currentBuffs, EffectStatusIDs.ANTI_BOT);
                bool hasOpenChat = c.ReadOpenChat();
                bool stopWithChat = ProfileSingleton.GetCurrent().UserPreferences.stopWithChat;

                bool canAutobuff = !hasAntiBot && !(hasOpenChat && stopWithChat);

                if (canAutobuff)
                {
                    // Itera sobre o mapeamento de buffs para verificar quais precisam ser ativados.
                    foreach (var entry in this.buffMapping)
                    {
                        EffectStatusIDs buffId = entry.Key;
                        Key hotkey = entry.Value;

                        // Se o buff não estiver ativo, simula o pressionamento da tecla.
                        if (!hasBuff(currentBuffs, buffId))
                        {
                            this.useAutobuff(hotkey);
                            Thread.Sleep(this.delay);
                        }
                    }
                }

                Thread.Sleep(500);
                return 0;
            });

            return autobuffItemThread;
        }

        // OTIMIZAÇÃO: Novo método auxiliar que lê todos os buffs e os retorna em um HashSet.
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

        // OTIMIZAÇÃO: O método agora verifica o buff contra o HashSet, uma operação muito mais rápida.
        public bool hasBuff(HashSet<EffectStatusIDs> currentBuffs, EffectStatusIDs buff)
        {
            return currentBuffs.Contains(buff);
        }

        public void AddKeyToBuff(EffectStatusIDs status, Key key)
        {
            if (this.buffMapping.ContainsKey(status))
            {
                this.buffMapping.Remove(status);
            }

            if (FormUtils.IsValidKey(key))
            {
                this.buffMapping.Add(status, key);
            }
        }

        public void ClearKeyMapping()
        {
            this.buffMapping.Clear();
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
