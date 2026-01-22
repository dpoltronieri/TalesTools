# TalesTools Functionality Documentation

This document provides detailed documentation for the key functional tabs and core systems of the TalesTools application.

## 1. Autopot

**Description:**
The Autopot feature automatically uses healing items (Potions) to maintain the character's HP and SP levels. It is designed to be the primary survival tool.

**Key Parameters:**
- **HP Key**: The keyboard key assigned to the HP potion.
- **HP %**: The percentage threshold below which the HP potion will be used.
- **SP Key**: The keyboard key assigned to the SP potion.
- **SP %**: The percentage threshold below which the SP potion will be used.
- **Delay (ms)**: The time interval (in milliseconds) between checks/potion usage (Default: 15ms).
- **First Heal**: Determines the priority between HP and SP healing (Options: "HP" or "SP").
- **Stop when Critical Wound (Brisa Leve)**: If enabled, pauses Autopot when the "Critical Wound" status is active, preventing waste of potions.
- **Equip Before/After**: Allows configuring a key to equip specific gear (e.g., healing bonus gear) before the healing loop starts and another key to equip gear after the healing loop ends.

**Logic:**
1.  **Safety Checks**: Verifies if the character is in a city (if "Stop in City" is enabled), has "Anti-Bot" status, "Berserk" status, or is in "Competitive" mode. If any are true, it stops.
2.  **Priority Loop**:
    -   If **HP Priority**: Checks HP first. If HP < HP%, it enters a loop pressing the HP Key until HP >= HP%.
        -   *Intelligent SP Check*: Inside the HP loop, every 4 HP potions, it checks if SP < SP%. If so, it uses one SP potion to prevent SP bottoming out during intense damage.
    -   If **SP Priority**: Checks SP first. Logic is similar but prioritizes SP recovery.
3.  **Equipment Swapping**: If configured, it triggers the "Equip Before" key before entering the healing loop and "Equip After" key when the target HP is reached.

## 2. Yggdrasil

**Description:**
A specialized mode of Autopot designed for high-impact recovery items like Yggdrasil Berries or Seeds. It shares the core engine with Autopot but simplifies the interface and logic for burst healing.

**Key Parameters:**
- **HP Key**: Key for the Yggdrasil item.
- **HP %**: Percentage threshold to use the item.
- **SP Key**: Key for the SP recovery item (often the same Yggdrasil item in some configurations, or a separate SP item).
- **SP %**: Percentage threshold for SP recovery.
- **Delay**: Execution delay (Default: 50ms - typically slower than standard potions to avoid waste/over-usage).

**Differences from Autopot:**
-   **Simplified UI**: Does not show "Stop when Critical Wound" or "Equip Before/After" options, as Yggdrasil healing is typically instant and full, bypassing some standard status checks or gear swapping needs.
-   **Logic**: It strictly follows the HP/SP thresholds to trigger the keys without the complex "Switch/Equip" logic.

## 3. Skill Timer

**Description:**
A utility to automatically cast skills or use items at fixed time intervals. Useful for maintaining buffs that don't have a status icon or for repetitive actions.

**Key Parameters:**
- **Lane 1-4**: The tool provides 4 independent timer lanes.
- **Key**: The key to be pressed.
- **Delay (s)**: The time interval in seconds between each press.

**Logic:**
-   Each lane operates independently.
-   Once enabled, the system waits for the specified **Delay** and then simulates the **Key** press.
-   It repeats indefinitely while the feature is active.
-   Commonly used for skills like "Magnum Break" (for the bonus), food buffs, or other timed consumables.

## 4. AutoSwitch Heal

**Description:**
A smart equipment switcher that reacts to the character's HP and SP levels. It allows automatically swapping between "Tank/Defensive" gear and "Damage/Offensive" gear based on current health/mana.

**Key Parameters:**
-   **HP Switching**:
    -   **Less HP % / Key**: When HP is *below* this percentage, press this key (e.g., equip Shield/GR Card).
    -   **More HP % / Key**: When HP is *above* this percentage, press this key (e.g., equip Two-Handed Sword/Damage Card).
-   **SP Switching**:
    -   **Less SP % / Key**: When SP is *below* this percentage, press this key (e.g., equip MP regen gear).
    -   **More SP % / Key**: When SP is *above* this percentage, press this key (e.g., equip standard gear).
-   **Equip Delay**: The minimum time between equipment switches (Default: 3 seconds) to prevent spamming/glitching.

**Logic:**
1.  **Monitoring**: Constantly monitors current HP and SP.
2.  **Condition Check**:
    -   If `HP < Less HP %`: Triggers "Less HP Key".
    -   Else If `HP > More HP %`: Triggers "More HP Key".
    -   (Similar logic for SP).
3.  **Safety**: Includes standard checks (Anti-Bot, City status) to prevent switching at inappropriate times.

---

## 5. Core Systems & Tools

