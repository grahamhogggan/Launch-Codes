using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessagePanel : MonoBehaviour
{
    public static List<string> messages;
    public int maxMessages;
    private TMP_Text messageText;
    void Awake()
    {
        messages = new List<string>();
    }
    // Start is called before the first frame update
    void Start()
    {
        messageText = GetComponentInChildren<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(messages.Count > maxMessages)
        {
            messages.RemoveAt(0);
        }
        messageText.text = string.Join("\n", messages);
    }
}
