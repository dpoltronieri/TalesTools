# Project DNA: TalesTools

This document outlines the core principles, standards, and technical guidelines for the TalesTools project.

## 1. Coding Standards

- **Namespaces**: Use file-scoped namespaces (`namespace MyNamespace;`) for new files to reduce nesting.
- **Constructors**: Prefer primary constructors (`public class MyClass(MyService service)`) for new classes where applicable.
- **Style**: Adhere to the existing code style. For new components, follow the official [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions).
- **Principles**: Apply SOLID principles to promote maintainable and scalable code.

## 2. Technical Stack

- **Framework**: .NET Framework 4.7.2
- **Application Model**: Windows Forms (WinForms)
- **Language**: C#

## 3. Dependency Management

- **Pattern**: Dependencies are managed manually. There is no formal Dependency Injection (DI) container.
- **Instantiation**: Objects and forms are instantiated directly (e.g., `var myForm = new MyForm();`). When refactoring or adding features, consider opportunities to decouple components, even without a formal DI framework.

## 4. Project Structure & Compilation

- **Adding New Files**: When creating new `.cs` files (Models, Forms, Utils, etc.), you **MUST** explicitly add them to the `TalesTools.csproj` file. This project does not use glob patterns for including source files; each file is listed individually in an `<ItemGroup>` with a `<Compile Include="..." />` tag. Failure to do this will result in build errors as the new code will not be part of the compilation.