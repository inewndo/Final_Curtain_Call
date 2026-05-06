using System.Collections;
using UnityEngine;

public class StageControls : MonoBehaviour
{
    /*  WHAT THIS SCRIPT IS FOR 
     * TURNING ON AND OFF STAGE LIGHTS 
     * CLOSING/OPENING CURTAINS 
     * STARTING TO PLAY MUSICAL AUDIO 
     * 
     */

    public GameObject ComputerUI;
    private CCPlayer playercc; //for turning off the script briefly to bring back cursor & stop input

    public AudioSource audioSource;
    public float fadeDuration = 10f; //sec to reach full vol

    public float curtainOpenSpeed;
    public float curtainNewLX, curtainNewRX;
    public Transform CurtainLeft, CurtainRight; //these r the pivots
    public GameObject ControllerLight; // parent light
    //light flicker is an animation attached to main light, so turning on the lights resets the animation 
    //so that they flicker first and then turn on, and the flickering slows down. 

    public bool LightsOn, CurtainOn; //switching between these states instead of having a bunch of fucntions,
                                     //also allows to implement cooldown later (maybe if you flick the lights on and off too much they explode lol)

    public float curtainLeftOriginalX, curtainRightOriginalX; //storing original scales to revert to 

    private void Awake()
    {
        playercc = FindFirstObjectByType<CCPlayer>();

        curtainLeftOriginalX = CurtainLeft.transform.localScale.x;
        curtainRightOriginalX = CurtainRight.transform.localScale.x;
    }

    private void Update()
    {
        if (CurtainOn)
        {
            float newLX = Mathf.Lerp(CurtainLeft.localScale.x, curtainNewLX, Time.deltaTime * curtainOpenSpeed);
            float newRX = Mathf.Lerp(CurtainRight.localScale.x, curtainNewRX, Time.deltaTime * curtainOpenSpeed * curtainOpenSpeed);

            CurtainLeft.transform.localScale = new Vector3(newLX, CurtainLeft.localScale.y, CurtainLeft.localScale.z);

            CurtainRight.transform.localScale = new Vector3(newRX, CurtainRight.localScale.y, CurtainRight.localScale.z); 


            //lerp positions , and then lerp them back next time you press the button :D
        }

        if (!CurtainOn)
        {
            //drawing curtains open

            float revertLX = Mathf.Lerp(CurtainLeft.localScale.x, curtainLeftOriginalX, Time.deltaTime * curtainOpenSpeed);
            float revertRX = Mathf.Lerp(CurtainRight.localScale.x, curtainRightOriginalX, Time.deltaTime * curtainOpenSpeed);

            CurtainLeft.transform.localScale = new Vector3(revertLX, CurtainLeft.localScale.y, CurtainLeft.localScale.z) ;
            CurtainRight.transform.localScale = new Vector3(revertRX, CurtainRight.localScale.y, CurtainRight.localScale.z);




        }

        if (LightsOn)
        {
            ControllerLight.SetActive(true);
        }

        if (!LightsOn)
        {
            ControllerLight.SetActive(false);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        
        //when enter trigger pop up ui prompt and buttons to turn on and off stuff (ui is computer screen)
        ComputerUI.SetActive(true);
        playercc.enabled = false;
        Cursor.lockState = CursorLockMode.None; Cursor.visible = true;

        if(Input.GetKeyDown(KeyCode.F))
        {
            ComputerUI.SetActive(false); playercc.enabled = true;
            Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
        } //close ui


    }

    public void OnChangeLights()
    {
        if (!LightsOn) LightsOn = true;
        else if (LightsOn) LightsOn = false;
        //switch states :D

        ComputerUI.SetActive(false); playercc.enabled = true;
        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;



    }

    public void OnChangeCurtain()
    {
        if (!CurtainOn) CurtainOn = true;
        else if (CurtainOn) CurtainOn = false;
        //switch states

        ComputerUI.SetActive(false); playercc.enabled = true;
        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;




    }

    public void onStartMusic()
    {
        StartCoroutine(FadeIN(audioSource, fadeDuration));
        ComputerUI.SetActive(false); playercc.enabled = true;
        Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false;
    }

    private IEnumerator FadeIN(AudioSource audio, float duration)
    {
        audio.volume = 0;
        audio.Play();

        while(audio.volume < 1)
        {
            audio.volume += Time.deltaTime / duration; 
            yield return null;
        }

        audio.volume = 1;
    }





}
