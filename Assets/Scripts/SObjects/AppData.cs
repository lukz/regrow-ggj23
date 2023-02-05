using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AppData", menuName = "Game/AppData", order = 0)]
public class AppData : ScriptableObject
{
	public Assets assets;
	public GameData data;
}
