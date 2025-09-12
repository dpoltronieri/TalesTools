using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Utils;
using Newtonsoft.Json;

namespace _4RTools.Model
{
    public class DebuffsRecovery : Action
    {
        public static string ACTION_NAME_DEBUFFS_RECOVERY = "DebuffsRecovery";
        public string actionName { get; set; }
        private _4RThread thread;
        public int delay { get; set; } = 100;
        public Dictionary<EffectStatusIDs, Key> debuffMapping { get; set; } = new Dictionary<EffectStatusIDs, Key>();

        public DebuffsRecovery(string actionName)
        {
            this.actionName = actionName;
        }

        public void Start()
        {
            Client roClient = ClientSingleton.GetClient();
            if (roClient != null)
            {
                Stop();
                this.thread = RecoveryThread(roClient);
                _4RThread.Start(this.thread);
            }
        }

        public _4RThread RecoveryThread(Client c)
        {
            _4RThread healingThread = new _4RThread(_ =>
            {
                if (KeyboardHookHelper.HandlePriorityKey()) return 0;

                // OTIMIZAÇÃO: Lê todos os status (incluindo debuffs) da memória uma vez por ciclo.
                HashSet<EffectStatusIDs> currentStatus = GetCurrentBuffsAsSet(c);

                bool hasOpenChat = c.ReadOpenChat();
                bool stopWithChat = ProfileSingleton.GetCurrent().UserPreferences.stopWithChat;

                if (!(hasOpenChat && stopWithChat))
                {
                    // Itera sobre o mapeamento de debuffs para verificar se algum está ativo.
                    foreach (var entry in debuffMapping)
                    {
                        EffectStatusIDs debuffId = entry.Key;
                        Key hotkey = entry.Value;

                        // Se o debuff estiver ativo, simula o pressionamento da tecla para curá-lo.
                        if (hasDebuff(currentStatus, debuffId))
                        {
                            this.useRecovery(hotkey);
                            Thread.Sleep(this.delay);
                        }
                    }
                }

                Thread.Sleep(100);
                return 0;
            });

            return healingThread;
        }

        // OTIMIZAÇÃO: Novo método auxiliar que lê todos os status e os retorna em um HashSet.
        public HashSet<EffectStatusIDs> GetCurrentBuffsAsSet(Client c)
        {
            var activeStatus = new HashSet<EffectStatusIDs>();
            for (int i = 1; i < Constants.MAX_BUFF_LIST_INDEX_SIZE; i++)
            {
                uint currentStatusId = c.CurrentBuffStatusCode(i);
                if (currentStatusId != uint.MaxValue)
                {
                    activeStatus.Add((EffectStatusIDs)currentStatusId);
                }
            }
            return activeStatus;
        }

        // OTIMIZAÇÃO: O método agora verifica o debuff contra o HashSet, uma operação muito mais rápida.
        public bool hasDebuff(HashSet<EffectStatusIDs> currentStatus, EffectStatusIDs debuff)
        {
            return currentStatus.Contains(debuff);
        }

        public void AddKeyToDebuff(EffectStatusIDs status, Key key)
        {
            if (debuffMapping.ContainsKey(status))
            {
                debuffMapping.Remove(status);
            }

            if (FormUtils.IsValidKey(key))
            {
                debuffMapping.Add(status, key);
            }
        }

        public void ClearKeyMapping()
        {
            this.debuffMapping.Clear();
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
            return actionName;
        }

        private void useRecovery(Key key)
        {
            if (key != Key.None)
                Interop.PostMessage(ClientSingleton.GetClient().process.MainWindowHandle, Constants.WM_KEYDOWN_MSG_ID, (Keys)Enum.Parse(typeof(Keys), key.ToString()), 0);
        }
    }
}
