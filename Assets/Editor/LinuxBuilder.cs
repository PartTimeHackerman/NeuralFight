using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

class LinuxBuilder : MonoBehaviour
{

    [MenuItem("Build/Linux64Headless")]
    public static void MyBuild()
    {
        buildLinux();
    }

    static void buildLinux()
    {

        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = getMLScenes();
        buildPlayerOptions.locationPathName = "build/sumo.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.options = BuildOptions.EnableHeadlessMode;// | BuildOptions.Development | BuildOptions.AllowDebugging;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    static void buildLinuxHumanoid2D()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/animation.unity" };
        buildPlayerOptions.locationPathName = "build/sumo.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux;
        buildPlayerOptions.options = BuildOptions.EnableHeadlessMode;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    static string[] getMLScenes()
    {
        List<string> paths = new List<string>();
        paths.Add("Assets/sceneChooser.unity");
        DirectoryInfo dir = new DirectoryInfo("Assets/MLScenes");
        FileInfo[] info = dir.GetFiles("*.unity");
        for (int i = 0; i < info.Length; i++)
        {
            paths.Add("Assets/MLScenes/" + info[i].Name);
            
        }

        foreach (var path in paths)
        {
            Debug.Log(path);
        }
        return paths.ToArray();
    }
}
