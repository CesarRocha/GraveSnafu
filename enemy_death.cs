using UnityEngine;
using System.Collections;

public class enemy_death : MonoBehaviour
{

	public GameObject DeathItemObj1;
	public GameObject DeathItemObj2;
    public GameObject BodyPartObj1;
    public GameObject BodyPartObj2;
    public GameObject BodyPartObj3;
    public GameObject BodyPartObj4;
    public GameObject BodyPartObj5;
    public GameObject BodyPartObj6;
    public GameObject BodyPartObj7;
    public GameObject BodyPartObj8;
    public GameObject DeathItemObj3;
    public GameObject SmokePuffObj;


	void Start () 
	{
  	}

    void Update()
    {
    }


    //Function 
    public void CreateDeathItemObj1(Vector2 position)
	{
        GameObject SmokePuff = (GameObject)Instantiate((SmokePuffObj));
        SmokePuff.transform.position = position;

		GameObject newDeathObject1 = (GameObject)Instantiate(DeathItemObj1);
        newDeathObject1.transform.position = position;
        Destroy(SmokePuff, 1);
	}


    //Function 
    public void CreateDeathItemObj2(Vector2 position)
    {
        GameObject SmokePuff = (GameObject)Instantiate((SmokePuffObj));
        SmokePuff.transform.position = position;

        GameObject newDeathObject2 = (GameObject)Instantiate(DeathItemObj2);
        newDeathObject2.transform.position = position;
        Destroy(SmokePuff, 1);
    }


    //Function 
    public void CreateDeathItemObj3(Vector2 position)
    {
        GameObject SmokePuff = (GameObject)Instantiate((SmokePuffObj));
        SmokePuff.transform.position = position;

        GameObject newDeathObject3 = (GameObject)Instantiate(DeathItemObj3);
        newDeathObject3.transform.position = position;
        Destroy(SmokePuff, 1);
    }


    //Function 
    public void Explode(Vector2 position)
    {
       GameObject SmokePuff = (GameObject)Instantiate((SmokePuffObj));
       SmokePuff.transform.position = position;

       GameObject BodyPart1 = (GameObject)Instantiate((BodyPartObj1));
       BodyPart1.transform.position = position;
       BodyPart1.transform.rotation = transform.rotation;

       GameObject BodyPart2 = (GameObject)Instantiate((BodyPartObj2));
       BodyPart2.transform.position = position;

       GameObject BodyPart3 = (GameObject)Instantiate((BodyPartObj3));
       BodyPart3.transform.position = position;

       GameObject BodyPart4 = (GameObject)Instantiate((BodyPartObj4));
       BodyPart4.transform.position = position;

       GameObject BodyPart5 = (GameObject)Instantiate((BodyPartObj5));
       BodyPart5.transform.position = position;

       GameObject BodyPart6 = (GameObject)Instantiate((BodyPartObj6));
       BodyPart6.transform.position = position;

       GameObject BodyPart7 = (GameObject)Instantiate((BodyPartObj7));
       BodyPart7.transform.position = position;

       GameObject BodyPart8 = (GameObject)Instantiate((BodyPartObj8));
       BodyPart8.transform.position = position;
    }
}