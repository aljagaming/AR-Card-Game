using System.IO;
using System.Collections;
using UnityEngine;
using Vuforia;
using UnityEngine.UI;
using TMPro;

public class CreateImageTarget : MonoBehaviour
{

    //add it so that each card can be followed only once aka array or something that keeps seeing if the thing is followed 

    string dataSetPath = "Vuforia/cardDeck.xml";
    
    string[] ranks = { "ace", "king", "queen", "jack", "ten", "nine", "eight", "seven", "six", "five", "four", "three", "two" };
    string[] suits = { "clubs", "spades", "hearts", "diamonds" };

    string[] alreadyInUse = new string[52];

    private GameObject[] redArmy = new GameObject[3];

    private GameObject[] blueArmy = new GameObject[3];

   

    public GameObject common;
    public GameObject rare;
    public GameObject epic;
    public GameObject legendary;


    public GameObject Chicken;
    public GameObject IceBoy;
    public GameObject Skeleton;
    public GameObject Spider;
    public GameObject Drone;
    public GameObject Turtle;
    public GameObject Orc;
    public GameObject WolfBoss;
    public GameObject DogKnight;
    public GameObject HumpbackWhale;
    public GameObject MonsterX;
    public GameObject Golem;
    public GameObject Dragon;

    
    //Game Logic
    //two sides A and B
    //both defend their "hearth"

    private GameObject referencePlane;
    private GameObject blueTurnIndicator;
    private GameObject redTurnIndicator;
    private GameObject popUp;
    private TMP_Text popUpText;

    private GameObject blueBase;
    private GameObject redBase;


    
    private bool redTurn; // whos turn it is, red is to the left blue is to the right;
    private bool blueTurn;


    private GeneralisedController scriptInstance;

    






    // Start is called before the first frame update
    
    void Start()
    {

        //referencePlane=GameObject.Find("ReferencePlane");
        blueTurnIndicator=GameObject.Find("blueturn");
        redTurnIndicator=GameObject.Find("redturn");

        blueBase=GameObject.Find("blueBase");
        redBase=GameObject.Find("redBase");
        popUp=GameObject.Find("PopUp");

        popUpText = popUp.GetComponent<TMP_Text>();
        popUp.SetActive(false);


        




        if(UnityEngine.Random.Range(0, 2)==0){

            redTurn=false;
            ChangeTurns();
        }else{
            redTurn=true;
            ChangeTurns();
        }



        VuforiaApplication.Instance.OnVuforiaInitialized += OnVuforiaInitialized;
       
       
       
       
        referencePlane=GameObject.Find("ReferencePlane");
        /*
        
        
        GameObject plane = Instantiate(rare, Vector3.zero, Quaternion.identity);


        GameObject dogKnight = Instantiate(HumpbackWhale, plane.transform);
    
        Debug.Log("DogKnight is now a child of the plane!");
        
        */
        


    }
    

    void OnVuforiaInitialized(VuforiaInitError error)
    {
        if (error == VuforiaInitError.NONE)
        OnVuforiaStarted();
    }

