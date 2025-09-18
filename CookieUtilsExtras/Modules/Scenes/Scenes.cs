using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Eflatun.SceneReference;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CookieUtils.Extras.Scenes
{
    public static class Scenes
    {
        private static ScenesData _data;
     
        public static SceneGroup ActiveGroup { get; private set; }
        public static event Action<SceneGroup> OnGroupLoaded = delegate { };

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static async void Initialize()
        {
            _data = ScenesData.GetScenesData();
            
            if (_data.bootstrapScene.UnsafeReason != SceneReferenceUnsafeReason.Empty) {
                if (!IsSceneLoaded(_data.bootstrapScene)) {
                    await SceneManager.LoadSceneAsync(_data.bootstrapScene.BuildIndex);
                    Debug.Log("[CookieUtils.Extras.Scenes] Loaded bootstrap scene");
                }
            }

            await LoadGroup(_data.startingGroup);
        }

        public static async Task LoadGroup(string groupName)
        {
            var targetGroup = _data.groups.Find(g => g.name == groupName);
            if (targetGroup == null) {
                Debug.LogError($"[CookieUtils.Extras.Scenes] Group {groupName} not found!");
                return;
            }
            
            await UnloadScenes();
            
            ActiveGroup = targetGroup;

            var loadTasks = new Task[targetGroup.scenes.Count];

            for (int i = 0; i < targetGroup.scenes.Count; i++) {
                var scene = targetGroup.scenes[i];
                int buildIndex = scene.scene.BuildIndex;
                var operation = SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
                if (operation == null) continue;
                
                operation.completed += _ =>
                    SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
                
                loadTasks[i] = Task.FromResult(operation);
            }
            
            await Task.WhenAll(loadTasks);
            Debug.Log($"[CookieUtils.Extras.Scenes] Loaded group {groupName}");
            OnGroupLoaded(targetGroup);
        }

        public static async Task UnloadScenes()
        {
            int rawSceneCount = SceneManager.sceneCount;
            int sceneCount = rawSceneCount - (_data.bootstrapScene.UnsafeReason != SceneReferenceUnsafeReason.Empty ? 1 : 0);
            
            List<Task> tasks = new(sceneCount);
            
            for (int i = 0; i < rawSceneCount; i++) {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.buildIndex == _data.bootstrapScene.BuildIndex) continue;
               
                var operation = SceneManager.UnloadSceneAsync(scene);
                tasks.Add(Task.FromResult(operation));
            }

            await Task.WhenAll(tasks);
            
            if (sceneCount > 0) Debug.Log($"[CookieUtils.Extras.Scenes] Unloaded {sceneCount} scenes");
        }

        private static bool IsSceneLoaded(SceneReference scene)
        {
            int loadedCount = SceneManager.sceneCount;

            for (int i = 0; i < loadedCount; i++) {
                if (SceneManager.GetSceneAt(i).buildIndex == scene.BuildIndex)
                    return true;
            }

            return false;
        }
    }
}
