using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Infrastructure.Effects
{
    [RequireComponent(typeof(Image))]
    public sealed class BlurEffect : MonoBehaviour
    {
        private readonly int Size = Shader.PropertyToID("_Size");
        private Material _blurMaterial;

        private void Awake()
        {
            _blurMaterial = _blurMaterial ? _blurMaterial : GetComponent<Image>().material;
        }

        public void Initialize(float initialSize)
        {
            _blurMaterial = _blurMaterial ? _blurMaterial : GetComponent<Image>().material;
            SetBlurSize(initialSize);
        }

        public UniTask UpdateBlur(float targetValue, float duration)
        {
            float current = _blurMaterial.GetFloat(Size);
            return DOVirtual.Float(current, targetValue, duration, SetBlurSize).ToUniTask();
        }

        private void SetBlurSize(float value)
        {
            _blurMaterial.SetFloat(Size, value);
        }
    }
}