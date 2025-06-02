using System.IO;
using UnityEngine;
using Vuforia;

public class CreateImageTarget : MonoBehaviour
{

    //add it so that each card can be followed only once aka array or something that keeps seeing if the thing is followed 

    string dataSetPath = "Vuforia/cardDeck.xml";
    
    string[] ranks = { "ace", "king", "queen", "jack", "ten", "nine", "eight", "seven", "six", "five", "four", "three", "two" };
    string[] suits = { "clubs", "spades", "hearts", "diamonds" };

    string[] alreadyInUse = new string[52];
   

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









    // Start is called before the first frame update
    
    void Start()
    {
        VuforiaApplication.Instance.OnVuforiaInitialized += OnVuforiaInitialized;
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
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localRotation = Quaternion.identity;


            Debug.Log($"Spawned prefab for target: {targetName}");
        
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

    
}