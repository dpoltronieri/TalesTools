# Tales Tools - Ragnatales

Este projeto é uma adaptação do projeto 4RTools para uso pessoal e como objeto de estudo, onde acrescento funcionalidades na ferramenta para atender às minhas necessidades jogando no servidor Ragnatales.

**ESTA VERSÃO É UMA VERSÃO ESTENDIDA DA ORIGINAL (biancaazuma), CONTENDO TODAS AS FUNCIONALIDADES ORIGINAIS E ALGUMAS CUSTOMIZADAS.**

## Rodando o projeto

Caso você queira usar esta versão, você pode baixar a release mais recente na raiz do projeto, ou usar o Visual Studio 2022 para abrir o `TalesTools.sln` e gerar suas próprias releases.

## ⚠️ Atenção: Perfis (Profiles)

**A pasta `Profile` desta versão NÃO é compatível com a versão da biancaazuma ou versões anteriores.**
É necessário recriar seus perfis do zero nesta nova versão para garantir o funcionamento correto de todas as novas funcionalidades.

---

# Documentação de Funcionalidades do TalesTools

Este documento fornece documentação detalhada para as principais abas funcionais e sistemas principais do aplicativo TalesTools.

## 1. Autopot

**Descrição:**
O recurso Autopot usa automaticamente itens de cura (Poções) para manter os níveis de HP e SP do personagem. Foi projetado para ser a principal ferramenta de sobrevivência.

**Parâmetros Chave:**
- **HP Key**: A tecla atribuída à poção de HP.
- **HP %**: A porcentagem abaixo da qual a poção de HP será usada.
- **SP Key**: A tecla atribuída à poção de SP.
- **SP %**: A porcentagem abaixo da qual a poção de SP será usada.
- **Delay (ms)**: O intervalo de tempo (em milissegundos) entre as verificações/uso de poção (Padrão: 15ms).
- **First Heal (Primeira Cura)**: Determina a prioridade entre cura de HP e SP (Opções: "HP" ou "SP").
- **Stop when Critical Wound (Brisa Leve)**: Se ativado, pausa o Autopot quando o status "Ferimento Crítico" (Critical Wound) está ativo, evitando desperdício de poções.
- **Equip Before/After (Equipar Antes/Depois)**: Permite configurar uma tecla para equipar um item específico (ex: item de bônus de cura) antes do loop de cura começar e outra tecla para equipar um item após o término do loop.

**Lógica:**
1.  **Verificações de Segurança**: Verifica se o personagem está em uma cidade (se "Stop in City" estiver ativado), tem status "Anti-Bot", "Berserk" (Frenesi) ou está em modo "Competitivo". Se algum for verdadeiro, ele para.
2.  **Loop de Prioridade**:
    -   Se **HP Priority**: Verifica HP primeiro. Se HP < HP%, entra em um loop pressionando a tecla de HP até HP >= HP%.
        -   *Verificação Inteligente de SP*: Dentro do loop de HP, a cada 4 poções de HP, ele verifica se SP < SP%. Se sim, usa uma poção de SP para evitar que o SP zere durante dano intenso.
    -   Se **SP Priority**: Verifica SP primeiro. A lógica é semelhante, mas prioriza a recuperação de SP.
3.  **Troca de Equipamento**: Se configurado, aciona a tecla "Equip Before" antes de entrar no loop de cura e a tecla "Equip After" quando o HP alvo é atingido.

## 2. Yggdrasil

**Descrição:**
Um modo especializado do Autopot projetado para itens de recuperação de alto impacto, como Frutos ou Sementes de Yggdrasil. Compartilha o motor central com o Autopot, mas simplifica a interface e a lógica para cura explosiva (burst healing).

**Parâmetros Chave:**
- **HP Key**: Tecla para o item Yggdrasil.
- **HP %**: Porcentagem limite para usar o item.
- **SP Key**: Tecla para o item de recuperação de SP (frequentemente o mesmo item Yggdrasil em algumas configurações, ou um item de SP separado).
- **SP %**: Porcentagem limite para recuperação de SP.
- **Delay**: Atraso de execução (Padrão: 50ms - tipicamente mais lento que poções padrão para evitar desperdício/uso excessivo).

**Diferenças do Autopot:**
-   **UI Simplificada**: Não mostra as opções "Stop when Critical Wound" ou "Equip Before/After", pois a cura de Yggdrasil é tipicamente instantânea e total, ignorando algumas verificações de status padrão ou necessidades de troca de equipamento.
-   **Lógica**: Segue estritamente os limites de HP/SP para acionar as teclas sem a lógica complexa de "Troca/Equipar".

