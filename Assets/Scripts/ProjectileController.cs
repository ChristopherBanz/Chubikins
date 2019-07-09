using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float torque;
    public float bulletDuration;
    public int damage = 1;
    Rigidbody2D rb;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("BulletDeath");
    }


    IEnumerator BulletDeath()
    {
        yield return new WaitForSeconds(bulletDuration);
        Destroy(gameObject);
        yield return null;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 15);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(damage);
        }
    }
}
