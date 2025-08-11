using System;
using CookieUtils.Runtime.Timer;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Runtime.Health.Healthbar
{
    public class Healthbar : MonoBehaviour
    {
        public float maxValue = 100;
        
        [Space(10f)]
        [SerializeField, Foldout("References")] private Image foreground;
        [SerializeField, Foldout("References")] private Image dealtDamage;

        public float Value
        {
            get => _value;
            set
            {

                if (value < Value) _timeSinceHit = 0;
                
                _value = Math.Clamp(value, 0, maxValue);
            }
        }

        private float _timeSinceHit = float.MinValue; 
        
        private float _value;

        private void TweenFill(Image image, float targetValue)
        {
            DOVirtual.Float(image.fillAmount, targetValue, 0.1f,
                v =>
                {
                    if (image)    
                        image.fillAmount = v;
                });
        }

        private void Update()
        {
            _timeSinceHit += Time.deltaTime;
            if (_timeSinceHit > 1)
            {
                _timeSinceHit = float.MinValue;
                TweenFill(dealtDamage, _value / maxValue);
            }
        }
    }
}