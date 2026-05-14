using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DescriptionManaager : MonoBehaviour
{
    [Header("UI")]
    public GameObject textPannel;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI lineText;
    public Transform cameraTransform;
    //private Interactable currrentInteractable;

    private int lineIndex; //line index whichx we are currently on in the dialogue
    private bool isActive; //track if we are currently in dialogue
    private ObjectData currentNode;

    //lock player movemnt and cursor when in dialogue
    private CCPlayer player;



    private void Awake()
    {
        //start w dialogue hidden
        if (textPannel != null) textPannel.SetActive(false);
        player = FindFirstObjectByType<CCPlayer>();
    }

    private void OnEnable()
    {
        CCPlayer.OnDescriptionRequested += StartDescription;
    }
    private void OnDisable()
    {
        CCPlayer.OnDescriptionRequested -= StartDescription;
    }

    private void Update()
    {
        if (!isActive) return; //if no dialogue active then return
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Advanced();
        }
    }
    void StartDescription(ObjectData objectData)
    {
        player.DisableInput();
        if (objectData == null)
        {
            Debug.Log("Data is null");
            return;
        }
        currentNode = objectData;
        lineIndex = 0;
        isActive = true;
        if (textPannel != null) textPannel.SetActive(true);
        ShowLine();

    }
    void ShowLine()
    {
        if (currentNode == null)
        {
            EndDescription();
            return;
        }
        if (displayName != null) displayName.text = currentNode.displayName;
        if (currentNode.lines == null || currentNode.lines.Length == 0)
        {
            FinishNode();
            return;
        }
        lineIndex = Mathf.Clamp(lineIndex, 0, currentNode.lines.Length - 1);
        if (lineText != null) lineText.text = currentNode.lines[lineIndex];
    }
    void FinishNode()
    {
        if (currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            lineIndex = 0;
            return;
        }
        EndDescription();
    }

    void Advanced()
    {
        //if node is finished and dialogue
        if (currentNode == null)
        {
            EndDescription();

            return;
        }
        //move to next line
        lineIndex++;
        //if there are still line to read in the node then show them
        if (currentNode.lines != null && lineIndex < currentNode.lines.Length)
        {
            //if we have smth
            if (lineText != null)
            {
                //take the text of outTMP obj and change it to wtv the current line is
                lineText.text = currentNode.lines[lineIndex];
                return;
            }
        }
        //otherwise we have reached the end
        FinishNode();
    }
    void EndDescription()
    {
        //reset dialogue stuff and turn back to the game
        textPannel.SetActive(false);

        isActive = false;
        currentNode = null;
        lineIndex = 0;
        player.EnableInput();
    }
    
    
}
