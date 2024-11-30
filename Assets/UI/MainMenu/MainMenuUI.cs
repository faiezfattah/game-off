using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private PlayerSave save;
    private VisualElement doc;

    private void Awake()
    {
        doc = GetComponent<UIDocument>().rootVisualElement;

        doc.Q<Button>("Continue").clicked += () =>
        {
            SceneManager.LoadScene("LoadSaves");
        };
    }
}
