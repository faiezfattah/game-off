using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
[Preserve]
public class MouseCenteredInputProcessor : InputProcessor<Vector2> { 
    static MouseCenteredInputProcessor() {
        Initialize();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize() {
        InputSystem.RegisterProcessor<MouseCenteredInputProcessor>();
    }
    public bool normalize = true;
    public override Vector2 Process(Vector2 value, InputControl control) {
        Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
        Vector2 mouseVector = value - screenCenter;
        return normalize ? mouseVector.normalized : mouseVector;
    }
}
