using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.HealthSystem.Editor
{ 
    public class EditMask : EditorWindow
    {
        public static Action<List<string>> OnMaskChanged;
        
        private static readonly string MaskFolder = "CookieUtils";
        private static readonly string MaskName = "MaskData.asset";

        [SerializeField] private VisualTreeAsset window;
        
        [MenuItem("Cookie Utils/Health/Edit Mask")]
        public static void ShowWindow()
        {
            GetWindow<EditMask>("Edit Mask", true);
        }

        private void CreateGUI()
        {
            window.CloneTree(rootVisualElement);
            
            var mask = GetMask();

            var list = rootVisualElement.Q<PropertyField>("List");

            list.BindProperty(new SerializedObject(mask).FindProperty("masks"));

            list.RegisterCallback<ChangeEvent<string>>(_ => OnMaskChanged?.Invoke(mask.masks));
        }

        public static MaskData GetMask()
        {
            if (!AssetDatabase.AssetPathExists($"Assets/{MaskFolder}/{MaskName}")) {
                if (!AssetDatabase.IsValidFolder($"Assets/{MaskFolder}")) {
                    AssetDatabase.CreateFolder("Assets", MaskFolder);
                }
                
                AssetDatabase.CreateAsset(CreateInstance<MaskData>(), $"Assets/{MaskFolder}/{MaskName}");
            }

            return AssetDatabase.LoadAssetAtPath<MaskData>($"Assets/{MaskFolder}/{MaskName}");
        }
    }
}