## 3. Skill Timer (Temporizador de Habilidades)

**Descrição:**
Um utilitário para conjurar habilidades ou usar itens automaticamente em intervalos de tempo fixos. Útil para manter buffs que não possuem um ícone de status ou para ações repetitivas.

**Parâmetros Chave:**
- **Lane 1-4**: A ferramenta fornece 4 faixas de temporizador independentes.
- **Key**: A tecla a ser pressionada.
- **Delay (s)**: O intervalo de tempo em segundos entre cada pressionamento.

**Lógica:**
-   Cada faixa opera de forma independente.
-   Uma vez ativado, o sistema espera pelo **Delay** especificado e então simula o pressionamento da **Key**.
-   Repete indefinidamente enquanto o recurso estiver ativo.
-   Comumente usado para habilidades como "Impacto Explosivo" (pelo bônus), comidas, ou outros consumíveis temporizados.

## 4. AutoSwitch Heal (Troca Automática de Cura)

**Descrição:**
Um trocador de equipamentos inteligente que reage aos níveis de HP e SP do personagem. Permite trocar automaticamente entre equipamento "Tank/Defensivo" e equipamento de "Dano/Ofensivo" com base na vida/mana atual.

**Parâmetros Chave:**
-   **HP Switching**:
    -   **Less HP % / Key**: Quando HP está *abaixo* desta porcentagem, pressiona esta tecla (ex: equipar Escudo/Carta GR).
    -   **More HP % / Key**: Quando HP está *acima* desta porcentagem, pressiona esta tecla (ex: equipar Espada de Duas Mãos/Carta de Dano).
-   **SP Switching**:
    -   **Less SP % / Key**: Quando SP está *abaixo* desta porcentagem, pressiona esta tecla (ex: equipar item de regen de MP).
    -   **More SP % / Key**: Quando SP está *acima* desta porcentagem, pressiona esta tecla (ex: equipar equipamento padrão).
-   **Equip Delay**: O tempo mínimo entre trocas de equipamento (Padrão: 3 segundos) para evitar spam/glitches.

**Lógica:**
1.  **Monitoramento**: Monitora constantemente o HP e SP atuais.
2.  **Verificação de Condição**:
    -   Se `HP < Less HP %`: Aciona "Less HP Key".
    -   Senão Se `HP > More HP %`: Aciona "More HP Key".
    -   (Lógica similar para SP).
3.  **Segurança**: Inclui verificações padrão (Anti-Bot, Status de Cidade) para evitar trocas em momentos inapropriados.

---

## 5. Sistemas Principais e Ferramentas

### Ragnarok Client
**Descrição:**
Gerencia a conexão entre a ferramenta e o cliente do jogo.
-   **Gerenciamento de Servidor**: Permite aos usuários configurar endereços de memória (`hpAddress`, `nameAddress`, `mapAddress`) para diferentes versões de servidor. As configurações são salvas em `supported_servers.json`.
-   **Anexar Processo**: O usuário seleciona o processo específico do cliente Ragnarok para anexar a ferramenta.
-   **Segurança em Cidades**: Mantém uma lista de "Mapas de Cidade" (`city_name.json`) onde a automação ofensiva (como Autopot) é desativada automaticamente para evitar comportamento suspeito em zonas seguras.

### Sistema de Perfis (Profile System)
**Descrição:**
Um sistema abrangente de gerenciamento de configurações.
-   **Perfis**: Permite salvar configurações distintas (ajustes de Autopot, Macros, Teclas) em arquivos JSON nomeados.
-   **Gerenciamento**: Usuários podem Criar, Carregar e Deletar perfis.
-   **Vínculo com Personagem**: Lembra automaticamente qual perfil foi usado por último para um nome de personagem específico, facilitando a troca contínua entre personagens.

### Status Atual (Dashboard)
**Descrição:**
O hub de controle principal para o estado da aplicação.
-   **Alternar Global (ON/OFF)**: Um interruptor mestre que ativa ou desativa *todas* as funcionalidades de automação.
    -   **Atalho**: Atalho de teclado configurável para alternar este estado.
    -   **Visual**: A interface fica Verde (ON) ou Vermelha (OFF), e o Ícone na Bandeja muda para refletir o status.
    -   **Áudio**: Toca efeitos sonoros distintos ("Speech On" / "Speech Off") para feedback auditivo.
