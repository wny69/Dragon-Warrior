using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{


[SerializeField] private float speed = 10f;
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        if (hit) return;
        float movementspeed = speed * Time.deltaTime * direction;
        transform.Translate(movementspeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > 5) gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void  OnTriggerEnter2D(Collider2D collision)
    {
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
        if (Mathf.Sign(localScaleX) != direction)
        localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
   
}
