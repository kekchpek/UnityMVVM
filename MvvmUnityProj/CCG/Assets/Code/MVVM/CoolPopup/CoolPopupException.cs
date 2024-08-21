using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace CCG.MVVM.CoolPopup
{
    public class CoolPopupException : Exception
    {
        public CoolPopupException()
        {
        }

        protected CoolPopupException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CoolPopupException(string message) : base(message)
        {
        }

        public CoolPopupException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}