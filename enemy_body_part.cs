using UnityEngine;
using System.Collections;

public class enemy_body_part : MonoBehaviour
{

    public Vector3  myPosition;
    public Vector3  newPosition;
    public bool     MovesFromCenter;
    float           speed = 20;
    SpriteRenderer  spriteRenderer;
    float           fadeOutTimeRemaining;
    public float    timebeforefade = 10.0f;
    public float    fadeOutDuration = 5.0f;
    public float    TooClose = 1.8f;
    public bool     rotate;


    // Use this for initialization
    void Start()    
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        fadeOutTimeRemaining = 0.0f;
        Vector2 randomDistance = Random.insideUnitCircle * 2;
        transform.position = transform.position + new Vector3(0.0f, 0.0f, 6.0f);
        newPosition = transform.position + new Vector3(randomDistance.x, randomDistance.y, 6.0f);

        if (rotate)
        {
            Vector3 euler = transform.eulerAngles;
            euler.z = Random.Range(0f, 360f);
            transform.eulerAngles = euler;
        }

        GameObject dayne = GameObject.FindGameObjectWithTag("Player");
        float distance = Vector2.Distance(this.transform.position, dayne.transform.position);
        if (distance < TooClose)
        {
            fadeOutDuration = 0.0f;
            timebeforefade = 0.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MovesFromCenter)
            transform.position = Vector3.Lerp(transform.position, newPosition, speed / 60);


        if (timebeforefade > 0.0f)
            timebeforefade -= Time.deltaTime;
        else
        {
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, Mathf.Lerp(1.0f, 0.0f, fadeOutTimeRemaining / fadeOutDuration));
            fadeOutTimeRemaining += Time.deltaTime;
        }

       if (fadeOutTimeRemaining >= fadeOutDuration)
           Destroy(gameObject);
    }
}
