# Features: TalesTools

This document catalogs the existing and planned features of the TalesTools application.

## Existing Features

The following features are implemented, primarily corresponding to a form in the `Forms/` directory:

- **Server Management**: Add, remove, and manage game servers (`AddServerForm`, `ServersForm`).
- **Profile Management**: Create and manage user profiles (`ProfileForm`).
- **Autopot**: Automatically use HP/SP recovery items (`AutopotForm`).
- **Auto-Buffing**:
    - Manage status-based buffs (`AutoBuffStatusForm`).
    - Manage skill-based buffs (`SkillAutoBuffForm`).
    - Manage item/stuff-based buffs (`StuffAutoBuffForm`).
- **Automated Switching**:
    - General-purpose item/gear switching (`AutoSwitchForm`).
    - Healing-specific item/gear switching (`AutoSwitchHealForm`).
- **Macros**:
    - Song-specific macros for Bard/Dancer classes (`MacroSongForm`).
    - Gear-switching macros (`MacroSwitchForm`).
- **Skill Timers**: Display cooldowns or durations for skills (`SkillTimerForm`).
- **AHK Integration**: Simple AutoHotkey script management (`AHKForm`).
- **ATK/DEF Mode**: A specialized mode for attack/defense (`ATKDEFForm`).
- **Client Updater/Patcher**: Tools for updating the game client (`ClientUpdaterForm`, `AutoPatcher`).
- **Custom Buttons**: User-defined custom action buttons (`CustomButtonForm`).
- **Developer Tools**: A form for development and debugging (`DevForm`).

## Planned Modules

- **Smart Buffs**: A tool to verify if the `brisaLeveBuffs` are properly applied (partially developed).
- **Development Tab**: An informative tab displaying active character buffs, their names, and unknown buff IDs to help identify new statuses and update `EffectStatusIDs.cs`.
- **Tormenta Buffs**: A dedicated buff manager that listens for a specific hotkey to check and apply buffs (with options to re-apply all or only those that are inactive).
- **Gameplay Warnings**: A player companion that highlights inactive critical buffs (e.g., `PROTECTARMOR`) and notifies the user if a buff application fails (e.g., if `RED_HERB_ACTIVATOR` is missing from the inventory).
- **Advanced Macro Engine**: A more powerful macro system supporting complex conditional logic.
