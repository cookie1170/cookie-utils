using UnityEngine;
using UnityEngine.InputSystem;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal class DebugUI_Canvas : MonoBehaviour
    {
        private const float DefaultYOffset = 1;

        private const float YOffset = 0.75f;
        private static readonly Prefab<DebugUI_Panel> PanelPrefab = "Panel";

        private Canvas _canvas;
        private float _hideTime;
        private GameObject _host;
        private bool _isHovering;
        private bool _isLockedOn;
        private float _mouseCheckTime;
        private DebugUI_Panel _panel;
        private Renderer _renderer;
        private State _state = State.Hidden;
        private float _timeSinceHovered = float.PositiveInfinity;
        private float _timeSinceLastCheck;
        private Bounds Bounds => _renderer ? _renderer.bounds : new Bounds(transform.position, Vector3.one);

        private void Awake() {
            _mouseCheckTime = CookieDebug.DebuggingSettings.mouseCheckTime;
            _hideTime = CookieDebug.DebuggingSettings.hideTime;
            _timeSinceLastCheck = Random.Range(0, _mouseCheckTime);
        }

        private void Update() {
            if (!_host) {
                Clear();

                return;
            }

            if (!CookieDebug.IsDebugMode || !_panel) return;

            _timeSinceLastCheck += Time.unscaledDeltaTime;
            _timeSinceHovered += Time.unscaledDeltaTime;

            if (_state != State.Hidden) UpdatePosition();

            if (_timeSinceLastCheck < _mouseCheckTime) return;

            _timeSinceLastCheck = 0;

            switch (_state) {
                case State.Hidden: {
                    if (CheckIntersection()) ChangeState(State.Hovered);

                    break;
                }

                case State.Hovered: {
                    _isHovering = CheckIntersection();
                    if (_timeSinceHovered > _hideTime && !_isHovering) ChangeState(State.Hidden);

                    if (_isHovering) _timeSinceHovered = 0;

                    break;
                }
            }
        }

        private void OnEnable() {
            CookieDebug.OnLockedOn += OnLockOn;
            CookieDebug.OnDebugUICleared += Clear;
        }

        private void OnDisable() {
            CookieDebug.OnDebugUICleared -= Clear;
            CookieDebug.OnLockedOn -= OnLockOn;
            _panel?.lockButton?.onClick.RemoveListener(OnLockOn);
        }

        internal void Init(GameObject host) {
            _host = host;
        }

        internal void Clear() {
            Destroy(gameObject);
            _panel?.lockButton?.onClick.RemoveListener(OnLockOn);
        }

        private void OnLockOn() {
            if (CheckIntersection()) ChangeState(_state = _state == State.Locked ? State.Hovered : State.Locked);
        }

        internal DebugUI_Panel GetPanel() {
            if (_panel) return _panel;

            _renderer = _host.GetComponentInChildren<Renderer>();

            _canvas = gameObject.AddComponent<Canvas>();
            _canvas.renderMode = RenderMode.WorldSpace;
            _canvas.worldCamera = Camera.main;

            _panel = Instantiate<DebugUI_Panel>(PanelPrefab, transform);
            _panel.gameObject.SetActive(false);
            _panel.lockButton.onClick.AddListener(OnLockOn);
            _panel.Init(this);

            return _panel;
        }

        private void ChangeState(State state) {
            if (!_panel) return;

            switch (state) {
                case State.Hidden: {
                    _canvas.enabled = false;
                    _panel.gameObject.SetActive(false);

                    break;
                }

                case State.Locked:
                case State.Hovered: {
                    UpdatePosition();
                    _canvas.enabled = CookieDebug.IsDebugMode;
                    _panel.gameObject.SetActive(CookieDebug.IsDebugMode);

                    break;
                }
            }

            _panel.SetLocked(state == State.Locked);

            _state = state;
        }

        private bool CheckIntersection() {
            if (!_panel) return false;

            Vector2 mousePos = Mouse.current.position.ReadValue();

            if (_panel.gameObject.activeSelf && _panel.Image.rectTransform.rect.Contains(
                    _panel.Image.rectTransform.InverseTransformPoint(
                        _canvas.worldCamera.ScreenToWorldPoint(mousePos)
                    )
                )) return true;

            return Bounds.IntersectRay(_canvas.worldCamera.ScreenPointToRay(mousePos));
        }

        private void UpdatePosition() {
            Vector3 pos = _renderer.OrNull()?.transform.position ?? _host.transform.position;

            if (_renderer)
                pos.y = _renderer.bounds.max.y + YOffset;
            else pos.y += DefaultYOffset;

            transform.position = pos;
        }

        private enum State
        {
            Locked,
            Hovered,
            Hidden,
        }
    }
}