using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class AutomatedBuild : MonoBehaviour 
{
    public static void BuildWebGL()
    {
        BuildPlayerOptions options;
        //options.scenes = new string[] { "Assets/Scene1.unity" };
        options.scenes = new string[] { 
            "MainMenu.unity",
            "Level1.unity" 
        };

        options.targetGroup = BuildTargetGroup.WebGL;
        options.target = BuildTarget.WebGL;
        options.locationPathName = "Build/WebGL";

        BuildPipeline.BuildPlayer(options);
    }
}
