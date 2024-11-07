using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSave", menuName = "Scriptable Objects/PlayerSave")]
public class PlayerSave : ScriptableObject
{
    private void OnEnable()
    {
        Debug.Log("Enabled");
    }

}
