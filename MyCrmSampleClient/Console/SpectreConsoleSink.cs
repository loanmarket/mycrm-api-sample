using System;
using System.Globalization;
using System.IO;
using System.Text;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Parsing;
using Spectre.Console;

namespace MyCrmSampleClient.Console;

public sealed class SpectreConsoleSink : ILogEventSink
{
    private readonly IAnsiConsole _console;
    private readonly IFormatProvider _formatProvider;
    
    public SpectreConsoleSink(
        IAnsiConsole console,
        IFormatProvider formatProvider = null)
    {
        _console = console ?? throw new ArgumentNullException(nameof(console));
        _formatProvider = formatProvider ?? CultureInfo.InvariantCulture;
    }

    public void Emit(LogEvent logEvent)
    {
        var line = BuildMarkup(logEvent);
     
        if (string.IsNullOrEmpty(line))
        {
            return;
        }
        
        _console.Markup(line);
    }

    private string RenderProperty(PropertyToken propertyToken, LogEvent logEvent)
    {
        if (!logEvent.Properties.TryGetValue(propertyToken.PropertyName, out var propertyValue))
        {
            using var writer = new StringWriter(_formatProvider);
            propertyToken.Render(logEvent.Properties, writer, _formatProvider);
            return writer.ToString();
        }

        // Serilog renders string scalars with surrounding quotes by default.
        // In this console sink we prefer the literal string representation unless the template
        // explicitly specifies a format (e.g. :j for JSON).
        if (propertyValue is ScalarValue { Value: string } && string.IsNullOrEmpty(propertyToken.Format))
        {
            using var writer = new StringWriter(_formatProvider);
            propertyValue.Render(writer, "l", _formatProvider);
            var rendered = writer.ToString();
            return propertyToken.Alignment.HasValue
                ? ApplyAlignment(rendered, propertyToken.Alignment.Value)
                : rendered;
        }

        using (var writer = new StringWriter(_formatProvider))
        {
            propertyToken.Render(logEvent.Properties, writer, _formatProvider);
            return writer.ToString();
        }
    }

    private static string ApplyAlignment(string value, Alignment alignment)
    {
        if (string.IsNullOrEmpty(value) || alignment.Width <= 0)
        {
            return value;
        }

        if (value.Length >= alignment.Width)
        {
            return value;
        }

        return alignment.Direction == AlignmentDirection.Left
            ? value.PadRight(alignment.Width)
            : value.PadLeft(alignment.Width);
    }

    private string BuildMarkup(LogEvent logEvent)
    {
        var markup = new StringBuilder();

        var timestampStyle = GetTimestampStyleName();
        AppendStyled(markup, timestampStyle, "[");
        AppendStyled(markup, timestampStyle, logEvent.Timestamp.ToString("HH:mm:ss", _formatProvider));
        AppendStyled(markup, timestampStyle, " ");
        AppendStyled(markup, GetLevelStyleName(logEvent.Level), GetLevelMoniker(logEvent.Level));
        AppendStyled(markup, timestampStyle, "] ");

        foreach (var token in logEvent.MessageTemplate.Tokens)
        {
            switch (token)
            {
                case TextToken literal:
                    AppendStyled(markup, null, literal.Text);
                    break;
                case PropertyToken propertyToken:
                    AppendStyled(markup, GetPropertyStyleName(propertyToken, logEvent), RenderProperty(propertyToken, logEvent));
                    break;
            }
        }

        if (logEvent.Exception != null)
        {
            markup.AppendLine();
            AppendStyled(markup, GetExceptionStyleName(), logEvent.Exception.ToString());
        }

        markup.AppendLine();
        return markup.ToString();
    }

    private static void AppendStyled(StringBuilder output, string styleName, string value)
    {
        if (output == null || string.IsNullOrEmpty(value))
        {
            return;
        }

        var escaped = Markup.Escape(value);
        if (string.IsNullOrWhiteSpace(styleName))
        {
            output.Append(escaped);
            return;
        }

        output.Append('[')
            .Append(styleName)
            .Append(']')
            .Append(escaped)
            .Append("[/]");
    }

    private static string GetPropertyStyleName(PropertyToken propertyToken, LogEvent logEvent)
    {
        if (!logEvent.Properties.TryGetValue(propertyToken.PropertyName, out var propertyValue))
        {
            return null;
        }

        if (propertyValue is ScalarValue { Value: null })
        {
            return "grey";
        }

        if (propertyValue is ScalarValue { Value: string })
        {
            return "deepskyblue1";
        }

        if (propertyValue is ScalarValue { Value: bool })
        {
            return "yellow";
        }

        if (propertyValue is ScalarValue { Value: sbyte or byte or short or ushort or int or uint or long or ulong or float or double or decimal })
        {
            return "teal";
        }

        if (propertyValue is SequenceValue || propertyValue is StructureValue || propertyValue is DictionaryValue)
        {
            return "aqua";
        }

        return "deepskyblue1";
    }

    private static string GetLevelStyleName(LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Fatal => "red bold",
            LogEventLevel.Error => "red",
            LogEventLevel.Warning => "yellow",
            LogEventLevel.Debug => "grey",
            LogEventLevel.Verbose => "grey",
            _ => null
        };
    }

    private static string GetTimestampStyleName() => "grey";

    private static string GetExceptionStyleName() => "red";

    private static string GetLevelMoniker(LogEventLevel level)
    {
        return level switch
        {
            LogEventLevel.Verbose => "VRB",
            LogEventLevel.Debug => "DBG",
            LogEventLevel.Information => "INF",
            LogEventLevel.Warning => "WRN",
            LogEventLevel.Error => "ERR",
            LogEventLevel.Fatal => "FTL",
            _ => "UNK"
        };
    }
}

public static class SpectreConsoleSinkExtensions
{
    public static LoggerConfiguration SpectreConsole(
        this LoggerSinkConfiguration sinkConfiguration,
        LogEventLevel restrictedToMinimumLevel = LogEventLevel.Verbose,
        IFormatProvider formatProvider = null,
        IAnsiConsole console = null)
    {
        if (sinkConfiguration == null)
        {
            throw new ArgumentNullException(nameof(sinkConfiguration));
        }

        return sinkConfiguration.Sink(
            new SpectreConsoleSink(console ?? AnsiConsole.Console, formatProvider),
            restrictedToMinimumLevel);
    }
}
