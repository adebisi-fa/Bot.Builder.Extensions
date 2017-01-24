using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adfa.Bot.Builder.Extensions
{
    [Serializable]
    public class ChoiceOption
    {
        public static ChoiceOption New(string text, string description = null) =>
           new ChoiceOption { Text = text, Description = description ?? text };

        public int Index { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return $"{Index} - {Description}";
        }
    }
}
