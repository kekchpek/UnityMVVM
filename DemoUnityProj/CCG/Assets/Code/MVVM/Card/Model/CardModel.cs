using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityMVVM.ViewModelCore.Bindable;

namespace CCG.MVVM.Card.Model
{
    public class CardModel : ICardMutableModel
    {
        private Mutable<int> _health = new();
        private Mutable<int> _attack = new();
        private Mutable<int> _mana = new();
        private Mutable<int> _indexInHand = new();
        private Mutable<string> _description = new();
        private Mutable<string> _title = new();
        private Mutable<Texture2D> _icon = new();
        
        private bool _isPlayed;

        public IBindable<int> Health => _health;

        public IBindable<int> Attack => _attack;

        public IBindable<int> Mana => _mana;
        
        public IBindable<string> Description => _description;
        
        public IBindable<string> Title => _title;
        
        public IBindable<Texture2D> Icon => _icon;

        public IBindable<int> IndexInHand => _indexInHand;

        public event Action Played;
        public event Action Destroyed;

        public CardModel(int health, int attack, int mana,
            string description, string title, Texture2D icon)
        {
            _health.Value = health;
            _attack.Value = attack;
            _mana.Value = mana;
            _description.Value = description;
            _title.Value = title;
            _icon.Value = icon;
        }
        
        public void Play()
        {
            Played?.Invoke();
        }

        public void SetHealth(int health)
        {
            _health.Value = health;
        }

        public void SetAttack(int attack)
        {
            _attack.Value = attack;
        }

        public void SetMana(int mana)
        {
            _mana.Value = mana;
        }

        public void SetDescription(string desc)
        {
            _description.Value = desc;
        }

        public void SetTitle(string title)
        {
            _title.Value = title;
        }

        public void SetIcon(Texture2D icon)
        {
            _icon.Value = icon;
        }

        public void SetIndexInHand(int i)
        {
            _indexInHand.Value = i;
        }

        public void Destroy()
        {
            Destroyed?.Invoke();
        }
    }
}