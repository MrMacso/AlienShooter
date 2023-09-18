using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public int Coins;
    public int Health = 6;
    public Vector2 Position;
    public Vector2 Velocity;
}

[Serializable]
public class GameData
{ 
    public List<PlayerData> PlayerDatas = new List<PlayerData>();

    public string GameName;

    public string CurrentLevelName;
    public List<LevelData> LevelDatas = new List<LevelData>();
}

[Serializable]
public class LevelData
{
    public string LevelName;
    public List<CoinData> CoinDatas = new List<CoinData>();
}

[Serializable]
public class CoinData
{
    public string Name;
    public bool IsCollected;
}