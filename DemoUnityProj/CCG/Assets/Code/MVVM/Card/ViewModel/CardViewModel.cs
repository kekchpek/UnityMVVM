using CCG.Config;
using CCG.Core.Input;
using CCG.Core.Screen;
using CCG.MVVM.Card.Model;
using CCG.MVVM.Hand.Model;
using UnityEngine;
using UnityMVVM.ViewModelCore.Bindable;

namespace CCG.MVVM.Card.ViewModel
{
    public class CardViewModel : UnityMVVM.ViewModelCore.ViewModel, ICardViewModel
    {
        
        private const float HndArcMaxAngle = 120;
        
        private readonly IInputController _inputController;
        private readonly ICardModel _card;
        private readonly IHandModel _handModel;
        private readonly IScreenAdapter _screenAdapter;

        private Mutable<Vector2> _position = new();
        private Mutable<float> _rotation = new();
        private Mutable<bool> _isSelected = new();
        private Mutable<bool> _isOverBoard = new();
        
        
        private Vector2 _positionInHand;
        private float _rotationInHand;
        
        private float HandArcWidth => 100 * _handModel.MaxCardsCount * ConfigData.CardScale;
        
        public IBindable<Vector2> Position => _position;
        public IBindable<float> Rotation => _rotation;
        public IBindable<int> Health => _card.Health;
        public IBindable<int> Attack => _card.Attack;
        public IBindable<int> Mana => _card.Mana;
        public IBindable<string> Description => _card.Description;
        public IBindable<string> Title => _card.Title;
        public IBindable<Texture2D> Icon => _card.Icon;
        public IBindable<bool> IsSelected => _isSelected;
        public IBindable<bool> IsOverBoard => _isOverBoard;


        public CardViewModel(IInputController inputController, ICardPayload payload,
            IHandModel handModel, IScreenAdapter screenAdapter)
        {
            _inputController = inputController;
            _card = payload.Card;
            _handModel = handModel;
            _screenAdapter = screenAdapter;
            _card.Destroyed += Destroy;
            _card.IndexInHand.Bind(UpdateHandPosition);
            _handModel.IsArchPattern.Bind(UpdateHandPosition);
        }

        private void UpdateHandPosition()
        {
            var cardsCount = _handModel.GetCards().Length;
            var screenCenter = _screenAdapter.ScreenActiveAreaWidth / 2f;
            if (_handModel.IsArchPattern.Value)
            {
                var rotationStep = HndArcMaxAngle / _handModel.MaxCardsCount;
                var rotation = 0 - rotationStep * (0.5f * (cardsCount - 1) - _card.IndexInHand.Value);
                var y = 141 * ConfigData.CardScale * Mathf.Cos(rotation * Mathf.PI / 180f);
                var x = screenCenter + HandArcWidth / 2f * Mathf.Sin(rotation * Mathf.PI / 180f);
                SetPositionAndRotationInHand(
                    _screenAdapter.ScreenActiveAreaToWorld(new Vector2(x, y)), -rotation);
            }
            else
            {
                var positionStepX = 200f * ConfigData.CardScale;
                var positionX = screenCenter - positionStepX * ((cardsCount - 1) / 2f - _card.IndexInHand.Value);
                SetPositionAndRotationInHand(
                    _screenAdapter.ScreenActiveAreaToWorld(new Vector2(positionX, 
                        141f * ConfigData.CardScale)), 0f);
            }
        }

        public void OnMouseClickDown()
        {
            _inputController.MousePositionChanged += OnMousePositionChanged;
            _inputController.MouseUp += MouseUp;
            _isSelected.Value = true;
            _rotation.Value = 0;
            _position.Value = _inputController.MousePosition;
        }

        public void OnCardEnterBoard()
        {
            _isOverBoard.Value = true;
        }

        public void OnCardExitBoard()
        {
            _isOverBoard.Value = false;
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

        private void OnMousePositionChanged(Vector2 pos)
        {
            _position.Value = pos;
        }

        private void MouseUp()
        {
            _inputController.MousePositionChanged -= OnMousePositionChanged;
            _inputController.MouseUp -= MouseUp;
            _isSelected.Value = false;
            if (_isOverBoard.Value)
            {
                _card.Play();
            }
            else
            {
                UpdateHandPosition();
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