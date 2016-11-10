using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adfa.Bot.Builder.Extensions
{
    public static class DialogContextExtension
    {
        public static void RegisterLoginDialog(this IContainer container, BaseLoginDialog loginDialog)
        {
            var builder = new ContainerBuilder();
            builder.Register(c => loginDialog).As<BaseLoginDialog>().SingleInstance();
            builder.Update(container);
        }

        public static bool IsInRole(this IBotData context, string role)
        {
            var userContext = LoadUser(context);
            return userContext.Roles != null && userContext.Roles.Any(
                r => r.ToLower() == role.ToLower()
            );
        }

        public static bool IsAuthenticated(this IBotData context)
        {
            if (context == null)
                return false;

            var userContext = LoadUser(context);
            return !string.IsNullOrEmpty(userContext?.Username);
        }

        public static UserContext LoadUser(this IBotData context)
        {
            UserContext user;

            if (!context.UserData.TryGetValue("User", out user))
                return new UserContext();

            return user;
        }

        public static void SaveUser(this IBotData context, string username, string[] roles)
        {
            context.UserData.SetValue("User", new UserContext { Roles = roles, Username = username });
        }

        public static void ClearUser(this IBotData context)
        {
            context.UserData.RemoveValue("User");
        }

        public static async Task RequiresAnyRole(this IDialogContext context, string[] roles, ResumeAfter<bool> resumeAfter, string loginOperationDescription = "carry out this operation")
        {
            if (roles == null || roles.Length == 0)
                throw new ArgumentException("Roles can neither be null nor be an empty array.", nameof(roles));

            if (resumeAfter == null)
                throw new ArgumentNullException(nameof(resumeAfter));

            var dialog = GetLoginDialog();
            if (dialog != null)
            {
                dialog.LoginRequiredMessage = $"You must be a member of all or any of {string.Join(", ", roles)} role(s) to {loginOperationDescription}";
                dialog.LoginOperationDescription = loginOperationDescription;
                dialog.RequiredRoles = roles;
            }

            if (!roles.Any(r => context.IsInRole(r)))
                if (dialog != null)
                    context.Call(
                        dialog,
                        resumeAfter
                    );
                else
                    await resumeAfter(context, new MockupAwaitable<bool>(false));
            else
                await resumeAfter(context, new MockupAwaitable<bool>(true));
        }

        public static async Task RequiresAuthentication(this IDialogContext context, ResumeAfter<bool> resumeAfter, string loginOperationDescription = "carry out this operation")
        {
            if (resumeAfter == null)
                throw new ArgumentNullException(nameof(resumeAfter));

            var dialog = GetLoginDialog();
            if (dialog != null)
            {
                dialog.LoginRequiredMessage = $"You are required to login before you can {loginOperationDescription}.";
                dialog.LoginOperationDescription = loginOperationDescription;
                dialog.RequiredRoles = new string[] { };
            }

            if (!context.IsAuthenticated())
                if (dialog != null)
                    context.Call(
                        dialog,
                        resumeAfter
                    );
                else
                    await resumeAfter(context, new MockupAwaitable<bool>(false));
            else
                await resumeAfter(context, new MockupAwaitable<bool>(true));
        }

        private static BaseLoginDialog GetLoginDialog()
        {
            BaseLoginDialog loginDialog;
            return Conversation.Container.TryResolve<BaseLoginDialog>(out loginDialog) ? loginDialog : null;
        }
    }
}
