using CookieUtils.Health;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CookieUtils.Editor
{
	public static class CreateFromHierarchyMenu
	{
		[MenuItem("GameObject/CookieUtils/Setup", true)]
		public static bool SetupValidate()
		{
			GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();
			return !(HasObjectWithName("AudioManager", objects) &&
			         HasObjectWithName("TimerManager", objects) &&
			         HasObjectWithName("PoolManager", objects));
		}
		
		[MenuItem("GameObject/CookieUtils/Setup", false, 1)]
		public static void Setup()
		{
			GameObject[] objects = SceneManager.GetActiveScene().GetRootGameObjects();
			foreach (string s in new[] { "AudioManager", "TimerManager", "PoolManager" })
			{
				if (!HasObjectWithName(s, objects))
				{
					GameObject prefab = LoadPrefab(s);
					
					if (!prefab) continue;

					PrefabUtility.InstantiatePrefab(prefab);
				}
			}
		}

		private static bool HasObjectWithName(string name, GameObject[] objects)
		{
			foreach (GameObject obj in objects)
			{
				if (obj.name == name) return true;
			}

			return false;
		}
		
		[MenuItem("GameObject/CookieUtils/Hitbox Square", true),
		 MenuItem("GameObject/CookieUtils/Hitbox Circle", true)]
		public static bool CreateHitboxValidate()
		{
			GameObject selectedObject = Selection.activeGameObject;
			if (!selectedObject) return true;
			return !selectedObject.GetComponentInChildren<Hitbox>();
		}
		
		[MenuItem("GameObject/CookieUtils/Hurtbox Square", true),
		 MenuItem("GameObject/CookieUtils/Hurtbox Circle", true)]
		public static bool CreateHurtboxValidate()
		{
			GameObject selectedObject = Selection.activeGameObject;
			if (!selectedObject) return true;
			return !selectedObject.GetComponentInChildren<Hurtbox>();
		}
		
		[MenuItem("GameObject/CookieUtils/Hitbox Square", false, 2)]
		public static void CreateHitboxSquare(MenuCommand menuCommand)
		{
			InstantiatePrefab(menuCommand, "HitboxSquare",
				"Create hitbox square", "Hitbox");
		}
		
		[MenuItem("GameObject/CookieUtils/Hitbox Circle", false, 2)]
		public static void CreateHitboxCircle(MenuCommand menuCommand)
		{
			InstantiatePrefab(menuCommand, "HitboxCircle",
				"Create hitbox circle", "Hitbox");
		}
		
		[MenuItem("GameObject/CookieUtils/Hurtbox Square", false, 2)]
		public static void CreateHurtboxSquare(MenuCommand menuCommand)
		{
			InstantiatePrefab(menuCommand, "HurtboxSquare",
				"Create hurtbox square", "Hurtbox");
		}
		
		[MenuItem("GameObject/CookieUtils/Hurtbox Circle", false, 2)]
		public static void CreateHurtboxCircle(MenuCommand menuCommand)
		{
			InstantiatePrefab(menuCommand, "HurtboxCircle",
				"Create hurtbox circle", "Hurtbox");
		}

		[MenuItem("GameObject/CookieUtils/Healthbar", false, 3)]
		public static void CreateHealthbar(MenuCommand menuCommand)
		{
			InstantiatePrefab(menuCommand, "Healthbar",
				"Create healthbar", "Healthbar");
		}

		private static GameObject LoadPrefab(string prefabName)
		{
			string[] guids = AssetDatabase.FindAssets($"t:Prefab {prefabName}");
			
			if (guids.Length == 0)
			{
				Debug.LogWarning($"Prefab {prefabName} was not found");
				return null;
			}
			
			string prefabPath = AssetDatabase.GUIDToAssetPath(guids[0]);
			
			GameObject prefab =  AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

			if (!prefab)
			{
				Debug.LogWarning($"Prefab at path {prefabPath} was not found");
				return null;
			}

			return prefab;
		}

		private static void InstantiatePrefab(MenuCommand menuCommand, string prefabName, string undoName, [CanBeNull] string objectName = null)
		{
			GameObject prefab = LoadPrefab(prefabName);
			
			if (!prefab) return;
			
			GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
			
			if (!instance)
			{
				Debug.LogWarning("Prefab instance is null");
				return;
			}

			if (objectName != null) instance.name = objectName;
			
			GameObject parent = Selection.activeGameObject;
			
			if (menuCommand.context as GameObject != null)
				parent = menuCommand.context as GameObject;
			
			if (parent) {GameObjectUtility.SetParentAndAlign(instance, parent);}
				
			Undo.RegisterCreatedObjectUndo(instance, undoName);

			Selection.activeGameObject = instance;
		}
	}
}
