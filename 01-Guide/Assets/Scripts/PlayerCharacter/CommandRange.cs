using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CommandRange : MonoBehaviour
{
    public List<GameObject> targetObj = new List<GameObject>();
    public Text myText;
    public GameObject selectedEnemyObj;
    public int selectedTarget;

    //Probably create scriptable object to pass through values
    public PlayerCharacter myPlayer = new PlayerCharacter();


    private void Awake()
    {
        myPlayer = this.gameObject.GetComponentInParent<PlayerCharacter>();
        this.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (selectedTarget < targetObj.Count - 1)
            {
                selectedTarget++;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            selectedTarget--;
            if (selectedTarget <= -1)
            {
                selectedTarget = 0;
            }
        }

        if (selectedTarget < targetObj.Count && targetObj[selectedTarget] != null)
        {
            selectedEnemyObj = targetObj[selectedTarget];
            myText.text = targetObj[selectedTarget].gameObject.name.ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Target"))
        {
            if (other.transform.parent != null)
            {
                targetObj.Add(other.transform.parent.gameObject);
            }
            else
            {
                targetObj.Add(other.transform.gameObject);
            }

            Debug.Log(selectedEnemyObj);
        }
    }
    private void OnDisable()
    {
        targetObj.Clear();
        selectedEnemyObj = null;
    }



    public void SendDamage()
    {
        //Send Events to playercharacter to deduct sp at the same time send event to deduct enemies health.
        if (selectedEnemyObj != null)
        {
            myPlayer.CurrentSP -= 1;
         //   selectedEnemyObj.GetComponent<EnemyStatus>().TakeDamage(myPlayer.AttackPoint);
            myPlayer.commandMode = false;
        }
    }

    public void ConfirmTarget()
    {
        TargetEventSystem.current.ConfirmTargetSelect(selectedEnemyObj);
    }
}
