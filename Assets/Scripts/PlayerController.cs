using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public float attackDuration = 1f;

    private CharacterController characterController;
    private Animator animator;
    private bool isAttacking = false;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isAttacking)
        {
            return;
        }

        HandleMovement();
        HandleAttack();
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            characterController.Move(direction * speed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(PerformAttack("isPunching"));
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(PerformAttack("isKicking"));
        }
    }

    IEnumerator PerformAttack(string attackTrigger)
    {
        isAttacking = true;
        animator.SetBool("isRunning", false);
        animator.SetTrigger(attackTrigger);

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }
}