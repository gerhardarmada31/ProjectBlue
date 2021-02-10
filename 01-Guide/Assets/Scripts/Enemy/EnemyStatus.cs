using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// , ITakeDamage
public class EnemyStatus : MonoBehaviour
{
    //Create an scriptable object later... for separation stuff
    public int hp = 3;
    public int attack = 5;

    public PlayerCharacter_SO playerStats;

    // Start is called before the first frame update
    void Start()
    {
        TargetEventSystem.current.onConfirmTargetSelect += TakeDamage;
    }

    private void TakeDamage(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            Debug.Log("TakeDamage");
            hp-=playerStats.attackPoint;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDisable()
    {
        TargetEventSystem.current.onConfirmTargetSelect -= TakeDamage;
    }

    // public void TakeDamage(int takeDamge)
    // {
    //    hp -= takeDamge;
    // }
}
