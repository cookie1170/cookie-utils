using CookieUtils.Debugging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DebugUISampleObject : MonoBehaviour, IDebugDrawer, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float deltaWeight = 10f;
    [SerializeField] private float posWeight = 10f;
    [SerializeField] private Rigidbody2D rb;
    private Camera _camera;
    private bool _isFrozen;
    private bool _isHeld;

    private void Awake() {
        _camera = Camera.main;
    }

    public void SetUpDebugUI(IDebugUIBuilderProvider provider) {
        provider.GetFor(this)
            .StringField("Name", () => name, val => name = val)
            .BoolField(
                "Frozen", () => _isFrozen, val => {
                    _isFrozen = val;
                    rb.constraints = _isFrozen ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;
                }
            )
            .FoldoutGroup("Stats")
            .FoldoutGroup("Transform")
            .Vector3Field("Position", () => transform.position, val => rb.MovePosition(val))
            .IntField("Rotation", () => Mathf.RoundToInt(transform.eulerAngles.z), val => rb.SetRotation(val))
            .EndGroup()
            .IfGroup(() => rb.linearVelocity.sqrMagnitude > 0.25f || rb.angularVelocity > 0.5f)
            .FoldoutGroup("Rigidbody")
            .Vector2Field("Velocity", () => rb.linearVelocity, val => rb.linearVelocity = val)
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
            Vector2 force = GetDeltaForce() + GetPosForce(mousePos);
            rb.AddForceAtPosition(force, rb.ClosestPoint(mousePos), ForceMode2D.Force);
        }
    }

    private Vector2 GetPosForce(Vector3 mousePos) => (Vector2)(mousePos - transform.position) * posWeight;

    private Vector2 GetDeltaForce() =>
        (Vector2)(
            _camera.ScreenToWorldPoint(
                Mouse.current.delta.ReadValue()
            ) - // cursed but I literally do not know a better way of doing it D:
            _camera.ScreenToWorldPoint(Vector3.zero)) * deltaWeight;

    public void OnPointerDown(PointerEventData eventData) {
        _isHeld = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        _isHeld = false;
    }

    #endregion
}