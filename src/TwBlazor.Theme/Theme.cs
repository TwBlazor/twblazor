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
        var flexbox = new TwBlazorFlexbox
        {
            Row = "flex-row",
            RowReverse = "flex-row-reverse",
            Col = "flex-col",
            ColReverse = "flex-col-reverse",
            Wrap = "flex-wrap",
            NoWrap = "flex-nowrap",
            WrapReverse = "flex-wrap-reverse",
            Grow = "grow",
            GrowNone = "grow-0",
            Shrink = "shrink",
            ShrinkNone = "shrink-0",
            Justify = new()
            {
                Start = "justify-start",
                Center = "justify-center",
                End = "justify-end",
                Between = "justify-between",
                Around = "justify-around",
                Evenly = "justify-evenly"
            },
            Align = new()
            {
                Start = "items-start",
                Center = "items-center",
                End = "items-end",
                Stretch = "items-stretch",
                Baseline = "items-baseline"
            },
            AlignContent = new()
            {
                Start = "content-start",
                Center = "content-center",
                End = "content-end",
                Between = "content-between",
                Around = "content-around",
                Evenly = "content-evenly",
                Stretch = "content-stretch"
            },
            AlignSelf = new()
            {
                Start = "self-start",
                Center = "self-center",
                End = "self-end",
                Stretch = "self-stretch",
                Baseline = "self-baseline"
            }
        };

        // Screen-anchor combinations, composed from the flexbox alignment scale above rather than
        // retyped literals, so "items-*"/"justify-*" have exactly one home.
        var anchor = new TwAnchorPosition
        {
            Center = $"{flexbox.Align.Center} {flexbox.Justify.Center}",
            CenterLeft = $"{flexbox.Align.Center} {flexbox.Justify.Start}",
            CenterRight = $"{flexbox.Align.Center} {flexbox.Justify.End}",
            TopCenter = $"{flexbox.Align.Start} {flexbox.Justify.Center}",
            TopLeft = $"{flexbox.Align.Start} {flexbox.Justify.Start}",
            TopRight = $"{flexbox.Align.Start} {flexbox.Justify.End}",
            BottomCenter = $"{flexbox.Align.End} {flexbox.Justify.Center}",
            BottomLeft = $"{flexbox.Align.End} {flexbox.Justify.Start}",
            BottomRight = $"{flexbox.Align.End} {flexbox.Justify.End}",
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
                Success = "text-green-800",
                Danger = "text-red-700",
                Warning = "text-yellow-800",
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
            Lightest = new()
            {
                Primary = $"bg-purple-50",
                Accent = $"bg-fuchsia-50",
                Success = $"bg-green-50",
                Danger = $"bg-red-50",
                Warning = $"bg-yellow-50",
                Info = $"bg-blue-50",
                Light = $"bg-white",
                Dark = $"bg-gray-50"
            },
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
            },
            Darkest = new()
            {
                Primary = $"bg-purple-950",
                Accent = $"bg-fuchsia-950",
                Success = $"bg-green-950",
                Danger = $"bg-red-950",
                Warning = $"bg-yellow-950",
                Info = $"bg-blue-950",
                Light = $"bg-gray-950",
                Dark = $"bg-black"
            }
        };

        var darkBackground = new TwBackgroundColor
        {
            Lightest = new()
            {
                Primary = $"dark:bg-purple-50",
                Accent = $"dark:bg-fuchsia-50",
                Success = $"dark:bg-green-50",
                Danger = $"dark:bg-red-50",
                Warning = $"dark:bg-yellow-50",
                Info = $"dark:bg-blue-50",
                Dark = $"dark:bg-gray-50"
            },
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
            },
            Darkest = new()
            {
                Primary = $"dark:bg-purple-950",
                Accent = $"dark:bg-fuchsia-950",
                Success = $"dark:bg-green-950",
                Danger = $"dark:bg-red-950",
                Warning = $"dark:bg-yellow-950",
                Info = $"dark:bg-blue-950",
                Dark = $"dark:bg-black",
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

        var neutralSurface = new TwSurfacePalette
        {
            Background = "bg-[oklch(100%_0_0)] dark:bg-[oklch(25.33%_0.016_252.42)]",
            BackgroundSubtle = "bg-[oklch(98%_0_0)] dark:bg-[oklch(23.26%_0.014_253.1)]",
            Border = "border-[oklch(95%_0_0)] dark:border-[oklch(21.15%_0.012_254.09)]",
            BorderSubtle = "border-[oklch(98%_0_0)] dark:border-[oklch(23.26%_0.014_253.1)]",
            Hover = "hover:bg-[oklch(98%_0_0)] dark:hover:bg-[oklch(23.26%_0.014_253.1)]",
            Elevated = "bg-[oklch(98%_0_0)] dark:bg-[oklch(34%_0.018_253)]",
            BorderStrong = "border-[oklch(21%_0.006_285.885)]/25 dark:border-[oklch(97.807%_0.029_256.847)]/20"
        };

        var neutralText = new TwNeutralTextPalette
        {
            Heading = "text-[oklch(21%_0.006_285.885)] dark:text-[oklch(97.807%_0.029_256.847)]",
            Secondary = "text-[oklch(40%_0.006_285.885)] dark:text-[oklch(88%_0.02_256.847)]",
            Muted = "text-[oklch(50%_0.006_285.885)] dark:text-[oklch(78%_0.02_256.847)]",
            Subtle = "text-[oklch(60%_0.006_285.885)] dark:text-[oklch(68%_0.02_256.847)]"
        };

        var display = new TwBlazorDisplay
        {
            Block = "block",
            InlineBlock = "inline-block",
            Flex = "flex",
            InlineFlex = "inline-flex",
            Grid = "grid",
            InlineGrid = "inline-grid",
            Hidden = "hidden",
            Contents = "contents"
        };

        var spacing = new TwBlazorSpacing
        {
            Gap = new()
            {
                Sm = "gap-1",
                Md = "gap-2",
                Lg = "gap-3",
                Xl = "gap-4"
            },
            InteractiveRowPadding = "px-4 py-3",
            PushEnd = "ml-auto",
            Padding = new()
            {
                Tight = "p-1",
                Compact = "p-2",
                Standard = "p-3",
                Comfortable = "p-4"
            },
            Margin = new()
            {
                Tight = "m-1",
                Compact = "m-2",
                Standard = "m-3",
                Comfortable = "m-4"
            }
        };

        var interaction = new TwBlazorInteraction
        {
            DisabledOpacity = "opacity-40",
            PointerCursor = "cursor-pointer",
            DisabledCursor = "cursor-not-allowed",
            ReadonlyCursor = "cursor-default",
            PointerEventsNone = "pointer-events-none"
        };

        var sizing = new TwBlazorSizing
        {
            FullWidth = "w-full",
            FullHeight = "h-full",
            Full = "w-full h-full",
            Icon = new()
            {
                Xs = "size-3",
                Sm = "size-4",
                Md = "size-5",
                Lg = "size-6",
                Xl = "size-8"
            }
        };

        var rounded = new TwBlazorRounded
        {
            None = "rounded-none",
            Sm = "rounded-sm",
            Md = "rounded",
            Lg = "rounded-lg",
            Full = "rounded-full",
            DefaultRounded = Rounded.Md,
            RoundedTop = new()
            {
                None = "rounded-t-none",
                Sm = "rounded-t-sm",
                Md = "rounded-t",
                Lg = "rounded-t-lg",
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
        };

        var shadows = new TwBlazorShadow
        {
            None = "shadow-none",
            Sm = "shadow-sm",
            Md = "shadow",
            Lg = "shadow-lg",
            Xl = "shadow-xl",
            HoverSm = "hover:shadow-sm",
            HoverMd = "hover:shadow",
            HoverLg = "hover:shadow-xl",
            ActiveMd = "active:shadow",
            DefaultShadow = Shadow.Sm
        };

        var borderWidth = new TwBlazorBorderWidth
        {
            None = "border-0",
            Thin = "border",
            Thick = "border-2",
            AccentEdge = "border-l-4"
        };

        // Combined border home: one width scale plus semantic and neutral color, so a component theme
        // never has to reach into three separate places to build a border class.
        var border = new TwBlazorBorder
        {
            Width = borderWidth,
            Colors = borderColors,
            Neutral = new()
            {
                Base = neutralSurface.Border,
                Subtle = neutralSurface.BorderSubtle,
                Strong = neutralSurface.BorderStrong
            }
        };

        var positioning = new TwBlazorPositioning
        {
            Static = "static",
            Relative = "relative",
            Absolute = "absolute",
            Fixed = "fixed",
            Sticky = "sticky"
        };

        var textTransform = new TwBlazorTextTransform
        {
            Uppercase = "uppercase",
            Lowercase = "lowercase",
            Capitalize = "capitalize",
            NormalCase = "normal-case"
        };

        var transparentBackground = "bg-transparent";

        return new TwBlazorTheme
        {
            Anchor = anchor,
            Position = positioning,
            Display = display,
            Flexbox = flexbox,
            Spacing = spacing,
            Sizing = sizing,
            Interaction = interaction,
            Border = border,
            TextTransform = textTransform,
            Colors = new()
            {
                TextColors = text,
                DarkTextColors = darkText,
                HoverColors = hoverColors,
                Transparent = transparentBackground,
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
                        Accent = "bg-fuchsia-700 hover:bg-fuchsia-800 active:bg-fuchsia-900 text-gray-100",
                        Success = $"{background.Medium.Success} hover:bg-green-700 active:bg-green-800 {text.Medium.Dark}",
                        Danger = "bg-red-700 hover:bg-red-800 active:bg-red-900 text-gray-100",
                        Warning = $"{background.Medium.Warning} hover:bg-yellow-600 active:bg-yellow-700 {text.Medium.Dark}",
                        Info = $"{background.Medium.Info} hover:bg-blue-700 active:bg-blue-800 {text.Medium.Light}",
                        Light = $"{background.Medium.Light} hover:bg-gray-50 active:bg-gray-100 {text.Medium.Dark}",
                        Dark = $"{background.Dark.Dark} hover:bg-gray-800 active:bg-gray-700 {text.Medium.Light}",
                    },
                    Outlined = new()
                    {
                        Primary = $"{text.Medium.Primary} {darkText.Light.Primary} {transparentBackground} {hoverColors.Primary} border {borderColors.Primary}",
                        Accent = $"{text.Medium.Accent} {darkText.Light.Accent} {transparentBackground} {hoverColors.Accent} border {borderColors.Accent}",
                        Success = $"{text.Medium.Success} {darkText.Light.Success} {transparentBackground} {hoverColors.Success} border {borderColors.Success}",
                        Danger = $"{text.Medium.Danger} {darkText.Light.Danger} {transparentBackground} {hoverColors.Danger} border {borderColors.Danger}",
                        Warning = $"{text.Medium.Warning} {darkText.Light.Warning} {transparentBackground} {hoverColors.Warning} border {borderColors.Warning}",
                        Info = $"{text.Medium.Info} {darkText.Light.Info} {transparentBackground} {hoverColors.Info} border {borderColors.Info}",
                        Light = $"{text.Light.Dark} {transparentBackground} {hoverColors.Light} border {borderColors.Light}",
                        Dark = $"{text.Medium.Dark} {transparentBackground} {hoverColors.Dark} border {borderColors.Dark}",
                    },
                    Text = new()
                    {
                        Primary = $"{text.Medium.Primary} {darkText.Light.Primary} {transparentBackground} {hoverColors.Primary}",
                        Accent = $"{text.Medium.Accent} {darkText.Light.Accent} {transparentBackground} {hoverColors.Accent}",
                        Success = $"{text.Medium.Success} {darkText.Light.Success} {transparentBackground} {hoverColors.Success}",
                        Danger = $"{text.Medium.Danger} {darkText.Light.Danger} {transparentBackground} {hoverColors.Danger}",
                        Warning = $"{text.Medium.Warning} {darkText.Light.Warning} {transparentBackground} {hoverColors.Warning}",
                        Info = $"{text.Medium.Info} {darkText.Light.Info} {transparentBackground} {hoverColors.Info}",
                        Light = $"{text.Light.Dark} {transparentBackground} {hoverColors.Light}",
                        Dark = $"{text.Dark.Dark} {transparentBackground} {hoverColors.Dark}",
                    },
                },
                NeutralSurface = neutralSurface,
                NeutralText = neutralText
            },
            Shadows = shadows,
            Rounded = rounded,
            // Tip - to reduce your own tailwind css files you only have to declare the components you use.
            Components =
            [
                new TwAlertTheme
                {
                    Colors = new()
                    {
                        Primary = $"{background.Light.Primary} {darkBackground.Dark.Primary} {text.Dark.Primary} {darkText.Light.Primary} {borderWidth.AccentEdge} {borderColors.Primary}",
                        Accent = $"{background.Light.Accent} {darkBackground.Dark.Accent} {text.Dark.Accent} {darkText.Light.Accent} {borderWidth.AccentEdge} {borderColors.Accent}",
                        Success = $"{background.Light.Success} {darkBackground.Dark.Success} {text.Dark.Success} {darkText.Light.Success} {borderWidth.AccentEdge} {borderColors.Success}",
                        Danger = $"{background.Light.Danger} {darkBackground.Dark.Danger} {text.Dark.Danger} {darkText.Light.Danger} {borderWidth.AccentEdge} {borderColors.Danger}",
                        Warning = $"{background.Light.Warning} {darkBackground.Dark.Warning} {text.Dark.Warning} {darkText.Light.Warning} {borderWidth.AccentEdge} {borderColors.Warning}",
                        Info = $"{background.Light.Info} {darkBackground.Dark.Info} {text.Dark.Info} {darkText.Light.Info} {borderWidth.AccentEdge} {borderColors.Info}",
                        Light = $"{background.Light.Light} {darkBackground.Dark.Light} {text.Medium.Dark} {darkText.Medium.Dark} {borderWidth.AccentEdge} {borderColors.Light}",
                        Dark = $"{background.Light.Dark} {darkBackground.Dark.Dark} {text.Medium.Dark} {darkText.Light.Dark} {borderWidth.AccentEdge} {borderColors.Dark}",
                    },
                },
                new TwBreadcrumbTheme
                {
                    List = $"{display.InlineFlex} {flexbox.Wrap} {spacing.Gap.Lg}",
                    Item = $"{display.Flex} {flexbox.Align.Center} {spacing.Gap.Md}",
                    Separator = $"font-bold {neutralText.Subtle}",
                    Label = "wrap-break-word"
                },
                new TwButtonTheme
                {
                    Base = $"{anchor.Center} transition-colors duration-200 text-sm {display.InlineFlex} h-8 overflow-hidden focus:outline-none focus-visible:outline-none touch-manipulation",
                    Padding = "px-6",
                    DensePadding = "px-3 py-1.5",
                    IconButton = $"{anchor.Center} {rounded.Full} {display.Flex} focus:outline-none h-8 w-8 text-sm/6",
                    Typography = "font-medium",
                    Uppercase = $"{textTransform.Uppercase} tracking-wide",
                    DisabledCursor = interaction.DisabledCursor,
                    ReadonlyCursor = interaction.ReadonlyCursor,
                    DefaultCursor = interaction.PointerCursor,
                    DisabledFilled = $"bg-[oklch(21%_0.006_285.885)]/15 dark:bg-[oklch(97.807%_0.029_256.847)]/15 text-[oklch(21%_0.006_285.885)]/40 dark:text-[oklch(97.807%_0.029_256.847)]/40 {interaction.DisabledCursor} shadow-none",
                    DisabledOutlined = $"border border-[oklch(21%_0.006_285.885)]/15 dark:border-[oklch(97.807%_0.029_256.847)]/15 text-[oklch(21%_0.006_285.885)]/40 dark:text-[oklch(97.807%_0.029_256.847)]/40 {transparentBackground} {interaction.DisabledCursor}",
                    DisabledText = $"text-[oklch(21%_0.006_285.885)]/40 dark:text-[oklch(97.807%_0.029_256.847)]/40 {transparentBackground} {interaction.DisabledCursor}"
                },
                new TwCardTheme
                {
                    Container = "px-6 py-5",
                    Bordered = $"{borderWidth.Thin} {neutralSurface.Border}",
                    Title = $"text-lg font-semibold {neutralText.Heading} wrap-break-word"
                },
                new TwCheckboxTheme
                {
                    Colors = checkBoxRadioButtonColors,
                    Base = $"peer {interaction.PointerCursor} appearance-none {borderWidth.Thick} border-[oklch(95%_0_0)] dark:border-[oklch(21.15%_0.012_254.09)] transition-colors duration-200 ease-in-out",
                    Disabled = $"{interaction.DisabledOpacity} {interaction.DisabledCursor}",
                    Hover = $"{interaction.PointerCursor} hover:border-[oklch(21%_0.006_285.885)]/40 dark:hover:border-[oklch(97.807%_0.029_256.847)]/40",
                    LabelBase = $"{display.Flex} {flexbox.Align.Center} {positioning.Relative} select-none min-h-[24px] {spacing.Gap.Md}",
                    LabelInteractiveCursor = interaction.PointerCursor,
                    LabelNonInteractiveCursor = interaction.PointerEventsNone,
                    LabelDisabled = interaction.DisabledOpacity,
                    IconWrapper = $"{positioning.Absolute} opacity-0 peer-checked:opacity-100 translate-x-1/4",
                    IndeterminateIconWrapper = $"{positioning.Absolute} translate-x-1/4"
                },
                new TwChipTheme
                {
                    Base = $"{anchor.Center} transition-colors duration-200 {display.InlineFlex} gap-1.5 font-medium {shadows.Sm} touch-manipulation",
                    CloseButton = $"{anchor.Center} {display.Flex} hover:bg-[oklch(21%_0.006_285.885)]/10 dark:hover:bg-[oklch(97.807%_0.029_256.847)]/10 {rounded.Full} {sizing.Icon.Sm} text-center",
                    Sm = "text-[10px] leading-none px-1.5 py-1 h-5",
                    Md = "text-xs leading-none px-2 py-1 h-6",
                    Lg = "text-sm leading-none px-2.5 py-1.5 h-8"
                },
                new TwCollapseTheme
                {
                    Container = $"tw-collapse {borderWidth.Thin} {neutralSurface.Border}",
                    Trigger = $"{display.Flex} w-full {flexbox.Align.Center} {flexbox.Justify.Between} {spacing.Gap.Md} {spacing.InteractiveRowPadding} text-left font-medium {neutralText.Heading} transition-colors duration-200 {neutralSurface.Hover} focus:outline-none focus-visible:ring-2 focus-visible:ring-inset focus-visible:ring-blue-500 touch-manipulation",
                    Icon = $"{spacing.PushEnd} {sizing.Icon.Sm} {flexbox.ShrinkNone} transition-transform duration-300 {display.Flex}",
                    IconOpen = "rotate-180",
                    Content = $"overflow-hidden border-t {neutralSurface.Border} transition-colors duration-300"
                },
                new TwColorPickerTheme
                {
                    Swatch = $"block h-7 w-7 ring-1 ring-inset ring-[oklch(21%_0.006_285.885)]/10 dark:ring-[oklch(97.807%_0.029_256.847)]/15 {shadows.Sm} flex-shrink-0 transition-[box-shadow,opacity] duration-200",
                    SwatchDisabled = interaction.DisabledOpacity,
                    SwatchHover = "hover:ring-[oklch(21%_0.006_285.885)]/20 dark:hover:ring-[oklch(97.807%_0.029_256.847)]/25",
                    InputContainer = $"{display.Flex} {flexbox.Align.Center} {spacing.Gap.Md}",
                    DialogPosition = $"{positioning.Absolute} top-full left-0 mt-2 z-50",
                    DialogSurface = $"tw-color-picker-dialog {neutralSurface.Background} ring-1 ring-[oklch(21%_0.006_285.885)]/10 dark:ring-[oklch(97.807%_0.029_256.847)]/15 {spacing.Padding.Standard} w-64",
                    PreviewSwatch = $"flex-1 min-w-0 h-6 {rounded.Full} ring-1 ring-inset ring-[oklch(21%_0.006_285.885)]/10 dark:ring-[oklch(97.807%_0.029_256.847)]/15 {shadows.Sm}",
                    SelectorSquare = $"{positioning.Relative} {sizing.FullWidth} h-48 overflow-hidden ring-1 ring-inset ring-[oklch(21%_0.006_285.885)]/10 dark:ring-[oklch(97.807%_0.029_256.847)]/10 cursor-crosshair touch-none",
                    SelectorThumb = $"{positioning.Absolute} {sizing.Icon.Sm} {rounded.Full} border-2 border-white ring-1 ring-black/10 {shadows.Lg} {interaction.PointerEventsNone}",
                    SliderTrack = $"{sizing.FullWidth} h-2.5 {rounded.Full} overflow-hidden ring-1 ring-inset ring-[oklch(21%_0.006_285.885)]/10 dark:ring-[oklch(97.807%_0.029_256.847)]/10 {interaction.PointerEventsNone}",
                    SliderThumb = $"{positioning.Absolute} top-1/2 {sizing.Icon.Sm} {rounded.Full} border-2 border-white ring-1 ring-black/10 {shadows.Lg} {interaction.PointerEventsNone}",
                    AlphaLabel = $"text-xs font-medium {neutralText.Secondary}",
                    ActionBar = $"{display.Flex} {flexbox.Justify.End} {spacing.Gap.Md} pt-1",
                    ControlRow = $"{display.Flex} {flexbox.Align.Center} {spacing.Gap.Md} {spacing.Padding.Compact} {neutralSurface.BackgroundSubtle}"
                },
                new TwDatePickerTheme
                {
                    Header = $"{neutralSurface.Elevated} text-center font-medium {rounded.RoundedTop.Lg} border-b {neutralSurface.BorderStrong}",
                    WeekdaysHeader = $"{anchor.Center} {text.Medium.Primary} {darkText.Light.Primary} h-8 {display.Flex} text-xs font-semibold tracking-wide ",
                    Base = $"{positioning.Absolute} {interaction.PointerCursor} {borderWidth.None} text-center text-sm font-medium transition-colors duration-200 top-full left-0 {flexbox.Row} md:flex-row {flexbox.Align.Center} z-50 mt-1 {neutralSurface.Elevated} px-2 pb-2 {borderWidth.Thin} {neutralSurface.BorderStrong}",
                    ActiveClass = "bg-purple-50 dark:bg-purple-500/30",
                    ButtonClass = $"{spacing.Padding.Compact} h-8 {display.Flex} {flexbox.Align.Center} {sizing.FullWidth} {flexbox.Justify.Center}",
                    RangeClass = "bg-purple-100 dark:bg-purple-500/20",
                    RangeMonthCaptionClass = $"text-center {text.Medium.Primary} {darkText.Light.Primary} text-sm font-semibold mt-2",
                    PrevMonthClass = "text-gray-400 dark:text-gray-600",
                    DefaultFormat = "dd/MM/yyyy",
                    DefaultDateTimeFormat = "dd/MM/yyyy HH:mm",
                    DefaultDateTimeFormat12Hour = "dd/MM/yyyy hh:mm tt",
                    DefaultRangeSeparator = " - "
                },
                new TwGroupsTheme
                {
                    Gap = spacing.Gap.Md,
                    FieldsetBase = "border-none p-0 m-0",
                    HorizontalLayout = $"{display.Flex} {flexbox.Row} {flexbox.Wrap}",
                    VerticalLayout = $"{display.Flex} {flexbox.Col}",
                    CheckboxGroupDisabled = $"{interaction.DisabledOpacity} {interaction.PointerEventsNone}",
                    RadioGroupDisabled = $"{interaction.DisabledOpacity} {interaction.PointerEventsNone}",
                    ButtonGroupBase = display.InlineFlex,
                    ButtonGroupVertical = flexbox.Col,
                    ButtonGroupHorizontal = flexbox.Row,
                    ButtonGroupFullWidth = sizing.FullWidth,
                    ButtonGroupFullWidthRow = "[&>*]:flex-1",
                    ChipGroupBase = $"{display.Flex} {flexbox.Wrap}",
                    ChipGroupAlignStart = flexbox.Justify.Start,
                    ChipGroupAlignCenter = flexbox.Justify.Center,
                    ChipGroupAlignEnd = flexbox.Justify.End
                },
                new TwDialogTheme
                {
                    Backdrop = $"{positioning.Fixed} inset-0 z-[110] {display.Flex} {spacing.Padding.Comfortable} bg-black/50 overflow-y-auto overscroll-contain",
                    Surface = $"{positioning.Relative} {display.Flex} {sizing.FullWidth} {flexbox.Col} {neutralText.Heading} {neutralSurface.Background} max-h-[calc(100vh-2rem)]",
                    Header = $"{display.Flex} {flexbox.Align.Center} {flexbox.Justify.Between} {spacing.Gap.Xl} px-6 py-4 border-b {neutralSurface.Border} flex-shrink-0",
                    Title = "text-lg font-semibold",
                    CloseButton = $"transition-colors duration-200 {rounded.Lg} {spacing.Padding.Tight} {neutralText.Subtle} {neutralSurface.Hover} hover:text-[oklch(21%_0.006_285.885)] dark:hover:text-[oklch(97.807%_0.029_256.847)] flex-shrink-0 focus:outline-none focus:ring-2 focus:ring-offset-1 focus:ring-offset-transparent focus:ring-blue-500/20",
                    Content = "px-6 py-4 overflow-y-auto",
                    FullScreen = $"{sizing.Full} max-w-none",
                    FullWidth = sizing.FullWidth,
                    SmallWidth = "sm:max-w-lg",
                    MediumWidth = "sm:max-w-xl",
                    LargeWidth = "sm:max-w-3xl"
                },
                new TwInputTheme
                {
                    DefaultInputVariant = InputVariant.Filled,
                    TextfieldBase = $"{sizing.FullWidth} min-w-0 max-w-full {neutralText.Heading} transition-colors duration-200 text-base {display.Block} ease-in-out placeholder:text-[oklch(21%_0.006_285.885)]/50 dark:placeholder:text-[oklch(97.807%_0.029_256.847)]/50 focus:outline-none",
                    SelectBase = $"{sizing.FullWidth} {neutralText.Heading} transition-colors duration-200 appearance-none text-base {display.Block} pr-10 py-2 focus:outline-none bg-[url('data:image/svg+xml;charset=utf-8,%3Csvg%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%20fill%3D%22none%22%20viewBox%3D%220%200%2020%2020%22%3E%3Cpath%20stroke%3D%22%236b7280%22%20stroke-linecap%3D%22round%22%20stroke-linejoin%3D%22round%22%20stroke-width%3D%221.5%22%20d%3D%22m6%208%204%204%204-4%22%2F%3E%3C%2Fsvg%3E')] bg-[length:1.5em_1.5em] bg-[right_0.5rem_center] bg-no-repeat",
                    InputLegendBase = $"text-base font-medium {neutralText.Secondary} mb-3",
                    LabelBase = $"{display.Block} mb-2 text-xs font-normal tracking-wide {neutralText.Muted}",
                    OutlinedBorder = $"border-1 {neutralSurface.BorderStrong}",
                    FilledBorder = $"border-b-2 {neutralSurface.BorderStrong}",
                    FocusBorder = "focus:border-purple-600 dark:focus:border-purple-500",
                    FilledBackgroundColor = neutralSurface.Elevated
                },
                new TwPaginationTheme
                {
                    Base = $"{anchor.Center} {display.Flex} h-8 leading-tight select-none border-1 {neutralSurface.Border} px-3 mx-0.5 tabular-nums",
                    ActiveButton = $"{text.Medium.Primary} {darkText.Medium.Primary} {background.Light.Primary} {darkBackground.Light.Primary} font-bold hover:bg-purple-100 {interaction.PointerCursor}",
                    Buttons = $"{neutralText.Muted} {neutralSurface.Background} {neutralSurface.Hover} hover:text-[oklch(21%_0.006_285.885)] dark:hover:text-[oklch(97.807%_0.029_256.847)] {interaction.PointerCursor}"
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
                    Base = $"{display.Block} {sizing.FullWidth} min-w-0 appearance-none overflow-hidden {rounded.Full} border-none bg-[oklch(95%_0_0)] dark:bg-[oklch(21.15%_0.012_254.09)] indeterminate:animate-pulse [&::-webkit-progress-bar]:rounded-full [&::-webkit-progress-bar]:bg-transparent [&::-webkit-progress-value]:rounded-full [&::-webkit-progress-value]:transition-[width] [&::-webkit-progress-value]:duration-300 [&::-moz-progress-bar]:rounded-full [&::-moz-progress-bar]:transition-[width] [&::-moz-progress-bar]:duration-300",
                    Small = "h-1.5",
                    Medium = "h-2.5",
                    Large = "h-4"
                },
                new TwTabTheme
                {
                    TabBase = $"{positioning.Relative} tracking-wide font-medium text-sm transition-colors duration-300",
                    TabPadding = "py-5 px-6",
                    TabDensePadding = "px-4",
                    ActiveIndicator = "after:absolute after:bottom-0 after:left-0 after:right-0 after:h-0.5 after:bg-current after:scale-x-100 after:transition-transform after:duration-300",
                    InactiveIndicator = "hover:text-[oklch(21%_0.006_285.885)] dark:hover:text-[oklch(97.807%_0.029_256.847)] after:absolute after:bottom-0 after:left-0 after:right-0 after:h-0.5 after:bg-current after:scale-x-0 hover:after:scale-x-100 after:transition-transform after:duration-300",
                    DisabledTab = $"{interaction.DisabledOpacity} {interaction.DisabledCursor}",
                    TabListContainer = $"{display.Flex} {flexbox.Wrap} border-t border-l border-r border-b-2 {neutralSurface.Border} {shadows.Sm}",
                    PanelContainer = $"p-6 border-l border-r border-b {neutralSurface.Border}",
                    Background = neutralSurface.Background,
                },
                new TwSkeletonTheme
                {
                    Base = $"{positioning.Relative} overflow-hidden bg-[oklch(95%_0_0)] dark:bg-[oklch(21.15%_0.012_254.09)] {display.Block}",
                    Text = $"{sizing.FullWidth} h-4",
                    Circle = "size-12",
                    Rectangle = $"{sizing.FullWidth} h-24",
                    MeasuringWrapper = "invisible",
                    Pulse = "animate-pulse",
                    Wave = "tw-skeleton-wave"
                },
                new TwSidebarTheme
                {
                    Navbar = $"{background.Dark.Primary} {darkBackground.Dark.Primary} {shadows.Sm} {spacing.Padding.Standard} {sizing.FullWidth} {display.Flex} {flexbox.Align.Center} flex-shrink-0 z-40 min-h-[56px]",
                    NavbarContent = $"{display.Flex} {sizing.FullWidth} {flexbox.Wrap} {flexbox.Align.Center} {spacing.Gap.Xl}",
                    NavbarBrand = flexbox.ShrinkNone,
                    NavbarNavigation = $"{flexbox.Align.Center} {spacing.Gap.Md}",
                    NavbarActions = $"{spacing.PushEnd} {display.Flex} {flexbox.Align.Center} {spacing.Gap.Md}",
                    NavbarToggle = $"order-first {flexbox.ShrinkNone} text-xl text-white lg:hidden",
                    NavbarLink = $"{display.InlineFlex} {flexbox.Align.Center} {spacing.Gap.Md} px-3 py-2 text-sm {text.Light.Light} hover:bg-white/10 rounded-md transition-colors",
                    NavbarLinkActive = "bg-white/15 font-semibold",
                    NavbarMobileMenu = $"{background.Dark.Primary} {darkBackground.Dark.Primary} {shadows.Lg} {spacing.Padding.Standard}",
                    Sidebar = $"transition-transform duration-200 {shadows.Sm} h-dvh w-64 flex-shrink-0 {neutralSurface.Background} {spacing.Padding.Comfortable} z-[100] ease-in-out overflow-auto overscroll-contain",
                    NavigationItemBase = $"{spacing.Gap.Md} min-w-0 transition-colors duration-200 {display.Flex} {flexbox.Align.Center} {neutralText.Secondary} {neutralSurface.Hover} {spacing.InteractiveRowPadding} text-sm focus:outline-none focus-visible:ring-inset focus-visible:ring-2 focus-visible:ring-blue-500 {interaction.PointerCursor}",
                    NavigationItemActive = "bg-[oklch(95%_0_0)] dark:bg-[oklch(21.15%_0.012_254.09)] text-[oklch(21%_0.006_285.885)] dark:text-[oklch(97.807%_0.029_256.847)] font-semibold",
                    NavigationDropdownContainer = neutralSurface.BackgroundSubtle,
                    NavigationItemActiveLevelDeep = $"border-l-2 {border.Neutral.Base}",
                    NavigationDropdownContainerDeep = $"border-l-2 border-b-2 pl-2 {border.Neutral.Base}",
                    MainContent = $"{sizing.FullWidth} flex-1 overflow-y-auto transition-[margin] duration-200 ease-in-out left-0",
                    MainContentRoot = $"{transparentBackground} {neutralSurface.Background} h-dvh {sizing.FullWidth} {display.Flex} {flexbox.Col} transition-[margin] duration-300 ease-in-out {neutralText.Heading} overflow-x-hidden"
                },
                new TwTreeListTheme
                {
                    Container = $"{display.Flex} {flexbox.Col} text-sm",
                    Group = $"mt-1 ml-4 pl-2 py-1 {neutralSurface.BackgroundSubtle} {display.Flex} {flexbox.Col}",
                    Row = $"group/row {spacing.Gap.Md} min-w-0 transition-colors duration-200 {display.Flex} {flexbox.Align.Center} {neutralText.Heading} px-2 py-1.5 text-sm group-focus-visible:ring-inset group-focus-visible:ring-2 group-focus-visible:ring-blue-500 {interaction.PointerCursor}",
                    RowDisabled = $"{interaction.DisabledOpacity} {interaction.PointerEventsNone}",
                    ToggleSlot = $"{display.InlineFlex} {sizing.Icon.Sm} {flexbox.ShrinkNone} {flexbox.Align.Center} {flexbox.Justify.Center}",
                    ToggleIcon = $"{display.InlineFlex} {sizing.Icon.Sm} {flexbox.ShrinkNone} {flexbox.Align.Center} {flexbox.Justify.Center} transition-transform duration-200",
                    ToggleIconOpen = "rotate-180 translate-y-px",
                    Label = "truncate group-hover/row:underline",
                    ItemIcon = $"{display.InlineFlex} {sizing.Icon.Sm} {flexbox.ShrinkNone} {flexbox.Align.Center} {flexbox.Justify.Center} {neutralText.Muted}"
                },
                new TwRadioButtonTheme
                {
                    Colors = checkBoxRadioButtonColors,
                    Base = $"peer {sizing.Icon.Md} {interaction.PointerCursor} appearance-none {rounded.Full} {borderWidth.Thick} border-[oklch(95%_0_0)] dark:border-[oklch(21.15%_0.012_254.09)] transition-colors duration-200 ease-in-out",
                    Disabled = $"{interaction.DisabledOpacity} {interaction.DisabledCursor}",
                    Hover = $"{interaction.PointerCursor} hover:border-[oklch(21%_0.006_285.885)]/40 dark:hover:border-[oklch(97.807%_0.029_256.847)]/40",
                    LabelBase = $"{display.Flex} {flexbox.Align.Center} {positioning.Relative} select-none min-h-[24px] {spacing.Gap.Md}",
                    LabelInteractiveCursor = interaction.PointerCursor,
                    LabelNonInteractiveCursor = interaction.PointerEventsNone,
                    LabelDisabled = interaction.DisabledOpacity,
                    IconWrapper = $"{positioning.Absolute} left-0 {sizing.Icon.Md} {display.Flex} {flexbox.Align.Center} {flexbox.Justify.Center} opacity-0 peer-checked:opacity-100 {interaction.PointerEventsNone}"
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
                    Wrapper = $"{positioning.Relative} {display.Flex} {flexbox.Align.Center} {sizing.FullWidth} h-6 select-none",
                    Base = $"peer {positioning.Absolute} inset-0 z-20 {sizing.Full} m-0 appearance-none {transparentBackground} {interaction.PointerCursor} focus:outline-none focus-visible:outline-none touch-manipulation",
                    Track = $"{interaction.PointerEventsNone} {positioning.Absolute} inset-x-0 top-1/2 h-1.5 -translate-y-1/2 {rounded.Full} bg-[oklch(95%_0_0)] dark:bg-[oklch(21.15%_0.012_254.09)] overflow-hidden",
                    Fill = sizing.FullHeight,
                    Thumb = $"{interaction.PointerEventsNone} {positioning.Absolute} top-1/2 z-10 {sizing.Icon.Md} -translate-x-1/2 -translate-y-1/2 {rounded.Full} bg-white dark:bg-gray-100 {borderWidth.Thick} shadow-md ring-1 ring-black/5 transition-transform duration-100 ease-out peer-hover:scale-110 peer-active:scale-95",
                    Bubble = $"{interaction.PointerEventsNone} {positioning.Absolute} bottom-full z-10 -translate-x-1/2 mb-2 whitespace-nowrap rounded-md bg-gray-900 dark:bg-gray-700 px-2 py-1 text-xs font-medium text-white {shadows.Lg} opacity-0 scale-95 transition-[opacity,transform] duration-100 ease-out peer-hover:opacity-100 peer-hover:scale-100 peer-focus-visible:opacity-100 peer-focus-visible:scale-100 tabular-nums"
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
                    Switch = $"{positioning.Absolute} top-1/2 start-0.5 -translate-y-1/2 {sizing.Icon.Md} bg-gray-100 {rounded.Full} {shadows.Lg} transition-transform duration-300 ease-in-out peer-checked:translate-x-full peer-checked:shadow-lg",
                    Track = $"{positioning.Absolute} inset-0 bg-[oklch(95%_0_0)] dark:bg-[oklch(21.15%_0.012_254.09)] {rounded.Full} transition-[background-color,opacity] duration-300 ease-in-out peer-disabled:{interaction.DisabledOpacity} peer-disabled:pointer-events-none shadow-inner",
                    Base = "peer sr-only",
                    LabelBase = $"{display.InlineFlex} {flexbox.Align.Center} {spacing.Gap.Md} select-none",
                    LabelInteractiveCursor = interaction.PointerCursor,
                    LabelNonInteractiveCursor = interaction.PointerEventsNone,
                    LabelDisabled = interaction.DisabledOpacity
                },
                new TwSpinnerTheme
                {
                    Colors = new()
                    {
                        Primary = "border-t-purple-600 dark:border-t-purple-500",
                        Accent = "border-t-fuchsia-600 dark:border-t-fuchsia-500",
                        Success = "border-t-green-600 dark:border-t-green-500",
                        Danger = "border-t-red-600 dark:border-t-red-500",
                        Warning = "border-t-yellow-500 dark:border-t-yellow-500",
                        Info = "border-t-blue-600 dark:border-t-blue-500",
                        Light = "border-t-white",
                        Dark = "border-t-gray-900",
                    },
                    Wrapper = $"{display.InlineFlex} {flexbox.Align.Center} {spacing.Gap.Md}",
                    Base = $"{display.InlineBlock} {rounded.Full} animate-spin",
                    Track = neutralSurface.Border,
                    LightTrack = "border-white/25",
                    DarkTrack = "border-gray-900/15",
                    Small = $"{sizing.Icon.Md} border-3",
                    Medium = $"{sizing.Icon.Xl} border-5",
                    Large = "size-12 border-5",
                    Label = $"text-sm {neutralText.Muted}"
                },
                new TwTimePickerTheme
                {
                    PickerRoot = positioning.Relative,
                    IconWrapper = $"{positioning.Absolute} top-0 start-0 h-10.5 {display.Flex} {anchor.Center} ps-2 w-10 {interaction.PointerCursor}",
                    IconGlyph = $"{neutralText.Subtle} {sizing.Icon.Md}",
                    TextfieldPadding = "pl-10 pr-3",
                    PanelPosition = $"{positioning.Absolute} top-full left-0 z-50 mt-2 {rounded.Lg} {shadows.Lg}",
                    BodySurface = $"{neutralSurface.Elevated} {spacing.Padding.Comfortable} {shadows.Xl} {rounded.Lg} {borderWidth.Thin} {neutralSurface.BorderStrong} text-center font-medium {neutralText.Heading}",
                    BodyRoot = "",
                    BodyInner = $"{spacing.Padding.Compact} text-center font-medium {neutralText.Heading}",
                    ContentRow = $"{display.Flex} {anchor.Center} {spacing.Gap.Lg}",
                    Column = $"{display.Flex} {flexbox.Col} {flexbox.Align.Center} {spacing.Gap.Sm}",
                    StepButton = $"{interaction.PointerCursor} {neutralText.Subtle} hover:text-purple-600 dark:hover:text-purple-400",
                    NumberWrapper = $"{display.Flex} {flexbox.Align.Center} {flexbox.Justify.Center}",
                    NumberInput = $"w-12 {transparentBackground} text-center text-lg font-semibold {neutralText.Heading} border-b-2 border-[oklch(95%_0_0)] dark:border-[oklch(21.15%_0.012_254.09)] transition-colors duration-200 py-1 focus:outline-none",
                    Separator = $"text-lg font-semibold {neutralText.Subtle} px-1 self-center",
                    AmPmWrapper = $"{display.Flex} {flexbox.Align.Center} ml-2",
                    AmPmButtonClass = "min-w-12"
                },
                new TwTableTheme
                {
                    Base = $"{neutralText.Heading} {sizing.FullWidth} text-sm text-left rtl:text-right",
                    Bordered = $"{borderWidth.Thin} {neutralSurface.Border}",
                    Header = $"{neutralSurface.BackgroundSubtle} {textTransform.Uppercase} text-xs font-semibold tracking-wide border-b {neutralSurface.Border}",
                    Body = neutralSurface.Background,
                    BodyStriped = "[&>tr:nth-child(even)]:bg-[oklch(98%_0_0)] [&>tr:nth-child(even)]:dark:bg-[oklch(23.26%_0.014_253.1)]",
                    BodyHoverable = "[&>tr:hover]:!bg-purple-50 [&>tr:hover]:dark:!bg-purple-900/20 [&>tr]:transition-colors [&>tr]:duration-200",
                    RowDivider = "divide-y divide-[oklch(95%_0_0)] dark:divide-[oklch(21.15%_0.012_254.09)]",
                    Footer = $"border-t {neutralSurface.Border}"
                },
                new TwToastTheme
                {
                    Colors = new()
                    {
                        Primary = $"{background.Light.Primary} {darkBackground.Light.Primary} {text.Dark.Primary} {borderWidth.AccentEdge} {borderColors.Primary}",
                        Accent = $"{background.Light.Accent} {darkBackground.Light.Accent} {text.Dark.Accent} {borderWidth.AccentEdge} {borderColors.Accent}",
                        Success = $"{background.Light.Success} {darkBackground.Light.Success} {text.Dark.Success} {borderWidth.AccentEdge} {borderColors.Success}",
                        Danger = $"{background.Light.Danger} {darkBackground.Light.Danger} {text.Dark.Danger} {borderWidth.AccentEdge} {borderColors.Danger}",
                        Warning = $"{background.Light.Warning} {darkBackground.Light.Warning} {text.Dark.Warning} {borderWidth.AccentEdge} {borderColors.Warning}",
                        Info = $"{background.Light.Info} {darkBackground.Light.Info} {text.Dark.Info} {borderWidth.AccentEdge} {borderColors.Info}",
                        Light = $"{background.Light.Light} {darkBackground.Light.Light} {text.Dark.Dark} {borderWidth.AccentEdge} {borderColors.Light}",
                        Dark = $"{background.Dark.Dark} {text.Light.Light} {borderWidth.AccentEdge} {borderColors.Dark}"
                    },
                    HeaderClasses = $"{spacing.Gap.Md} flex-1 min-w-0 {display.Flex} {flexbox.Col}",
                    IconContainer = "flex-shrink-0",
                    Title = "font-semibold text-sm wrap-break-word",
                    Message = "text-sm wrap-break-word",
                    Container = $"{spacing.Gap.Md} {positioning.Fixed} bottom-0 right-4 {spacing.Padding.Comfortable} z-50 {display.Flex} {flexbox.Col} max-w-md",
                    Toast = $"{spacing.Gap.Md} {shadows.Sm} transition-colors duration-200 {display.Flex} {flexbox.Align.Start} {spacing.Padding.Comfortable} ease-in-out",
                    ToastWidth = "max-w-[300px]",
                    Timestamp = "text-xs opacity-70",
                    CloseButton = $"{rounded.Full} flex-shrink-0 {spacing.Padding.Tight} hover:bg-[oklch(21%_0.006_285.885)]/10 dark:hover:bg-[oklch(97.807%_0.029_256.847)]/10 transition-colors focus:outline-none focus:ring-2 focus:ring-offset-1 focus:ring-offset-transparent focus:ring-current/40"
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
