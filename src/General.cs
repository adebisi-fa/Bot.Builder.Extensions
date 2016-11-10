using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adfa.Bot.Builder.Extensions
{
    public static partial class DialogContextExtensions
    {
        public static IEnumerable<ChoiceOption> ToChoiceOptions(this IEnumerable<string> options)
        {
            for (int i = 1; i <= options.Count(); i++)
            {
                var choiceString = options.ElementAt(i - 1);

                var textDescription = new string[] { choiceString, choiceString };
                if (choiceString.Contains("|"))
                    textDescription = choiceString.Split('|');

                var choice = ChoiceOption.New(textDescription[0], textDescription[1]);
                choice.Index = i;
                yield return choice;
            }
        }

        public static bool TryParseAndQuit<T>(this IDialogContext context, IMessageActivity activity, T value = default(T)) => TryParseAndQuit(context, activity.Text, value: value);

        public static bool TryParseAndQuit<T>(this IDialogContext context, string message, bool forceQuit = true, T value = default(T))
        {
            var quitWords = new string[] { "quit", "exit", "return", "cancel", "leave", "terminate", "abandon", "end", "bye", "close", "done" };
            if (quitWords.Any(w => message.ToLower().Contains(w)))
            {
                if (forceQuit)
                    context.Done<T>(value);
                return true;
            }
            return false;
        }
    }
}
