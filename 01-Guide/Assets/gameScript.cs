using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {


        // myPlayer.Experience = 900;
        //int x = myPlayer.Experience;


    }

    // Update is called once per frame
    void Update()
    {
        PlayerExp myPlayer = new PlayerExp();
        myPlayer.Experience = 3600;
    }
}
