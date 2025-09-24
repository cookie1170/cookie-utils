using System;
using System.Collections.Generic;
using Eflatun.SceneReference;
using UnityEngine;

namespace CookieUtils.Extras.SceneManager
{
    [Serializable]
#if ALCHEMY
    [Alchemy.Inspector.DisableAlchemyEditor]
#endif
    public class SceneGroup
    {
        public string name;
        public List<SceneData> scenes;
    }
    
    [Serializable]
#if ALCHEMY
    [Alchemy.Inspector.DisableAlchemyEditor]
#endif
    public class SceneGroupReference
    {
        public string name;
        public SceneGroup Group => ScenesData.GetScenesData().FindSceneGroupFromName(name);
    }

    [Serializable]
#if ALCHEMY
    [Alchemy.Inspector.DisableAlchemyEditor]
#endif
    public class SceneData
    {
        public SceneReference scene;
        public SceneType type;
        [Tooltip("Whether to reload the scene if it's already loaded")]
        public bool reloadIfExists = false;
        public string Name => scene.Name;
    }

    public enum SceneType
    {
        Active,
        Environment,
        UI,
        Other
    }
}