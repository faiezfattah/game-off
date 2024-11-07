using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private PlayerSave save;
    private VisualElement doc;

    private void Awake()
    {
        doc = GetComponent<UIDocument>().rootVisualElement;

        doc.Q<Button>("Button").clicked += () =>
        {
            SceneManager.LoadScene(nameof(MainMenu));
        };
    }
}
