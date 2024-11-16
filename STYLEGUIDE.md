# CIMS Style Guide

## Introduction

For the **CIMS** project, we have chosen to adopt a style guide to ensure that our code is consistent and easy to read.
This document outlines the rules that we have chosen to follow.

For the most part, we have chosen to follow the [Microsoft Naming Conventions](<https://learn.microsoft.com/en-us/previous-versions/dotnet/netframework-1.1/xzf533w0(v=vs.71)>).
A `.editorconfig` file is included in the repository to enforce these conventions.

Please format your code according to the rules outlined in this document before submitting a pull request.
In Rider, you can use `Ctrl + Alt + L` to format your code.

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
├── DTOs/
└── Interfaces/
```

## Blazor

### Naming Conventions

- For _Components_, use the suffix `Component`.
- Method Naming: Use verbs that describe the action (e.g., `OnSubmit`, `HandleClick`).
- Event Naming: Use the `On` prefix followed by the event name (e.g., `OnClick`, `OnChange`).

### File Structure

- Place each component in its own file with a .razor extension.
- Group components into folders based on their functionality or module.
- Avoid nesting folders more than one level deep.

```markdown
/Pages
└── Home.razor
└── About.razor
/Components
└── Shared
└── NavBarComponent.razor
└── Forms
└── LoginForm.razor
```
### HTML

To enforce this in Rider you can change the formatting settings for razor files (Editor/Code Style/ASP.NET (Razor).

- Processing Instructions
    - Attributes format: `Each attribute on separate line`
    - Attributes indenting: `Align by first attribute`
- Inside of Tag Header
    - Attributes format: `Each attribute on separate line`
    - Attributes indenting: `Align by first attribute`
    - Sort attributes: `Checked`
    - Sort class attributes: `Checked`

#### Maximum of two HMTL-attribute per line

To reduce line length and improve readability, limit the number of HTML attributes per line to two. For more than two attributes, separate them into new lines. If there are only two attributes, it's the decision of the developer to split them or not.

##### From:

```html
<button class="navbar-toggler" type="button" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">Button</button>
```

##### To:

```html
<button
    class="navbar-toggler"
    type="button"
    aria-controls="navbarNav"
    aria-expanded="false"
    aria-label="Toggle navigation">
  Button
</button>
```

#### No nesting of HMTL-elements in one line

To ensure clarity of the HTML hierarchy, avoid nesting HTML elements in one line.

##### From:

```html
<div><span>html</span></div>
```

##### To:

```html
<div>
    <span>html</span>
</div>
```

#### Add section comments where applicable

To improve readability and overview of the code, add section comments where applicable.

##### Example:

```html
<!-- Main Form Frame -->
<form>...</form>
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
