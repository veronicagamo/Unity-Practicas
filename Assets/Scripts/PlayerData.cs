using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int playerId;
    public string playerName;
    public int playerLevel;

    public PlayerData()
    {
    }

    public PlayerData(int id, string name, int level)
    {
        playerId = id;
        playerName = name;
        playerLevel = level;
    }
}
