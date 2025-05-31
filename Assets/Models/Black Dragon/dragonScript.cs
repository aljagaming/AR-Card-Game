using UnityEngine;

public class DragonAttackController : MonoBehaviour
{
    private Animator animator;
    

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // This function will be called by the button
    public void TriggerAttack()
    {
        animator.SetTrigger("makeAttack");        
    }
}
