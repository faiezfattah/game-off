using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

#if UNITY_EDITOR
[InitializeOnLoad]
#endif
public class MouseCenteredInputProcessor : InputProcessor<Vector2> { 
    static MouseCenteredInputProcessor() {
        Initialize();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Initialize() {
        InputSystem.RegisterProcessor<MouseCenteredInputProcessor>();
    }

    Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
    public override Vector2 Process(Vector2 value, InputControl control) {
        Vector2 mouseVector = value - screenCenter;
        return mouseVector.normalized;
    }
}
