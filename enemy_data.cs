using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemy_data : MonoBehaviour
{

    public float    healthPoints;
    public float    attackDamagePerSecond;
    public float    fadeOutDuration;
	public int      deathObjectDropChancePercent;
    public int      fryObjectDropPercent;

    Animator        anim;
    public bool     isDying;
    public bool     isDead;
    public bool     isHurt;

    SpriteRenderer  spriteRenderer;
    int             hitsTaken;

    AudioClip       mySound;

    float           elapsedTime;
    public float    minTimeBetweenSounds;
    public float    maxTimeBetweenSounds;
    float           myTimeBetweenSound;
    

    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isDying = false;
        isDead = false;
        hitsTaken = 0;

        myTimeBetweenSound = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
    }
    

    // Update is called once per frame
    void Update ()
    {

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= myTimeBetweenSound )
        {
            GameObject sound = (GameObject)Instantiate(Resources.Load("sound_source"));
            sound.GetComponent<play_sound_effect>().PlaySound(mySound, 0.25f);
            elapsedTime = 0.0f;
        }

        SpeedBoostCheck();

        spriteRenderer.color = new Color(1.0f, (1.0f - (hitsTaken * 0.3f)), (1.0f - (hitsTaken * 0.3f)), 1.0f);
        anim.ResetTrigger("takeDamage");
        AnimatorStateInfo animState = anim.GetCurrentAnimatorStateInfo(0);
        isHurt = false;
        
        if (animState.IsName("hurt") || animState.IsName("death") || animState.IsName("dead"))
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isMeleeAttacking", false);

            if (animState.IsName("death"))
                isDying = true;

            if (animState.IsName("dead"))
                isDead = true;

            if (animState.IsName("hurt"))
                isHurt = true;
        }

        else if (!isDying && !isDead && !GetComponent<enemy_movement>().isCollidingWithCastle)
        {
            anim.SetBool("isMoving", true);
            anim.ResetTrigger("takeDamage");
            anim.SetBool("isMeleeAttacking", false);
        }

        if (isDying)
            anim.SetTrigger("dead");

        if (isDead)
            Destroy(gameObject);
    }


    //Function 
    public void DoDamageToEnemy(float damage)
    {
        healthPoints -= damage;
        if (healthPoints <= 0)
        {
            if(!isDead)
                DestroyEnemy();

            hitsTaken++;
        }
        else
        {
            anim.SetTrigger("takeDamage");
            hitsTaken++;
        }
    }

 
    //Function 
    public void DestroyEnemy()
    {
        isDead = true;
        int randomNumber = (int) Random.Range(0.0f, 100.0f);
        anim.SetTrigger("die");


        GameObject dayne    = GameObject.FindGameObjectWithTag("Player");
        float distance      = Vector2.Distance(this.transform.position, dayne.transform.position);
        Vector3 position    = this.transform.position;


        if (randomNumber <= deathObjectDropChancePercent && distance > 2.55f)
        {
            Destroy(collider2D);

            List<GameObject> obstacles = new List<GameObject>();
            obstacles.AddRange(GameObject.FindGameObjectsWithTag("obstacle"));
            obstacles.AddRange(GameObject.FindGameObjectsWithTag("spire"));
            bool tooCloseToOtherObstacle = false;

            foreach (GameObject obstacle in obstacles)
            {
                float distanceToObstacle = Vector2.Distance(transform.position, obstacle.transform.position);
                if (distanceToObstacle < 1.8f)
                    tooCloseToOtherObstacle = true;
            }

            if (!tooCloseToOtherObstacle)
            {
                randomNumber = (int)Random.Range(0.0f, 100.0f);
                if (randomNumber >= 50)
                    GetComponent<enemy_death>().CreateDeathItemObj1(position);
                else
                    GetComponent<enemy_death>().CreateDeathItemObj2(position);
            }
            else
                GetComponent<enemy_death>().Explode(position);
        }
        else if (distance > 1.8f)
            GetComponent<enemy_death>().Explode(position);
        else
            GetComponent<enemy_death>().CreateDeathItemObj3(position);  
    }


    //Function 
    public void SpeedBoostCheck()
    {        
        GameObject[] spires = GameObject.FindGameObjectsWithTag("spire");
        bool spireInRange = false;
        GetComponent<enemy_movement>().speedMultiplier = 1.0f;

        foreach (GameObject spire in spires)
        {
            float distance = Vector2.Distance(this.transform.position, spire.transform.position);

            if (distance < spire.GetComponent<obstacle_data>().speedBuffRadius)
            {
                spireInRange = true;
                GetComponent<enemy_movement>().hasSpeedBoost = true;
                GetComponent<enemy_movement>().speedMultiplier *= spire.GetComponent<obstacle_data>().speedMultiplierBuffPerSecond;
                break;
            }
        }
        if (!spireInRange)
            GetComponent<enemy_movement>().speedMultiplier = 1.0f;

    }

    public void SetSound(AudioClip sound)
    {
        mySound = sound;
    }
}
