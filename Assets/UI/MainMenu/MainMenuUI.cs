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
    }
}
