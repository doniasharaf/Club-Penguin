using Data;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;

namespace GamePlay
{
    [RequireComponent(typeof(Image), typeof(Button))]

    /// <summary>
    /// Represents a single card in the memory game.
    /// Handles its visual state (front/back), interactivity, and flip animations.
    /// Raises <see cref="CardClicked"/> when the user clicks the card.
    /// </summary>
    public class CardBehavior : MonoBehaviour
    {
        public UnityAction<CardBehavior> CardClicked;

        [SerializeField] private Sprite backSprite;

        private Image _cardImage;
        private Button _button;
        private bool _isFlipped;
        private bool _isActive;
        private bool _isAnimating;
        public CardData Data { get; set; }
        private void Awake()
        {
            _cardImage = GetComponent<Image>();
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnCardClicked);
            _isActive = true;
        }

        public void DeactivateCard()
        {
            _button.interactable = false;
            _isActive = false;
        }

        public void ShowCard()
        {
            AnimateFlip(() =>
            {
                _cardImage.sprite = Data.CardSprite;
                _isFlipped = true;
            });
        }

        public void HideCard()
        {
            AnimateFlip(() =>
             {
                 _cardImage.sprite = backSprite;
                 _isFlipped = false;
             });
        }

        private void FlipCard()
        {
            if (_isAnimating || !_isActive) return;
            if (_isFlipped)
            {
                HideCard();
            }
            else
            {
                ShowCard();
            }
        }

        private void AnimateFlip(Action action)
        {
            _isAnimating = true;

            RectTransform rt = _cardImage.rectTransform;

            float startY = 0f;
            float midY = startY + 180;
            float endY = startY + 360;

            Quaternion startRot = Quaternion.Euler(0f, startY, 0f);
            Quaternion midRot = Quaternion.Euler(0f, midY, 0f);
            Quaternion endRot = Quaternion.Euler(0f, endY, 0f);
            Tween.Rotate(rt, startRot, midRot, 0.2f, Tween.EaseInOutQuad, () =>
            {
                action?.Invoke();
                Tween.Rotate(rt, midRot, endRot, 0.2f, Tween.EaseInOutQuad, () =>
                {
                    _isAnimating = false;
                });
            });
        }
        private void OnCardClicked()
        {
            if (_isAnimating || !_isActive || _isFlipped) return;
            FlipCard();
            CardClicked?.Invoke(this);
        }
    }
}