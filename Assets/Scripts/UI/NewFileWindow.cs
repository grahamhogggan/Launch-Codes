using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
public class NewFileWindow : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private EditCodeButton editor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ToggleWindow()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    public void CreateFile()
    {
        string name = nameInput.text;
        string fullPathName = editor.v.codeDirectory + "/" + name + ".txt";
        if (!File.Exists(fullPathName))
        {
            File.WriteAllText(fullPathName, "#" + name + "#");
        }
        ToggleWindow();
        editor.SetEditingPath(fullPathName);
        editor.InitFileChoices();
    }
}
