using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private PlayerData playerData;
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
        root.Q<Button>("SaveButton").clicked += playerData.SaveData;
        root.Q<Button>("QuitButton").clicked += Application.Quit;

        root.RemoveFromClassList("visible");

        inputReader.EscapeEvent += TogglePause;
    }
}