    // Load and activate a data set at the given path.
    void OnVuforiaStarted()
    {

        string targetName;
        for (int i = 0; i < suits.Length; i++)
        {
            for (int j = 0; j < ranks.Length; j++)
            {
                targetName = ranks[j] + "of" + suits[i];
                var mImageTarget = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(dataSetPath, targetName);


            
                mImageTarget.OnTargetStatusChanged += OnTargetStatusChanged;
            }
        }


        //add the end of move card
        var blackCard = VuforiaBehaviour.Instance.ObserverFactory.CreateImageTarget(dataSetPath, "blackcard");
        blackCard.OnTargetStatusChanged += OnTargetStatusChanged;

    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {

        string targetName = behaviour.TargetName;


        if(targetName== "blackcard"){

            Debug.Log("End of turn");
            endOfTurn();
            return;
        }


        for (int i=0; i<alreadyInUse.Length;i++){
            if(alreadyInUse[i]==targetName){
                return;
            }else{
                if(alreadyInUse[i]==null){
                    alreadyInUse[i]=targetName;
                    break;
                }
            }

        }



        GameObject planeToPlace = GetPrefabPlane(targetName);
        //needs change to that function 


        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED )
        {

           
            GameObject plane = Instantiate(planeToPlace, behaviour.transform);
            plane.transform.localPosition = Vector3.zero;
            plane.transform.localRotation = Quaternion.identity;
            //spawnedPrefabs[targetName] = spawned;


            GameObject monsterPrefab = GetPrefabMonster(targetName);
            GameObject monster = Instantiate(monsterPrefab, plane.transform); // parented to plane


            scriptInstance=monster.GetComponent<GeneralisedController>();

            scriptInstance.att=(int)(scriptInstance.att*(1+plainBonuses(targetName)));
            scriptInstance.def=(int)(scriptInstance.def*(1+plainBonuses(targetName)));



            if(redTurn){
                scriptInstance.turnTowards=blueBase;

                for (int i=0; i<redArmy.Length; i++){
                    if(redArmy[i]==null){
                        redArmy[i]=plane;
                    }
                }



            }else{

                scriptInstance.turnTowards=redBase;

                for (int i=0; i<blueArmy.Length; i++){
                    if(blueArmy[i]==null){
                        blueArmy[i]=plane;
                    }
                }

            }


        
        }
        

    }




    GameObject GetPrefabPlane(string targetName)
    {
        // Determine which prefab to use based on the suit in target name
        if (targetName.Contains("spades"))
            return legendary;
        else if (targetName.Contains("hearts"))
            return epic;
        else if (targetName.Contains("diamonds"))
            return rare;
        else if (targetName.Contains("clubs"))
            return common;
        else
            return null;
    }



    GameObject GetPrefabMonster(string targetName)
    {
    
        if (targetName.Contains("ace"))
            return Chicken;
        else if (targetName.Contains("two"))
            return IceBoy;
        else if (targetName.Contains("three"))
            return Skeleton;
        else if (targetName.Contains("four"))
            return Spider;
        else if (targetName.Contains("five"))
            return Drone;
        else if (targetName.Contains("six"))
            return Turtle;
        else if (targetName.Contains("seven"))
            return Orc;
        else if (targetName.Contains("eight"))
            return WolfBoss;
        else if (targetName.Contains("nine"))
            return DogKnight;
        else if (targetName.Contains("ten"))
            return HumpbackWhale;
        else if (targetName.Contains("jack"))
            return MonsterX;
        else if (targetName.Contains("queen"))
            return Golem;
        else if (targetName.Contains("king"))
            return Dragon;
        else
            return null;
    }



    void ChangeTurns(){
        if(redTurn){
            redTurn=false;
            blueTurn=true;
            redTurnIndicator.SetActive(false);
            blueTurnIndicator.SetActive(true);

            writePopUp("Blue's Turn");
        }else{
            redTurn=true;
            blueTurn=false;
            blueTurnIndicator.SetActive(false);
            redTurnIndicator.SetActive(true);
            writePopUp("Red's Turn");
        }
    }


    void endOfTurn(){

    }


    float plainBonuses(string targetName){

        // Determines how much points to return 
        if (targetName.Contains("spades"))
            return 0.15f;
        else if (targetName.Contains("hearts"))
            return 0.10f;
        else if (targetName.Contains("diamonds"))
            return 0.5f;
        else if (targetName.Contains("clubs"))
            return 0f;
        else
            return 0f;
    }


    void writePopUp(string message){
        popUpText.text = message;
        popUp.SetActive(true);
        StartCoroutine(HidePopup(3f));
    }

    IEnumerator HidePopup(float delay)
    {
        yield return new WaitForSeconds(delay);  
        popUp.SetActive(false);             
    }



}