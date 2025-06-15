using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
public class EditCodeButton : MonoBehaviour
{
    public TMP_InputField editor;
    private bool editorActive;
    public Vehicle v;
    public FileSelectMenu fileMenu;
    private string editingPath;
    // Start is called before the first frame update
    void Start()
    {
        editorActive = editor.gameObject.activeSelf;
        v = GameObject.FindGameObjectWithTag("mainVehicle").GetComponent<Vehicle>();
        editingPath = v.mainCodePath;
        fileMenu.SetChoices(Directory.EnumerateFiles(v.codeDirectory));
        fileMenu.editor = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void ToggleEditor()
    {
        if (editorActive)
        {
            editor.gameObject.SetActive(false);
            editorActive = false;
        }
        else
        {
            editor.gameObject.SetActive(true);
            editorActive = true;
            editor.text = File.ReadAllText(editingPath);
        }
    }
    public void Save()
    {
        if (editorActive)
        {
            File.WriteAllText(editingPath, editor.text);
            v.InitCodeFiles();
            v.Start();
        }

    }
    public void SetEditingPath(string path)
    {
        Save();
        editingPath = path;
        ToggleEditor();
        if (!editorActive)
        {
            ToggleEditor();

        }
    }
}
