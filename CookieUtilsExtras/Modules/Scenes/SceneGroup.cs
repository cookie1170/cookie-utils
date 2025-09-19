using System;
using System.Collections.Generic;
using Eflatun.SceneReference;

namespace CookieUtils.Extras.Scenes
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
        public SceneGroup group;
    }

    [Serializable]
#if ALCHEMY
    [Alchemy.Inspector.DisableAlchemyEditor]
#endif
    public class SceneData
    {
        public SceneReference scene;
        public string Name => scene.Name;
        public SceneType type;
    }

    public enum SceneType
    {
        Active,
        Environment,
        UI,
        Other
    }
}