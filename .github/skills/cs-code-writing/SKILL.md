---
name: cs-code-writing
description: 'Write robust C# code with documented interfaces, inherited XML docs, and explicit exception contracts. Use when asked to design interfaces, implement classes with <inheritdoc />, define sync/async methods consistently, or enforce that implementations only throw documented exceptions. Applies to serialization, IO adapters, service boundaries, and library APIs.'
---

# C# Code Writing Skill

A focused skill for authoring high-quality C# interfaces and implementations that:
- Include comprehensive XML documentation (summary, params, returns, remarks)
- Explicitly document allowed exceptions in the interface
- Use `<inheritdoc />` in implementations to inherit docs and keep behavior aligned
- Mirror sync/async method contracts and exceptions

## When to Use This Skill
- Creating or updating public interfaces in libraries or shared modules
- Implementing classes that must only throw documented exceptions
- Designing serialization and IO layers (files, environment, network)
- Ensuring async methods (`Task`, `Task<T>`) follow the same contract as sync counterparts

## Prerequisites
- Clear understanding of the API surface and data models
- Decision on which exceptions are allowed and how they map to failure modes
- Familiarity with XML documentation comments and `<inheritdoc />`

## Step-by-Step Workflow

1. Define Interface Methods with XML Docs:
   - Add `/// <summary>`, `/// <param>`, `/// <returns>`, `/// <remarks>` as needed
   - Add explicit `/// <exception>` entries for all allowed exceptions
   - Keep sync/async variants consistent in naming and exception lists

2. Implement with `<inheritdoc />` and Validations:
   - Put `<inheritdoc />` on class and methods
   - Validate inputs early (e.g., `ArgumentNullException` for null)
   - Throw only exceptions documented by the interface
   - Use clear messages to aid debugging

3. Align Sync and Async Behavior:
   - Same validation and exceptions
   - For placeholder async errors, prefer `Task.FromException` or `throw` inside `async` when implemented

4. Verify Contracts:
   - Build the project and check for missing XML docs on public APIs
   - Smoke-test methods to confirm documented exceptions are thrown

## Example from This Repository
- Interfaces: `src/Core/Serialization/Interfaces.cs`
  - `ISRunnerConfigurationLoader`: `SRunnerConfiguration Load()`, `Task<SRunnerConfiguration> LoadAsync()`
    - Exceptions: `InvalidOperationException`, `UnauthorizedAccessException`, `System.IO.IOException`
  - `ISRunnerConfigurationExporter`: `void Export(SRunnerConfiguration configuration)`, `Task ExportAsync(SRunnerConfiguration configuration)`
    - Exceptions: `ArgumentNullException`, `InvalidOperationException`, `UnauthorizedAccessException`, `System.IO.IOException`
- Implementations:
  - `src/Core/Serialization/HomeDirectoryLoader.cs`: uses `<inheritdoc />`, throws documented `InvalidOperationException`
  - `src/Core/Serialization/HomeDirectoryExporter.cs`: validates null config, throws `ArgumentNullException` and `InvalidOperationException` only

## Patterns
- Inherit docs:
  ```csharp
  /// <inheritdoc />
  public class MyLoader : ISomeLoader { /* ... */ }
  ```
- Validate inputs:
  ```csharp
  if (arg is null) throw new ArgumentNullException(nameof(arg));
  ```
- Mirror sync/async exceptions:
  ```csharp
  // sync
  throw new InvalidOperationException("Destination unavailable.");
  // async
  return Task.FromException(new InvalidOperationException("Destination unavailable."));
  ```

## Troubleshooting
- Undocumented exceptions: add to interface XML docs or refactor to documented ones
- Divergent sync/async behavior: share helper methods to centralize validation
- Missing IntelliSense details: ensure `<param>`/`<returns>` are present and accurate

## Validation Checklist
- [ ] Interface methods have XML docs and explicit `<exception>` tags
- [ ] Implementations use `<inheritdoc />` and throw only documented exceptions
- [ ] Sync and async methods share the same validation and exceptions
- [ ] Build passes; smoke tests confirm expected exceptions

## References
- Microsoft Docs: XML documentation comments in C#
- .NET Best Practices for exceptions
