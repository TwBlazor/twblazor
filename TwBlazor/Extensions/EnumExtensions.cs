// Copyright (c) 2025 Jack Shuter @ TwBlazor - twblazor.com
// Licensed under the MIT License. See LICENSE.txt in the project root for license information.

using System.ComponentModel;
using System.Reflection;

namespace TwBlazor.Extensions;

public static class EnumExtensions
{
    public static string GetDescriptionFromName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        if (field is null)
            return value.ToString();

        var attribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .SingleOrDefault() as DescriptionAttribute;

        return attribute?.Description ?? value.ToString();
    }

    public static T? GetNameFromDescription<T>(string description) where T : struct, Enum
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);

        var field = fields
            .SelectMany(f => f.GetCustomAttributes(typeof(DescriptionAttribute), false),
                (f, a) => new { Field = f, Att = a })
            .SingleOrDefault(a => ((DescriptionAttribute)a.Att).Description == description);

        if (field is null)
            return null;

        var constantValue = field.Field.GetRawConstantValue();
        if (constantValue is null)
            return null;

        return (T)constantValue;
    }
}