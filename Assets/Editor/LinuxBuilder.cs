﻿using System;
using System.Collections.Generic;
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
        buildPlayerOptions.scenes = new[] { "Assets/dog.unity"};
        buildPlayerOptions.locationPathName = "build/sumo.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux;
        buildPlayerOptions.options = BuildOptions.EnableHeadlessMode;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }

    static void buildLinuxHumanoid2D()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/humanoid2d.unity" };
        buildPlayerOptions.locationPathName = "build/sumo.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux;
        buildPlayerOptions.options = BuildOptions.EnableHeadlessMode;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
