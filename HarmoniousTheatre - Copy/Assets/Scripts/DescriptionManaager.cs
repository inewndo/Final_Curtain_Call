using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DescriptionManaager : MonoBehaviour
{
    [Header("UI")]
    public GameObject textPannel;
    public TextMeshProUGUI displayName;
    public TextMeshProUGUI lineText;
    public Transform cameraTransform;
    public Transform choicesContainer; //parent object where choice buttons will spawn
    public Button choiceButtonPrefab;
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
        ClearChoices();
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
            if (ChoicesAreShowing()) return;
            Advanced();
        }
    }
    void StartDescription(ObjectData objectData)
    {
        player.DisableInput();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
    bool HasChoices(ObjectData node)
    {
        return node != null && node.choices != null && node.choices.Length > 0;
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
   
    void ShowChoices(DialogueChoice[] choices)
    {
        ClearChoices();
        if (choicesContainer == null || choiceButtonPrefab == null)
        {
            Debug.Log("choices are not wired");
            return;
        }

        foreach (DialogueChoice choice in choices)
        {
            Button bttn = Instantiate(choiceButtonPrefab, choicesContainer);

            Debug.Log("ButtonSpawm");

            TextMeshProUGUI tmp = bttn.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null) tmp.text = choice.choiceText;

            //cache next node in a local var
            ObjectData next = choice.nextNode;

            //lambda
            bttn.onClick.AddListener(() =>
            {
                Choose(next);
            });
        }
    }

    void FinishNode()
    {
        //if choice exist then show, else if next node exists- continue automotaicall, else end dialogue
        if (HasChoices(currentNode))
        {
            ShowChoices(currentNode.choices);
            Debug.Log("FinishNode");
            return;
        }
        //auto continue text
        if (currentNode.nextNode != null)
        {
            currentNode = currentNode.nextNode;
            lineIndex = 0;
            ShowLine();
            return;
        }
        EndDescription();
    }
    bool ChoicesAreShowing()
    {
        return choicesContainer != null && choicesContainer.childCount > 0;

        //bool showing = choicesContainer != null && choicesContainer.childCount > 0;
        //Debug.Log(showing);
        //return;
    }
    void ShowLine()
    {
        ClearChoices();
        if (currentNode == null || currentNode.lines == null)
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
    

   
    void Choose(ObjectData nextNode)
    {
        //remove buttons asap so ui feels responsive
        ClearChoices();

        //if no n ext node this ends convo
        if (nextNode == null)
        {
            EndDescription();
            return;
        }
        //otherwise go to chosen node
        currentNode = nextNode;
        lineIndex = 0;
        ShowLine();
    }
    void ClearChoices()
    {
        //if no choice container then exit the function
        if (choicesContainer == null) return;

        for (int i = choicesContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(choicesContainer.GetChild(i).gameObject);
        }
    }
    void EndDescription()
    {
        //reset dialogue stuff and turn back to the game
        textPannel.SetActive(false);

        isActive = false;
        currentNode = null;
        lineIndex = 0;
        player.EnableInput();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    
}
