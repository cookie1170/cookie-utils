using System.Collections.Generic;
using UnityEngine;

namespace CookieUtils
{
    public static class Updater
    {
        private static readonly List<UpdateData> RegisteredUpdates = new();
        private static readonly List<FixedUpdateData> RegisteredFixedUpdates = new();

        internal static void Update()
        {
            for (int i = RegisteredUpdates.Count - 1; i >= 0; i--)
            {
                UpdateData data = RegisteredUpdates[i];
                if (data.HasHost)
                {
                    if (!data.Host)
                    {
                        Deregister(data.Obj);
                        return;
                    }

                    if (!ShouldUpdate(data.Host))
                        return;
                }

                data.Obj.Update();
            }
        }

        internal static void FixedUpdate()
        {
            for (int i = RegisteredFixedUpdates.Count - 1; i >= 0; i--)
            {
                FixedUpdateData data = RegisteredFixedUpdates[i];
                if (data.HasHost)
                {
                    if (!data.Host)
                    {
                        Deregister(data.Obj);
                        return;
                    }

                    if (!ShouldUpdate(data.Host))
                        return;
                }

                data.Obj.FixedUpdate();
            }
        }

        private static bool ShouldUpdate(Object host)
        {
            if (host is MonoBehaviour mb)
            {
                return mb.enabled && mb.gameObject.activeInHierarchy;
            }

            if (host is GameObject go)
            {
                return go.activeInHierarchy;
            }

            return true;
        }

        internal static void Clear()
        {
            RegisteredUpdates.Clear();
            RegisteredFixedUpdates.Clear();
        }

        public static void Register(IUpdate obj)
        {
            if (IsRegistered(obj, out _))
                return;

            RegisteredUpdates.Add(new(obj));
        }

        public static void Register(IFixedUpdate obj)
        {
            if (IsRegistered(obj, out _))
                return;

            RegisteredFixedUpdates.Add(new(obj));
        }

        public static void Deregister(IUpdate obj)
        {
            if (!IsRegistered(obj, out int index))
                return;

            RegisteredUpdates.RemoveAt(index);
        }

        public static void Deregister(IFixedUpdate obj)
        {
            if (!IsRegistered(obj, out int index))
                return;

            RegisteredFixedUpdates.RemoveAt(index);
        }

        public static bool IsRegistered(IFixedUpdate obj, out int index)
        {
            index = RegisteredFixedUpdates.FindIndex(u => u.Obj == obj);

            return index != -1;
        }

        public static bool IsRegistered(IUpdate obj, out int index)
        {
            index = RegisteredUpdates.FindIndex(u => u.Obj == obj);
            return index != -1;
        }

        public static void Register<T>(T obj)
            where T : IFixedUpdate, IUpdate
        {
            Register((IUpdate)obj);
            Register((IFixedUpdate)obj);
        }

        public static void Deregister<T>(T obj)
            where T : IFixedUpdate, IUpdate
        {
            Deregister((IUpdate)obj);
            Deregister((IFixedUpdate)obj);
        }

        public static void AddTo(this IUpdate obj, Object host)
        {
            if (!IsRegistered(obj, out int index))
            {
                Debug.LogWarning($"Can't add an unregistered IUpdate to host {host.name}");
                return;
            }

            UpdateData data = RegisteredUpdates[index];

            data.HasHost = true;
            data.Host = host;

            RegisteredUpdates[index] = data;
        }

        public static void AddTo(this IFixedUpdate obj, Object host)
        {
            if (!IsRegistered(obj, out int index))
            {
                Debug.LogWarning($"Can't add an unregistered IFixedUpdate to host {host.name}");
                return;
            }

            FixedUpdateData data = RegisteredFixedUpdates[index];

            data.HasHost = true;
            data.Host = host;

            RegisteredFixedUpdates[index] = data;
        }

        public static void AddTo<T>(this T obj, Object host)
            where T : IUpdate, IFixedUpdate
        {
            AddTo((IUpdate)obj, host);
            AddTo((IFixedUpdate)obj, host);
        }

        private struct UpdateData
        {
            public IUpdate Obj;
            public bool HasHost;
            public Object Host;

            public UpdateData(IUpdate obj)
            {
                Obj = obj;
                HasHost = false;
                Host = null;
            }
        }

        private struct FixedUpdateData
        {
            public IFixedUpdate Obj;
            public bool HasHost;
            public Object Host;

            public FixedUpdateData(IFixedUpdate obj)
            {
                Obj = obj;
                HasHost = false;
                Host = null;
            }
        }
    }

    public interface IUpdate
    {
        void Update();
    }

    public interface IFixedUpdate
    {
        void FixedUpdate();
    }
}