-   **Status de Cura**: Um sub-interruptor dedicado especificamente para funções de cura (Autopot/Yggdrasil). Isso permite pausar a cura (ex: durante uma mecânica específica) sem parar outros macros ou ferramentas.

### Transfer Item (Transferir Item)
**Descrição:**
Um utilitário para gerenciamento de inventário.
-   **Função**: Transfere rapidamente itens entre o Inventário e o Armazém/Carrinho.
-   **Uso**: Enquanto a **Transfer Key** configurada estiver pressionada, a ferramenta simula `Alt + Botão Direito` repetidamente.
-   **Lógica**: Usa inputs de baixo nível para realizar o atalho específico do Ragnarok para mover itens, acelerando significativamente as corridas de reabastecimento.

### Priority Key (Tecla de Prioridade)
**Descrição:**
Um recurso de segurança de substituição manual.
-   **Função**: Quando a **Priority Key** é mantida pressionada, a ferramenta **pausa todas as threads de automação**.
-   **Caso de Uso**: Permite que o usuário assuma o controle manual total do personagem (ex: para conjurar uma habilidade específica, mover-se com precisão ou digitar) sem que os inputs do bot interfiram ou interrompam.
-   **Configuração**:
    -   **Key**: A tecla que aciona a pausa.
    -   **Delay**: A responsividade da verificação (tipicamente baixa para garantir pausa imediata).

---

## 6. Abas de Configuração e Gerenciamento

### Aba Config (Configurações)
**Descrição:**
Permite personalização global do comportamento e interface da aplicação.

**Recursos Chave:**
-   **Visibilidade de Abas**: Alterna a visibilidade das principais abas de recursos (ex: esconder "Macro Songs" se não for necessário) para limpar a interface.
-   **Filtros de Autobuff**: Mostra/esconde granularmente grupos específicos de habilidades (Espadachim, Mago, etc.) ou grupos de itens (Comidas, Poções) nas abas de Autobuff.
-   **Preferências de Segurança e Automação**:
    -   **Stop Buffs/Heal in City**: Previne gastar consumíveis em zonas seguras.
    -   **Stop with Chat Open**: Pausa a automação quando a barra de chat do jogo está ativa (para evitar digitar macros no chat).
    -   **Stop Buffs on Rein (Montaria)**: Previne tentativas de conjuração enquanto montado.
    -   **Get Off Rein**: Opção para desmontar automaticamente (usando `Get Off Rein Key`) se necessário.
-   **Ammo Switch**: Configura teclas (`Ammo 1`, `Ammo 2`) para alternar entre tipos de munição.
-   **Ordem de Buffs**: Interface de arrastar e soltar para priorizar a ordem em que os buffs são verificados/aplicados.

### Aba Profiles (Perfis)
**Descrição:**
A interface para gerenciar perfis de usuário.

**Recursos Chave:**
-   **Criar Perfil**: Adiciona uma nova configuração de perfil nomeada.
-   **Remover Perfil**: Deleta um perfil existente (o perfil Default não pode ser deletado).
-   **Atribuir ao Personagem**: Vincula o perfil selecionado atualmente ao personagem ativo no cliente do jogo. A ferramenta carregará automaticamente este perfil quando esse personagem for detectado.

### Aba Dev (Desenvolvedor)
**Descrição:**
Uma ferramenta de diagnóstico e monitoramento para desenvolvedores e usuários avançados.

**Recursos Chave:**
-   **Monitor de Buffs (Ativo)**:
    -   Exibe em tempo real uma grade com todos os buffs ativos detectados na memória.
    -   Permite identificar IDs de buffs desconhecidos.
    -   Permite **Nomear e Salvar** buffs desconhecidos para facilitar a identificação futura.
    -   Botão **Clear & Reload** para limpar a lista e forçar uma nova leitura.

---

## 7. Abas de Funcionalidades Principais

### Skill Spammer
**Descrição:**
Um spammer de teclas de uso geral (AutoKey).

**Modos:**
-   **Compatibility (Compatibilidade)**: 
    -   Usa mensagens padrão do Windows (`PostMessage`) para simular input. 
    -   Compatível com a maioria das versões do jogo.
    -   Suporta recursos **No Shift** e **Mouse Flick**.
-   **Synchronous (Síncrono)**:
    -   Usa chamadas bloqueantes (`SendMessage`) para garantir que o input seja processado antes de continuar.
    -   Envia explicitamente eventos de Tecla Baixo e Tecla Cima, o que pode ser mais confiável para certas habilidades.
    -   Suporta **No Shift** e **Mouse Flick**.
