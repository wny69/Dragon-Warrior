using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private LayerMask targetLayer; // assign Enemy + Ground layers in Inspector

    private float direction;
    private bool hit;
    private float lifetime;
    private Animator animator;
    private BoxCollider2D boxCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (!hit)
        {
            float movementSpeed = speed * Time.deltaTime * direction;
            transform.Translate(movementSpeed, 0, 0);
        }

        // lifetime always counts — deactivates even if animation event is never called
        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }

 private void OnTriggerEnter2D(Collider2D collision)
{
    // temporarily remove layer check to test
    Enemy_Sideways enemy = collision.GetComponent<Enemy_Sideways>();
    if (enemy != null)
    {
        Debug.Log("Hit enemy!");
        enemy.StopEnemy();
    }

    hit = true;
    boxCollider.enabled = false;
    animator.SetTrigger("explode");
}

    public void SetDirection(float _direction)
    {
        lifetime = 0;
        direction = _direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    // called by animation event at end of explode animation
    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}