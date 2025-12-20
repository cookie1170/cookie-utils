using UnityEngine;
using Object = UnityEngine.Object;

namespace CookieUtils.Debugging
{
    // ReSharper disable once InconsistentNaming
    internal static class UGUIDebugUI_Helper
    {
        internal static readonly Prefab<DebugUI_FoldoutGroup> FoldoutGroupPrefab = "FoldoutGroup";
        internal static readonly Prefab<DebugUI_Label> LabelPrefab = "Label";
        internal static readonly Prefab<GameObject> GroupPrefab = "Group";
        internal static readonly Prefab<DebugUI_Button> ButtonPrefab = "Button";
        internal static readonly Prefab<DebugUI_FloatField> FloatFieldPrefab = "Fields/FloatField";
        internal static readonly Prefab<DebugUI_IntField> IntFieldPrefab = "Fields/IntField";
        internal static readonly Prefab<DebugUI_BoolField> BoolFieldPrefab = "Fields/BoolField";
        internal static readonly Prefab<DebugUI_StringField> StringFieldPrefab =
            "Fields/StringField";
        internal static readonly Prefab<DebugUI_Vector2Field> Vector2FieldPrefab =
            "Fields/Vector2Field";
        internal static readonly Prefab<DebugUI_Vector3Field> Vector3FieldPrefab =
            "Fields/Vector3Field";
        internal static readonly Prefab<DebugUI_Panel> PanelPrefab = "Panel";

        internal class Prefab<T>
            where T : Object
        {
            private readonly string _path;
            private T _prefabCached;

            private Prefab(string path) => _path = path;

            private T Get()
            {
                if (!_prefabCached)
                    _prefabCached = Resources.Load<T>($"DebugUI/Prefabs/{_path}");

                return _prefabCached;
            }

            public static implicit operator T(Prefab<T> prefab) => prefab.Get();

            public static implicit operator Prefab<T>(string path) => new(path);
        }
    }
}
