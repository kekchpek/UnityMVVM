using CCG.Config;
using CCG.MVVM.Card.ViewModel;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityMVVM;
using UnityMVVM.Pool;

namespace CCG.MVVM.Card.View
{
    [RequireComponent(typeof(RectTransform))]
    public class CardView : ViewBehaviour<ICardViewModel>, IPointerDownHandler, ICardView, IPoolableView
    {

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private float _changeStatAnimationTime;

        private Tween _attackTween;
        private int _attack;
        private Tween _healthTween;
        private int _health;
        private Tween _manaTween;
        private int _mana;

        private Tween _positionTween;
        private Tween _rotationTween;
        
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _attackText;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _manaText;
        [SerializeField] private RawImage _icon;
        [SerializeField] private GameObject _selectedParticles;
        [SerializeField] private GameObject _overBoardParticles;

        private bool _isSelected;
        private bool _isOverBoard;
        
        public void OnTakenFromPool()
        {
            gameObject.SetActive(true);
        }
        
        protected override void OnViewModelSet()
        {
            base.OnViewModelSet();
            
            SmartBind(ViewModel!.Health, SetHealth, false);
            SetHealthWithoutAnimation(ViewModel.Health.Value);
            
            SmartBind(ViewModel.Mana, SetMana, false);
            SetManaWithoutAnimation(ViewModel.Mana.Value);
            
            SmartBind(ViewModel.Attack, SetAttack, false);
            SetAttackWithoutAnimation(ViewModel.Attack.Value);
            
            SmartBind(ViewModel.Description, SetDescription);
            SmartBind(ViewModel.Title, SetTitle);
            SmartBind(ViewModel.Icon, SetIcon);
            SmartBind(ViewModel.IsSelected, SetIsSelected);
            SmartBind(ViewModel.IsOverBoard, SetIsOverBoard);
            SmartBind(ViewModel.Rotation, SetRotation);
            SmartBind(ViewModel.Position, SetPosition);
 
            transform.localScale *= ConfigData.CardScale;
        }

        private void SetPosition(Vector2 position)
        {
            _positionTween?.Kill();
            _positionTween = transform.DOMove(ViewModel!.Position.Value,
                (position - (Vector2)transform.position).magnitude / _moveSpeed);
        }

        private void SetRotation(float rotation)
        {
            _rotationTween?.Kill();
            _rotationTween = transform.DORotate(new Vector3(0f, 0f, ViewModel!.Rotation.Value),
                Mathf.Abs(transform.rotation.z - ViewModel.Rotation.Value) / _rotationSpeed);
        }

        private void SetHealth(int health)
        {
            _healthTween?.Kill();
            _healthTween = DOTween.To(() => _health, SetHealthWithoutAnimation, ViewModel!.Health.Value, _changeStatAnimationTime);
        }

        private void SetHealthWithoutAnimation(int health)
        {
            _health = health;
            _healthText.text = _health.ToString();
        }

        private void SetAttack(int attack)
        {
            _attackTween?.Kill();
            _attackTween = DOTween.To(() => _attack, SetAttackWithoutAnimation, ViewModel!.Attack.Value, _changeStatAnimationTime);
        }

        private void SetAttackWithoutAnimation(int attack)
        {
            _attack = attack;
            _attackText.text = _attack.ToString();
        }

        private void SetMana(int mana)
        {
            _manaTween?.Kill();
            _manaTween = DOTween.To(() => _mana, SetManaWithoutAnimation, ViewModel!.Mana.Value, _changeStatAnimationTime);
        }

        private void SetManaWithoutAnimation(int mana)
        {
            _mana = mana;
            _manaText.text = _mana.ToString();
        }

        private void SetDescription(string description)
        {
            _descriptionText.text = description;
        }

        private void SetTitle(string title)
        {
            _titleText.text = title;
        }

        private void SetIcon(Texture2D icon)
        {
            _icon.texture = icon;
        }

        private void SetIsSelected(bool isSelected)
        {
            _isSelected = isSelected;
            UpdateParticles();
        }

        private void SetIsOverBoard(bool isOverBoard)
        {
            _isOverBoard = isOverBoard;
            UpdateParticles();
        }


        private void UpdateParticles()
        {
            if (!_isSelected)
            {
                _selectedParticles.SetActive(false);
                _overBoardParticles.SetActive(false);
            }
            else
            {
                if (_isOverBoard)
                {
                    _selectedParticles.SetActive(false);
                    _overBoardParticles.SetActive(true);
                }
                else
                {
                    _selectedParticles.SetActive(true);
                    _overBoardParticles.SetActive(false);
                }
            }
        }
        
        public void OnEnterToBoard()
        {
            ViewModel!.OnCardEnterBoard();
        }

        public void OnExitFromBoard()
        {
            if (isActiveAndEnabled)
            {
                ViewModel!.OnCardExitBoard();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            ViewModel!.OnMouseClickDown();
        }

        protected override void OnViewModelClear()
        {
            base.OnViewModelClear();
            _attackTween.Kill();
            _healthTween.Kill();
            _manaTween.Kill();
            _positionTween.Kill();
            _rotationTween.Kill();
        }

        public void OnReturnToPool()
        {
            var go = gameObject;
            go.SetActive(false);
            transform.SetParent(null);
            DontDestroyOnLoad(go);
        }
    }
}