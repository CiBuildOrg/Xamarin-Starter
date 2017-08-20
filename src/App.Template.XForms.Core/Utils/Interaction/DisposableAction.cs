using System;

namespace App.Template.XForms.Core.Utils.Interaction
{
    public class DisposableAction : IDisposable
    {
        private readonly Action _action;

        public DisposableAction(Action action)
        {
            _action = action;
        }


        public void Dispose()
        {
            _action();
            GC.SuppressFinalize(this);
        }
    }
}