using AsyncReactAwait.Bindable;
using CCG.Config;
using CCG.Models.Hand.Model;
using CCG.MVVM.Card.Model;
using UnityEngine;

namespace CCG.MVVM.Card.ViewModel
{
    public class CardViewModel : UnityMVVM.ViewModelCore.ViewModel, ICardViewModel
    {
        
        private const float HndArcMaxAngle = 120;
        
        private readonly ICardModel _card;
        private readonly IHandModel _handModel;

        private readonly Mutable<Vector2> _position = new();
        private readonly Mutable<float> _rotation = new();
        private readonly Mutable<bool> _isSelected = new();
        private readonly Mutable<bool> _isOverBoard = new();
        
        
        private Vector2 _positionInHand;
        private float _rotationInHand;
        
        private float HandArcWidth => 0.1f * _handModel.MaxCardsCount * ConfigData.CardScale;
        
        public IBindable<Vector2> PositionInHand => _position;
        public IBindable<float> RotationInHand => _rotation;
        public IBindable<int> Health => _card.Health;
        public IBindable<int> Attack => _card.Attack;
        public IBindable<int> Mana => _card.Mana;
        public IBindable<string> Description => _card.Description;
        public IBindable<string> Title => _card.Title;
        public IBindable<Texture2D> Icon => _card.Icon;
        public IBindable<bool> IsSelected => _isSelected;
        public IBindable<bool> IsOverBoard => _isOverBoard;


        public CardViewModel(
            ICardPayload payload,
            IHandModel handModel)
        {
            _card = payload.Card;
            _handModel = handModel;
            _card.Destroyed += Destroy;
            _card.IndexInHand.Bind(UpdateHandPosition);
            _handModel.CardsCount.Bind(UpdateHandPosition);
            _handModel.IsArchPattern.Bind(UpdateHandPosition);
        }

        private void UpdateHandPosition()
        {
            var cardsCount = _handModel.GetCards().Length;
            var screenCenter = 0.5f;
            if (_handModel.IsArchPattern.Value)
            {
                var rotationStep = HndArcMaxAngle / _handModel.MaxCardsCount;
                var rotation = 0 - rotationStep * (0.5f * (cardsCount - 1) - _card.IndexInHand.Value);
                var y = 0.1f * ConfigData.CardScale * Mathf.Cos(rotation * Mathf.PI / 180f);
                var x = screenCenter + HandArcWidth / 2f * Mathf.Sin(rotation * Mathf.PI / 180f);
                SetPositionAndRotationInHand(
                    new Vector2(x, y), -rotation);
            }
            else
            {
                var positionStepX = 0.07f * ConfigData.CardScale;
                var positionX = screenCenter - positionStepX * ((cardsCount - 1) / 2f - _card.IndexInHand.Value);
                SetPositionAndRotationInHand(
                    new Vector2(positionX, 0.1f * ConfigData.CardScale), 0f);
            }
        }

        public void OnMouseClickDown()
        {
            _isSelected.Value = true;
        }

        public void OnCardEnterBoard()
        {
            _isOverBoard.Value = true;
        }

        public void OnCardExitBoard()
        {
            _isOverBoard.Value = false;
        }

        public void OnMouseClickUp()
        {
            _isSelected.Value = false;
            if (_isOverBoard.Value)
            {
                _card.Play();
            }
        }

        private void SetPositionAndRotationInHand(Vector2 position, float rotation)
        {
            _positionInHand = position;
            _rotationInHand = rotation;
            if (!_isSelected.Value)
            {
                _position.Value = _positionInHand;
                _rotation.Value = _rotationInHand;
            }
        }

        protected override void OnDestroyInternal()
        {
            _card.Destroyed -= Destroy;
            _card.IndexInHand.Unbind(UpdateHandPosition);
            _handModel.IsArchPattern.Unbind(UpdateHandPosition);
            base.OnDestroyInternal();
        }
    }
}