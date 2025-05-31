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



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        
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
