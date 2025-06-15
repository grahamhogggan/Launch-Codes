using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FileSelectMenu : MonoBehaviour
{
    public EditCodeButton editor;
    private TMP_Dropdown dropdown;
    // Start is called before the first frame update
    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();

    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SetChoices(IEnumerable<string> choices)
    {
        List<string> names = new List<string>();
        foreach (string str in choices)
        {
            string[] pieces = str.Split("/");
            string lastPiece = pieces[pieces.Length - 1];
            string name = lastPiece.Split("\\")[lastPiece.Split("\\").Length - 1];
            name = name.Split(".")[0];
            if (!names.Contains(name))
            {
                names.Add(name);
            }
        }
        List<TMP_Dropdown.OptionData> ls = new List<TMP_Dropdown.OptionData>();
        foreach (string name in names)
        {
            ls.Add(new TMP_Dropdown.OptionData(name));

        }
        dropdown.options = ls;
    }
    public void OnChoiceChanged()
    {
        string currentChoice = dropdown.captionText.text;
        string path = editor.v.codeDirectory + "/" + currentChoice + ".txt";
        Debug.Log(path);
        editor.SetEditingPath(path);
    }
}
