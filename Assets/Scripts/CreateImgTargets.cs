using System.IO;
using UnityEngine;
using Vuforia;

public class CreateImageTarget : MonoBehaviour
{

    //add it so that each card can be followed only once aka array or something that keeps seeing if the thing is followed 

    string dataSetPath = "Vuforia/cardDeck.xml";
    
    string[] ranks = { "ace", "king", "queen", "jack", "ten", "nine", "eight", "seven", "six", "five", "four", "three", "two" };
    string[] suits = { "clubs", "spades", "hearts", "diamonds" };

    public GameObject common;
    public GameObject rare;
    public GameObject epic;
    public GameObject legendary;



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

                if (mImageTarget == null)
                {
                    Debug.LogError("Failed to create image target: " + targetName);
                    continue;
                }

            
                mImageTarget.OnTargetStatusChanged += OnTargetStatusChanged;

                // Initialize dictionary entry with null (no prefab spawned yet)
                //spawnedPrefabs[targetName] = null;
            }
        }

    }

    void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        string targetName = behaviour.TargetName;
        GameObject prefabToPlace = GetPrefabForTarget(targetName);
        


        if (status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED )
        {

           
                GameObject spawned = Instantiate(prefabToPlace, behaviour.transform);
                spawned.transform.localPosition = Vector3.zero;
                spawned.transform.localRotation = Quaternion.identity;
                //spawnedPrefabs[targetName] = spawned;

                Debug.Log($"Spawned prefab for target: {targetName}");
        
        }
        

    }




    GameObject GetPrefabForTarget(string targetName)
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


}