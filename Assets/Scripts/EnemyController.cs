using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState { Searching, Following, Death, Idle };


public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject pickup = null;

    GameObject target = null;
    public GameObject shadow;

    EnemyState currentState = EnemyState.Searching;

    public float distanceFromTarget;
    public float health;
    public float speed;
    bool pickDirection = false;
    Vector3 randomDirection;
    private AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip deathSound;



    void Start()
    {
        audioSource = GetComponent<AudioSource>();
            target = GameObject.FindGameObjectWithTag("Player");
        
    }

    void Update()
    {
        if (target.GetComponent<PlayerController>().isDead == true)
        {
            currentState = EnemyState.Idle;
        }

        switch (currentState)
        {
            case (EnemyState.Searching):
                Search();
                break;
            case (EnemyState.Following):
                Follow();
                break;
            case (EnemyState.Death):
                //StartCoroutine("Die");
                break;
            case (EnemyState.Idle):
                Idle();
                break;
        }


        if (currentState != EnemyState.Idle || GameController._instance.isGameOver == false)
        {
            if (withinRange(distanceFromTarget) && (currentState != EnemyState.Death))
            {
                currentState = EnemyState.Following;
            }
            else if (!withinRange(distanceFromTarget) && (currentState != EnemyState.Death))
            {
                currentState = EnemyState.Searching;
            }
        }

        if (health <= 0)
        {
            currentState = EnemyState.Death;
        }
    }

    private bool withinRange(float range)
    {
        return  Vector3.Distance(target.transform.position, transform.position) <= range;
    }

    IEnumerator Wandering()
    {
        pickDirection = true;
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        float xDir = Random.Range(-1f, 1f);
        float yDir = Random.Range(-1f, 1f);

        randomDirection = new Vector3(xDir, yDir, 0);
        pickDirection = false;
    }

    public void TakeDamage (int damage)
    {
        PlaySound(hitSound);
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine("Die");
        }
    }
    void Search()
    {
        if (!pickDirection)
        {
            StartCoroutine("Wandering");
        }

        transform.position += randomDirection * speed * Time.deltaTime;
        if (withinRange(distanceFromTarget)){
            currentState = EnemyState.Following;
        }
    }

    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
    }

    IEnumerator Die()
    {
        GetComponent<SpriteRenderer>().sprite = null;
        shadow.GetComponent<SpriteRenderer>().sprite = null;
        GetComponent<Collider2D>().enabled = false;
        PlaySound(deathSound);
        GameObject.Instantiate(pickup, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    void Idle()
    {
        return;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle") {
            randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        }

        if (collision.gameObject.tag == "Player")
        {
            currentState = EnemyState.Idle;

            collision.gameObject.GetComponent<PlayerController>().Death();

        }

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.PlayOneShot(clip);
    }

}
