# CIMS Style Guide

## Introduction

For the **CIMS** project, we have chosen to adopt a style guide to ensure that our code is consistent and easy to read.
This document outlines the rules that we have chosen to follow.

For the most part, we have chosen to follow the [Microsoft Naming Conventions](<https://learn.microsoft.com/en-us/previous-versions/dotnet/netframework-1.1/xzf533w0(v=vs.71)>).
A `.editorconfig` file is included in the repository to enforce these conventions.

Please format your code according to the rules outlined in this document before submitting a pull request.

## Extra Naming Conventions

- For _Services_, _Repositories_, _Controllers_ and _Clients_, use the suffix `Service`, `Repository`, `Controller` and `Client` respectively.
- For _Models_, _Entities_ and _DTOs_, use the suffix `Model`, `Entity` and `DTO` respectively.

## Documentation

- Use XML documentation comments for all public types and members.
  - Use the `<summary>` tag to describe the purpose of the type or member.
  - Use the `<param>` tag to describe the parameters of a method.
  - Use the `<returns>` tag to describe the return value of a method.
- If a method is complex, add a comment to explain it, even if its not public.

## Dependency Injection

- Use constructor injection for mandatory dependencies. (Except for [Blazor](#dependency-injection-in-blazor))
- Register services in the `RegisterServices.cs` file.
- For every registered service, add a interface and use this for dependency injection instead of the concrete class.

## Error Handling

- Nether throw over repository layer!
  - Return a `Result` or `Option` object instead.
  - Ensure catching exceptions where necessary.
- If a problem occurs return a `ProblemDetails` object in the Controller with the appropriate error metadata.

## LINQ

- Always use LINQ methods instead of query syntax.
- Use LINQ where possible to make the code more readable.

## Folder Structure

> [Blazor files](#file-structure)

The files should be structured in the following way:

```markdown
ProjectRoot/
├── Services/
├── Repositories/
├── Controllers/
├── Clients/
├── Models/
├── Entities/
├── Dtos/
└── Interfaces/
```

## Blazor

### Naming Conventions

- For _Pages_ and _Dialogs_, use the suffix `Page` and `Dialog` respectively.
- Method Naming: Use verbs that describe the action (e.g., `OnSubmit`, `HandleClick`).
- Event Naming: Use the `On` prefix followed by the event name (e.g., `OnClick`, `OnChange`).

### File Structure

- Place each component in its own file with a .razor extension.
- Group components into folders based on their functionality or module.
- Avoid nesting folders more than one level deep.

```markdown
/Pages
└── HomePage.razor
└── AboutPage.razor
/Components
└── /Shared
└── /Forms
└───── LoginForm.razor
└── /Home
└───── WelcomeBanner.razor
└── NavBar.razor
```

### Use of `@code`

- Place logic directly related to the UI in the .razor file's @code block.
- Use @code sparingly; for more complex logic, move it to a `\*.razor.cs` partial code-behind file to separate the UI from logic.

### Binding and Event Handling

- Prefer `@bind=""` syntax for two-way data binding, ensuring it is explicit about the property it binds to.
- Use `@on` event directives (e.g., `@onclick`, `@onchange`) for clarity when handling events.

### CSS Styling Guidelines

- Prefer CSS Isolation for component-specific styles using `ComponentName.razor.css`.
- Use CSS classes instead of inline styles for reusability.

### Dependency Injection In Blazor

- In _Blazor_ it is not possible to use constructor injection.
- Use the `[Inject]` attribute to inject services into components.
  - If a `razor.cs` file is present, inject services in it.
