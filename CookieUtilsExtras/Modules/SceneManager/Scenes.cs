using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CookieUtils.Extras.SceneManager
{
    [PublicAPI]
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
            
            if (_data.bootstrapScene.UnsafeReason != SceneReferenceUnsafeReason.Empty) {
                if (!IsSceneLoaded(_data.bootstrapScene)) {
                    await LoadBootstrapScene();
                } else Debug.Log("[CookieUtils.Extras.SceneManager] Bootstrap scene already loaded");
                
                FindSceneTransition();
            }

            if (_data.startingGroup.Group != null)
                await LoadGroup(_data.startingGroup, false);
        }

        private static async Task LoadBootstrapScene()
        {
            await UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_data.bootstrapScene.BuildIndex);
            Debug.Log("[CookieUtils.Extras.SceneManager] Loaded bootstrap scene");
        }

        private static void FindSceneTransition()
        {
            var transition = UnityEngine.Object.FindFirstObjectByType<SceneTransition>(FindObjectsInactive.Include); // ugly but only called once so should be fine
            if (!transition) return;

            Debug.Log("[CookieUtils.Extras.SceneManager] Found scene transition in bootstrap scene");
            _transition = transition;
        }

#if DEBUG_CONSOLE
        [IngameDebugConsole.ConsoleMethod("load", "Loads the specified scene group")]
#endif
        public static async Task LoadGroup(string groupName)
        {
            await LoadGroup(groupName, true);
        }
        
        public static async Task LoadGroup(string groupName, bool useTransition)
        {
            var targetGroup = _data.FindSceneGroupFromName(groupName);

            await LoadGroup(targetGroup, useTransition);
        }

        public static async Task LoadGroup(SceneGroupReference group, bool useTransition = true)
        {
            await LoadGroup(group.Group, useTransition);
        }

        public static async Task LoadGroup(SceneGroup targetGroup, bool useTransition = true)
        {
            if (!_data.useSceneManager) {
                Debug.LogWarning("[CookieUtils.Extras.SceneManager] Scene manager disabled! Can't load scene group");
                return;
            }
            
            if (_transition && useTransition) await _transition.PlayForwards();
            if (ActiveGroup != null) await UnloadSceneGroup(ActiveGroup, targetGroup);
            
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
            Debug.Log($"[CookieUtils.Extras.SceneManager] Loaded group {targetGroup.name}");
            OnGroupLoaded(targetGroup);
            
            if (_transition && useTransition) _ = _transition.PlayBackwards();
        }

        public static async Task UnloadSceneGroup(SceneGroup group, SceneGroup newGroup = null)
        {
            if (!_data.useSceneManager) {
                Debug.LogWarning("[CookieUtils.Extras.SceneManager] Scene manager disabled! Can't unload scene group");
                return;
            }
            
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
            Debug.Log($"[CookieUtils.Extras.SceneManager] Unloaded group {group.name}");
        }

        public static async Task UnloadAllScenes()
        {
            if (!_data.useSceneManager) {
                Debug.LogWarning("[CookieUtils.Extras.SceneManager] Scene manager disabled! Can't unload all scenes");
                return;
            }
            
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
            
            if (sceneCount > 0) Debug.Log($"[CookieUtils.Extras.SceneManager] Unloaded {sceneCount} scenes");
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
