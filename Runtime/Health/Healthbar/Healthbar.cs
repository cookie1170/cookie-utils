using System;
using CookieUtils.Timer;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace CookieUtils.Health.Healthbar
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
                if (!didAwake) return;
                
                if (value <= 0) return;
                
                if (value < Value) timer.Restart();
                
                timer ??= new CountdownTimer(this, 1f, destroyOnComplete: false, ignoreNullAction: true);
                
                float targetValue = value / maxValue;
                timer.OnComplete = () => TweenFill(dealtDamage, targetValue);
                TweenFill(foreground, targetValue);
                
                _value = Math.Clamp(value, 0, maxValue);
            }
        }

        private float _value;

        private CountdownTimer timer;

        private void TweenFill(Image image, float targetValue)
        {
            DOVirtual.Float(image.fillAmount, targetValue, 0.1f,
                v => image.fillAmount = v);
        }
    }
}