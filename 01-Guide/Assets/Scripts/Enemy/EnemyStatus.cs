using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// , ITakeDamage
public class EnemyStatus : MonoBehaviour, ITakeDamage
{
    //Create an scriptable object later... for separation stuff
    public int hp = 3;
    public int attack = 5;

    public PlayerCharacter_SO playerStats;

    // Start is called before the first frame update
    void Start()
    {
        TargetEventSystem.current.onConfirmTargetSelect += ObjectTargeted;
    }

    private void ObjectTargeted(GameObject obj)
    {
        //only work if time is moving.
        if (obj == this.gameObject)
        {
            Debug.Log("TakeDamage");
            hp-= playerStats.totalDmg;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Death();
    }

    private void OnDisable()
    {
        TargetEventSystem.current.onConfirmTargetSelect -= ObjectTargeted;
    }

    public void Death()
    {
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void TakeDamage(int takeDamge)
    {
            hp-=takeDamge;
    }
}
