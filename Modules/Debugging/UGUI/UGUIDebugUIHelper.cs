using UnityEngine;

namespace CookieUtils.Debugging
{
    internal static class UGUIDebugUIHelper
    {
        internal static readonly Prefab<DebugUI_FoldoutGroup> FoldoutGroupPrefab = "FoldoutGroup";
        internal static readonly Prefab<DebugUI_Label> LabelPrefab = "Label";
        internal static readonly Prefab<GameObject> GroupPrefab = "Group";
        internal static readonly Prefab<DebugUI_Button> ButtonPrefab = "Button";
        internal static readonly Prefab<DebugUI_FloatField> FloatFieldPrefab = "Fields/FloatField";
        internal static readonly Prefab<DebugUI_IntField> IntFieldPrefab = "Fields/IntField";
        internal static readonly Prefab<DebugUI_BoolField> BoolFieldPrefab = "Fields/BoolField";
        internal static readonly Prefab<DebugUI_StringField> StringFieldPrefab = "Fields/StringField";
        internal static readonly Prefab<DebugUI_Vector2Field> Vector2FieldPrefab = "Fields/Vector2Field";
        internal static readonly Prefab<DebugUI_Vector3Field> Vector3FieldPrefab = "Fields/Vector3Field";
    }
}