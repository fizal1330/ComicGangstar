using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.XR;

public class PlaneController : MonoBehaviour
{
    //plane speed
    public float moveSpeed = 15f;
    //mobile joystic
    [SerializeField] FixedJoystick joystic;
    //explosion
    [SerializeField] GameObject explosion;
    //smoke
    [SerializeField] GameObject smoke;
    //rogidbody of plane
    private Rigidbody2D rb;
    //horizontal and vertical axis
    private Vector2 movementAxis;
    //plane life
    int maxLife = 100;
    int currentLife;
    bool isExploded = false;

    //SCREEN BOUNDS
    Vector3 minScreenBounds;
    Vector3 maxScreenBounds;

    [Space(10)]
    [Header("AUDIOS")]
    public AudioSource audiosource;
    public AudioClip coinCollectAudio;
    public AudioClip boostCollectAudio;
    public AudioClip powerupCollectAudio;
    public AudioClip explosionAudio;
    public AudioClip hitAudio;

    private void OnEnable()
    {
        Actions.onMissileHit += ModifyLife;
        Actions.onBoostPicked += Boost;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isExploded= false;
        //assigning currentLife
        currentLife = maxLife;
        //setting screen bounds
        minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }

    void Update()
    {
        //assigning joystic input
        movementAxis.x = joystic.Horizontal;
        movementAxis.y= joystic.Vertical;
    }

    private void FixedUpdate()
    {
        if(!isExploded)
        {
            //changing the position of plane with respond to joystic
            rb.MovePosition(rb.position + movementAxis * moveSpeed * Time.fixedDeltaTime);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, minScreenBounds.x + 1, maxScreenBounds.x - 1), Mathf.Clamp(transform.position.y, minScreenBounds.y + 1, maxScreenBounds.y - 1), transform.position.z);
        }     
    }

    void ModifyLife()
    {
        //checking if the plane is above 0%
        if(currentLife >= 40)
        {
            currentLife = currentLife - 30;
        }
        else
        {
            audiosource.clip = explosionAudio;
            audiosource.Play();
            isExploded = true;
            GameObject effect = Instantiate(explosion, transform.position, Quaternion.identity);
            rb.isKinematic= true;
            Actions.onPlaneCrash();
            Destroy(effect, 2f);
        }
    }

    void Boost()
    {
        //boost, fully repairs the plane
        if(currentLife != maxLife)
        {
            currentLife = maxLife;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        switch(collision.gameObject.tag)
        {
            case "Missile":
                Actions.onMissileHit();
                audiosource.clip = hitAudio;
                audiosource.Play();
                Destroy(collision.gameObject);
                break;

            case "Coin":
                Actions.onCoinCollected();
                audiosource.clip = coinCollectAudio;
                audiosource.Play();
                Destroy(collision.gameObject);
                break;

            case "Boost":
                Actions.onBoostPicked();
                audiosource.clip = boostCollectAudio;
                audiosource.Play();
                Destroy(collision.gameObject);
                break;

            case "PowerUp":
                Actions.onPoweUp();
                audiosource.clip = powerupCollectAudio;
                audiosource.Play();
                Destroy(collision.gameObject);
                break;
        }
    }
    private void OnDisable()
    {
        Actions.onMissileHit -= ModifyLife;
        Actions.onBoostPicked -= Boost;
    }
}
