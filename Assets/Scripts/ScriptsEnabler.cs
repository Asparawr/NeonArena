using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptsEnabler : MonoBehaviour
{
    public List<string> scriptsToEnable;
    public void EnableScripts()
    {
        foreach (string script in scriptsToEnable)
        {
            (GetComponent(script) as MonoBehaviour).enabled = true;
        }
    }
}
