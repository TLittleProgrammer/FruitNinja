using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Runtime.Infrastructure.Combo
{
    [RequireComponent(typeof(RectTransform))]
    public class ComboView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fruitsCounter;
        [SerializeField] private TMP_Text _xCounterText;
        [SerializeField] private Animator _animator;

        private readonly int _showCombo = Animator.StringToHash("ComboShow");
        private RectTransform _rectTransform;
        private string _fruitsCounterInitialText;
        private string _xCounterInitialText;
        private Vector2 _rectSize;

        public RectTransform RectTransform => _rectTransform;
        public Vector2 RectSize => _rectSize;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _fruitsCounterInitialText = _fruitsCounter.text;
            _xCounterInitialText = _xCounterText.text;

            _rectSize = new Vector2(_rectTransform.rect.width / 2f, _rectTransform.rect.height / 2f);
        }
        
        public void ShowCombo(int fruits)
        {
            gameObject.SetActive(true);
            StartCoroutine(PlayAnimation(fruits));
        }

        public void SetPosition(Vector2 position)
        {
            _rectTransform.position = position;
        }

        private IEnumerator PlayAnimation(int fruits)
        {
            _fruitsCounter.text = String.Format(_fruitsCounter.text, fruits);
            _xCounterText.text = String.Format(_xCounterText.text, fruits);

            _animator.Play(_showCombo);

            yield return new WaitForSeconds(1f);
            
            gameObject.SetActive(false);

            _fruitsCounter.text = _fruitsCounterInitialText;
            _xCounterText.text  = _xCounterInitialText;
        }
        
        public class Pool : MonoMemoryPool<ComboView>
        {
        }
    }
}