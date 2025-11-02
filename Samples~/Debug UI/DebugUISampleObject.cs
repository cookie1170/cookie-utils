using CookieUtils.Debugging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DebugUISampleObject : MonoBehaviour, IDebugDrawer, IPointerDownHandler, IPointerUpHandler
{
    public void SetUpDebugUI(IDebugUI_BuilderProvider provider) {
        IDebugUI_Builder root = provider.GetFor(this);
        root.StringField("Name", () => name, val => name = val);
        root.BoolField("Frozen", () => _isFrozen, val => _isFrozen = val);

        IDebugUI_Group stats = root.FoldoutGroup("Stats");

        IDebugUI_Group transformGroup = stats.FoldoutGroup("Transform");
        transformGroup.Vector3Field("Position", () => transform.position, val => rb.MovePosition(val));
        transformGroup.IntField("Rotation", () => Mathf.RoundToInt(transform.eulerAngles.z),
            val => rb.SetRotation(val));

        IDebugUI_If ifMoving =
            stats.IfGroup(() => rb.linearVelocity.sqrMagnitude > 0.25f || rb.angularVelocity > 0.5f);

        IDebugUI_Group rigidbodyFoldout = ifMoving.FoldoutGroup("Rigidbody");
        rigidbodyFoldout.Vector2Field("Velocity", () => rb.linearVelocity, val => rb.linearVelocity = val);
        rigidbodyFoldout.FloatField("Angular velocity", () => rb.angularVelocity, val => rb.angularVelocity = val);

        IDebugUI_Group elseGroup = ifMoving.ElseGroup();
        elseGroup.Label("Not moving!");

        root.Button("Destroy", () => Destroy(gameObject));
    }

    #region Dragging

    [SerializeField] private float deltaWeight = 10f;
    [SerializeField] private float posWeight = 10f;
    [SerializeField] private Rigidbody2D rb;
    private Camera _camera;
    private bool _isFrozen;
    private bool _isHeld;

    private void Awake() {
        _camera = Camera.main;
    }

    private void FixedUpdate() {
        rb.constraints = _isFrozen ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.None;

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