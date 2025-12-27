using CookieUtils.Timers;
using UnityEngine;

namespace CookieUtils.Samples.Timer
{
    public class TimerSample : MonoBehaviour
    {
        private float _selectedTime = 10;
        private CountdownTimer _timer;

        private void Awake()
        {
            _timer = new CountdownTimer(10).AddTo(this);
            _timer.Start();
        }

        private void OnGUI()
        {
            GUILayout.Label($"Time: {_timer.GetDisplayTime()}");
            _selectedTime = GUILayout.HorizontalSlider(_selectedTime, 1, 20);
            if (GUILayout.Button("Restart"))
                _timer.Restart(_selectedTime);
        }
    }
}
