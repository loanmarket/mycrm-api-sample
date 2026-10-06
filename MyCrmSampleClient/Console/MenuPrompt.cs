using System;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace MyCrmSampleClient.Console;

/// <summary>Signals navigation to the parent menu, independently of request cancellation.</summary>
public sealed class MenuBackException : Exception
{
    public MenuBackException() : base("Return to the previous menu.") { }
}

/// <summary>Adds Escape navigation to selection, text and confirmation prompts.</summary>
public static class MenuPrompt
{
    public static T Show<T>(IPrompt<T> prompt)
    {
        var console = AnsiConsole.Console;

        try
        {
            return prompt.Show(new EscapeConsole(console));
        }
        catch (MenuBackException)
        {
            // Spectre's list prompt skips cursor restoration when input throws.
            // Clear the abandoned prompt after its render hook has been disposed.
            console.Cursor.Show(true);
            console.Clear(true);
            throw;
        }
    }

    public static T Ask<T>(string text) => Show(new TextPrompt<T>(text));

    public static bool Confirm(string text, bool defaultValue = true) =>
        Show(new ConfirmationPrompt(text) { DefaultValue = defaultValue });

    private sealed class EscapeConsole(IAnsiConsole console) : IAnsiConsole
    {
        public Profile Profile => console.Profile;
        public IAnsiConsoleCursor Cursor => console.Cursor;
        public IAnsiConsoleInput Input { get; } = new EscapeInput(console.Input);
        public IExclusivityMode ExclusivityMode => console.ExclusivityMode;
        public RenderPipeline Pipeline => console.Pipeline;

        public void Clear(bool home) => console.Clear(home);

        public void Write(IRenderable renderable) => console.Write(renderable);
    }

    private sealed class EscapeInput(IAnsiConsoleInput input) : IAnsiConsoleInput
    {
        public bool IsKeyAvailable() => input.IsKeyAvailable();

        public ConsoleKeyInfo? ReadKey(bool intercept) => Check(input.ReadKey(intercept));

        public async Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken) =>
            Check(await input.ReadKeyAsync(intercept, cancellationToken).ConfigureAwait(false));

        private static ConsoleKeyInfo? Check(ConsoleKeyInfo? key)
        {
            if (key?.Key == ConsoleKey.Escape) throw new MenuBackException();

            return key;
        }
    }
}