-   **Speed Boost**:
    -   Usa APIs de simulação de hardware de baixo nível (`mouse_event`) para cliques do mouse.
    -   **Recursos Desativados**: *Não* suporta **No Shift** ou **Mouse Flick** (essas opções são desativadas na interface quando Speed Boost está ativo).
    -   **Caso de Uso**: Para cenários que exigem a taxa de input mais rápida possível, ignorando algumas camadas de segurança/compatibilidade.

**Recursos Adicionais:**
-   **No Shift**: Injeta um pressionamento da tecla Shift antes da ação. Isso efetivamente age como um "Ataque Parado" (prevenindo que o personagem ande em direção ao alvo se clicado).
-   **Mouse Flick**: Micro-movimentos do cursor do mouse (-1/+1 pixel) durante cliques. Isso pode ajudar a registrar cliques em jogos que ignoram input estático ou para atualizar o alvo do cursor.
-   **Ammo Switch**: Se ativado na Config, alterna automaticamente entre as teclas `Ammo 1` e `Ammo 2` a cada ciclo de spam.
-   **Get Off Rein**: Desmonta automaticamente o personagem (se montado) antes de atacar, garantindo que a ação de ataque não seja bloqueada pelo status de montaria.

### Autobuff - Skills (Habilidades)
**Descrição:**
Mantém automaticamente buffs próprios derivados de habilidades de classe.

**Recursos Chave:**
-   **Grupos de Classe**: Buffs são organizados por classe (Espadachim, Arqueiro, Mago, etc.) para fácil navegação.
-   **Lógica**: A ferramenta monitora o status do personagem. Se um buff configurado estiver faltando, pressiona a tecla atribuída para reconjurá-lo.
-   **Configuração**:
    -   **Key**: Atribua a tecla para a habilidade específica.
    -   **Delay**: Intervalo de tempo entre verificações de status.

### Autobuff - Stuffs (Consumíveis)
**Descrição:**
Usa automaticamente itens consumíveis para manter buffs baseados em itens.

**Recursos Chave:**
-   **Grupos de Itens**: Organizados por tipo (Poções, Comidas, Elementais, Caixas, Pergaminhos, Etc.).
-   **Lógica**: Semelhante ao Autobuff de Habilidades, verifica a presença do efeito do item (ex: status "Poção do Despertar") e consome o item se o efeito estiver faltando.
-   **Configuração**:
    -   **Key**: Atribua a tecla para o item.
    -   **Delay**: Intervalo de tempo entre verificações de status.

### Auto Switch
**Descrição:**
Um trocador de equipamentos "Baseado em Estado".

**Recursos Chave:**
-   **Gatilho**: Detecta quando um buff próprio específico (ex: "Espírito", "Rapidez") está ativo.
-   **Ação**:
    -   **Item Key**: Equipa um item (ex: uma arma que se beneficia do buff).
    -   **Skill Key**: Opcionalmente conjura uma habilidade após equipar.
    -   **Next Item Key**: Equipa outro item (ex: trocando de volta) após a ação.
-   **Caso de Uso**: Sequências de troca de equipamento especializadas acionadas pelo estado do seu personagem.

### ATK x DEF
**Descrição:**
Um sistema de troca de modo para posturas "Ofensiva" vs "Defensiva".

**Recursos Chave:**
-   **Lanes**: Suporta até 8 configurações independentes.
-   **Configuração por Lane**:
    -   **Spammer Key**: A tecla usada para atacar (ex: uma tecla de spam de habilidade).
    -   **ATK Set**: Uma lista de teclas (Equipamentos) para equipar ao entrar no modo de Ataque.
    -   **DEF Set**: Uma lista de teclas (Equipamentos) para equipar ao entrar no modo de Defesa.
    -   **Switch Delay**: Atraso entre trocas de equipamento.
-   **Lógica**: Projetado para permitir transição rápida entre equipamento de dano total (enquanto ataca) e equipamento tanque total (ao parar ou defender).

### Macro Songs (Macro de Músicas)
**Descrição:**
Um sistema de macro dedicado para Bardos e Odaliscas gerenciarem o ciclo de músicas e troca de instrumentos.

**Recursos Chave:**
-   **Lanes**: 8 faixas de macro.
-   **Sequência**:
    1.  **Trigger**: Tecla para iniciar o macro.
    2.  **Equip Instrument**: Troca para o instrumento musical.
    3.  **Macro Entries**: Sequência de habilidades de música para tocar.
    4.  **Equip Dagger**: Troca para uma adaga (tática padrão de Ragnarok para cancelar a animação/delay da música).
