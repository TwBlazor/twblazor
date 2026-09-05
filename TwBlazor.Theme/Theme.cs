using System.Reflection.Metadata;
using TwBlazor.Configuration;
using TwBlazor.Configuration.Color;
using TwBlazor.Configuration.Components;
using TwBlazor.Enums;

[assembly: MetadataUpdateHandler(typeof(TwBlazor.Theme.ThemeUpdateHandler))]

namespace TwBlazor.Theme;

public static class Theme
{
    #region CodeExample GetStartedTheme
    public static TwBlazorTheme CreateDefaultTheme()
    {
        var position = new TwPosition
        {
            Center = "items-center justify-center",
            CenterLeft = "items-center justify-start",
            CenterRight = "items-center justify-end",
            TopCenter = "items-start justify-center",
            TopLeft = "items-start justify-start",
            TopRight = "items-start justify-end",
            BottomCenter = "items-end justify-center",
            BottomLeft = "items-end justify-start",
            BottomRight = "items-end justify-end",
        };

        var text = new TwTextColor
        {
            Light =
            {
                Primary = "text-purple-200",
                Accent = "text-fuchsia-200",
                Success = "text-green-200",
                Danger = "text-red-200",
                Warning = "text-yellow-200",
                Info = "text-blue-200",
                Light = "text-white",
                Dark = "text-gray-200"
            },
            Medium =
            {
                Primary = "text-purple-600",
                Accent = "text-fuchsia-600",
                Success = "text-green-600",
                Danger = "text-red-600",
                Warning = "text-yellow-600",
                Info = "text-blue-600",
                Light = "text-gray-100",
                Dark = "text-gray-950",
            },
            Dark =
            {
                Primary = "text-purple-900",
                Accent = "text-fuchsia-900",
                Success = "text-green-900",
                Danger = "text-red-900",
                Warning = "text-yellow-900",
                Info = "text-blue-900",
                Light = "text-gray-300",
                Dark = "text-gray-900"
            }
        };

        var darkText = new TwTextColor
        {
            Light =
            {
                Primary = "dark:text-purple-200",
                Accent = "dark:text-fuchsia-200",
                Success = "dark:text-green-200",
                Danger = "dark:text-red-200",
                Warning = "dark:text-yellow-200",
                Info = "dark:text-blue-200",
                Dark = "dark:text-gray-200"
            },
            Medium =
            {
                Primary = "dark:text-purple-600",
                Accent = "dark:text-fuchsia-600",
                Success = "dark:text-green-600",
                Danger = "dark:text-red-600",
                Warning = "dark:text-yellow-600",
                Info = "dark:text-blue-600",
                Light = "dark:text-white",
                Dark = "dark:text-gray-950",
            },
            Dark =
            {
                Primary = "dark:text-purple-900",
                Accent = "dark:text-fuchsia-900",
                Success = "dark:text-green-900",
                Danger = "dark:text-red-900",
                Warning = "dark:text-yellow-900",
                Info = "dark:text-blue-900",
                Dark = "dark:text-gray-900"
            }
        };

        var borderColors = new TwBlazorPalette
        {
            Primary = "border-purple-600 dark:border-purple-500",
            Accent = "border-fuchsia-600 dark:border-fuchsia-400",
            Success = "border-green-600 dark:border-green-400",
            Danger = "border-red-600 dark:border-red-400",
            Warning = "border-yellow-600 dark:border-yellow-400",
            Info = "border-blue-600 dark:border-blue-400",
            Light = "border-gray-100 dark:border-gray-700",
            Dark = "border-gray-900 dark:border-gray-700"
        };

        var hoverColors = new TwBlazorPalette
        {
            Primary = "hover:bg-purple-50 dark:hover:bg-purple-900/20",
            Accent = "hover:bg-fuchsia-50 dark:hover:bg-fuchsia-900/20",
            Success = "hover:bg-green-50 dark:hover:bg-green-900/20",
            Danger = "hover:bg-red-50 dark:hover:bg-red-900/20",
            Warning = "hover:bg-yellow-50 dark:hover:bg-yellow-900/20",
            Info = "hover:bg-blue-50 dark:hover:bg-blue-900/20",
            Light = "hover:bg-gray-100/10",
            Dark = "hover:bg-gray-900/10"
        };

        var background = new TwBackgroundColor
        {
            Light = new()
            {
                Primary = $"bg-purple-200",
                Accent = $"bg-fuchsia-200",
                Success = $"bg-green-200",
                Danger = $"bg-red-200",
                Warning = $"bg-yellow-200",
                Info = $"bg-blue-200",
                Light = $"bg-white",
                Dark = $"bg-gray-200"
            },
            Medium = new()
            {
                Primary = $"bg-purple-600",
                Accent = $"bg-fuchsia-600",
                Success = $"bg-green-600",
                Danger = $"bg-red-600",
                Warning = $"bg-yellow-600",
                Info = $"bg-blue-600",
                Light = $"bg-gray-100",
                Dark = $"bg-gray-600",
            },
            Dark = new()
            {
                Primary = $"bg-purple-900",
                Accent = $"bg-fuchsia-900",
                Success = $"bg-green-900",
                Danger = $"bg-red-900",
                Warning = $"bg-yellow-900",
                Info = $"bg-blue-900",
                Light = $"bg-gray-300",
                Dark = $"bg-gray-900"
            }
        };

        var darkBackground = new TwBackgroundColor
        {
            Light = new()
            {
                Primary = $"dark:bg-purple-200",
                Accent = $"dark:bg-fuchsia-200",
                Success = $"dark:bg-green-200",
                Danger = $"dark:bg-red-200",
                Warning = $"dark:bg-yellow-200",
                Info = $"dark:bg-blue-200",
                Dark = $"dark:bg-gray-200",
            },
            Medium = new()
            {
                Primary = $"dark:bg-purple-600",
                Accent = $"dark:bg-fuchsia-600",
                Success = $"dark:bg-green-600",
                Danger = $"dark:bg-red-600",
                Warning = $"dark:bg-yellow-600",
                Info = $"dark:bg-blue-600",
                Light = $"dark:bg-white",
                Dark = $"dark:bg-gray-600",
            },
            Dark = new()
            {
                Primary = $"dark:bg-purple-900",
                Accent = $"dark:bg-fuchsia-900",
                Success = $"dark:bg-green-900",
                Danger = $"dark:bg-red-900",
                Warning = $"dark:bg-yellow-900",
                Info = $"dark:bg-blue-900",
                Dark = $"dark:bg-gray-900",
            }
        };

        var checkBoxRadioButtonColors = new TwBlazorPalette()
        {
            Primary = "checked:bg-purple-600 checked:border-purple-600 dark:checked:bg-purple-500 dark:checked:border-purple-500",
            Accent = "checked:bg-fuchsia-600 checked:border-fuchsia-600 dark:checked:bg-fuchsia-500 dark:checked:border-fuchsia-500",
            Success = "checked:bg-green-600 checked:border-green-600 dark:checked:bg-green-500 dark:checked:border-green-500",
            Danger = "checked:bg-red-600 checked:border-red-600 dark:checked:bg-red-500 dark:checked:border-red-500",
            Warning = "checked:bg-yellow-600 checked:border-yellow-600 dark:checked:bg-yellow-500 dark:checked:border-yellow-500",
            Info = "checked:bg-blue-600 checked:border-blue-600 dark:checked:bg-blue-500 dark:checked:border-blue-500",
            Light = "checked:bg-white checked:border-gray-900 dark:checked:bg-white dark:checked:border-gray-500",
            Dark = "checked:bg-gray-900 checked:border-gray-900 dark:checked:bg-gray-900 dark:checked:border-gray-900"
        };

        // Neutral (non-semantic) surface tokens shared by any component that just wants "the"
        // card/dialog/popover look - reused below instead of retyping the same literal classes.
        // Add more weights here as needed; see TwSurfacePalette's remarks for why each stays a
        // single full class-name string.
        var neutralSurface = new TwSurfacePalette
        {
            Background = "bg-white dark:bg-gray-800",
            BackgroundSubtle = "bg-gray-50 dark:bg-gray-900",
            Border = "border-gray-200 dark:border-gray-700",
            BorderSubtle = "border-gray-100 dark:border-gray-700",
            Hover = "hover:bg-gray-100 dark:hover:bg-gray-700"
        };

        // Neutral (non-semantic) text tokens - same idea as neutralSurface, but for the default
        // body/heading text color and its quieter variants. "Heading" standardizes a couple of
        // components that had drifted to text-gray-900 instead of the gray-950 used everywhere else.
        var neutralText = new TwNeutralTextPalette
        {
            Heading = "text-gray-950 dark:text-white",
            Secondary = "text-gray-700 dark:text-gray-300",
            Muted = "text-gray-600 dark:text-gray-400",
            Subtle = "text-gray-500 dark:text-gray-400"
        };

        // "opacity-38" was a typo for "opacity-40" that had spread to several components (and even
        // showed up alongside the correct value within a couple of them) - reusing this one token
        // keeps every disabled state at the intended 40%.
        const string disabledOpacity = "opacity-40";

        // Small layout/shape tokens repeated identically across otherwise-unrelated components -
        // reused below so the value only needs to change in one place, and any future drift (like
        // the disabled-opacity typo above, or Pagination's stray dark:border-gray-800) shows up as a
        // single wrong assignment here instead of being scattered across the file.
        const string defaultGap = "gap-2";
        const string comfortablePadding = "p-4";
        const string compactPadding = "p-2";
        const string interactiveRowPadding = "px-4 py-3";
        const string roundedLg = "rounded-lg";
        const string roundedTopLg = "rounded-t-lg";
        const string pointerCursor = "cursor-pointer";

        return new TwBlazorTheme
        {
            Position = position,
            Colors = new()
            {
                TextColors = text,
                DarkTextColors = darkText,
                HoverColors = hoverColors,
                BorderColors = borderColors,
                LightBackground = background,
                DarkBackground = darkBackground,
                FocusRingBase = "focus-visible:ring-2",
                FocusColors = new()
                {
                    Primary = "focus:ring-purple-500/20",
                    Accent = "focus:ring-fuchsia-500/20",
                    Success = "focus:ring-green-500/20",
                    Danger = "focus:ring-red-500/20",
                    Warning = "focus:ring-yellow-500/20",
                    Info = "focus:ring-blue-500/20",
                    Light = "focus:ring-white/20",
                    Dark = "focus:ring-gray-900/20"
                },
                SurfaceColors = new()
                {
                    Filled = new()
                    {
                        Primary = $"{background.Medium.Primary} hover:bg-purple-700 active:bg-purple-800 {text.Medium.Light}",
                        Accent = $"{background.Medium.Accent} hover:bg-fuchsia-700 active:bg-fuchsia-800 {text.Medium.Light}",
                        Success = $"{background.Medium.Success} hover:bg-green-700 active:bg-green-800 {text.Medium.Light}",
                        Danger = $"{background.Medium.Danger} hover:bg-red-700 active:bg-red-800 {text.Medium.Light}",
                        Warning = $"{background.Medium.Warning} hover:bg-yellow-600 active:bg-yellow-700 {text.Medium.Light}",
                        Info = $"{background.Medium.Info} hover:bg-blue-700 active:bg-blue-800 {text.Medium.Light}",
                        Light = $"{background.Medium.Light} hover:bg-gray-50 active:bg-gray-100 {text.Medium.Dark}",
                        Dark = $"{background.Dark.Dark} hover:bg-gray-800 active:bg-gray-700 {text.Medium.Light}",
                    },
                    Outlined = new()
                    {
                        Primary = $"{text.Medium.Primary} bg-transparent {hoverColors.Primary} border {borderColors.Primary}",
                        Accent = $"{text.Medium.Accent} bg-transparent {hoverColors.Accent} border {borderColors.Accent}",
                        Success = $"{text.Medium.Success} bg-transparent {hoverColors.Success} border {borderColors.Success}",
                        Danger = $"{text.Medium.Danger} bg-transparent {hoverColors.Danger} border {borderColors.Danger}",
                        Warning = $"{text.Medium.Warning} bg-transparent {hoverColors.Warning} border {borderColors.Warning}",
                        Info = $"{text.Medium.Info} bg-transparent {hoverColors.Info} border {borderColors.Info}",
                        Light = $"{text.Light.Dark} bg-transparent {hoverColors.Light} border {borderColors.Light}",
                        Dark = $"{text.Medium.Dark} bg-transparent {hoverColors.Dark} border {borderColors.Dark}",
                    },
                    Text = new()
                    {
                        Primary = $"{text.Medium.Primary} bg-transparent {hoverColors.Primary}",
                        Accent = $"{text.Medium.Accent} bg-transparent {hoverColors.Accent}",
                        Success = $"{text.Medium.Success} bg-transparent {hoverColors.Success}",
                        Danger = $"{text.Medium.Danger} bg-transparent {hoverColors.Danger}",
                        Warning = $"{text.Medium.Warning} bg-transparent {hoverColors.Warning}",
                        Info = $"{text.Medium.Info} bg-transparent {hoverColors.Info}",
                        Light = $"{text.Light.Dark} bg-transparent {hoverColors.Light}",
                        Dark = $"{text.Dark.Dark} bg-transparent {hoverColors.Dark}",
                    },
                },
                NeutralSurface = neutralSurface,
                NeutralText = neutralText
            },
            Shadows = new()
            {
                None = "shadow-none",
                Sm = "shadow-sm",
                Md = "shadow",
                Lg = "shadow-lg",
                HoverSm = "hover:shadow-sm",
                HoverMd = "hover:shadow",
                HoverLg = "hover:shadow-xl",
                ActiveMd = "active:shadow",
                DefaultShadow = Shadow.Sm
            },
            Rounded = new()
            {
                None = "rounded-none",
                Sm = "rounded-sm",
                Md = "rounded",
                Lg = roundedLg,
                Full = "rounded-full",
                DefaultRounded = Rounded.Lg,
                RoundedTop = new()
                {
                    None = "rounded-t-none",
                    Sm = "rounded-t-sm",
                    Md = "rounded-t",
                    Lg = roundedTopLg,
                    Full = "rounded-t-full"
                },
                RoundedBottom = new()
                {
                    None = "rounded-b-none",
                    Sm = "rounded-b-sm",
                    Md = "rounded-b",
                    Lg = "rounded-b-lg",
                    Full = "rounded-b-full"
                },
                RoundedStart = new()
                {
                    None = "rounded-s-none",
                    Sm = "rounded-s-sm",
                    Md = "rounded-s",
                    Lg = "rounded-s-lg",
                    Full = "rounded-s-full"
                },
                RoundedEnd = new()
                {
                    None = "rounded-e-none",
                    Sm = "rounded-e-sm",
                    Md = "rounded-e",
                    Lg = "rounded-e-lg",
                    Full = "rounded-e-full"
                }
            },
            // Tip - to reduce your own tailwind css files you only have to declare the components you use.
            Components =
            [
                new TwAlertTheme
                {
                    Colors = new()
                    {
                        Primary = $"{background.Light.Primary} {darkBackground.Dark.Primary} {text.Medium.Primary} {darkText.Light.Primary} border-l-4 {borderColors.Primary}",
                        Accent = $"{background.Light.Accent} {darkBackground.Dark.Accent} {text.Medium.Accent} {darkText.Light.Accent} border-l-4 {borderColors.Accent}",
                        Success = $"{background.Light.Success} {darkBackground.Dark.Success} {text.Medium.Success} {darkText.Light.Success} border-l-4 {borderColors.Success}",
                        Danger = $"{background.Light.Danger} {darkBackground.Dark.Danger} {text.Medium.Danger} {darkText.Light.Danger} border-l-4 {borderColors.Danger}",
                        Warning = $"{background.Light.Warning} {darkBackground.Dark.Warning} {text.Medium.Warning} {darkText.Light.Warning} border-l-4 {borderColors.Warning}",
                        Info = $"{background.Light.Info} {darkBackground.Dark.Info} {text.Medium.Info} {darkText.Light.Info} border-l-4 {borderColors.Info}",
                        Light = $"{background.Light.Light} {darkBackground.Dark.Light} {text.Medium.Dark} {darkText.Medium.Dark} border-l-4 {borderColors.Light}",
                        Dark = $"{background.Light.Dark} {darkBackground.Dark.Dark} {text.Medium.Dark} {darkText.Light.Dark} border-l-4 {borderColors.Dark}",
                    },
                },
                new TwBreadcrumbTheme
                {
                    List = "inline-flex flex-wrap gap-3",
                    Item = "flex items-center gap-1",
                    Separator = "font-bold text-gray-300",
                    Label = "wrap-break-word"
                },
                new TwButtonTheme
                {
                    Base = $"{position.Center} transition-colors duration-200 text-sm inline-flex h-8 overflow-hidden focus:outline-none focus-visible:outline-none touch-manipulation",
                    Padding = "px-6",
                    DensePadding = "px-3 py-1.5",
                    IconButton = $"{position.Center} rounded-full flex focus:outline-none h-8 w-8 text-sm/6",
                    Typography = "font-medium",
                    Uppercase = "uppercase tracking-wide",
                    DisabledCursor = "cursor-not-allowed",
                    ReadonlyCursor = "cursor-default",
                    DefaultCursor = pointerCursor,
                    DisabledFilled = "bg-gray-900/15 dark:bg-white/15 text-gray-900/40 dark:text-white/40 cursor-not-allowed shadow-none",
                    DisabledOutlined = "border border-gray-900/15 dark:border-white/15 text-gray-900/40 dark:text-white/40 bg-transparent cursor-not-allowed",
                    DisabledText = "text-gray-900/40 dark:text-white/40 bg-transparent cursor-not-allowed"
                },
                new TwCardTheme
                {
                    Container = "px-6 py-5",
                    Bordered = $"border {neutralSurface.Border}",
                    Title = $"text-lg font-semibold {neutralText.Heading} wrap-break-word"
                },
                new TwCheckboxTheme
                {
                    Colors = checkBoxRadioButtonColors,
                    Base = $"peer h-5 w-5 {pointerCursor} appearance-none border-2 border-gray-300 dark:border-gray-600 transition-colors duration-200 ease-in-out",
                    Disabled = $"{disabledOpacity} cursor-not-allowed",
                    Hover = $"{pointerCursor} hover:border-gray-600 dark:hover:border-gray-300",
                    LabelBase = $"flex items-center relative select-none min-h-[24px] {defaultGap}",
                    LabelInteractiveCursor = pointerCursor,
                    LabelNonInteractiveCursor = "pointer-events-none",
                    LabelDisabled = disabledOpacity,
                    IconWrapper = "absolute opacity-0 peer-checked:opacity-100 translate-x-1/4"
                },
                new TwChipTheme
                {
                    Base = $"{position.Center} transition-colors duration-200 inline-flex gap-1.5 font-medium shadow-sm touch-manipulation",
                    CloseButton = $"{position.Center} flex hover:bg-gray-100/20 dark:hover:bg-gray-800/20 rounded-full w-4 h-4 text-center",
                    Sm = "text-xs px-2 py-0.5 h-6",
                    Md = "text-sm px-2.5 py-1 h-8",
                    Lg = "text-sm px-3 py-1.5 h-10"
                },
                new TwCollapseTheme
                {
                    Container = $"tw-collapse border {neutralSurface.Border}",
                    Trigger = $"flex w-full items-center justify-between {defaultGap} {interactiveRowPadding} text-left font-medium {neutralText.Heading} transition-colors duration-200 hover:bg-gray-50 dark:hover:bg-gray-700/50 focus:outline-none focus-visible:ring-2 focus-visible:ring-inset focus-visible:ring-blue-500 touch-manipulation",
                    Icon = "ml-auto h-4 w-4 shrink-0 transition-transform duration-300 flex",
                    IconOpen = "rotate-180",
                    Content = $"overflow-hidden border-t {neutralSurface.Border} transition-colors duration-300"
                },
                new TwColorPickerTheme
                {
                    Swatch = "h-7 w-7 rounded-md ring-1 ring-inset ring-black/10 dark:ring-white/15 shadow-sm flex-shrink-0 transition-[box-shadow,opacity] duration-200",
                    SwatchDisabled = disabledOpacity,
                    SwatchHover = "hover:ring-black/20 dark:hover:ring-white/25",
                    InputContainer = $"flex items-center {defaultGap}",
                    DialogPosition = "absolute top-full left-0 mt-2 z-50",
                    DialogSurface = $"tw-color-picker-dialog {neutralSurface.Background} rounded-xl shadow-xl ring-1 ring-gray-200 dark:ring-gray-700 {comfortablePadding} w-64",
                    PreviewSwatch = $"w-11 h-11 {roundedLg} ring-1 ring-inset ring-black/10 dark:ring-white/15 shadow-sm flex-shrink-0",
                    SelectorSquare = $"relative w-full h-48 {roundedLg} overflow-hidden ring-1 ring-inset ring-black/10 dark:ring-white/10 cursor-crosshair touch-none",
                    SelectorThumb = "absolute w-3.5 h-3.5 rounded-full border-2 border-white ring-1 ring-black/20 shadow-md pointer-events-none",
                    SliderTrack = "w-full h-2.5 rounded-full overflow-hidden ring-1 ring-inset ring-black/10 dark:ring-white/10 pointer-events-none",
                    SliderThumb = "absolute top-1/2 w-4 h-4 rounded-full border-2 border-white ring-1 ring-black/20 shadow-md pointer-events-none",
                    AlphaLabel = $"text-xs font-medium {neutralText.Secondary} w-10",
                    ActionBar = $"flex justify-end {defaultGap} pt-3 border-t {neutralSurface.BorderSubtle}"
                },
                new TwDatePickerTheme
                {
                    Header = $"{neutralSurface.Background} text-center font-medium {roundedTopLg} border-b {neutralSurface.Border}",

                    WeekdaysHeader = $"{position.Center} {text.Medium.Primary} {darkText.Light.Primary} h-8 flex text-xs font-semibold tracking-wide ",
                    Base = $"absolute {pointerCursor} border-0 text-center text-sm py-2 font-medium transition-colors duration-200 top-full left-0 flex flex-row md:flex-row items-center z-50 mt-1 {neutralSurface.Background} {compactPadding} border {neutralSurface.Border}",
                    ActiveClass = "bg-purple-50 dark:bg-purple-500/30",
                    ButtonClass = $"{compactPadding} h-8 flex items-center w-full justify-center"
                },
                new TwGroupsTheme
                {
                    Gap = defaultGap,
                    FieldsetBase = "border-none p-0 m-0",
                    HorizontalLayout = "flex flex-row flex-wrap",
                    VerticalLayout = "flex flex-col",
                    CheckboxGroupDisabled = $"{disabledOpacity} pointer-events-none",
                    RadioGroupDisabled = $"{disabledOpacity} pointer-events-none",
                    ButtonGroupBase = "inline-flex",
                    ButtonGroupVertical = "flex-col",
                    ButtonGroupHorizontal = "flex-row",
                    ButtonGroupFullWidth = "w-full",
                    ButtonGroupFullWidthRow = "[&>*]:flex-1",
                    ChipGroupBase = "flex flex-wrap",
                    ChipGroupAlignStart = "justify-start",
                    ChipGroupAlignCenter = "justify-center",
                    ChipGroupAlignEnd = "justify-end"
                },
                new TwDialogTheme
                {
                    Backdrop = $"fixed inset-0 z-[110] flex {comfortablePadding} bg-gray-900/50 dark:bg-black/70 overflow-y-auto overscroll-contain",
                    Surface = $"relative flex w-full flex-col {neutralText.Heading} {neutralSurface.Background} max-h-[calc(100vh-2rem)]",
                    Header = $"flex items-center justify-between gap-4 px-6 py-4 border-b {neutralSurface.Border} flex-shrink-0",
                    Title = "text-lg font-semibold",
                    CloseButton = $"transition-colors duration-200 {roundedLg} p-1 {neutralText.Subtle} {neutralSurface.Hover} hover:text-gray-700 dark:hover:text-gray-200 flex-shrink-0 focus:outline-none focus:ring-2 focus:ring-offset-1 focus:ring-offset-transparent focus:ring-blue-500/20",
                    Content = "px-6 py-4 overflow-y-auto",
                    FullScreen = "w-full h-full max-w-none",
                    FullWidth = "w-full",
                    SmallWidth = "sm:max-w-lg",
                    MediumWidth = "sm:max-w-xl",
                    LargeWidth = "sm:max-w-3xl"
                },
                new TwInputTheme
                {
                    DefaultInputVariant = InputVariant.Filled,
                    TextfieldBase = $"w-full min-w-0 max-w-full {neutralText.Heading} transition-colors duration-200 text-base block ease-in-out placeholder:text-gray-500 dark:placeholder:text-gray-400 focus:outline-none",
                    SelectBase = $"w-full {neutralText.Heading} transition-colors duration-200 appearance-none text-base block pr-10 py-2 focus:outline-none bg-[url('data:image/svg+xml;charset=utf-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20fill%3D%22none%22%20viewBox%3D%220%200%2020%2020%22%3E%3Cpath%20stroke%3D%22%236b7280%22%20stroke-linecap%3D%22round%22%20stroke-linejoin%3D%22round%22%20stroke-width%3D%221.5%22%20d%3D%22m6%208%204%204%204-4%22%2F%3E%3C%2Fsvg%3E')] bg-[length:1.5em_1.5em] bg-[right_0.5rem_center] bg-no-repeat",
                    InputLegendBase = $"text-base font-medium {neutralText.Secondary} mb-3",
                    LabelBase = $"block mb-2 text-xs font-normal tracking-wide {neutralText.Muted}",
                    OutlinedBorder = "border-1 border-gray-300 dark:border-gray-600",
                    FilledBorder = "border-b-2 border-gray-300 dark:border-gray-600",
                    FocusBorder = "focus:border-purple-600 dark:focus:border-purple-500",
                    FilledBackgroundColor = "bg-gray-100 dark:bg-gray-900/85"
                },
                new TwPaginationTheme
                {
                    Base = $"{position.Center} flex h-8 leading-tight select-none border-1 {neutralSurface.Border} px-3 mx-0.5 tabular-nums",
                    ActiveButton = $"{text.Medium.Primary} {darkText.Medium.Primary} {background.Light.Primary} {darkBackground.Light.Primary} font-bold hover:bg-purple-100 {pointerCursor}",
                    Buttons = $"{text.Medium.Dark} {background.Medium.Light} {neutralSurface.Hover} dark:bg-gray-800 dark:text-gray-400 dark:hover:text-white {pointerCursor}"
                },
                new TwProgressTheme
                {
                    Colors = new()
                    {
                        Primary = "[&::-webkit-progress-value]:bg-purple-600 dark:[&::-webkit-progress-value]:bg-purple-500 [&::-moz-progress-bar]:bg-purple-600 dark:[&::-moz-progress-bar]:bg-purple-500",
                        Accent = "[&::-webkit-progress-value]:bg-fuchsia-600 dark:[&::-webkit-progress-value]:bg-fuchsia-500 [&::-moz-progress-bar]:bg-fuchsia-600 dark:[&::-moz-progress-bar]:bg-fuchsia-500",
                        Success = "[&::-webkit-progress-value]:bg-green-600 dark:[&::-webkit-progress-value]:bg-green-500 [&::-moz-progress-bar]:bg-green-600 dark:[&::-moz-progress-bar]:bg-green-500",
                        Danger = "[&::-webkit-progress-value]:bg-red-600 dark:[&::-webkit-progress-value]:bg-red-500 [&::-moz-progress-bar]:bg-red-600 dark:[&::-moz-progress-bar]:bg-red-500",
                        Warning = "[&::-webkit-progress-value]:bg-yellow-500 dark:[&::-webkit-progress-value]:bg-yellow-400 [&::-moz-progress-bar]:bg-yellow-500 dark:[&::-moz-progress-bar]:bg-yellow-400",
                        Info = "[&::-webkit-progress-value]:bg-blue-600 dark:[&::-webkit-progress-value]:bg-blue-500 [&::-moz-progress-bar]:bg-blue-600 dark:[&::-moz-progress-bar]:bg-blue-500",
                        Light = "[&::-webkit-progress-value]:bg-white dark:[&::-webkit-progress-value]:bg-white [&::-moz-progress-bar]:bg-blue-600 dark:[&::-moz-progress-bar]:bg-blue-500",
                        Dark = "[&::-webkit-progress-value]:bg-gray-900 dark:[&::-webkit-progress-value]:bg-gray-900 [&::-moz-progress-bar]:bg-blue-600 dark:[&::-moz-progress-bar]:bg-blue-500"
                    },
                    Base = "block w-full min-w-0 appearance-none overflow-hidden rounded-full border-none bg-gray-200 dark:bg-gray-700 indeterminate:animate-pulse [&::-webkit-progress-bar]:rounded-full [&::-webkit-progress-bar]:bg-transparent [&::-webkit-progress-value]:rounded-full [&::-webkit-progress-value]:transition-[width] [&::-webkit-progress-value]:duration-300 [&::-moz-progress-bar]:rounded-full [&::-moz-progress-bar]:transition-[width] [&::-moz-progress-bar]:duration-300",
                    Small = "h-1.5",
                    Medium = "h-2.5",
                    Large = "h-4"
                },
                new TwTabTheme
                {
                    TabBase = "relative tracking-wide font-medium text-sm transition-colors duration-300",
                    TabPadding = "py-5 px-6",
                    TabDensePadding = "px-4",
                    ActiveIndicator = "after:absolute after:bottom-0 after:left-0 after:right-0 after:h-0.5 after:bg-current after:scale-x-100 after:transition-transform after:duration-300",
                    InactiveIndicator = "hover:text-gray-900 dark:hover:text-gray-200 after:absolute after:bottom-0 after:left-0 after:right-0 after:h-0.5 after:bg-current after:scale-x-0 hover:after:scale-x-100 after:transition-transform after:duration-300",
                    DisabledTab = $"{disabledOpacity} cursor-not-allowed",
                    TabListContainer = $"flex flex-wrap border-t border-l border-r border-b-2 {neutralSurface.Border} shadow-sm",
                    PanelContainer = $"p-6 border-l border-r border-b {neutralSurface.Border}",
                    Background = neutralSurface.Background,
                },
                new TwSkeletonTheme
                {
                    Base = "relative overflow-hidden bg-gray-200 dark:bg-gray-700 block",
                    Text = "w-full h-4",
                    Circle = "size-12",
                    Rectangle = "w-full h-24",
                    MeasuringWrapper = "invisible",
                    Pulse = "animate-pulse",
                    Wave = "tw-skeleton-wave"
                },
                new TwSidebarTheme
                {
                    Navbar = $"{background.Dark.Primary} {darkBackground.Dark.Primary} shadow-sm p-3 w-full flex items-center flex-shrink-0 z-40 h-[56px]",
                    Sidebar = $"transition-transform duration-200 shadow-sm h-dvh w-64 flex-shrink-0 {neutralSurface.Background} {comfortablePadding} z-[100] ease-in-out overflow-auto overscroll-contain",
                    NavigationItemBase = $"{defaultGap} min-w-0 transition-colors duration-200 flex items-center text-gray-700 dark:text-gray-200 {neutralSurface.Hover} {interactiveRowPadding} text-sm focus:outline-none focus-visible:ring-inset focus-visible:ring-2 focus-visible:ring-blue-500 {pointerCursor}",
                    NavigationItemActive = "bg-gray-200/80 dark:bg-gray-900/30 text-gray-600 dark:text-white font-semibold",
                    NavigationDropdownContainer = "bg-gray-50 dark:bg-gray-900/30",
                    MainContent = "w-full flex-1 overflow-y-auto transition-[margin] duration-200 ease-in-out left-0",
                    MainContentRoot = "h-dvh w-full flex flex-col transition-[margin] duration-300 ease-in-out dark:bg-gray-900 bg-transparent dark:text-white overflow-x-hidden"
                },
                new TwRadioButtonTheme
                {
                    Colors = checkBoxRadioButtonColors,
                    Base = $"peer h-5 w-5 {pointerCursor} appearance-none rounded-full border-2 border-gray-300 dark:border-gray-600 transition-colors duration-200 ease-in-out",
                    Disabled = $"{disabledOpacity} cursor-not-allowed",
                    Hover = $"{pointerCursor} hover:border-gray-600 dark:hover:border-gray-300",
                    LabelBase = $"flex items-center relative select-none min-h-[24px] {defaultGap}",
                    LabelInteractiveCursor = pointerCursor,
                    LabelNonInteractiveCursor = "pointer-events-none",
                    LabelDisabled = disabledOpacity,
                    IconWrapper = "absolute left-0 w-5 h-5 flex items-center justify-center opacity-0 peer-checked:opacity-100 pointer-events-none"
                },
                new TwSliderTheme
                {
                    Colors = new()
                    {
                        Primary = $"{background.Medium.Primary} {darkBackground.Medium.Primary}",
                        Accent = $"{background.Medium.Accent} {darkBackground.Medium.Accent}",
                        Success = $"{background.Medium.Success} {darkBackground.Medium.Success}",
                        Danger = $"{background.Medium.Danger} {darkBackground.Medium.Danger}",
                        Warning = $"{background.Medium.Warning} {darkBackground.Medium.Warning}",
                        Info = $"{background.Medium.Info} {darkBackground.Medium.Info}",
                        Light = $"{background.Medium.Light} {darkBackground.Medium.Light}",
                        Dark = $"{background.Dark.Dark} {darkBackground.Dark.Dark}",
                    },
                    Wrapper = "relative flex items-center w-full h-6 select-none",
                    Base = $"peer absolute inset-0 z-20 w-full h-full m-0 appearance-none bg-transparent {pointerCursor} focus:outline-none focus-visible:outline-none touch-manipulation",
                    Track = "pointer-events-none absolute inset-x-0 top-1/2 h-1.5 -translate-y-1/2 rounded-full bg-gray-200 dark:bg-gray-700 overflow-hidden",
                    Fill = "h-full",
                    Thumb = "pointer-events-none absolute top-1/2 z-10 size-5 -translate-x-1/2 -translate-y-1/2 rounded-full bg-white dark:bg-gray-100 border-2 shadow-md ring-1 ring-black/5 transition-transform duration-100 ease-out peer-hover:scale-110 peer-active:scale-95",
                    Bubble = "pointer-events-none absolute bottom-full z-10 -translate-x-1/2 mb-2 whitespace-nowrap rounded-md bg-gray-900 dark:bg-gray-700 px-2 py-1 text-xs font-medium text-white shadow-lg opacity-0 scale-95 transition-[opacity,transform] duration-100 ease-out peer-hover:opacity-100 peer-hover:scale-100 peer-focus-visible:opacity-100 peer-focus-visible:scale-100 tabular-nums"
                },
                new TwSwitchTheme
                {
                    Colors = new()
                    {
                        Primary = "peer-checked:bg-purple-600 dark:peer-checked:bg-purple-500",
                        Accent = "peer-checked:bg-fuchsia-600 dark:peer-checked:bg-fuchsia-500",
                        Success = "peer-checked:bg-green-600 dark:peer-checked:bg-green-500",
                        Danger = "peer-checked:bg-red-600 dark:peer-checked:bg-red-500",
                        Warning = "peer-checked:bg-yellow-600 dark:peer-checked:bg-yellow-500",
                        Info = "peer-checked:bg-blue-600 dark:peer-checked:bg-blue-500",
                        Light = "peer-checked:bg-white dark:peer-checked:bg-gray-300",
                        Dark = "peer-checked:bg-gray-900 dark:peer-checked:bg-gray-800",
                    },
                    Switch = "absolute top-1/2 start-0.5 -translate-y-1/2 size-5 bg-gray-100 rounded-full shadow-lg transition-transform duration-300 ease-in-out peer-checked:translate-x-full peer-checked:shadow-lg",
                    Track = $"absolute inset-0 bg-gray-300 dark:bg-gray-600 rounded-full transition-[background-color,opacity] duration-300 ease-in-out peer-disabled:{disabledOpacity} peer-disabled:pointer-events-none shadow-inner",
                    Base = "peer sr-only",
                    LabelBase = $"inline-flex items-center {defaultGap} select-none",
                    LabelInteractiveCursor = pointerCursor,
                    LabelNonInteractiveCursor = "pointer-events-none",
                    LabelDisabled = disabledOpacity
                },
                new TwSpinnerTheme
                {
                    Colors = new()
                    {
                        Primary = "border-t-purple-600 dark:border-t-purple-500",
                        Accent = "border-t-fuchsia-600 dark:border-t-fuchsia-500",
                        Success = "border-t-green-600 dark:border-t-green-500",
                        Danger = "border-t-red-600 dark:border-t-red-500",
                        Warning = "border-t-yellow-500 dark:border-t-yellow-400",
                        Info = "border-t-blue-600 dark:border-t-blue-500",
                        Light = "",
                        Dark = "",
                    },
                    Wrapper = $"inline-flex items-center {defaultGap}",
                    Base = "inline-block rounded-full animate-spin",
                    Track = neutralSurface.Border,
                    Small = "size-4 border-2",
                    Medium = "size-8 border-5",
                    Large = "size-12 border-5",
                    Label = $"text-sm {neutralText.Muted}"
                },
                new TwTimePickerTheme
                {
                    PickerRoot = "relative",
                    IconWrapper = $"absolute top-0 start-0 h-10.5 flex {position.Center} ps-2 w-10 {pointerCursor}",
                    IconGlyph = $"{neutralText.Subtle} h-5 w-5",
                    TextfieldPadding = "pl-10 pr-3",
                    PanelPosition = $"absolute top-full left-0 z-50 mt-2 {roundedLg} shadow-lg",
                    BodySurface = $"{neutralSurface.Background} {comfortablePadding} shadow-xl {roundedLg} border {neutralSurface.Border} text-center font-medium dark:text-white",
                    BodyRoot = "",
                    BodyInner = $"{compactPadding} text-center font-medium dark:text-white",
                    ContentRow = $"flex {position.Center} gap-3",
                    Column = "flex flex-col items-center gap-1",
                    StepButton = $"{pointerCursor} {neutralText.Subtle} hover:text-purple-600 dark:hover:text-purple-400",
                    NumberWrapper = "flex items-center justify-center",
                    NumberInput = $"w-12 bg-transparent text-center text-lg font-semibold {neutralText.Heading} border-b-2 border-gray-300 dark:border-gray-600 transition-colors duration-200 py-1 focus:outline-none",
                    Separator = $"text-lg font-semibold {neutralText.Subtle} px-1 self-center",
                    AmPmWrapper = "flex items-center ml-2",
                    AmPmButtonClass = "min-w-12"
                },
                new TwTableTheme
                {
                    Base = $"{neutralText.Heading} w-full text-sm text-left rtl:text-right overflow-hidden",
                    Bordered = $"border {neutralSurface.Border} shadow-sm",
                    Header = $"{background.Light.Dark} dark:bg-gray-950/50 uppercase text-xs font-semibold tracking-wide",
                    HeaderBorderedCells = "[&_th]:border-b-2 [&_th]:border-gray-200 [&_th]:dark:border-gray-700",
                    Body = $"{background.Medium.Light} {darkBackground.Dark.Dark}",
                    BodyStriped = "[&>tr:nth-child(even)]:bg-gray-50 [&>tr:nth-child(odd)]:dark:bg-gray-800/50 [&>tr:nth-child(even)]:dark:bg-gray-800",
                    BodyHoverable = "[&>tr:hover]:!bg-gray-100 [&>tr:hover]:dark:!bg-gray-800 [&>tr]:transition-colors [&>tr]:duration-200",
                    BorderedCells = "[&_td]:border [&_td]:border-gray-200 [&_td]:dark:border-gray-700",
                    BorderedHeaderCells = "[&_th]:border [&_th]:border-gray-200 [&_th]:dark:border-gray-700"
                },
                new TwToastTheme
                {
                    Colors = new()
                    {
                        Primary = $"{background.Light.Primary} {darkBackground.Light.Primary} {text.Dark.Primary} border-l-4 {borderColors.Primary}",
                        Accent = $"{background.Light.Accent} {darkBackground.Light.Accent} {text.Dark.Accent} border-l-4 {borderColors.Accent}",
                        Success = $"{background.Light.Success} {darkBackground.Light.Success} {text.Dark.Success} border-l-4 {borderColors.Success}",
                        Danger = $"{background.Light.Danger} {darkBackground.Light.Danger} {text.Dark.Danger} border-l-4 {borderColors.Danger}",
                        Warning = $"{background.Light.Warning} {darkBackground.Light.Warning} {text.Dark.Warning} border-l-4 {borderColors.Warning}",
                        Info = $"{background.Light.Info} {darkBackground.Light.Info} {text.Dark.Info} border-l-4 {borderColors.Info}",
                        Light = $"{background.Light.Light} {darkBackground.Light.Light} {text.Dark.Dark} border-l-4 {borderColors.Light}",
                        Dark = $"{background.Dark.Dark} {darkBackground.Dark.Dark} {text.Light.Light} border-l-4 {borderColors.Dark}"
                    },
                    HeaderClasses = $"{defaultGap} flex-1 min-w-0 flex flex-col",
                    IconContainer = "flex-shrink-0",
                    Title = "font-semibold text-sm wrap-break-word",
                    Message = "text-sm wrap-break-word",
                    Container = $"{defaultGap} fixed bottom-0 right-4 {comfortablePadding} z-50 flex flex-col max-w-md",
                    Toast = $"{defaultGap} shadow-sm transition-colors duration-200 flex items-start {comfortablePadding} ease-in-out",
                    ToastWidth = "max-w-[300px]",
                    Timestamp = "text-xs opacity-70",
                    CloseButton = "rounded-full flex-shrink-0 p-1 hover:bg-black/10 dark:hover:bg-white/10 transition-colors focus:outline-none focus:ring-2 focus:ring-offset-1 focus:ring-offset-transparent focus:ring-current/40"
                }
            ]
        };
    }
    #endregion

    public static TwBlazorTheme DefaultTheme { get; internal set; } = CreateDefaultTheme();
}

internal static class ThemeUpdateHandler
{
    internal static void UpdateApplication(Type[]? _) => Theme.DefaultTheme = Theme.CreateDefaultTheme();
}

