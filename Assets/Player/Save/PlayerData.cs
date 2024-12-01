using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/PlayerData")]
public class PlayerData : ScriptableObject
{
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "savedData.json");
    private static int LoadedSaveIdx;
    private List<PlayerSavedData> mSavedData = new();

    //here exist all data
    //the no-save exist for ui
    //this data need to be rebuild.
    [Header("Save Data")]
    public int health
    {
        get
        {
            if (mSavedData.Count - 1 < LoadedSaveIdx)
                return 3;

            return mSavedData[LoadedSaveIdx].health;
        }
        set
        {
            if (mSavedData.Count - 1 < LoadedSaveIdx)
                return;

            mSavedData[LoadedSaveIdx].health = value;
        }
    }

    public Vector3 checkPoint
    {
        get
        {
            if (mSavedData.Count - 1 < LoadedSaveIdx)
                return default;

            return mSavedData[LoadedSaveIdx].checkPoint;
        }
        set
        {
            if (mSavedData.Count - 1 < LoadedSaveIdx)
                return;

            mSavedData[LoadedSaveIdx].checkPoint = value;
        }
    }

    /// <summary>
    /// Always use try add on this array.
    /// </summary>

    public List<Type> Powers { private set; get; } = new();

    [Header("No-save Data")]
    public float stamina;
    public Vector3 lastSafePlace;

    //Function and utilites

    public void OnSceneChanged(Scene _, Scene scene)
    {
        if (mSavedData.Count - 1 < LoadedSaveIdx)
            return;

        var activeSceneName = scene.name;
        if (activeSceneName.StartsWith("STAGE"))
            mSavedData[LoadedSaveIdx].world = activeSceneName;
    }

    public void SetActiveSave(int i)
    {
        LoadedSaveIdx = i;

        var saveData = SaveCount - 1 < LoadedSaveIdx ? null : mSavedData[LoadedSaveIdx];
        if (saveData != null)
            SceneManager.LoadScene(saveData.world);
    }

    public void BeginNewSave()
    {
        mSavedData.Add(new());
        LoadedSaveIdx = mSavedData.Count - 1;
    }

    private void OnEnable()
    {
        Debug.Log($"{nameof(PlayerData)} enabled");
        Powers.Clear();
        LoadData();

        SceneManager.activeSceneChanged += OnSceneChanged;
    }
    public bool TryAddPower(PowerItem power)
    {
        if (Powers.Count >= 3) return false;
        if (Powers.Contains(PowerItem.GetPowerType(power.type))) return false;
        Powers.Add(PowerItem.GetPowerType(power.type));
        Debug.Log("added: " + power.ToString());

        return true;
    }
    public void ReplacePower(PowerItem newPower, PowerItem oldPower)
    {
        Powers.Remove(PowerItem.GetPowerType(oldPower.type));
        Powers.Add(PowerItem.GetPowerType(oldPower.type));
    }

    public List<PlayerSavedData> LoadedSaves =>
        mSavedData;
    public int SaveCount =>
        mSavedData.Count;

    private void OnDisable()
    {
        Debug.Log($"{nameof(PlayerData)} disabled");
        SaveData();
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    public void SaveData()
    {
        // Convert the mSavedData list to JSON
        string json = JsonUtility.ToJson(new SerializableListWrapper<PlayerSavedData>(mSavedData), true);

        // Save the JSON to a file
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"Data saved to {SaveFilePath}");
    }

    public void LoadData()
    {
        if (File.Exists(SaveFilePath))
        {
            // Read JSON from the file
            string json = File.ReadAllText(SaveFilePath);

            // Deserialize JSON back to the list
            SerializableListWrapper<PlayerSavedData> wrapper = JsonUtility.FromJson<SerializableListWrapper<PlayerSavedData>>(json);
            mSavedData = wrapper?.List ?? new List<PlayerSavedData>();
            Debug.Log("Data loaded successfully");
        }
        else
        {
            Debug.LogWarning("Save file not found, starting with empty data.");
            mSavedData = new List<PlayerSavedData>();
        }
    }

    [Serializable]
    private class SerializableListWrapper<T>
    {
        public List<T> List;

        public SerializableListWrapper(List<T> list)
        {
            List = list;
        }
    }
    //todo: implement rebuild
}