-   **Delay**: Atraso configurável entre ações na sequência.

### Macro Switch (Macro de Troca)
**Descrição:**
Um sequenciador de macro genérico e multiuso.

**Recursos Chave:**
-   **Lanes**: 10 faixas de macro.
-   **Sequência**: Permite definir uma cadeia de teclas a serem pressionadas em ordem.
-   **Configuração por Passo**:
    -   **Key**: A tecla a pressionar.
    -   **Delay**: O tempo de espera após este passo.
    -   **Click**: Se deve clicar com o mouse após este passo.
-   **Trigger**: A primeira tecla na sequência age como o gatilho.

### Debuffs (Auto Buff Status)
**Descrição:**
Cura automaticamente efeitos de status negativos (Debuffs).

**Recursos Chave:**
-   **Debuffs Padrão**: Monitora e cura males comuns como Silêncio, Cegueira, Confusão, Veneno, Alucinação e Maldição.
    -   **Status Key**: Uma atribuição de tecla única que pode ser usada para todos esses debuffs padrão (ideal para "Poção Verde" ou "Panaceia").
-   **Debuffs Especiais/3rd Job**: Monitora debuffs específicos de alto nível como Ferimento Crítico, Sono Profundo, Incêndio, Congelamento, etc.
    -   **New Status Key**: Uma atribuição de tecla dedicada para curar essas condições específicas.

---

# Backlog do Projeto TalesTools

Este documento descreve o roteiro (roadmap), recursos planejados, melhorias arquiteturais e otimizações técnicas para o projeto TalesTools.

## 1. Módulos Planejados

### 1.1. Smart Buffs (Ferramenta de Verificação)
- **Objetivo**: Garantir que mecânicas complexas de buff (especificamente `brisaLeve` - Mudança de Elemento) sejam aplicadas corretamente e sincronizadas com o estado da ferramenta.
- **Descrição**: Uma ferramenta que verifica ativamente o estado da memória contra o estado esperado para buffs "Brisa Leve". Como esses buffs geralmente não têm ícones padrão ou se comportam de maneira diferente, essa ferramenta fornecerá uma verificação de "verdade" para evitar que o bot pense que tem um buff quando não tem (ou vice-versa).
- **Status**: Parcialmente desenvolvido (veja `DevForm`).

### 1.2. Aba de Desenvolvimento
- **Objetivo**: Auxiliar desenvolvedores e usuários avançados na engenharia reversa e configuração.
- **Descrição**: Um painel informativo que exibe:
    -   Lista em tempo real de TODOS os buffs ativos do personagem encontrados na memória.
    -   Seus IDs numéricos internos (`EffectStatusIDs`).
    -   Seus nomes conhecidos (ou "Unknown" para novos IDs).
-   **Caso de Uso**: Essencial para identificar novos buffs adicionados ao servidor do jogo, permitindo que os usuários atualizem o `EffectStatusIDs.cs` sem recompilar a lógica principal às cegas.

### 1.3. Tormenta Buffs (Buff Manual em Burst)
- **Objetivo**: Fornecer uma sequência de buff semiautomática controlada pelo usuário.
- **Descrição**: Um gerenciador especializado para buffs que não devem estar sempre ativos (diferente do Autobuff padrão).
    -   **Gatilho**: Escuta uma tecla de atalho específica.
    -   **Ação**: Quando pressionado, verifica uma lista de buffs configurados.
    -   **Modos**:
        -   *Smart*: Conjura apenas buffs ausentes.
        -   *Force*: Reconjura TODOS os buffs da lista (útil para atualizar temporizadores).

### 1.4. Avisos de Gameplay (Companheiro do Jogador)
- **Objetivo**: Prevenir falhas devido à falta de recursos ou queda silenciosa de buffs.
- **Descrição**: Um sistema de notificação visual/auditiva.
    -   **Verificação de Recurso**: Alerta se um item necessário para um buff (ex: `RED_HERB_ACTIVATOR`, `Poção do Despertar`) acabou no inventário.
    -   **Queda Crítica de Buff**: Pisca um aviso se um buff de alta prioridade (ex: `PROTECTARMOR`, `Redenção`) cair inesperadamente.

### 1.5. Motor de Macro Avançado
- **Objetivo**: Habilitar automação complexa e baseada em decisões.
- **Descrição**: Atualizar o sistema de macro linear atual para suportar:
    -   **Condicionais**: `SE (HP < 50%) ENTÃO (Skill A) SENÃO (Skill B)`.
    -   **Loops**: Repetir ações X vezes.
    -   **Verificações de Estado**: Esperar por um buff/debuff específico antes de prosseguir.

