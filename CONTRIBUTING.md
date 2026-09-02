# Contributing Guidelines

Thanks for contributing! 🎉  
Whether it's fixing bugs, improving docs, or adding features your help is appreciated.

By submitting a pull request, you agree that your contribution is licensed
under the project's MIT License (see [LICENSE.txt](LICENSE.txt)) and may be
distributed as part of the project.

---

## Project Structure

```
TwBlazor/
├── TwBlazor/                     # Component library
│   ├── Builders/
│   ├── Components/
│   ├── Configuration/
│   ├── Enums/
│   ├── Extensions/
│   ├── Models/
│   ├── Services/
│   ├── Utilities/
│   └── wwwroot/
├── TwBlazor.Docs/                # Documentation content (shared)
│   ├── Generated/
│   ├── Layout/
│   ├── Pages/
│   └── wwwroot/
├── TwBlazor.Docs.Compiler/       # Build-time docs compiler
│   ├── CodeExampleExtractor.cs
│   ├── RegionSnippetExtractor.cs
│   ├── CodeGenerator.cs
│   ├── Paths.cs
│   └── SnippetTextUtils.cs
├── TwBlazor.Server/              # Server-side host
│   ├── Components/
│   │   ├── Layout/
│   │   └── Pages/
│   └── Program.cs
├── TwBlazor.Wasm/                # WASM project
│   └── Program.cs
├── TwBlazor.WasmHost/            # WASM host
│   ├── Pages/
│   │   └── _Host.cshtml
│   └── Program.cs
├── TwBlazor.Theme/               # Theme definitions
│   └── Theme.cs
└── TwBlazor.Tests/               # Unit tests
```

## CSS

The easiest way to build the Tailwind CSS file(s) for TwBlazor is to use the custom `watch-tailwind.ps1` PowerShell script located in the root of the repository. This script will automatically build the Tailwind CSS file(s) for both the TwBlazor source and documentation site and will continue to watch for changes to the input CSS files and rebuild as necessary.

You can run the script by executing the following command in the root of the repository:
`.\watch-tailwind.ps1`

You can also manually build the TwBlazor Tailwind CSS file(s) by navigating to the correct directory e.g. 

### TwBlazor:

- `cd ./TwBlazor` for the TwBlazor source CSS.
- `npx @tailwindcss/cli -i ./wwwroot/css/input.css -o ./wwwroot/css/twblazor.css --watch`

---

### TwBlazor Docs:

- `cd ./TwBlazor.Docs` for the TwBlazor documentation site CSS.
- `npx @tailwindcss/cli -i ./wwwroot/css/input.css -o ./wwwroot/css/output.css --watch`


**NOTE: You may also have to run a clean and rebuild of the solution to ensure the new CSS file is picked up by the projects if building manually, we advise you use `watch-tailwind.ps1` detailed above.**

---

## 📋 Code of Conduct
Be respectful and constructive. See `CODE_OF_CONDUCT.md`.

---

## ⚙️ Prerequisites
- Latest stable .NET SDK (.NET 10)

---

## 🚀 Getting Started

Run these commands from the root of the repository.

### Run the docs site (Blazor Server)

```
dotnet run --project TwBlazor.Server
```

### Run the docs site (Blazor WebAssembly)

```
dotnet run --project TwBlazor.WasmHost
```

This serves the compiled `TwBlazor.Wasm` app via the `TwBlazor.WasmHost` host project.

> Tip: swap `dotnet run` for `dotnet watch run` on either command to get automatic rebuilds on file changes. Console output will show the local URL to browse to (e.g. `https://localhost:xxxx`).

### Run the tests

```
dotnet test TwBlazor.Tests
```

### Rebuild the doc examples

Code snippets shown on the documentation site are extracted from real source (docs pages and a few production files) by `TwBlazor.BuildTools`. See [CODE_SNIPPETS.md](CODE_SNIPPETS.md) for how this works. This runs automatically before `TwBlazor.Docs` builds, but you can trigger it manually if generated snippets look stale:

```
dotnet run --project TwBlazor.BuildTools
```

---

## 🌿 Branching Strategy

We follow a **develop-main** workflow:

- **`main`** → Production-ready code, tagged releases only
- **`develop`** → Integration branch for all features and fixes
- **`feature/*`** → New features (e.g., `feature/toast-component`)
- **`bug/*`** → Bug fixes (e.g., `bug/fix-modal-close`)

### Merge Flow

```
feature/my-feature  →  develop  →  main
     bug/my-fix     ↗          ↗
```

**Important:** 
- ✅ PRs from `feature/*` or `bug/*` → `develop`
- ✅ PRs from `develop` → `main` (releases only)
- ❌ Direct PRs to `main` from feature/bug branches are **blocked**

---

## 🔀 Pull Requests

- Keep PRs **focused on a single change**
- Fork the repo and create a **feature branch** from `develop`:
  - `feature/my-feature`
  - `bug/my-bug`
- **Always target `develop`** for feature/bug PRs
- Make sure:
  - Project builds
  - Tests pass
- Add tests when changing logic
- Link related issues (e.g. Fixes #123)
- Use clear PR titles following [Conventional Commits](https://www.conventionalcommits.org/) format:  
  `<type>(<optional scope>): <description>`  
  **Valid types:** `feat`, `fix`, `docs`, `style`, `refactor`, `perf`, `test`, `build`, `ci`, `chore`, `revert`  
  **Examples:**
  - `feat(button): add outlined variant`
  - `fix(modal): resolve close event not firing`
  - `chore: update dependencies`  
  > ⚠️ PRs with titles that don't follow this format will **fail** the automated title lint check.
- Include screenshots/gifs for UI changes
- Avoid unrelated refactoring

---

## 🧱 Project Structure (example)

- `/src` → main code  
- `/docs` → documentation  
- `/tests` → unit tests  

---

## 🧑‍💻 Coding Guidelines

### Do:
- Write clean, readable code
- Keep components simple and focused
- Add comments for public APIs
- Follow existing patterns

### Don’t:
- Put complex logic in property getters/setters
- Modify component state unpredictably
- Break existing functionality

---

## 🧩 Component Guidelines (if applicable)

- Keep parameters simple (no hidden side effects)
- Use clear naming
- Avoid directly mutating inputs—use events/callbacks instead
- Keep UI and logic separated where possible

---

## 🧪 Testing

- Add tests for any non-trivial logic
- Ensure all tests pass before submitting
- Keep tests:
  - Small
  - Independent
  - Descriptive

### Naming
- `Subject_Action_ExpectedResult` — use the full component or method name as the subject  
  **Examples:**
  - `TwDataTable_Renders_WithEmptyItems`
  - `TwDataTable_Pageable_ShowsPaginationControls`
  - `UpdateApplication_DoesNothing_WhenOptionsIsNull`

---

## 🚫 Common Mistakes

- Mixing multiple changes in one PR
- Skipping tests for logic changes
- Breaking existing behaviour
- Large PRs without prior discussion

---

## 🔄 Keeping Your Branch Updated

- Pull latest changes from the main branch regularly
- Resolve conflicts early

---

## 💬 Before Large Changes

Open an issue first to discuss your idea.

---

## 🤖 CI / Checks

All PRs must pass:
- Build
- Tests
- Linting, formatting and SonarCloud checks

---

## ✅ Quick Checklist

Before submitting:
- [ ] Code builds  
- [ ] Tests pass  
- [ ] Tests added 
- [ ] PR is focused  
- [ ] Issue linked