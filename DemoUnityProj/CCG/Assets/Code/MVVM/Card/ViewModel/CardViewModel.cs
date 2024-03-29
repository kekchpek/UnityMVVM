﻿using AsyncReactAwait.Bindable;
using CCG.Models.Hand.Model;
using CCG.MVVM.Card.Model;
using UnityEngine;

namespace CCG.MVVM.Card.ViewModel
{
    public class CardViewModel : UnityMVVM.ViewModelCore.ViewModel, ICardViewModel
    {
        
        private readonly ICardModel _card;
        private readonly IHandModel _handModel;

        private readonly Mutable<Vector2> _position = new();
        private readonly Mutable<float> _rotation = new();
        private readonly Mutable<bool> _isSelected = new();
        private readonly Mutable<bool> _isOverBoard = new();
        
        
        private Vector2 _positionInHand;
        private float _rotationInHand;
        
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
            _card.Destroyed += OnCardDestroyed;
            _card.IndexInHand.Bind(UpdateHandPosition);
            _handModel.CardsCount.Bind(UpdateHandPosition);
            _handModel.IsArchPattern.Bind(UpdateHandPosition);
        }

        private void UpdateHandPosition()
        {
            if (_handModel.IsArchPattern.Value)
            {
                var data = CardMath.GetArchTransform(
                    _card.IndexInHand.Value, 
                    _handModel.CardsCount.Value, 
                    _handModel.MaxCardsCount);
                SetPositionAndRotationInHand(data.position, data.angle);
            }
            else
            {
                var data = CardMath.GetRegularTransform(
                    _card.IndexInHand.Value,
                    _handModel.CardsCount.Value);
                SetPositionAndRotationInHand(data.position, data.angle);
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

        private void OnCardDestroyed(ICardModel _)
        {
            Destroy();
        }

        protected override void OnDestroyInternal()
        {
            _card.Destroyed -= OnCardDestroyed;
            _card.IndexInHand.Unbind(UpdateHandPosition);
            _handModel.IsArchPattern.Unbind(UpdateHandPosition);
            base.OnDestroyInternal();
        }
    }
}