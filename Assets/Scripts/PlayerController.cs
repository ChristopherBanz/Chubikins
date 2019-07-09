using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    Vector3 movement;
    public Sprite deadSprite;

    public GameObject projectilePrefab;

    public float projectileSpeed;
    private float lastShot = 0;
    public float projectileDelay;
    public bool isDead = false;

    private AudioSource audioSource;
    public AudioClip throwSound;
    public AudioClip deathSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        float shootHorizontal = Input.GetAxis("ShootHorizontal");
        float shootVertical = Input.GetAxis("ShootVertical");

        movement = new Vector3(Input.GetAxis("MoveHorizontal"), Input.GetAxis("MoveVertical"), 0.0f);

        if (!isDead)
        {
            if ((shootVertical != 0 || shootHorizontal != 0) && Time.time > (lastShot + projectileDelay))
            {
                Throw(shootHorizontal, shootVertical);
                lastShot = Time.time;
            }
        }

    }

    private void FixedUpdate()
    {
        if (!isDead)
        {
            Movement(movement);
        }
    }


    void Throw(float x, float y)
    {
        PlaySound(throwSound);
        audioSource.Play();
        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation) as GameObject;
        projectile.AddComponent<Rigidbody2D>().gravityScale = 0;
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector3(
            (x < 0) ? Mathf.Floor(x) * projectileSpeed : Mathf.Ceil(x) * projectileSpeed,
            (y < 0) ? Mathf.Floor(y) * projectileSpeed : Mathf.Ceil(y) * projectileSpeed,
            0f);
    }
    private void Movement(Vector3 input)
    {
        transform.position += input * speed * Time.deltaTime;
    }

    public void Death()
    {
        isDead = true;
        PlaySound(deathSound);
        GameController._instance.GameOver();
        gameObject.GetComponent<SpriteRenderer>().sprite = deadSprite;
        Quaternion rotation = Quaternion.Euler(0, 0, -90);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 2f);
    }


    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play(); 
    }

}
