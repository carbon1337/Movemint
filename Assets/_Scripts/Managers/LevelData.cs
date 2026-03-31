using UnityEngine;

[CreateAssetMenu(menuName = "Data/Level Data")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class LevelData : ScriptableObject
{
    public string levelID;
}
