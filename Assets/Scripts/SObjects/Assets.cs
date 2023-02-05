using System.Collections.Generic;
using Roots.SObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "Assets", menuName = "Game/Assets", order = 1)]
public class Assets : ScriptableObject
{
	[field: SerializeField]
	public List<CardData> Cards { get; private set; }


}
