using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CookieUtils.Extras.SceneManager
{
    public static class Scenes
    {
        private static ScenesData _data;
     
        public static SceneGroup ActiveGroup { get; private set; }
        public static event Action<SceneGroup> OnGroupLoaded = delegate { };
        private static SceneTransition _transition;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static async void Initialize()
        {
            _transition = null;
            
            _data = ScenesData.GetScenesData();

            if (!_data.useSceneManager) return;
            
            if (_data.bootstrapScene.UnsafeReason != SceneReferenceUnsafeReason.Empty && !IsSceneLoaded(_data.bootstrapScene)) {
                await CreateBootstrapScene();
            }

            if (_data.startingGroup.Group != null)
                await LoadGroup(_data.startingGroup);
        }

        private static async Task CreateBootstrapScene()
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_data.bootstrapScene.BuildIndex);

            var objects = UnityEngine.SceneManagement.SceneManager.GetSceneAt(0).GetRootGameObjects();
            
            foreach (var gameObject in objects) {
                if (gameObject.TryGetComponent(out SceneTransition transition)) {
                    Debug.Log("[CookieUtils.Extras.Scenes] Found scene transition");
                    _transition = transition;
                    break;
                }
            }
                    
            Debug.Log("[CookieUtils.Extras.Scenes] Loaded bootstrap scene");
        }

#if DEBUG_CONSOLE
        [IngameDebugConsole.ConsoleMethod("load", "Loads the specified scene group")]
        #endif
        public static async Task LoadGroup(string groupName)
        {
            var targetGroup = _data.FindSceneGroupFromName(groupName);

            await LoadGroup(targetGroup);
        }

        public static async Task LoadGroup(SceneGroupReference group)
        {
            await LoadGroup(group.Group);
        }

        public static async Task LoadGroup(SceneGroup targetGroup)
        {
            if (ActiveGroup != null) await UnloadSceneGroup(ActiveGroup, targetGroup);
            if (_transition) await _transition.PlayForwards();
            
            ActiveGroup = targetGroup;

            var loadTasks = new List<Task>();

            foreach (var scene in targetGroup.scenes) {
                int buildIndex = scene.scene.BuildIndex;
                
                if (UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(buildIndex).isLoaded) continue;
                
                var operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
                
                if (operation == null) continue;
                
                if (scene.type == SceneType.Active)
                    operation.completed += _ =>
                        UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByBuildIndex(buildIndex));
                
                loadTasks.Add(Task.FromResult(operation));
            }

            await Task.WhenAll(loadTasks);
            Debug.Log($"[CookieUtils.Extras.Scenes] Loaded group {targetGroup.name}");
            OnGroupLoaded(targetGroup);
            
            if (_transition) _ = _transition.PlayBackwards();
        }

        public static async Task UnloadSceneGroup(SceneGroup group, SceneGroup newGroup = null)
        {
            if (group == null || group.scenes.Count == 0) return;
            
            int count = group.scenes.Count;

            var tasks = new List<Task>();

            for (int i = 0; i < count; i++) {
                var scene = group.scenes[i];
                
                if (!scene.reloadIfExists && newGroup != null) {
                    if (newGroup.scenes.Find(s => scene.scene.BuildIndex == s.scene.BuildIndex) != null)
                        continue;
                }

                var operation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene.scene.BuildIndex);
                tasks.Add(Task.FromResult(operation));
            }

            await Task.WhenAll(tasks);
            Debug.Log($"[CookieUtils.Extras.Scenes] Unloaded group {group.name}");
        }

        public static async Task UnloadAllScenes()
        {
            int rawSceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
            int sceneCount = rawSceneCount - (_data.bootstrapScene.UnsafeReason != SceneReferenceUnsafeReason.Empty ? 1 : 0);
            
            List<Task> tasks = new(sceneCount);
            
            for (int i = 0; i < rawSceneCount; i++) {
                var scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i);
                if (scene.buildIndex == _data.bootstrapScene.BuildIndex) continue;
               
                var operation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene);
                tasks.Add(Task.FromResult(operation));
            }

            await Task.WhenAll(tasks);
            
            if (sceneCount > 0) Debug.Log($"[CookieUtils.Extras.Scenes] Unloaded {sceneCount} scenes");
        }

        private static bool IsSceneLoaded(SceneReference scene)
        {
            int loadedCount = UnityEngine.SceneManagement.SceneManager.sceneCount;

            for (int i = 0; i < loadedCount; i++) {
                if (UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).buildIndex == scene.BuildIndex)
                    return true;
            }

            return false;
        }
    }
}
