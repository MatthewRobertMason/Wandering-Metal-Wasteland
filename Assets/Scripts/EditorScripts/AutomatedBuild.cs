using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AutomatedBuild : MonoBehaviour 
{
    public static void BuildWebGL()
    {
        //string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");
        string[] levels = new string[] { "Assets/Scene1.unity", "Assets/Scene2.unity" };

        // Build player.
        //BuildPipeline.BuildPlayer(levels, path + "/BuiltGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);
        
        BuildPlayerOptions options;
        options.scenes = new string[] { "Assets/Scene1.unity" };
        options.targetGroup = BuildTargetGroup.WebGL;
        options.target = BuildTarget.WebGL;
        options.locationPathName = ".\\Build\\WebGL";
    }
}
