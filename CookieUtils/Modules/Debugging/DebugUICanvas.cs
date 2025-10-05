using UnityEngine;
using UnityEngine.InputSystem;

namespace CookieUtils.Debugging
{
    internal class DebugUICanvas : MonoBehaviour
    {
        private DebugUIPanel _panelPrefab;
        private DebugUIPanel _panel;
        private Renderer _renderer;
        private Canvas _canvas;
        private Bounds Bounds => _renderer ? _renderer.bounds : new Bounds(transform.position, Vector3.one);
        private bool _isHovering;
        private bool _isLockedOn;
        private float _timeSinceHovered = float.PositiveInfinity;
        private float _timeSinceLastCheck;
        private float _mouseCheckTime;
        private float _hideTime;

        private void Awake()
        {
            _panelPrefab = Resources.Load<DebugUIPanel>("DebugUI/Prefabs/Panel");
            _mouseCheckTime = CookieDebug.DebuggingSettings.mouseCheckTime;
            _hideTime = CookieDebug.DebuggingSettings.hideTime;
            _timeSinceLastCheck = Random.Range(0, _mouseCheckTime);
            CookieDebug.OnLockedOn += OnLockOn;
        }

        private void OnLockOn()
        {
            if (_isHovering)
                _isLockedOn = !_isLockedOn;
        }
        
        public DebugUIPanel GetPanel(GameObject host)
        {
            if (_panel) return _panel;
            
            _panel = host.GetComponentInChildren<DebugUIPanel>();
            if (_panel) return _panel;

            _renderer = host.GetComponentInChildren<Renderer>();
            
            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.WorldSpace;
            _canvas.worldCamera = Camera.main;
            
            if (!_panelPrefab) _panelPrefab = Resources.Load<DebugUIPanel>("DebugUI/Prefabs/Panel");
            
            _panel = Instantiate(_panelPrefab, transform);
            _panel.gameObject.SetActive(false);
            
            return _panel;
        }

        private void Update()
        {
            if (!CookieDebug.IsDebugMode) return;
            
            _timeSinceLastCheck += Time.unscaledDeltaTime;
            _timeSinceHovered += Time.unscaledDeltaTime;
            if (_timeSinceLastCheck < _mouseCheckTime) return;

            _timeSinceLastCheck = 0;
            
            var mousePos = Mouse.current.position.ReadValue();
            
            bool doesIntersect = Bounds.IntersectRay(
                _canvas.worldCamera.ScreenPointToRay(mousePos)
            );

            if (!doesIntersect && _canvas.enabled) {
                doesIntersect = _panel.Image.rectTransform.rect.Contains(
                    _panel.Image.rectTransform.InverseTransformPoint(_canvas.worldCamera.ScreenToWorldPoint(mousePos)));
            }

            _isHovering = doesIntersect;

            bool isEnabled = (_isLockedOn || _timeSinceHovered <= _hideTime) && CookieDebug.IsDebugMode;

            if (isEnabled) {
                var color = _isLockedOn ? new Color(0.15f, 0.15f, 0.15f, 0.8f) : new Color(0.1f, 0.1f, 0.1f, 0.8f);
                _panel.Image.color = color;
            }

            _canvas.enabled = isEnabled;
            _panel.gameObject.SetActive(isEnabled);

            if (_isHovering) _timeSinceHovered = 0f;
        }
    }
}
