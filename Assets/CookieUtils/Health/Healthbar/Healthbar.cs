using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using CookieUtils.Timer;
using CookieUtils;

namespace CookieUtils.Health.Healthbar
{
    public class Healthbar : MonoBehaviour
    {
        public int maxValue = 100;

        public int Value
        {
            get => _value;
            set
            {
                if (value <= 0) return;
                
                _value = Math.Clamp(value, 0, maxValue);
                
                if (_timer == null || !_timer.gameObject.activeSelf)
                    _timer = this.CreateTimer(1f, destroyOnFinish: false, ignoreNullAction: true);
                
                float targetValue = (float)value / maxValue;
                _timer.Restart();
                _timer.OnComplete = () => TweenFill(dealtDamage, targetValue);
                TweenFill(foreground, targetValue);
            }
        }

        private int _value;

        [SerializeField] private Image foreground;
        [SerializeField] private Image dealtDamage;

        private Timer.Timer _timer;

        private void TweenFill(Image image, float targetValue)
        {
            DOVirtual.Float(image.fillAmount, targetValue, 0.1f,
                v => image.fillAmount = v);
        }

        private void OnDestroy()
        {
            if (_timer != null) _timer.Release();
        }
    }
}