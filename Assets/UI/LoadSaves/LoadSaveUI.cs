using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadSaveUI : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;
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

        for (int i = 0; i < playerData.SaveCount; i++)
        {
            var savedData = playerData.LoadedSaves[i];

            var loadSaveItemInstance = loadSaveItemVst.Instantiate();
            loadSaveItemInstance.Q<Label>("WorldLabel").text = savedData.world;
            if (savedData.world.StartsWith("STAGE "))
                loadSaveItemInstance.Q<ProgressBar>("ProgressBar").value = int.Parse(savedData.world.Split(" ")[1]);

            var idx = i;
            loadSaveItemInstance.Q<Button>("Button").clicked += () =>
            {
                Debug.Log("Setting save " + idx + " as active");

                playerData.SetActiveSave(idx);
            };

            listView.hierarchy.Add(loadSaveItemInstance);
        }
    }
}
