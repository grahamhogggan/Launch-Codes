using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
public class Vehicle : MonoBehaviour
{
    public string vehicleIdentifier;
    public List<Component> Components;
    public float commandClockSpeed = 0.1f;
    private double commandClock;
    public string[] codeFiles;
    private List<string[]> codeStack = new List<string[]>();
    private List<int> codeLines = new List<int>();
    private Dictionary<string, float> variables = new Dictionary<string, float>();

    private string[] telemetryCode;
    private string[] schedulerCode;
    private double schedulerDelay;
    private double schedulerClock;
    private int schedulerLine = 0;
    public Vector2 centerOfMassOffset;
    public string mainCodePath;
    public string codeDirectory;
    public GameObject explosionPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        codeDirectory = Application.dataPath + "/CODE/" + vehicleIdentifier;
        mainCodePath = codeDirectory + "/main.txt";
        if (!Directory.Exists(codeDirectory))
        {
            Directory.CreateDirectory(codeDirectory);
        }
        InitCodeFiles();

    }
    public void InitCodeFiles()
    {
        IEnumerable<string> codeFileNamesIE = Directory.EnumerateFiles(codeDirectory);
        List<string> codeFileNames = codeFileNamesIE.ToList();
        codeFiles = new string[codeFileNames.Count];
        for (int i = 0; i < codeFileNames.Count; i++)
        {
            codeFiles[i] = File.ReadAllText(codeFileNames[i]);
            if (codeFileNames[i].Replace("\\","/")==mainCodePath.Replace("\\","/"))
            {
                Debug.Log("main found");
                codeFiles[i] = codeFiles[0];
                codeFiles[0] = File.ReadAllText(mainCodePath);
            }
        }
        if (codeFiles.Length < 1)
        {
            string templateMain = "#telemetry#\n\n#main#\n\n#boot#\n\n#scheduler#";
            File.WriteAllText(mainCodePath, templateMain);
            codeFiles = new string[1];
            codeFiles[0] = templateMain;
        }
    }
    public void Start()
    {

        RecalulateCenterOfMass(centerOfMassOffset); 
        Components = new List<Component>(GetComponentsInChildren<Component>());
        foreach (Component component in Components)
        {
            component.InitializeComponent();
        }
        commandClock = 0;
        codeStack = new List<string[]>();
        codeLines = new List<int>();
        variables = new Dictionary<string, float>();
        codeStack.Add(CreateBlockCode(codeFiles[0], "main"));
        codeLines.Add(0);

        telemetryCode = CreateBlockCode(codeFiles[0], "telemetry");
        schedulerCode = CreateBlockCode(codeFiles[0], "scheduler");
        string[] boot = CreateBlockCode(codeFiles[0], "boot");
        foreach (string str in boot)
        {
            SendCommand(str);
        }
        schedulerDelay = 0;
        schedulerClock = 0;
        schedulerLine = 0;
    }
    public void RecalulateCenterOfMass(Vector2 newCenterOfMass)
    {
        centerOfMassOffset = newCenterOfMass;
        GetComponent<Rigidbody2D>().centerOfMass = (Vector2)centerOfMassOffset;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().gravityScale = 4000000 / Mathf.Pow((transform.position.y + 2000), 2);
            codeLines[0]++;
            SendCommand(codeStack[0][codeLines[0] - 1]);
            if (codeLines[0] >= codeStack[0].Length)
            {
                codeLines[0] = 0;
            }
        foreach (Component component in Components)
        {
            component.UpdateComponent(Time.deltaTime);
        }
        foreach (string str in telemetryCode)
        {
            SendCommand(str.Replace("delay", "???"));
        }
        schedulerClock += Time.deltaTime;
        if (schedulerClock > schedulerDelay)
        {
            schedulerClock = 0;
            schedulerDelay = 0;
            SendCommand(schedulerCode[schedulerLine]);
            schedulerLine++;
        }
        if (schedulerLine >= schedulerCode.Length)
        {
            schedulerLine = 0;
        }
    }
    public void SendCommand(string command)
    {
        //Debug.Log("Sent Command: " + command);
        while (command.Contains("  "))
        {
            command = command.Replace("  ", " ");
        }
        command = command.Replace("deltaTime", Time.deltaTime.ToString());
        command = command.Trim();
        string[] tokens = command.Split(" ");
        if (tokens[0] == "loop")
        {
            codeLines[0] = 0;
        }
        if (tokens[0] == "return")
        {
            codeStack.RemoveAt(0);
            codeLines.RemoveAt(0);
        }
        if (tokens[0] == "call")
        {
            string functionCalled = tokens[1];
            foreach (string asset in codeFiles)
            {
                if (asset.Split("#").Length < 3) continue;
                if (asset.Split("#")[1] == functionCalled)
                {
                    codeStack.Insert(0, CreateMethod(asset));
                    codeLines.Insert(0, 0);
                }
            }
        }
        if (tokens[0] == "fetch")
        {
            Component getter = GetVehicleComponent(tokens[1]);
            float val = getter.FetchVar(tokens[2]);
            string toSave = tokens[3];
            SaveVariable(toSave, val);
        }
        if (tokens[0] == "print")
        {
            if (variables.ContainsKey(tokens[1]))
            {
                Debug.Log("PrintVar: " + tokens[1] + " = " + variables[tokens[1]]);
            }
            else
            {
                string debug = "";
                for (int i = 1; i < tokens.Length; i++)
                {
                    debug += tokens[i] + " ";
                }
                Debug.Log("Print: " + debug);
            }
        }
        if (tokens[0] == "delay")
        {
            foreach (string key in variables.Keys)
            {
                command = command.Replace(key, variables[key].ToString());
            }
            tokens = command.Split(" ");
            schedulerClock = 0; 
            schedulerDelay = commandClockSpeed * double.Parse(tokens[1]);
        }
        if (tokens[0] == "compare")
        {
            string varToSave = tokens[3];
            if (variables.ContainsKey(tokens[1]))
            {
                tokens[1] = variables[tokens[1]].ToString();
            }
            if (variables.ContainsKey(tokens[2]))
            {
                tokens[2] = variables[tokens[2]].ToString();
            }
            float firstVal = float.Parse(tokens[1]);
            float secondVal = float.Parse(tokens[2]);
            float saveValue = 0;
            if (firstVal > secondVal)
            {
                saveValue = 1;
            }
            //Debug.Log(varToSave);
            //Debug.Log("Compare returned " + saveValue);
            SaveVariable(varToSave, saveValue);
        }
        if (tokens[0] == "branch")
        {
            string varToBranch = tokens[1];
            float val = variables[varToBranch];
            string branchPoint = tokens[2];
            if (val > 0)
            {
                int lineNum = codeLines[0];
                while (lineNum < codeStack[0].Length)
                {
                    string line = codeStack[0][lineNum];
                    while (line.Contains("  "))
                    {
                        line = line.Replace("  ", " ");
                    }
                    line = line.Trim();
                    if (line == "lbl " + branchPoint)
                    {
                        break;
                    }
                    lineNum++;
                }
                if (lineNum < codeStack[0].Length)
                {
                    codeLines[0] = lineNum;
                }
                else
                {
                    //Debug.Log("Error: Label not found");
                }
            }
        }
        if (tokens[0] == "goto")
        {
            string branchPoint = tokens[1];
            int lineNum = codeLines[0];
            while (lineNum < codeStack[0].Length)
            {
                string line = codeStack[0][lineNum];
                while (line.Contains("  "))
                {
                    line = line.Replace("  ", " ");
                }
                line = line.Trim();
                if (line == "lbl " + branchPoint)
                {
                    break;
                }
                lineNum++;
            }
            if (lineNum < codeStack[0].Length)
            {
                codeLines[0] = lineNum;
            }
            else
            {
                Debug.Log("Error: Label not found");
            }
        }
        //********Replaced variables
        foreach (string key in variables.Keys)
        {
            command = command.Replace(key, variables[key].ToString());
        }
        tokens = command.Split(" ");
        string component = tokens[0];
        Component componentOfInterest = GetVehicleComponent(component);
        if (componentOfInterest != null)
        {
            string newCommand = "";
            for (int i = 1; i < tokens.Length; i++)
            {
                newCommand += tokens[i] + " ";
            }
            if (newCommand.Length > 0)
                newCommand = newCommand.Substring(0, newCommand.Length - 1);
            componentOfInterest.ReceiveCommand(newCommand);
        }
    }
    public Component GetVehicleComponent(string name)
    {
        foreach (Component component in Components)
        {
            if (component.Identifier == name) return component;
        }
        return null;
    }
    private string[] CreateCode(string rawCode)
    {
        rawCode = rawCode.Replace("\n", " ");
        string[] theCode = rawCode.Split(";");
        return theCode;
    }
    private string[] CreateMethod(string rawCode)
    {
        rawCode = rawCode.Split("#")[rawCode.Split("#").Length - 1];
        rawCode = rawCode.Replace("\n", " ");
        string[] theCode = rawCode.Split(";");
        return theCode;
    }
    private void SaveVariable(string varName, float value)
    {
        if (variables.ContainsKey(varName))
        {
            variables[varName] = value;
        }
        else
        {
            variables.Add(varName, value);
        }
    }
    private string[] CreateBlockCode(string rawCode, string block)
    {
        rawCode = rawCode.Replace("\n", " ");
        string[] segments = rawCode.Split("#");
        int mainblockstart = System.Array.IndexOf(segments, block);
        string interestingBlock = segments[mainblockstart + 1];
        string[] theCode = interestingBlock.Split(";");
        return theCode;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position+transform.TransformDirection((Vector3)centerOfMassOffset), 1);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == gameObject.layer) return;
        if(collision.relativeVelocity.magnitude > 10)
        {
            Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
        }
    }
}
