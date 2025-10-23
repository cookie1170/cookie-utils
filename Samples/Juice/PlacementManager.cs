using CookieUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;

namespace Samples.Juice
{
    public class PlacementManager : MonoBehaviour
    {
        [SerializeField] private GameObject squarePrefab;

        private void Awake()
        {
            var action = new InputAction(binding: Mouse.current.leftButton.path);
            var cam = Camera.main;
            action.performed += _ => {
                Debug.Assert(cam != null, nameof(cam) + " != null");
                var position = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue()).With(z: 0);
                var result = Physics2D.OverlapCircle(position, 0.5f);
                if (!result) Instantiate(squarePrefab,
                    position, Quaternion.identity);
            };
            action.Enable();
        }
    }
}
