using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralisedController : MonoBehaviour
{
    

    private Animator animator;
    public int att;
    public int def;
    

    public bool hasAttackAnimation;
    public bool hasGetHitAnimation;
    public bool hasDieAnimation;

    private CreateImageTarget CreateImageTargetScriptInstance;

    

    [SerializeField] private ParticleSystem attParticle;
    [SerializeField] private ParticleSystem getHitParticle;
    [SerializeField] private ParticleSystem dieParticle;



    public GameObject turnTowards;
    public bool iAmOnRedTeam;

    private GeneralisedController enemyScript;


    private LineRenderer lineRenderer;


    private Vector3 rayOrigin; //hopefiully temporary


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        lineRenderer=GetComponent<LineRenderer>();

        CreateImageTargetScriptInstance=GameObject.Find("GameObject").GetComponent<CreateImageTarget>();




    
        //important note this has to be done this way because you can not assign static scene objects to prefabs 
        //so this needs to be found dynamically during runtime 
    }


    // Update is called once per frame
    void Update()
    {

        rayOrigin=transform.position + Vector3.up *0.4f;

        if (turnTowards!=null){

            // Determine which direction to rotate towards
            Vector3 targetDirection = turnTowards.transform.position - transform.position;

            // The step size is equal to speed times frame time.
            float singleStep = 2f * Time.deltaTime;
            targetDirection.y=0f;

            // Rotate the forward vector towards the target direction by one step
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);


            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(newDirection);
        }



        lineRenderer.SetPosition(0, rayOrigin); //this is just so player can tenatevly know where attack is
        lineRenderer.SetPosition(1, rayOrigin + transform.forward * 100);


        if(transform.name.Contains("Humpback")||transform.name.Contains("Spider")){
            lineRenderer.SetPosition(1, rayOrigin + -transform.forward * 100);
            
        }

    }


    public void TriggerAttack()
    {
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("Default");

        Debug.Log("Got to the GeneralisedController");

        Vector3 direction = transform.TransformDirection(Vector3.forward);

        if(transform.name.Contains("Humpback")||transform.name.Contains("Spider")){

            direction = transform.TransformDirection(-Vector3.forward);
        }


        if (Physics.Raycast(rayOrigin,direction, out hit, Mathf.Infinity, layerMask)){

            //Debug.DrawRay(rayOrigin, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red); 
            //Debug.Log("I hit: "+ hit.collider.gameObject.name);

            enemyScript=hit.transform.gameObject.GetComponents<GeneralisedController>()[0];
            makeDmg();


        }else{
            //say that it hit a base even tho the base is technically above so there is no chance to hit it but yes
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white); 
            //Debug.Log("Did not Hit"); 


            if(iAmOnRedTeam){
                CreateImageTargetScriptInstance.blueHealth=CreateImageTargetScriptInstance.blueHealth-att;
            }else{
                CreateImageTargetScriptInstance.redHealth=CreateImageTargetScriptInstance.redHealth-att;
            }
        }




        if(hasAttackAnimation){
            animator.SetTrigger("makeAttack");
        }

        if(attParticle!=null){
            attParticle.Play();
        }

    }
    
    void makeDmg(){
        int remainingHealth=enemyScript.def-att;

        if(remainingHealth<=0){
            enemyScript.TriggerDie();


            if(iAmOnRedTeam){
                CreateImageTargetScriptInstance.blueHealth=CreateImageTargetScriptInstance.blueHealth+remainingHealth;
            }else{
                CreateImageTargetScriptInstance.redHealth=CreateImageTargetScriptInstance.redHealth+remainingHealth;
            }

        }else{
            enemyScript.def=remainingHealth;
            enemyScript.TriggerGetHit();
        }

    }





    public void TriggerGetHit()
    {
        if(hasGetHitAnimation){
            animator.SetTrigger("makeGetHit");
        }
        
        if(getHitParticle!=null){
            getHitParticle.Play();
        }

    }

    public void TriggerDie()
    {

        if(hasDieAnimation){
            animator.SetTrigger("makeDie");
        }

        if(dieParticle!=null){
            dieParticle.Play();
        }

        Destroy(transform.parent.gameObject, 3f);

    }

}
