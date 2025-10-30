using CookieUtils;
using CookieUtils.Debugging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DebugUISampleObject : MonoBehaviour, IDebugDrawer, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float deltaWeight = 3f;
    [SerializeField] private float posWeight = 3f;
    [SerializeField] private Rigidbody2D rb;
    private Camera _camera;
    private bool _isHeld;

    private void Awake() {
        _camera = Camera.main;
    }

    private void Start() {
        CookieDebug.Register(this);
    }

    public void SetUpDebugUI(IDebugUIBuilderProvider provider) {
        provider.GetFor(this)
            .StringField("Name", () => name, val => name = val)
            .FoldoutGroup("Stats")
            .FoldoutGroup("Transform")
            .Label(() => $"Position is {transform.position.xy()}")
            .IntField("Rotation", () => Mathf.RoundToInt(transform.eulerAngles.z), val => rb.SetRotation(val))
            .EndGroup()
            .IfGroup(() => rb.linearVelocity.sqrMagnitude > 0.25f || rb.angularVelocity > 0.5f)
            .FoldoutGroup("Rigidbody")
            .Label(() => $"Velocity is {rb.linearVelocity}")
            .FloatField("Angular velocity", () => rb.angularVelocity, val => rb.angularVelocity = val)
            .EndGroup()
            .ElseGroup()
            .Label("Not moving!")
            .EndGroup()
            .EndGroup()
            .Button("Destroy", () => Destroy(gameObject));
    }

    #region Dragging

    private void FixedUpdate() {
        if (_isHeld) {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 force = Mouse.current.delta.ReadValue() * deltaWeight +
                            (Vector2)(mousePos - transform.position) * posWeight;
            rb.AddForceAtPosition(force, rb.ClosestPoint(mousePos), ForceMode2D.Force);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        _isHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        _isHeld = false;
    }

    #endregion
}