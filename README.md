<p align="center">
  <img src="./images/banner.svg" height="300" style="height: 300px;">
</p>
<p align="center">

[![Quality gate status](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=bugs)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=coverage)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=TwBlazor_twblazor&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=TwBlazor_twblazor)

</p>

<p align="center">
    <a href="https://twblazor.github.io/twblazor/" target="_blank">API Documentation (docfx)</a> &bullet; <a href="https://twblazor.com/" target="_blank">Component Documentation (twblazor.com)</a> &bullet; <a href="https://twblazor.com/get-started" target="_blank">Get Started</a>
</p>

## Why twblazor?

After years of working with Blazor, I have used many component frameworks, but never one that let me truly customise it to my liking without fighting the library to do so. That's why I built twblazor: instead of hiding styling behind opaque CSS or a confusing theming API, every component is styled with plain Tailwind CSS classes, so customising it is as straightforward as customising your own markup.

Combined with Tailwind's hot reload, this means you can fine-tune any button, switch, checkbox, or dropdown to match your brand, right down to the exact color, spacing, and radius, instantly seeing the results, all from a single typed theme file rather than hunting through component internals.

twblazor is, and always will be, fully open source under the MIT license. It's free to use in personal and commercial projects alike, and contributions, issues, and feedback from the community are always welcome.

This project is actively maintained, with new components, accessibility improvements, and documentation added on an ongoing basis.

## Setup

1. Install the [TwBlazor NuGet package](https://www.nuget.org/packages/TwBlazor) in your Blazor project.
```pwsh
$ dotnet add package TwBlazor --version 1.9.0
```
2. Head to the [Get Started guide](https://twblazor.com/get-started) for the rest of the setup - stylesheets, imports, providers, theming and dependency injection - covering both Interactive Server and WebAssembly Blazor Web Apps step by step.

## Dependencies 

- [Tailwind CSS](https://tailwindcss.com/) - A utility-first CSS framework for styling components.
- [Bootstrap Icons](https://icons.getbootstrap.com/) - An open-source icon library used for our TwIcon component.