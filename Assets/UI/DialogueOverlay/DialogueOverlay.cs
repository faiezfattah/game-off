using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueOverlay : MonoBehaviour
{
    [SerializeField]
    private InputReader inputReader;
    [SerializeField]
    private float textSpeed = 0.03f;
    [SerializeField]
    private Dialogue startingDialogue = null;

    private float timer;
    private string txt;
    private Queue<DialogueItem> dialogueItemsQueue = new();
    private UIDocument doc;
    private Label speakerLabel;
    private Label contentLabel;

    private void Awake()
    {
        doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement.Q("root");

        speakerLabel = root.Q<Label>("DialogueSpeakerLabel");
        contentLabel = root.Q<Label>("DialogueContentLabel");

        txt = contentLabel.text;

        Hide();

        if (startingDialogue != null)
            RunDialogue(startingDialogue);
    }

    private void Show()
    {
        inputReader.InteractEvent += FinishText;

        var root = doc.rootVisualElement.Q("root");
        root.AddToClassList("visible");

        FinishText();
    }

    private void Hide()
    {
        inputReader.InteractEvent -= FinishText;

        var root = doc.rootVisualElement.Q("root");
        root.RemoveFromClassList("visible");
    }

    private void FinishText()
    {
        if (contentLabel.text != txt)
        {
            contentLabel.text = txt;
        }
        else
        {
            contentLabel.text = string.Empty;

            if (dialogueItemsQueue.TryDequeue(out var next))
            {
                if (next.dialogueSpeaker != null)
                    speakerLabel.text = next.dialogueSpeaker.name;
                else
                    speakerLabel.text = "???";

                txt = next.text ?? string.Empty;
            }
            else
            {
                txt = string.Empty;
                Hide();
            }
        }
    }

    private void Update()
    {
        if (contentLabel.text.Length >= txt.Length)
            return;

        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer += textSpeed;
            if (contentLabel.text.Length < txt.Length)
            {
                contentLabel.text += txt[contentLabel.text.Length];
            }
        }
    }

    public void RunDialogue(Dialogue dialogue)
    {
        foreach (var dialogueItem in dialogue.items)
            dialogueItemsQueue.Enqueue(dialogueItem);

        Show();
    }

    public static bool StaticRunDialogue(Dialogue dialogue)
    {
        var dialogueOverlay = FindFirstObjectByType<DialogueOverlay>();
        if (dialogueOverlay == null)
            return false;

        dialogueOverlay.RunDialogue(dialogue);

        return true;
    }
}
