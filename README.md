<p align="center">
  <img src="./images/banner.svg" height="300" style="height: 300px;">
    [![Quality gate status](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)

    [![Bugs](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=bugs&token=6bbca7c19f7ba793b0e164a9805d518c9a175bfe)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)

    [![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)

    [![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)

    [![Coverage](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=coverage)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)

    [![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)

    [![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)

    [![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)
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