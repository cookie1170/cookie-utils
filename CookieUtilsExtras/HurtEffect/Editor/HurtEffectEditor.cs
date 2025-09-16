using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.Extras.HurtEffect.Editor
{
    [CustomEditor(typeof(HurtEffect))]
    public class HurtEffectEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset inspector;
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();
            
            inspector.CloneTree(root);
        
            var hurtEffect = (HurtEffect)target;

            var dataObjectInspectorPanel = root.Q<VisualElement>("DataObjectInspectorPanel");
            var hideOnUseDataObject = root.Q<VisualElement>("AnimationPanel");
            var createDataObject = root.Q<Button>("CreateDataObject");
            var dataTitle = root.Q<Foldout>("DataTitle");
            var useDataObject = root.Q<PropertyField>("UseDataObject");
            var dataObject = root.Q<PropertyField>("DataObject");
            var shakeCamera = root.Q<PropertyField>("ShakeCamera");
            var shakeForce = root.Q<PropertyField>("ShakeForce");
            var animateScale = root.Q<PropertyField>("AnimateScale");
            var scaleSettings = root.Q<PropertyField>("ScaleSettings");
            var animateRotation = root.Q<PropertyField>("AnimateRotation");
            var rotationSettings = root.Q<PropertyField>("RotationSettings");
            var animateFlash = root.Q<PropertyField>("AnimateFlash");
            var hideIfNoFlash = root.Q<VisualElement>("HideIfNoFlash");
            var overrideRenderer = root.Q<PropertyField>("OverrideRenderer");
            var rendererOverride = root.Q<PropertyField>("RendererOverride");
            var overrideMaterial = root.Q<PropertyField>("OverrideMaterial");
            var materialOverride = root.Q<PropertyField>("MaterialOverride");
            var previewButton = root.Q<Button>("Preview");

            previewButton.SetEnabled(EditorApplication.isPlaying);

            previewButton.RegisterCallback<ClickEvent>(_ => hurtEffect.OnHit(new(0, 0, Vector3.right)));

            useDataObject.RegisterValueChangeCallback(_ => CheckDataObject());
            
            dataObject.RegisterValueChangeCallback(_ => CheckDataObject());

            createDataObject.RegisterCallback<ClickEvent>(CreateDataObject);
            
            useDataObject.RegisterValueChangeCallback(_ =>
                dataObject.style.display = hurtEffect.useDataObject ? DisplayStyle.Flex : DisplayStyle.None);
            
            shakeCamera.RegisterValueChangeCallback(_ =>
                shakeForce.style.display = hurtEffect.shakeCamera ? DisplayStyle.Flex : DisplayStyle.None);

            animateScale.RegisterValueChangeCallback(_ =>
                scaleSettings.style.display = hurtEffect.animateScale ? DisplayStyle.Flex : DisplayStyle.None);

            animateRotation.RegisterValueChangeCallback(_ =>
                rotationSettings.style.display = hurtEffect.animateRotation ? DisplayStyle.Flex : DisplayStyle.None);

            animateFlash.RegisterValueChangeCallback(_ =>
                hideIfNoFlash.style.display = hurtEffect.animateFlash ? DisplayStyle.Flex : DisplayStyle.None);
            
            overrideRenderer.RegisterValueChangeCallback(_ =>
                rendererOverride.style.display = hurtEffect.overrideRenderer ? DisplayStyle.Flex : DisplayStyle.None);
            
            overrideMaterial.RegisterValueChangeCallback(_ =>
                materialOverride.style.display = hurtEffect.overrideMaterial ? DisplayStyle.Flex : DisplayStyle.None);
            
            return root;
            
            void CheckDataObject()
            {
                var dataInspectorCurrent = dataObjectInspectorPanel.Q<VisualElement>("DataInspector");
                if (dataInspectorCurrent != null) dataTitle.Remove(dataInspectorCurrent);
               
                if (hurtEffect.useDataObject && hurtEffect.data) {
                    hideOnUseDataObject.style.display = DisplayStyle.None;
                    createDataObject.style.display = DisplayStyle.None;
                    var dataInspector = new InspectorElement(hurtEffect.data) {
                        name = "DataInspector"
                    };
                    dataObjectInspectorPanel.style.display = DisplayStyle.Flex;
                    dataTitle.Add(dataInspector);
                    dataTitle.text = hurtEffect.data.name;
                } else {
                    createDataObject.style.display = DisplayStyle.Flex;
                    dataObjectInspectorPanel.style.display = DisplayStyle.None;
                    hideOnUseDataObject.style.display = DisplayStyle.Flex;
                }
            }
            
            void CreateDataObject(ClickEvent evt)
            {
                string path = EditorUtility.SaveFilePanelInProject("Create hurt effect  data", $"{hurtEffect.name}_EffectData", "asset",
                    "Choose a path for the data object");

                var data = CreateHurtEffectData();
                
                AssetDatabase.CreateAsset(data, path);
                hurtEffect.useDataObject = true;
                hurtEffect.data = data;
                
                CheckDataObject();
            }

            HurtEffectData CreateHurtEffectData()
            {
                var data = CreateInstance<HurtEffectData>();
                
                data.shakeCamera = hurtEffect.shakeCamera;
                data.shakeForce = hurtEffect.shakeForce;
                data.animateScale = hurtEffect.animateScale;
                data.scaleSettings = hurtEffect.scaleSettings;
                data.animateRotation = hurtEffect.animateRotation;
                data.rotationSettings = hurtEffect.rotationSettings;
                data.animateFlash = hurtEffect.animateFlash;
                data.flashInSettings = hurtEffect.flashInSettings;
                data.flashOutSettings = hurtEffect.flashOutSettings;
                data.flashColour = hurtEffect.flashColour;
                data.materialType = hurtEffect.materialType;
                
                return data;
            }
        }
    }
}
