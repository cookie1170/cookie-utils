using UnityEngine;
using UnityEngine.EventSystems;

namespace Samples.Juice
{
    public class SampleSquare : MonoBehaviour, IPointerClickHandler
    {
        private bool _isHovered;
        private CookieUtils.Health.Health _health;
        
        private void Awake()
        {
            _health = GetComponent<CookieUtils.Health.Health>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _health.Hit(new CookieUtils.Health.Health.HitInfo(20, 0.2f, Vector3.right));
        }
    }
}
