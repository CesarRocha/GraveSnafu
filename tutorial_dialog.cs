using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial_dialog : MonoBehaviour
{

    public bool      hasDayne;
    public bool      hasKrazar;
    public bool      beginWithDayne;
    public List<string> DayneDialog;
    public string[]  KrazarDialog;
    public float     timeBeforeDialogBegin;
    public AudioClip pageTurnSound;
    public AudioClip dialogBeginSound;
    public bool      UnlockFireball;
    public bool      UnlockLightning;
    public bool      UnlockFirestorm;

    float  timerToSwitch = 2.0f;

    public bool lastTutorialLevel;

    public List<int> dialogLinesToEnablePointer; 

    bool lastLevelAddedString = false;

    int  currentStringLineD = 0;
    int  currentStringLineK = 0;
    bool dialogFinished     = false;
    bool dialogBegins       = false;
    bool dayneTurn          = false;
    bool playedDialog       = false;

    Text dayneTextBox;
    Text krazarTextBox;

    SpriteRenderer  DayneWhitebox;
    SpriteRenderer  KrazarWhiteBox;
    SpriteRenderer  DaynePortraitBox;
    SpriteRenderer  DaynePortrait;

    GameObject[]    spawners;
    public int      nextstage;

    SpriteRenderer  NextButtonR;
    GameObject      nextButton;
    Vector3         interactionPos;

    GameObject      pointerFinger;

    public GameObject objectToDestroyUponDialogFinish;



	// Use this for initialization
	void Start ()
    {
        if(beginWithDayne)
            dayneTurn = true;

        dayneTextBox        = GameObject.Find("DayneDialogBox").GetComponent<Text>();
        DayneWhitebox       = GameObject.Find("DayneWhiteBox").GetComponent<SpriteRenderer>();
        DaynePortraitBox    = GameObject.Find("daynePortraitBox").GetComponent<SpriteRenderer>();
        DaynePortrait       = GameObject.Find("daynePortrait").GetComponent<SpriteRenderer>();

        krazarTextBox       = GameObject.Find("KrazarDialogBox").GetComponent<Text>();
        KrazarWhiteBox      = GameObject.Find("KrazarWhiteBox").GetComponent<SpriteRenderer>();

        NextButtonR         = GameObject.Find("NextButton").GetComponent<SpriteRenderer>();
        nextButton          = GameObject.Find("NextButton");
        pointerFinger       = GameObject.Find("pointer_finger");

	}


    // Update is called once per frame
    void Update()
    {
        
        if (!dialogBegins)
            CountDownTillDialog();


        if (dialogBegins && hasDayne && !hasKrazar)
            PlayDayneDialogScene();
        
        if (dialogBegins && hasDayne && hasKrazar)
            PlayDuoDialogScene();

        if (dialogBegins && !hasDayne && hasKrazar)
            PlayKrazarDialogScene();


        CheckForWin();
    }


    //Function 
    void CountDownTillDialog()
    {
        if (timeBeforeDialogBegin > 0.0f)
            timeBeforeDialogBegin -= Time.deltaTime;        
    }


    //Function 
    void PerformUnlocks()
    {
        GameObject spellController = GameObject.Find("spell_controller");

        bool onPC       = false;
        bool onTablet   = false;

        if (spellController.GetComponent<input_controller>().enabled)
            onTablet = true;
        else if (spellController.GetComponent<input_controller_pc>().enabled)
            onPC = true;

        if (UnlockFireball)
        {
            if (onPC)
                spellController.GetComponent<input_controller_pc>().fireballEnabled = true;
            if (onTablet)
                spellController.GetComponent<input_controller>().fireballEnabled = true;
        }

        if (UnlockFirestorm)
        {
            if (onPC)
                spellController.GetComponent<input_controller_pc>().fireStormEnabled = true;
            if (onTablet)
                spellController.GetComponent<input_controller>().fireStormEnabled = true;
        }

        if (UnlockLightning)
        {
            if (onPC)
                spellController.GetComponent<input_controller_pc>().lightningEnabled = true;
            if (onTablet)
                spellController.GetComponent<input_controller>().lightningEnabled   = true;
        }

    }

    
    //Function 
    void PlayDayneDialogScene()
    {
        GameObject spellController = GameObject.Find("spell_controller");

        bool onPC       = false;
        bool onTablet   = false;

        if (spellController.GetComponent<input_controller>().enabled)
            onTablet = true;
        else if (spellController.GetComponent<input_controller_pc>().enabled)
            onPC = true;


        if (!playedDialog)
        {
            GameObject soundEffect = (GameObject)Instantiate(Resources.Load("sound_source"));
            soundEffect.GetComponent<play_sound_effect>().PlaySound(dialogBeginSound, 1.0f);
            playedDialog = true;
        }


        if (!dialogFinished)
        {
            if (onPC)
                spellController.GetComponent<input_controller_pc>().unpauseTimeScaleOne = false;
            else if (onTablet)
                spellController.GetComponent<input_controller>().unpauseTimeScaleOne = false;
            
            DayneWhitebox.enabled    = true;
            NextButtonR.enabled      = true;
            DaynePortraitBox.enabled = true;
            DaynePortrait.enabled    = true;

            dayneTextBox.text = DayneDialog[currentStringLineD];
        }


        bool advanceDialog = false;
        if (Input.touchCount > 0 && !dialogFinished )
        {
            interactionPos = Vector3.zero;
            interactionPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            interactionPos.z = -1.0f;

            if( nextButton.collider2D.bounds.Contains(interactionPos) && Input.GetTouch(0).phase == TouchPhase.Ended )
                advanceDialog = true;
        }


        if( Input.GetMouseButtonUp(0) && !dialogFinished)
        {
            interactionPos = Camera.main.ScreenToWorldPoint( Input.mousePosition);
            interactionPos.z = -1.0f;

            if (nextButton.collider2D.bounds.Contains(interactionPos))
                advanceDialog = true;
        }


        //Function 
        if ( advanceDialog )
        {
            GameObject soundEffect = (GameObject)Instantiate(Resources.Load("sound_source"));
            soundEffect.GetComponent<play_sound_effect>().PlaySound(pageTurnSound, 1.0f);
            ++currentStringLineD;


            if (currentStringLineD == DayneDialog.Count)
            {
                dialogFinished = true;
                Time.timeScale = 1;
                DayneWhitebox.enabled = false;
                NextButtonR.enabled = false;
                DaynePortrait.enabled = false;
                DaynePortraitBox.enabled = false;
                dayneTextBox.text = "";

                if( objectToDestroyUponDialogFinish )
                    Destroy(objectToDestroyUponDialogFinish);
               
                if (lastTutorialLevel && lastLevelAddedString)
                {
                    GameObject victory = (GameObject)Instantiate(Resources.Load("victory_screen"));
                    victory.GetComponent<victory>().SetContinueScene(2);
                    PlayerPrefs.SetInt("gravesnafu_level1Unlocked", 1);
                }
                
            }
        }


        if (dialogFinished)
        {
            if (onPC)
                spellController.GetComponent<input_controller_pc>().unpauseTimeScaleOne = true;
            else if (onTablet)
                spellController.GetComponent<input_controller>().unpauseTimeScaleOne = true;

            PerformUnlocks();
        }
    }



    //Function 
    void CheckForWin()
    {
        bool allSpawnersDone = true;
        spawners = GameObject.FindGameObjectsWithTag("spawner");
        

        if (allSpawnersDone) 
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            

            if (enemies.Length == 0)
            {
                if (lastTutorialLevel && !lastLevelAddedString )
                {
                    DayneDialog.Add("Great job! Now lets move on to the real thing!");
                    dialogFinished       = false;
                    playedDialog         = false;
                    lastLevelAddedString = true;
                }
                else if( !lastTutorialLevel )
                    timerToSwitch -= Time.deltaTime;
                
                GameObject.Find("spell_controller").GetComponent<input_controller>().fireballEnabled  = false;
                GameObject.Find("spell_controller").GetComponent<input_controller>().lightningEnabled = false;
                GameObject.Find("spell_controller").GetComponent<input_controller>().fireStormEnabled = false;
            }


            if( timerToSwitch <= 0.0f )
                Application.LoadLevel(nextstage);
        }
    }


    //Function 
    void PlayDuoDialogScene()
    {
        if (!playedDialog)
        {
            GameObject soundEffect = (GameObject)Instantiate(Resources.Load("sound_source"));
            soundEffect.GetComponent<play_sound_effect>().PlaySound(dialogBeginSound, 1.0f);
            playedDialog = true;
        }

        Debug.Log("Duo");
        if (dayneTurn)
        {
            DayneWhitebox.enabled = true;
            NextButtonR.enabled = true;
            dayneTextBox.text = DayneDialog[currentStringLineD];
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                GameObject soundEffect = (GameObject)Instantiate(Resources.Load("sound_source"));
                soundEffect.GetComponent<play_sound_effect>().PlaySound(pageTurnSound, 1.0f);
                ++currentStringLineD;
                dayneTurn = !dayneTurn;
            }
        }
        else if (!dayneTurn)
        {
            KrazarWhiteBox.enabled = true;
            NextButtonR.enabled = true;
            krazarTextBox.text = KrazarDialog[currentStringLineK];
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                GameObject soundEffect = (GameObject)Instantiate(Resources.Load("sound_source"));
                soundEffect.GetComponent<play_sound_effect>().PlaySound(pageTurnSound, 1.0f);
                ++currentStringLineK;
                dayneTurn = !dayneTurn;
            }
        }

        if (currentStringLineD == DayneDialog.Count && currentStringLineK == KrazarDialog.Length)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Time.timeScale = 1;
                PerformUnlocks();
                Destroy(DayneWhitebox);
                Destroy(KrazarWhiteBox);
                Destroy(dayneTextBox);
                Destroy(krazarTextBox);
            }
    }


    //Function 
    void PlayKrazarDialogScene()
    {
        if (!playedDialog)
        {
            GameObject soundEffect = (GameObject)Instantiate(Resources.Load("sound_source"));
            soundEffect.GetComponent<play_sound_effect>().PlaySound(dialogBeginSound, 1.0f);
            playedDialog = true;
        }

        Debug.Log("Krazar");
        KrazarWhiteBox.enabled = true;
        NextButtonR.enabled = true;
        krazarTextBox.text = KrazarDialog[currentStringLineK];
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                GameObject soundEffect = (GameObject)Instantiate(Resources.Load("sound_source"));
                soundEffect.GetComponent<play_sound_effect>().PlaySound(pageTurnSound, 1.0f);
                ++currentStringLineK;
                dayneTurn = !dayneTurn;
            }
        

        if (currentStringLineK == KrazarDialog.Length)
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Time.timeScale = 1;
                PerformUnlocks();
                Destroy(DayneWhitebox);
                Destroy(KrazarWhiteBox);
                Destroy(dayneTextBox);
                Destroy(krazarTextBox);
            }
    }
}
