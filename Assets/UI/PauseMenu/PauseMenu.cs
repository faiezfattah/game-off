using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private InputReader inputReader;
    private UIDocument doc;

    private void TogglePause()
    {
        var root = doc.rootVisualElement.Q("root");

        Debug.Log("Toggle Pause");
        root.ToggleInClassList("visible");

        if (root.ClassListContains("visible"))
        {
            inputReader.Paused = true;
            Time.timeScale = 0;
        }
        else
        {
            inputReader.Paused = false;
            Time.timeScale = 1;
        }
    }
    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement.Q("root");

        root.Q<Button>("ContinueButton").clicked += TogglePause;

        root.RemoveFromClassList("visible");

        inputReader.EscapeEvent += TogglePause;
    }
}
