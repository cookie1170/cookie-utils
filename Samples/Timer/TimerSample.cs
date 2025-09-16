using CookieUtils.Timer;
using UnityEngine;

namespace Samples.Timer
{
    public class TimerSample : MonoBehaviour
    {
        private CountdownTimer _timer;
        private float _selectedTime = 10;
        
        private void Awake()
        {
            _timer = new(10, destroyCancellationToken);
            _timer.Start();
        }

        private void OnGUI()
        {
            GUILayout.Label($"Time: {_timer.GetDisplayTime()}");
            _selectedTime = GUILayout.HorizontalSlider(_selectedTime, 1, 20);
            if (GUILayout.Button("Restart")) {
                _timer.Restart(_selectedTime);
            }
        }
    }
}
