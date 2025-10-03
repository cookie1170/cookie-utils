using CookieUtils;
using UnityEngine;

public class DebugUISampleObject : MonoBehaviour, IDebugDrawer
{
    private void Awake()
    {
        CookieDebug.Register(this);
    }

    public IDebugUIBuilder DrawDebugUI(IDebugUIBuilderProvider provider)
    {
        return provider.Get(this)
            .Label("This is some cool debug text!")
            .Foldout("Stats")
            .Label($"Position is {transform.position}")
            .Label($"Rotation is {transform.eulerAngles}")
            .EndFoldout();
    }
}
