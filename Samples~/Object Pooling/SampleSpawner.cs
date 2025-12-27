using CookieUtils.ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CookieUtils.Samples.ObjectPooling
{
    public class SampleSpawner : MonoBehaviour
    {
        [SerializeField]
        private ObjectPoolingSampleObject prefab;
        private int _alive = 0;
        private float _spawnRate = 50;
        private float _timer = 1f / 50;
        private int _totalSpawned = 0;

        private void Update()
        {
            _timer -= Time.deltaTime;
            while (_timer < 0)
            {
                _timer += 1f / _spawnRate;
                _totalSpawned++;
                _alive++;

                ObjectPoolingSampleObject obj = prefab.Get(new Vector2(Random.Range(-9f, 98f), 5f));
                obj.OnReleaseAction = () => _alive--;
            }
        }

        private void OnGUI()
        {
            GUILayout.Label($"Total spawned: {_totalSpawned}");
            GUILayout.Label($"Alive: {_alive}");
            GUILayout.Label($"Spawn rate: {_spawnRate:0.0}/s");
            _spawnRate = GUILayout.HorizontalSlider(_spawnRate, 1f, 5000f);
        }
    }
}
