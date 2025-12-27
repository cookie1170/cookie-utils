using UnityEngine;

namespace CookieUtils.Samples.Updater
{
    public class UpdaterSampleObject : MonoBehaviour
    {
        private void Awake()
        {
            new Update().AddTo(this);
            new FixedUpdate().AddTo(this);
            new Both().AddTo(this);
        }

        class Update : IUpdate
        {
            void IUpdate.Update()
            {
                Debug.Log(
                    $"I'm being updated from a C# script! Time.deltaTime is {Time.deltaTime}"
                );
            }

            public Update()
            {
                CookieUtils.Updater.Register(this);
            }
        }

        class FixedUpdate : IFixedUpdate
        {
            void IFixedUpdate.FixedUpdate()
            {
                Debug.Log(
                    $"I'm being fixed updated from a C# script! Time.deltaTime is {Time.deltaTime}"
                );
            }

            public FixedUpdate()
            {
                CookieUtils.Updater.Register(this);
            }
        }

        class Both : IUpdate, IFixedUpdate
        {
            public void Update()
            {
                Debug.Log(
                    $"I'm being updated from {nameof(Both)}, Time.deltaTime is {Time.deltaTime}"
                );
            }

            public void FixedUpdate()
            {
                Debug.Log(
                    $"I'm being fixed updated from {nameof(Both)}, Time.deltaTime is {Time.deltaTime}"
                );
            }

            public Both()
            {
                CookieUtils.Updater.Register(this);
            }
        }
    }
}