### Ragnarok Client
**Description:**
Manages the connection between the tool and the game client.
-   **Server Management**: Allows users to configure memory addresses (`hpAddress`, `nameAddress`, `mapAddress`) for different server versions. Configurations are saved in `supported_servers.json`.
-   **Process Attachment**: The user selects the specific Ragnarok client process to attach the tool to.
-   **City Safety**: Maintains a list of "City Maps" (`city_name.json`) where offensive automation (like Autopot) is automatically disabled to prevent suspicious behavior in safe zones.

### Profile System
**Description:**
A comprehensive configuration management system.
-   **Profiles**: Allows saving distinct configurations (Autopot settings, Macros, Keys) into named JSON files.
-   **Management**: Users can Create, Load, and Delete profiles.
-   **Character Binding**: Automatically remembers which profile was last used for a specific character name, facilitating seamless switching between characters.

### Current Status (Dashboard)
**Description:**
The main control hub for the application's state.
-   **Global Toggle (ON/OFF)**: A master switch that enables or disables *all* automation features.
    -   **Hotkey**: Configurable keyboard shortcut to toggle this state.
    -   **Visuals**: The UI turns Green (ON) or Red (OFF), and the Tray Icon changes to reflect the status.
    -   **Audio**: Plays distinct sound effects ("Speech On" / "Speech Off") for audio feedback.
-   **Heal Status**: A dedicated sub-toggle specifically for healing functions (Autopot/Yggdrasil). This allows pausing healing (e.g., during a specific mechanic) without stopping other macros or tools.

### Transfer Item
**Description:**
A utility for inventory management.
-   **Function**: Rapidly transfers items between the Inventory and Storage/Cart.
-   **Usage**: While the configured **Transfer Key** is held down, the tool simulates `Alt + Right Click` repeatedly.
-   **Logic**: Uses low-level inputs to perform the specific Ragnarok shortcut for item moving, significantly speeding up re-supply runs.

### Priority Key (Tecla Prioridade)
**Description:**
A manual override safety feature.
-   **Function**: When the **Priority Key** is held down, the tool **pauses all automation threads**.
-   **Use Case**: Allows the user to take full manual control of the character (e.g., to cast a specific skill, move precisely, or type) without the bot's inputs interfering or interrupting.
-   **Configuration**:
    -   **Key**: The key that triggers the pause.
    -   **Delay**: The responsiveness of the check (typically low to ensure immediate pause).

---

## 6. Configuration & Management Tabs

### Config Tab
**Description:**
Allows global customization of the application's behavior and interface.

**Key Features:**
-   **Tab Visibility**: Toggle the visibility of major feature tabs (e.g., hide "Macro Songs" if not needed) to declutter the UI.
-   **Autobuff Filters**: Granularly show/hide specific skill groups (Swordman, Mage, etc.) or item groups (Foods, Potions) in the Autobuff tabs.
-   **Safety & Automation Preferences**:
    -   **Stop Buffs/Heal in City**: Prevents wasting consumables in safe zones.
    -   **Stop with Chat Open**: Pauses automation when the in-game chat bar is active (to prevent typing macros into chat).
    -   **Stop Buffs on Rein (Mount)**: Prevents casting attempts while mounted.
    -   **Get Off Rein**: Option to automatically dismount (using `Get Off Rein Key`) if needed.
-   **Ammo Switch**: Configures keys (`Ammo 1`, `Ammo 2`) to toggle between ammunition types.
-   **Buff Order**: Drag-and-drop interface to prioritize the order in which buffs are checked/applied.

### Profiles Tab
**Description:**
The interface for managing user profiles.

**Key Features:**
-   **Create Profile**: Add a new named profile configuration.
-   **Remove Profile**: Delete an existing profile (Default profile cannot be deleted).
-   **Assign to Character**: Binds the currently selected profile to the active character in the game client. The tool will auto-load this profile when that character is detected.

### Dev Tab
**Description:**
A diagnostic and monitoring tool for developers and advanced users.

**Key Features:**
-   **Brisa Leve Monitor**: A specialized grid view for the "Brisa Leve" (Element Change) mechanic.
    -   **Monitors**: Fire, Water, Wind, Ground, Dark, Ghost, and Holy statuses.
    -   **Displays**: Buff Name, Internal ID, Current Status (ON/OFF), and the mapped Key.
    -   **Usage**: Verifies if the tool correctly detects these specific status effects from the game memory.

---

## 7. Main Feature Tabs

### Skill Spammer
**Description:**
A general-purpose key spammer (AutoKey).

**Modes:**
-   **Compatibility**: 
    -   Uses standard Windows messaging (`PostMessage`) to simulate input. 
    -   Compatible with most game versions.
    -   Supports **No Shift** and **Mouse Flick** features.
-   **Synchronous**:
    -   Uses blocking calls (`SendMessage`) to ensure input is processed before continuing.
    -   Explicitly sends both Key Down and Key Up events, which can be more reliable for certain skills.
    -   Supports **No Shift** and **Mouse Flick**.
