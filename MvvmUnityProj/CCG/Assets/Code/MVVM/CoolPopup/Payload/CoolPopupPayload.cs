namespace CCG.MVVM.CoolPopup.Payload
{
    public class CoolPopupPayload : ICoolPopupPayload
    {
        public bool ThrowError { get; }

        public CoolPopupPayload(bool throwError)
        {
            ThrowError = throwError;
        }
        
    }
}