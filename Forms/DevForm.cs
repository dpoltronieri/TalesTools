#if DEBUG
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Input;
using _4RTools.Model;
using _4RTools.Utils;

namespace _4RTools.Forms
{
    public partial class DevForm : Form
    {
        // Define a estrutura para armazenar as informações dos buffs a serem exibidas.
        private static readonly Dictionary<string, EffectStatusIDs> brisaLeveBuffs = new Dictionary<string, EffectStatusIDs>
        {
            { "Fogo", EffectStatusIDs.PROPERTYFIRE },
            { "Água", EffectStatusIDs.PROPERTYWATER },
            { "Vento", EffectStatusIDs.PROPERTYWIND },
            { "Terra", EffectStatusIDs.PROPERTYGROUND },
            { "Sombrio", EffectStatusIDs.PROPERTYDARK },
            { "Fantasma", EffectStatusIDs.PROPERTYTELEKINESIS },
            { "Sagrado", EffectStatusIDs.ASPERSIO }
        };

        public DevForm()
        {
            InitializeComponent();
            InitializeDataGridView();
        }

        private void InitializeDataGridView()
        {
            // Popula o DataGridView com os dados estáticos dos buffs uma única vez.
            foreach (var entry in brisaLeveBuffs)
            {
                string buffName = entry.Key;
                EffectStatusIDs effectId = entry.Value;
                string enumName = effectId.ToString();
                int id = (int)effectId;
                // Adiciona a linha com as informações que não mudam.
                dgvBrisaLeveBuffs.Rows.Add(buffName, enumName, id, "...", "...");
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            Client client = ClientSingleton.GetClient();
            if (client == null)
            {
                // Se o cliente não for encontrado, exibe uma mensagem de status e limpa os dados.
                this.lblBrisaLeve.Text = "Procurando cliente do jogo...";
                ClearGridStatus();
                return;
            }

            Profile currentProfile = ProfileSingleton.GetCurrent();
            if (currentProfile == null || currentProfile.AutobuffSkill == null)
            {
                // Se o perfil ou a configuração de autobuff não estiverem carregados, exibe uma mensagem.
                this.lblBrisaLeve.Text = "Perfil não carregado ou sem configuração de buffs.";
                ClearGridStatus();
                return;
            }

            // Se tudo estiver OK, exibe o status de monitoramento.
            this.lblBrisaLeve.Text = "Monitorando Buffs - Brisa Leve";

            var autoBuffSkill = currentProfile.AutobuffSkill;
            // OTIMIZAÇÃO: Lê todos os buffs da memória uma vez por tick.
            HashSet<EffectStatusIDs> currentBuffs = autoBuffSkill.GetCurrentBuffsAsSet(client);

            // Atualiza o status e a tecla de cada buff na tabela.
            foreach (DataGridViewRow row in dgvBrisaLeveBuffs.Rows)
            {
                if (Enum.TryParse(row.Cells["colEnumName"].Value.ToString(), out EffectStatusIDs effectId))
                {
                    // Atualiza a coluna Status.
                    bool isActive = autoBuffSkill.hasBuff(currentBuffs, effectId);
                    row.Cells["colBuffStatus"].Value = isActive ? "ON" : "OFF";

                    // Atualiza a coluna da tecla relacionada.
                    if (autoBuffSkill.buffMapping.TryGetValue(effectId, out Key mappedKey))
                    {
                        row.Cells["colRelatedKey"].Value = mappedKey.ToString();
                    }
                    else
                    {
                        row.Cells["colRelatedKey"].Value = "N/A";
                    }
                }
            }
        }

        // Limpa os dados dinâmicos da grade para indicar que não há informações.
        private void ClearGridStatus()
        {
            foreach (DataGridViewRow row in dgvBrisaLeveBuffs.Rows)
            {
                row.Cells["colBuffStatus"].Value = "...";
                row.Cells["colRelatedKey"].Value = "...";
            }
        }

        private void DevForm_Load(object sender, EventArgs e)
        {
            // Código a ser executado quando o formulário carregar.
        }
    }
}
#endif