### 1.6. Controle de Masterball (Gerenciador de Mascotes)
- **Objetivo**: Automatizar a implantação e rotação de pets "Masterball" (Eddga, Fenrir, etc.) com alta confiabilidade.
- **Descrição**: Uma aba especializada para controlar prioridade e sequências de implantação de pets.
    -   **Comportamento**: Similar ao Auto Switch, mas otimizado para mecânicas de pet.
        -   **Confiabilidade**: Suporta o envio de múltiplos pressionamentos de tecla em um curto espaço de tempo para garantir a implantação (superando atrasos de animação ou lag).
        -   **Sequência**: Cicla através de pets utilitários (ex: Eddga para Provocar, Fenrir para Enlouquecedor) e termina em um mascote de combate "Constante" (ex: Satan Morroc, Lord Seyren).
    -   **Perfis**: Suporte para múltiplas estratégias de implantação selecionáveis via teclas de atalho.
        -   *Exemplo*: Pressione `F1` para carregar e executar o perfil "Seyren".
        -   *Exemplo*: Pressione `F2` para carregar e executar o perfil "Satan Morroc".

---

## 2. Otimizações Técnicas

### 2.1. Otimização de Leitura de Memória (Buffs)
- **Estado Atual**: Vários módulos (`Autopot`, `AutoSwitch`) iteram por todo o array de buffs (tamanho `MAX_BUFF_LIST_INDEX_SIZE`) múltiplas vezes por ciclo para verificar status individuais.
- **Otimização**: Implementar o padrão `GetCurrentBuffsAsSet` usado em `DebuffsRecovery` e `AutobuffSkill`.
    -   **Ação**: Ler todo o array de buffs da memória **uma vez** por ciclo de thread.
    -   **Armazenamento**: Armazenar IDs ativos em um `HashSet<EffectStatusIDs>`.
    -   **Consulta**: Substituir buscas lineares O(N) por buscas em hash set O(1).
-   **Impacto**: Reduz significativamente a sobrecarga de leitura de memória e uso de CPU nos loops principais de automação.

### 2.2. Refatoração do Gerenciamento de Configuração
- **Estado Atual**: As classes `UserPreferences` e `Profile` são monolíticas e armazenam preocupações mistas (preferências de UI, configurações lógicas centrais, atalhos).
- **Otimização**: Dividir a configuração em classes modulares e específicas por funcionalidade (ex: `AutopotConfig`, `MacroConfig`, `SystemConfig`).
-   **Benefício**: Serialização/desserialização mais fácil, código mais limpo e risco reduzido de quebrar toda a estrutura do perfil ao adicionar uma funcionalidade.

---

## 3. Melhorias na Arquitetura de Software

### 3.1. Injeção de Dependência (DI)
- **Estado Atual**: A aplicação depende fortemente de Singletons (`ProfileSingleton`, `ClientSingleton`) e acesso estático. Isso torna testes unitários difíceis e cria acoplamento forte.
- **Melhoria**: Transição para um padrão de Injeção de Dependência.
    -   **Fase 1**: Usar Injeção por Construtor para passar dependências (como `Client`, `Profile`) para Forms e Actions.
    -   **Fase 2**: Introduzir um container DI leve (ex: `Microsoft.Extensions.DependencyInjection`) para gerenciar o ciclo de vida dos serviços principais.

### 3.2. Padrão MVP / MVVM para Forms
- **Estado Atual**: A lógica de negócios é frequentemente misturada com o código Windows Forms (`.cs`), lidando com eventos de UI e lógica.
- **Melhoria**: Desacoplar a lógica da UI.
    -   **Model**: Dados puros e regras de negócios.
    -   **View**: O Form, lidando estritamente com entrada do usuário e renderização.
    -   **Presenter/ViewModel**: Uma camada intermediária para lidar com a lógica.
-   **Benefício**: Permite testar a lógica sem instanciar Forms de UI e torna o código mais legível.

---

# Atualização deste README

Este arquivo é gerado automaticamente combinando a documentação técnica e o backlog do projeto. Para atualizá-lo com as informações mais recentes, solicite ao Agente de IA responsável:

**"Por favor, reescreva o README.md combinando o conteúdo traduzido de DOCS.md e BACKLOG.md, mantendo o cabeçalho do projeto."**