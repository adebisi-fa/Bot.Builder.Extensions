using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adfa.Bot.Builder.Extensions
{
    public class MockupAwaitable
    {
        private MockupAwaitable()
        { }

        public static MockupAwaitable<IMessageActivity> CreateMessageActivity(IDialogContext context, string text)
        {
            var activity = context.MakeMessage();
            activity.Text = text;
            return new MockupAwaitable<IMessageActivity>(activity);
        }
    }
    public class MockupAwaitable<T> : IAwaitable<T>
    {
        T _instance;
        public MockupAwaitable(T instance)
        {
            _instance = instance;
        }

        public IAwaiter<T> GetAwaiter()
        {
            return new MockupAwaiter<T>(_instance);
        }
    }

    public class MockupAwaiter<T> : IAwaiter<T>
    {
        T _instance;

        public MockupAwaiter(T instance)
        {
            _instance = instance;
        }

        public bool IsCompleted
        {
            get
            {
                return true;
            }
        }

        public T GetResult()
        {
            return _instance;
        }

        public void OnCompleted(Action continuation)
        {
            if (continuation != null)
                continuation();
        }
    }

}
