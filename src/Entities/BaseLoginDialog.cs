using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adfa.Bot.Builder.Extensions
{
    [Serializable]
    public abstract class BaseLoginDialog : IDialog<bool>
    {
        public string LoginUsernameDescription { get; set; }
        public string LoginRequiredMessage { get; set; }
        public string[] RequiredRoles { get; set; }
        public string LoginOperationDescription { get; set; }

        public async Task StartAsync(IDialogContext context)
        {
            if (!string.IsNullOrEmpty(LoginRequiredMessage))
                await context.PostAsync(LoginRequiredMessage);

            await context.PostAsync($"To login{(string.IsNullOrEmpty(LoginRequiredMessage) ? "" : " with a more priviledged account")}, kindly enter {LoginUsernameDescription}.");
            context.Wait<IMessageActivity>(ReceiveUsernameAsync);
        }

        protected abstract Task ReceiveUsernameAsync(IDialogContext context, IAwaitable<IMessageActivity> result);
    }
}
