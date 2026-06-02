using System.IO;
using UnityEngine;

[System.Serializable]
public struct SaveData
{
    public PlayerSaveData playerSaveData;
    public GameSaveData gameSaveData;
}

public class SaveSystem
{
    private static SaveData saveData = new SaveData();

    public static string FileName { get => Application.persistentDataPath + "/save" + ".dat"; }


    public static void SaveGame()
    {
        GameManager.Instance.Save(ref saveData);
        File.WriteAllText(FileName, JsonUtility.ToJson(saveData));
    }

    public static void LoadGame()
    {
        string saveContent = File.ReadAllText(FileName);
        saveData = JsonUtility.FromJson<SaveData>(saveContent);

        GameManager.Instance.Load(saveData);
    }
}
