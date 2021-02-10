using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerManagerSO")]
public class PlayerCharacter_SO : ScriptableObject
{
    public int hp;
    public int maxHp;
    public int attackPoint;
    public Vector3 playerPosition;
    public bool hasTeleport = false;
    //public float playerUpBoost;

    //Jump point?
}
