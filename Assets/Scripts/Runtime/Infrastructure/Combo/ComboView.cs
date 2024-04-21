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
        [SerializeField] private TMP_Text _fruisCounter;
        [SerializeField] private TMP_Text _xCounterText;
        [SerializeField] private Animator _animator;

        private readonly int _showCombo = Animator.StringToHash("ShowCombo");
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
        
        public void ShowCombo(Vector3 screenPosition, int fruits, int xCounter)
        {
            gameObject.SetActive(true);
            StartCoroutine(PlayAnimation(screenPosition, fruits, xCounter));
        }

        private IEnumerator PlayAnimation(Vector3 screenPosition, int fruits, int xCounter)
        {
            _rectTransform.anchoredPosition = screenPosition;
            _fruisCounter.text = String.Format(_fruisCounter.text, fruits);
            _xCounterText.text = String.Format(_xCounterText.text, xCounter);

            _animator.Play(_showCombo);

            yield return new WaitForSeconds(1f);
            
            gameObject.SetActive(false);
        }
        
        public class Pool : MonoMemoryPool<ComboView>
        {
        }
    }
}