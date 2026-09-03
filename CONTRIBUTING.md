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

## 📦 Releasing & Versioning

Versions are not stored in a file: they come entirely from Git tags, read at
build time by [MinVer](https://github.com/adamralph/minver). You never need to
bump a version number by hand.

### `develop` → GitHub Packages (preview)

Every push to `develop` builds and publishes a **prerelease** NuGet package to
the [GitHub Packages feed](https://github.com/TwBlazor/twblazor/pkgs/nuget/TwBlazor)
automatically, versioned like `1.2.1-preview.4`. This happens on every merge, so
you don't need to do anything to get a preview package out.

### `develop` → `main` → NuGet.org (release)

A release is just a pull request from `develop` into `main`:

1. Open the PR as usual (see [Pull Requests](#-pull-requests) above).
2. Add **one** of these labels to say how the version should move:
   - `release:minor`, for backwards compatible changes, e.g. `1.1.0` → `1.2.0`
   - `release:major`, for breaking changes, e.g. `1.1.0` → `2.0.0`
   - A bot comments on the PR and the **Verify Release Bump Label** check stays
     red until exactly one label is applied.
3. Merge the PR.

Merging automatically:
- Tags the merge commit with the new version (e.g. `v1.2.0`)
- Publishes the **stable** package to both GitHub Packages and
  [NuGet.org](https://www.nuget.org/packages/TwBlazor)
- Creates a GitHub Release with the packed `.nupkg` attached

There is nothing further to do on `develop` afterwards. Its next build picks
up the new release tag on its own and continues previewing from there (e.g.
`1.2.1-preview.0`, then `.1`, and so on).

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
- Include screenshots/gifs for UI changes
- Avoid unrelated refactoring

### PR title

Every PR title is checked by an automated **Lint PR** check and must follow
[Conventional Commits](https://www.conventionalcommits.org/) format:

`<type>(<optional scope>): <subject>.`

- **Valid types:** `feat`, `fix`, `bug`, `docs`, `style`, `refactor`, `perf`, `test`, `build`, `ci`, `chore`, `revert`
- The subject (the part after `type:`) must be **10 to 100 characters**
- The subject must **end with a full stop**

**Examples:**
- `feat(button): add outlined variant.`
- `fix(modal): resolve close event not firing.`
- `chore: update dependencies.`

> ⚠️ PRs with titles that don't follow this format will **fail** the automated `Lint PR` check.

### PR description

Opening a PR prefills the description from
[`.github/pull_request_template.md`](.github/pull_request_template.md). The same
`Lint PR` check requires every section to actually be filled in:

- **Changes**, what does this PR change?
- **Testing**, how was it verified?
- **Checklist**, tick at least one box

An unedited template, or a section left as just the placeholder comment, will
**fail** the check.

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