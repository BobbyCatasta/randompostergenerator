using System.IO;
using UnityEngine;

/// <summary>
/// Static class handling game save/load operations.
/// </summary>
public static class SaveSystem
{
    /// <summary>
    /// Path to the save file.
    /// </summary>
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
    
    /// <summary>
    /// Saves the current game state to a file.
    /// </summary>
    /// <param name="saveData">The SaveData object containing game state.</param>
    public static void SaveGame(SaveData saveData)
    {
        string jsonString = JsonUtility.ToJson(saveData,true);
        File.WriteAllText(SavePath, jsonString);
        Debug.Log("Game Saved");
    }

    /// <summary>
    /// Deletes the save file if it exists.
    /// </summary>
    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("Save deleted");
        }
    }

    /// <summary>
    /// Loads game state from save file.
    /// </summary>
    /// <returns>Loaded SaveData object, or null if no save exists.</returns>
    public static SaveData LoadGame()
    {
        if (!File.Exists(SavePath))
        {
            Debug.LogWarning("No save file found");
            return null;
        }

        string json = File.ReadAllText(SavePath);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        return saveData;
    }
}