using CookieUtils.Extras.Scenes;
using UnityEngine;
using UnityEngine.UI;

public class LoadSceneGroupButton : MonoBehaviour
{
    [SerializeField] private string groupName;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => _ = Scenes.LoadGroup(groupName));
    }
}
