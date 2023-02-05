using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Global storage
/// </summary>
[CreateAssetMenu(fileName = "GameData", menuName = "Game/GameData")]
public class GameData : ScriptableObject
{
	public GameState gameState;
	
	[Serializable]
	public class GameState
	{
		// public IntVariable turnsLeft;
		// public IntVariable artifactsGot;
	}

    
    private void OnEnable()
    {
    }
    
}
