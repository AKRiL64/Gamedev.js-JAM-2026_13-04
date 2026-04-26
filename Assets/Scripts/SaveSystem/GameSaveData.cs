using System;
using System.Collections.Generic;

namespace SaveSystem
{
    [Serializable]
    public class GameSaveData
    {
        public List<HoneySaveData> honeyInventory = new List<HoneySaveData>();
        public List<string> queuedRecipeNames = new List<string>();
        public List<BeeSaveData> bees = new List<BeeSaveData>();
        public List<PotSaveData> pots = new List<PotSaveData>();
    }

    [Serializable]
    public struct HoneySaveData
    {
        public string honeyName;
        public int amount;
        
    }

    [Serializable]
    public struct BeeSaveData
    {
        public string beeName;
        public string assignedPlantName;
    }

    [Serializable]
    public struct PotSaveData
    {
        public string potID;
        public string plantName;
        public int currentGrowth;
        public bool isWatered;
    }
}