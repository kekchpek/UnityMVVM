using System;
using JetBrains.Annotations;

namespace SurvivedWarrior.MVVM.Models.Time
{
    public class CallbackCancelSource
    {
        private readonly Action _cancelAction;

        public CallbackCancelSource([NotNull] Action cancelAction)
        {
            _cancelAction = cancelAction ?? throw new ArgumentNullException(nameof(cancelAction));
        }

        public void Cancel() => _cancelAction();
    }
}