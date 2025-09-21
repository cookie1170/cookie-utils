using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace CookieUtils.Extras.Juice.Editor
{
    [CustomEditor(typeof(Effect))]
    public class EffectEditor : UnityEditor.Editor
    {
        [SerializeField] private VisualTreeAsset inspector;
        
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement root = new();
            
            inspector.CloneTree(root);
        
            var effect = (Effect)target;

            var dataObjectInspectorPanel = root.Q<VisualElement>("DataObjectInspectorPanel");
            var hideOnUseDataObject = root.Q<VisualElement>("AnimationPanel");
            var createDataObject = root.Q<Button>("CreateDataObject");
            var dataTitle = root.Q<Foldout>("DataTitle");
            var useDataObject = root.Q<PropertyField>("UseDataObject");
            var dataObject = root.Q<PropertyField>("DataObject");
            var shakeCamera = root.Q<PropertyField>("ShakeCamera");
            var shakeForce = root.Q<PropertyField>("ShakeForce");
            var spawnParticles = root.Q<PropertyField>("SpawnParticles");
            var hideIfNoParticles = root.Q<VisualElement>("HideIfNoParticles");
            var playAudio = root.Q<PropertyField>("PlayAudio");
            var hideIfNoAudio = root.Q<VisualElement>("HideIfNoAudio");
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
            
            CheckDataObject();
            
            previewButton.SetEnabled(EditorApplication.isPlaying);

            previewButton.RegisterCallback<ClickEvent>(_ => effect.Play());

            useDataObject.RegisterValueChangeCallback(_ => CheckDataObject());
            
            dataObject.RegisterValueChangeCallback(_ => CheckDataObject());

            createDataObject.RegisterCallback<ClickEvent>(CreateDataObject);
            
            useDataObject.RegisterValueChangeCallback(_ =>
                dataObject.style.display = effect.useDataObject ? DisplayStyle.Flex : DisplayStyle.None);

            shakeCamera.RegisterValueChangeCallback(_ =>
                shakeForce.style.display = effect.shakeCamera ? DisplayStyle.Flex : DisplayStyle.None);
            
            spawnParticles.RegisterValueChangeCallback(_ =>
                hideIfNoParticles.style.display = effect.spawnParticles ? DisplayStyle.Flex : DisplayStyle.None);

            playAudio.RegisterValueChangeCallback(_ =>
                hideIfNoAudio.style.display = effect.playAudio ? DisplayStyle.Flex : DisplayStyle.None);

            animateScale.RegisterValueChangeCallback(_ =>
                scaleSettings.style.display = effect.animateScale ? DisplayStyle.Flex : DisplayStyle.None);

            animateRotation.RegisterValueChangeCallback(_ =>
                rotationSettings.style.display = effect.animateRotation ? DisplayStyle.Flex : DisplayStyle.None);

            animateFlash.RegisterValueChangeCallback(_ =>
                hideIfNoFlash.style.display = effect.animateFlash ? DisplayStyle.Flex : DisplayStyle.None);
            
            overrideRenderer.RegisterValueChangeCallback(_ =>
                rendererOverride.style.display = effect.overrideRenderers ? DisplayStyle.Flex : DisplayStyle.None);
            
            overrideMaterial.RegisterValueChangeCallback(_ =>
                materialOverride.style.display = effect.overrideMaterial ? DisplayStyle.Flex : DisplayStyle.None);
            
            return root;
            
            void CheckDataObject()
            {
                var dataInspectorCurrent = dataObjectInspectorPanel.Q<VisualElement>("DataInspector");
                if (dataInspectorCurrent != null) dataTitle.Remove(dataInspectorCurrent);
               
                if (effect.useDataObject && effect.data) {
                    hideOnUseDataObject.style.display = DisplayStyle.None;
                    createDataObject.style.display = DisplayStyle.None;
                    var dataInspector = new InspectorElement(effect.data) {
                        name = "DataInspector"
                    };

                    dataInspector.Query<PropertyField>()
                        .ForEach(f => f.RegisterValueChangeCallback(_ => effect.UpdateData()));
                    
                    dataObjectInspectorPanel.style.display = DisplayStyle.Flex;
                    dataTitle.Add(dataInspector);
                    dataTitle.text = effect.data.name;
                } else {
                    createDataObject.style.display = DisplayStyle.Flex;
                    dataObjectInspectorPanel.style.display = DisplayStyle.None;
                    hideOnUseDataObject.style.display = DisplayStyle.Flex;
                }
            }
            
            void CreateDataObject(ClickEvent evt)
            {
                string path = EditorUtility.SaveFilePanelInProject("Create hurt effect  data", $"{effect.name}_EffectData", "asset",
                    "Choose a path for the data object");

                var data = CreateHurtEffectData();
                
                AssetDatabase.CreateAsset(data, path);
                effect.useDataObject = true;
                effect.data = data;
                
                CheckDataObject();
            }

            EffectData CreateHurtEffectData()
            {
                var data = CreateInstance<EffectData>();

                data.is2D = effect.is2D;
                data.shakeCamera = effect.shakeCamera;
                data.shakeForce = effect.shakeForce;
                data.spawnParticles = effect.spawnParticles;
                data.directionalParticles = effect.directionalParticles;
                data.particlePrefab = effect.particlePrefab;
                data.playAudio = effect.playAudio;
                data.spatialBlend = effect.spatialBlend;
                data.audioVolume = effect.audioVolume;
                data.audioDelay = effect.audioDelay;
                data.audioClips = effect.audioClips;
                data.animateScale = effect.animateScale;
                data.scaleAnimation = effect.scaleAnimation;
                data.animateRotation = effect.animateRotation;
                data.rotationAnimation = effect.rotationAnimation;
                data.animateFlash = effect.animateFlash;
                data.flashAnimation = effect.flashAnimation;
                data.flashColour = effect.flashColour;
                data.materialType = effect.materialType;
                
                return data;
            }
        }
    }
}
