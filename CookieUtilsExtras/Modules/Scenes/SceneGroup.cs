using System;
using System.Collections.Generic;
using System.Linq;
using Eflatun.SceneReference;

namespace CookieUtils.Extras.Scenes
{
    [Serializable]
    public class SceneGroup
    {
        public string name;
        public List<SceneData> scenes;
    }

    [Serializable]
    public struct SceneData
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