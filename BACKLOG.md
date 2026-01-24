# TalesTools Project Backlog

This document outlines the roadmap, planned features, architectural improvements, and technical optimizations for the TalesTools project.

## 1. Planned Modules

### 1.1. Smart Buffs (Verification Tool)
- **Goal**: Ensure complex buff mechanics (specifically `brisaLeve` - Element Change) are correctly applied and synchronized with the tool's state.
- **Description**: A tool that actively verifies the memory state against the expected state for "Brisa Leve" buffs. Since these buffs often don't have standard icons or behave differently, this tool will provide a "truth" check to prevent the bot from thinking it has a buff when it doesn't (or vice versa).
- **Status**: Partially developed (see `DevForm`).

### 1.2. Development Tab
- **Goal**: Assist developers and advanced users in reverse-engineering and configuration.
- **Description**: An informative dashboard that displays:
    -   Real-time list of ALL active character buffs found in memory.
    -   Their internal numeric IDs (`EffectStatusIDs`).
    -   Their known string names (or "Unknown" for new IDs).
-   **Use Case**: Essential for identifying new buffs added to the game server, allowing users to update `EffectStatusIDs.cs` without recompiling the core logic blind.

### 1.3. Tormenta Buffs (Manual Burst Buffing)
- **Goal**: provide a semi-automated buffing sequence controlled by the user.
- **Description**: A specialized manager for buffs that shouldn't be always-on (unlike standard Autobuff).
    -   **Trigger**: Listens for a specific hotkey.
    -   **Action**: When pressed, checks a list of configured buffs.
    -   **Modes**:
        -   *Smart*: Only casts missing buffs.
        -   *Force*: Recasts ALL buffs in the list (useful for refreshing timers).

### 1.4. Gameplay Warnings (Player Companion)
- **Goal**: Prevent failures due to missing resources or silent buff drops.
- **Description**: A visual/audio notification system.
    -   **Resource Check**: Alerts if an item required for a buff (e.g., `RED_HERB_ACTIVATOR`, `Awakening Potion`) is empty in the inventory.
    -   **Critical Buff Drop**: Flashes a warning if a high-priority buff (e.g., `PROTECTARMOR`, `Devotion`) falls off unexpectedly.

### 1.5. Advanced Macro Engine
- **Goal**: Enable complex, decision-making automation.
- **Description**: Upgrade the current linear macro system to support:
    -   **Conditionals**: `IF (HP < 50%) THEN (Skill A) ELSE (Skill B)`.
    -   **Loops**: Repeat actions X times.
    -   **State Checks**: Wait for a specific buff/debuff before proceeding.

### 1.6. Masterball Control (Mascot Manager)
- **Goal**: Automate the deployment and rotation of "Masterball" pets (Eddga, Fenrir, etc.) with high reliability.
- **Description**: A specialized tab to control pet priority and deployment sequences.
    -   **Behavior**: Similar to Auto Switch but optimized for pet mechanics.
        -   **Reliability**: Supports sending multiple keypresses in a short burst to guarantee deployment (overcoming animation delays or lag).
        -   **Sequence**: Cycle through utility pets (e.g., Eddga for Provoke, Fenrir for Mind Breaker) and end on a "Constant" combat mascot (e.g., Satan Morroc, Lord Seyren).
    -   **Profiles**: Support for multiple deployment strategies selectable via hotkeys.
        -   *Example*: Press `F1` to load and execute the "Seyren" profile.
        -   *Example*: Press `F2` to load and execute the "Satan Morroc" profile.

---

## 2. Technical Optimizations

### 2.1. Memory Read Optimization (Buffs)
- **Current State**: Several modules (`Autopot`, `AutoSwitch`) iterate through the entire buff array (size `MAX_BUFF_LIST_INDEX_SIZE`) multiple times per cycle to check for individual statuses (e.g., `hasBuff(..., ANTI_BOT)`).
- **Optimization**: Implement the `GetCurrentBuffsAsSet` pattern used in `DebuffsRecovery` and `AutobuffSkill`.
    -   **Action**: Read the entire buff array from memory **once** per thread cycle.
    -   **Storage**: Store active IDs in a `HashSet<EffectStatusIDs>`.
    -   **Lookup**: Replace O(N) linear searches with O(1) hash set lookups for checks like `Anti-Bot`, `Berserk`, etc.
