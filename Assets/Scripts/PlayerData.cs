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
}