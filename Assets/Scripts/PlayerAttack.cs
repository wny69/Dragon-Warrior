using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown = 0.5f; // ← add this

    [SerializeField] private Transform firePoint;

    [SerializeField] private GameObject[] fireballs;



    private Animator animator;
    private Playermovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<Playermovement>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime; // ← add this, otherwise it stays at 0 after attacking

        if (Mouse.current.leftButton.wasPressedThisFrame && cooldownTimer > attackCooldown && playerMovement.canAttack())
            Attack();
    }

    private void Attack()
    {
        cooldownTimer = 0;
        animator.SetTrigger("attack");
        fireballs[0].transform.position = firePoint.position;
        fireballs[0].GetComponent<Projectile>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0; // if all fireballs are active, overwrite the first one
    }
}