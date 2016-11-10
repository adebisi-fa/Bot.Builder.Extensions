using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adfa.Bot.Builder.Extensions
{
    [Serializable]
    public class UserContext
    {
        public string Username { get; set; }
        public string[] Roles { get; set; }
    }
}
