using System;
using System.Reflection;

namespace CCG.Tests.Editor.Core.SynchronizationContext
{
    public static class TestSynchronizationContext
    {

        private static MethodInfo _execMethod;

        public static void ExecutePendingTasks()
        {
            var ctx = System.Threading.SynchronizationContext.Current;
            if (_execMethod == null)
            {
                var ctxType = ctx.GetType();
                _execMethod = ctxType.GetMethod("Exec", BindingFlags.NonPublic | BindingFlags.Instance)!;
            }
            _execMethod.Invoke(ctx, Array.Empty<object>());
        }
        
        public static Exception TryExecutePendingTasks()
        {
            try
            {
                ExecutePendingTasks();
                return null;
            }
            catch (TargetInvocationException e)
            {
                return e.InnerException!;
            }
        }
        
    }
}