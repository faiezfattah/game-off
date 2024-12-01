using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;
    private VisualElement doc;
    private void Awake()
    {
        doc = GetComponent<UIDocument>().rootVisualElement;

        doc.Q<Button>("ContinueButton").clicked += () =>
        {
            Debug.Log("Continue clicked");
            SceneManager.LoadScene("LoadSaves");
        };

        doc.Q<Button>("NewGameButton").clicked += () =>
        {
            Debug.Log("NewGame clicked");

            playerData.BeginNewSave();
            SceneManager.LoadScene("STAGE 1");
        };
    }

    private void Start()
    {
        if (playerData.SaveCount < 1)
        {
            Debug.Log("SaveCount is " + playerData.SaveCount + " disabling continue...");
            doc.Q<Button>("ContinueButton").visible = false;
        }
    }
}
