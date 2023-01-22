namespace CCG.Models.Hand.Service
{
    public interface IHandService
    {
        void AddRandomCardToHand();
        void SwitchCardsPattern();
        void ChangeRandomCardStats();
        void ClearHand();
    }
}