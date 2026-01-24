# Architecture: TalesTools

This document provides a high-level overview of the TalesTools application architecture.

## 1. High-Level Architecture

The application follows a **Monolithic UI Architecture**. It is a single-project Windows Forms application with a clear separation of concerns into distinct namespaces.

- **`TalesTools.exe`**: The main executable.

## 2. Project Structure

The solution consists of a single project (`TalesTools.csproj`) organized into the following logical layers:

- **`Forms/`**: Contains all Windows Forms, representing the application's UI and features. Each form is responsible for its own user interactions and logic. The main container form (`Container.cs`) acts as the primary window.
- **`Presenters/`**: Contains Presenter classes and View interfaces for the MVP (Model-View-Presenter) pattern, decoupling UI logic from business logic (e.g., `AutopotPresenter`, `AHKPresenter`).
- **`Model/`**: Contains the application's business logic, data structures, and core domain models (e.g., `Profile`, `Autopot`, `Macro`).
- **`Utils/`**: Provides shared utilities and helper classes, such as `ProcessMemoryReader`, `KeyboardHook`, and other low-level functionality.
- **`Properties/` & `Resources/`**: Manages project settings, assembly information, and embedded resources like images and icons.

## 3. Data Flow

1.  **Startup**: The application starts via `Program.cs`, which instantiates and runs the main `Container` form.
2.  **User Interaction**: The user interacts with the various forms (`AHKForm`, `AutopotForm`, etc.), which are launched from the main `Container`.
3.  **Business Logic**: The forms utilize classes from the `Model/` namespace to perform core application logic.
4.  **Utilities**: Both `Forms` and `Model` layers use helper classes from `Utils/` for common tasks.

## 5. Design Patterns

### 5.1. MVP (Model-View-Presenter)
The application is transitioning to the MVP pattern to separate concerns and improve testability.

-   **Model**: Represents the data and business logic (e.g., `Autopot`, `AHK`).
-   **View**: The Interface (`IView`) defining the UI contract, implemented by the Form (e.g., `AutopotForm`, `AHKForm`).
-   **Presenter**: Orchestrates the interaction between the View and the Model, handling user events and updating the UI.

**Implemented Forms:**
-   `AutopotForm`
-   `AHKForm`
-   `AutoSwitchForm`
