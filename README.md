<p align="center">
  <img src="./images/banner.svg" height="300" style="height: 300px;">

  [![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_TwBlazor&metric=alert_status&token=6bbca7c19f7ba793b0e164a9805d518c9a175bfe)](https://sonarcloud.io/summary/new_code?id=TwBlazor_TwBlazor)
  [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_TwBlazor&metric=bugs&token=6bbca7c19f7ba793b0e164a9805d518c9a175bfe)](https://sonarcloud.io/summary/new_code?id=TwBlazor_TwBlazor)
  [![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_TwBlazor&metric=code_smells&token=6bbca7c19f7ba793b0e164a9805d518c9a175bfe)](https://sonarcloud.io/summary/new_code?id=TwBlazor_TwBlazor)
  [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_TwBlazor&metric=coverage&token=6bbca7c19f7ba793b0e164a9805d518c9a175bfe)](https://sonarcloud.io/summary/new_code?id=TwBlazor_TwBlazor)
  [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_TwBlazor&metric=duplicated_lines_density&token=6bbca7c19f7ba793b0e164a9805d518c9a175bfe)](https://sonarcloud.io/summary/new_code?id=TwBlazor_TwBlazor)
  [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_TwBlazor&metric=ncloc&token=6bbca7c19f7ba793b0e164a9805d518c9a175bfe)](https://sonarcloud.io/summary/new_code?id=TwBlazor_TwBlazor)
  [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_TwBlazor&metric=sqale_index&token=6bbca7c19f7ba793b0e164a9805d518c9a175bfe)](https://sonarcloud.io/summary/new_code?id=TwBlazor_TwBlazor)
</p>

## Setup

1. Install the [TwBlazor NuGet package](https://github.com/TwBlazor/TwBlazor/pkgs/nuget/TwBlazor) in your Blazor project.
```pwsh
$ dotnet add package TwBlazor --version x.x.x
```
2. In your `App.razor` file, include two stylesheet links along with the default content. The first link is for TwBlazor's core styling. The second is for TwIcon, which uses Bootstrap Icons.
```razor
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- ... -->
    <!-- TwBlazor Styles -->
    <link rel="stylesheet" href="@Assets["_content/TwBlazor/css/twblazor.css"]" />
    <!-- Bootstrap Icons -->
    <link rel="stylesheet" href="@Assets["_content/TwBlazor/icons/bootstrap-icons.min.css"]">
    <!-- ... -->
</head>
```
At the bottom of the body, include the TwBlazor.js script:
```razor
<body>
    <Routes @rendermode="InteractiveServer" />
    <ReconnectModal />
    <!-- ... -->
    <!-- TwBlazor Scripts -->
    <script src="@Assets["_content/TwBlazor/js/twblazor.js"]"></script>
    <!-- ... -->
    <script src="@Assets["_framework/blazor.web.js"]"></script>
</body>
```
3. In `_Imports.razor`, add references to the following:
```razor
@using TwBlazor.Components
@using TwBlazor.Enums
@using TwBlazor
```
4. Add a new static class `Theme` to configure the TwBlazor theme, ensuring Tailwind CSS targets this file for class compilation. You can view the default theme in the [TwBlazor.Theme](./TwBlazor.Theme/Theme.cs) project for reference and use it as a base for your own custom theme.
5. Register the TwBlazor services in `Program.cs`:
```csharp
// Add TwBlazor services in program.cs
builder.Services.AddTwBlazor(Theme.DefaultTheme);
```
6. Add TwToastProvider to your `MainLayout.razor` or `App.razor`
```
@inherits LayoutComponentBase
@using TwBlazor
@using TwBlazor.Components

<TwDialogProvider />
<TwToastProvider />

@* layout body *@
@Body
```
7. You are now ready to use TwBlazor! 🎉

## Dependencies 

- [Tailwind CSS](https://tailwindcss.com/) - A utility-first CSS framework for styling components.
- [Bootstrap Icons](https://icons.getbootstrap.com/) - An open-source icon library used for our TwIcon component.