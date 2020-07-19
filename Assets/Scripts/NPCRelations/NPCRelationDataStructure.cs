using UnityEngine;

[System.Serializable]
public class NPCRelations
{
	public string npcTag;
	public int npcFactionRating;

	NPCRelations(string npcStringTag,int npcINTFactionRating)
	{
		npcTag = npcStringTag;
		npcFactionRating = npcINTFactionRating;
	}
}
[System.Serializable]
public class NPCRealtionsArrray
{
	public string npcFaction;
	public NPCRelations[] npcRelations;
	public string[] myFriendlyTags;
	public string[] myEnemyTags;
	public LayerMask myFriendlyLayers;
	public LayerMask myEnemyLayers;

	NPCRealtionsArrray(string npcStringTagOfInterest,NPCRelations[] npcRelationsArray,string[] fTags,string[] eTags,LayerMask fLayers,LayerMask eLayers)
	{
		npcFaction = npcStringTagOfInterest;
		npcRelations = npcRelationsArray;
		myFriendlyTags = fTags;
		myEnemyTags = eTags;
		myFriendlyLayers = fLayers;
		myEnemyLayers = eLayers;
	}
}