using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadSaveUI : MonoBehaviour
{
    [SerializeField]
    private VisualTreeAsset loadSaveItemVst;
    [SerializeField]
    private UIDocument loadSavesUiDoc;

    private void Awake()
    {
        loadSavesUiDoc.rootVisualElement.Q<Button>("BackButton").clicked += () =>
        {
            SceneManager.LoadScene("MainMenu");
        };

        var listView = loadSavesUiDoc.rootVisualElement.Q<ListView>("GameSaves");
        listView.hierarchy.Add(loadSaveItemVst.Instantiate());
        listView.hierarchy.Add(loadSaveItemVst.Instantiate());
        listView.hierarchy.Add(loadSaveItemVst.Instantiate());
        listView.hierarchy.Add(loadSaveItemVst.Instantiate());
        listView.hierarchy.Add(loadSaveItemVst.Instantiate());
    }
}
