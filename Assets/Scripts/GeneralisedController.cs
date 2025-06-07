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

    

    [SerializeField] private ParticleSystem attParticle;
    [SerializeField] private ParticleSystem getHitParticle;
    [SerializeField] private ParticleSystem dieParticle;
    public GameObject turnTowards;

    



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();



    
        //important note this has to be done this way because you can not assign static scene objects to prefabs 
        //so this needs to be found dynamically during runtime 
    

        if(turnTowards!=null){
            Debug.Log("Found enemy base");
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (turnTowards!=null){

         // Determine which direction to rotate towards
        Vector3 targetDirection = turnTowards.transform.position - transform.position;

        // The step size is equal to speed times frame time.
        float singleStep = 2f * Time.deltaTime;

        targetDirection.y=0f;

        // Rotate the forward vector towards the target direction by one step
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        // Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, newDirection, Color.red);

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(newDirection);


        }

    }


    public void TriggerAttack()
    {
        if(hasAttackAnimation){
            animator.SetTrigger("makeAttack");
        }

        if(attParticle!=null){
            attParticle.Play();
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

    }

}
