using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class statsDisplay : MonoBehaviour
{

    
    private TMP_Text attText;
    private TMP_Text defText;
    GeneralisedController monsterStats;



    void Start()
    {
        // Get the text component attached to this Canvas object
        defText = transform.Find("displayDeffense").GetComponent<TMP_Text>();
        attText = transform.Find("displayAttack").GetComponent<TMP_Text>();

        // Find the parent's GeneralisedController
        monsterStats = GetComponentInParent<GeneralisedController>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.forward = Camera.main.transform.forward;
        attText.text="Att: "+monsterStats.att;
        defText.text="Def: "+monsterStats.def;
    }
}