-   **Impact**: Significantly reduces memory read overhead and CPU usage in the main automation loops.

### 2.2. Configuration Management Refactoring
- **Current State**: `UserPreferences` and `Profile` classes are monolithic and store mixed concerns (UI preferences, core logic settings, keybinds).
- **Optimization**: Split configuration into modular, feature-specific classes (e.g., `AutopotConfig`, `MacroConfig`, `SystemConfig`).
-   **Benefit**: Easier serialization/deserialization, cleaner code, and reduced risk of breaking the entire profile structure when adding one feature.

---

## 3. Software Architecture Improvements

### 3.1. Dependency Injection (DI)
- **Current State**: The application relies heavily on Singletons (`ProfileSingleton`, `ClientSingleton`) and static access. This makes unit testing difficult and creates tight coupling.
- **Improvement**: Transition towards a Dependency Injection pattern.
    -   **Phase 1**: Use Constructor Injection for passing dependencies (like `Client`, `Profile`) to Forms and Actions.
    -   **Phase 2**: Introduce a lightweight DI container (e.g., `Microsoft.Extensions.DependencyInjection`) to manage the lifecycle of core services (`ClientService`, `ProfileService`).

### 3.2. MVP / MVVM Pattern for Forms
- **Current State**: Business logic is often mixed within Windows Forms code (`.cs` files), handling both UI events and logic (e.g., validation, profile updates).
- **Improvement**: Decouple logic from UI.
    -   **Model**: Pure data and business rules (already largely exists in `Model/`).
    -   **View**: The Form, strictly handling user input and rendering.
    -   **Presenter/ViewModel**: An intermediate layer to handle the logic (e.g., `AutopotPresenter`).
-   **Benefit**: Allows testing logic without instantiating UI forms and makes code more readable.

#### Implementation Process
1.  **Interface Definition**: Create an `I<FormName>View` interface in the `Presenters` namespace. This interface defines the properties and events that the View (Form) exposes to the Presenter.
2.  **Presenter Creation**: Create a `<FormName>Presenter` class in the `Presenters` namespace. This class takes the Interface and the Model as dependencies. It subscribes to View events to update the Model and updates the View when the Model changes.
3.  **Form Refactoring**:
    -   Implement the `I<FormName>View` interface in the Form.
    -   Instantiate the Presenter in the Form's constructor.
    -   Remove direct Model manipulation and logic from the Form's code-behind.
    -   Delegate actions to the Presenter or fire events defined in the Interface.
4.  **Cleanup**: Remove unused event handlers from the `Designer.cs` file if they were replaced by generic event subscriptions in the Presenter.

#### Status Tracker
-   [x] **AutopotForm** (`AutopotPresenter`)
-   [x] **AHKForm** (`AHKPresenter`)
-   [x] **AutoSwitchForm** (`AutoSwitchPresenter`)
-   [x] **AutoSwitchHealForm** (`AutoSwitchHealPresenter`)
-   [x] **ATKDEFForm** (`ATKDEFPresenter`)
-   [x] **AddServerForm** (`AddServerPresenter`)
-   [x] **AutoBuffStatusForm** (`AutoBuffStatusPresenter`)
-   [x] **ClientUpdaterForm** (`ClientUpdaterPresenter`)
-   [x] **ConfigForm** (`ConfigPresenter`)
-   [x] **CustomButtonForm** (`CustomButtonPresenter`)
-   [x] **DevForm** (`DevPresenter`)
-   [x] **MacroSongForm** (`MacroSongPresenter`)
-   [x] **MacroSwitchForm** (`MacroSwitchPresenter`)
-   [x] **ProfileForm** (`ProfilePresenter`)
-   [ ] **ServersForm**
-   [ ] **SkillAutoBuffForm**
-   [ ] **SkillTimerForm**
-   [ ] **StuffAutoBuffForm**
-   [ ] **ToggleApplicationStateForm**
