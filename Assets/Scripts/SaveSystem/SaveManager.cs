using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public class SaveManager: MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }
        
        public GameSaveData CurrentData { get; private set; }
        public GameDatabase gameDatabase;

        private string saveFilePath;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            saveFilePath = Path.Combine(Application.persistentDataPath, "SaveData.json");
            LoadGame();
        }

        public void SaveGame()
        {
            string json = JsonUtility.ToJson(CurrentData, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("Save game saved to path: " + saveFilePath);
        }

        public void LoadGame()
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                CurrentData = JsonUtility.FromJson<GameSaveData>(json);
                Debug.Log("Game loaded from path: " + saveFilePath);
            }
            else
            {
                CurrentData = new GameSaveData();
                Debug.Log("No save file found in path: " + saveFilePath + " ; Starting new one");
            }
        }
    }
}