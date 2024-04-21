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

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _fruitsCounterInitialText = _fruitsCounter.text;
            _xCounterInitialText = _xCounterText.text;
        }
        
        public void ShowCombo(Vector3 screenPosition, int fruits)
        {
            gameObject.SetActive(true);
            StartCoroutine(PlayAnimation(screenPosition, fruits));
        }

        public void SetPosition(Vector2 position)
        {
            _rectTransform.anchoredPosition = position;
        }

        private IEnumerator PlayAnimation(Vector3 screenPosition, int fruits)
        {
            //_rectTransform.anchoredPosition = screenPosition;
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