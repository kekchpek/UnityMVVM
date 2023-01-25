using CCG.MVVM.Card;
using CCG.MVVM.Card.View;
using UnityEngine;

namespace CCG.MVVM.Components
{
    public class BoardDetector : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            other.GetComponent<ICardView>().OnEnterToBoard();
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            other.GetComponent<ICardView>().OnExitFromBoard();
        }
    }
}