-   **Speed Boost**:
    -   Uses lower-level hardware simulation APIs (`mouse_event`) for mouse clicks.
    -   **Disabled Features**: Does *not* support **No Shift** or **Mouse Flick** (these options are disabled in the UI when Speed Boost is active).
    -   **Use Case**: For scenarios requiring the absolute fastest possible input rate, bypassing some safety/compatibility layers.

**Additional Features:**
-   **No Shift**: Injects a Shift key press before the action. This effectively acts as a "Standstill Attack" (preventing the character from walking towards the target if clicked).
-   **Mouse Flick**: Micro-movements of the mouse cursor (-1/+1 pixel) during clicks. This can help register clicks in games that ignore static input or to refresh cursor targeting.
-   **Ammo Switch**: If enabled in Config, automatically toggles between `Ammo 1` and `Ammo 2` keys on every spam cycle.
-   **Get Off Rein**: Automatically dismounts the character (if mounted) before attacking, ensuring the attack action isn't blocked by the mount status.

### Autobuff - Skills
**Description:**
Automatically maintains self-buffs derived from class skills.

**Key Features:**
-   **Class Groups**: Buffs are organized by class (Swordman, Archer, Mage, etc.) for easy navigation.
-   **Logic**: The tool monitors the character's status. If a configured buff is missing, it presses the assigned key to recast it.
-   **Configuration**:
    -   **Key**: Assign the key for the specific skill.
    -   **Delay**: Time interval between status checks.

### Autobuff - Stuffs
**Description:**
Automatically uses consumable items to maintain item-based buffs.

**Key Features:**
-   **Item Groups**: Organized by type (Potions, Foods, Elementals, Boxes, Scrolls, Etc.).
-   **Logic**: Similar to Skill Autobuff, it checks for the presence of the item's effect (e.g., "Awakening Potion" status) and consumes the item if the effect is missing.
-   **Configuration**:
    -   **Key**: Assign the key for the item.
    -   **Delay**: Time interval between status checks.

### Auto Switch
**Description:**
A "State-Based" equipment switcher.

**Key Features:**
-   **Trigger**: Detects when a specific self-buff (e.g., "Spirit", "Quicken") is active.
-   **Action**:
    -   **Item Key**: Equips an item (e.g., a weapon that benefits from the buff).
    -   **Skill Key**: Optionally casts a skill after equipping.
    -   **Next Item Key**: Equips another item (e.g., swapping back) after the action.
-   **Use Case**: specialized gear swapping sequences triggered by your character's state.

### ATK x DEF
**Description:**
A mode-switching system for "Offensive" vs "Defensive" stances.

**Key Features:**
-   **Lanes**: Supports up to 8 independent configurations.
-   **Configuration per Lane**:
    -   **Spammer Key**: The key used for attacking (e.g., a skill spam key).
    -   **ATK Set**: A list of keys (Equipment) to equip when entering Attack mode.
    -   **DEF Set**: A list of keys (Equipment) to equip when entering Defense mode.
    -   **Switch Delay**: Delay between equipment swaps.
-   **Logic**: Designed to allow rapid transitioning between full damage gear (while attacking) and full tank gear (when stopping or defending).

### Macro Songs
**Description:**
A dedicated macro system for Bards and Dancers to manage song cycling and instrument swapping.

**Key Features:**
-   **Lanes**: 8 macro lanes.
-   **Sequence**:
    1.  **Trigger**: Key to start the macro.
    2.  **Equip Instrument**: Swaps to the musical instrument.
    3.  **Macro Entries**: Sequence of song skills to play.
    4.  **Equip Dagger**: Swaps to a dagger (standard Ragnarok tactic to cancel the song animation/delay).
-   **Delay**: Configurable delay between actions in the sequence.

### Macro Switch
**Description:**
A generic, multi-purpose macro sequencer.

**Key Features:**
-   **Lanes**: 10 macro lanes.
-   **Sequence**: Allows defining a chain of keys to be pressed in order.
-   **Per-Step Configuration**:
    -   **Key**: The key to press.
    -   **Delay**: The wait time after this step.
    -   **Click**: Whether to click the mouse after this step.
-   **Trigger**: The first key in the sequence acts as the trigger.

### Debuffs (Auto Buff Status)
**Description:**
Automatically cures negative status effects (Debuffs).

**Key Features:**
-   **Standard Debuffs**: Monitors and cures common ailments like Silence, Blind, Confusion, Poison, Hallucination, and Curse.
    -   **Status Key**: A single key assignment that can be used for all these standard debuffs (ideal for "Green Potion" or "Panacea").
-   **Special/3rd Job Debuffs**: Monitors specific high-level debuffs like Critical Wound, Deep Sleep, Burning, Freezing, etc.
    -   **New Status Key**: A dedicated key assignment for curing these specific conditions.
