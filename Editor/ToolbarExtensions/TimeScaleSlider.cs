// main toolbar cusotmization was only added in 6.3
#if UNITY_6000_3_OR_NEWER
using UnityEditor.Toolbars;
using UnityEngine;

namespace CookieUtils.Editor
{
    public static class TimeScaleSlider
    {
        private const string path = "Cookie Utils/Time Scale";
        private const float minTimeScale = 0f;
        private const float maxTimeScale = 2f;
        private const float unit = 1f / 100f;

        [MainToolbarElement(path, defaultDockPosition = MainToolbarDockPosition.Middle)]
        public static MainToolbarElement TimeSlider()
        {
            var content = new MainToolbarContent("Time Scale", "Time Scale");

            MainToolbarSlider slider = new(
                content,
                Time.timeScale / unit,
                minTimeScale / unit,
                maxTimeScale / unit,
                OnSliderValueChanged,
                true
            );

            slider.populateContextMenu = static menu =>
            {
                menu.ClearItems();

                menu.InsertAction(
                    0,
                    "Reset",
                    (action) =>
                    {
                        Time.timeScale = 1f;
                        MainToolbar.Refresh(path);
                    }
                );
            };

            return slider;
        }

        public static void OnSliderValueChanged(float newValue)
        {
            Time.timeScale = newValue * unit;
        }
    }
}
#endif
