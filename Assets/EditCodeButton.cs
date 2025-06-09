using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EditCodeButton : MonoBehaviour
{
    public TMP_InputField editor;
    private bool editorActive;
    private TextAsset[] code;
    private Vehicle v;
    // Start is called before the first frame update
    void Start()
    {
        editorActive = editor.gameObject.activeSelf;
        v = GameObject.FindGameObjectWithTag("mainVehicle").GetComponent<Vehicle>();
        code = v.codeFiles;
    }

    // Update is called once per frame
    void Update()
    {
        if (editorActive)
        {
        code[0] = new TextAsset(editor.text);
        }
    }
    public void ToggleEditor()
    {
        if (editorActive)
        {
            editor.gameObject.SetActive(false);
            editorActive = false;
            v.Start();
        }
        else
        {
            editor.gameObject.SetActive(true);
            editorActive = true;
            editor.text = code[0].ToString();
        }
    }
}
