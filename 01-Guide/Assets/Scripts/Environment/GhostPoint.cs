using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPoint : MonoBehaviour, ITakePosition
{
    public PlayerCharacter_SO playerStats;
    public GameObject myPlayerCharacter;
    public void TakePosition(Vector3 takeposition)
    {
        takeposition = this.gameObject.transform.position;
    }

    private void Awake()
    {


    }

    // Start is called before the first frame update
    void Start()
    {
        TargetEventSystem.current.onConfirmTargetSelect += OnTeleportTarget;
    }

    private void OnTeleportTarget(GameObject obj)
    {
        if (obj == this.gameObject)
        {
            Debug.Log("TeleportToTarget");
            playerStats.hasTeleport = true;
            playerStats.playerPosition = this.gameObject.transform.position;
            //myPlayerCharacter.transform.position = gameObject.transform.position;
        }
    }

    private void OnDisable()
    {
        TargetEventSystem.current.onConfirmTargetSelect -= OnTeleportTarget;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
