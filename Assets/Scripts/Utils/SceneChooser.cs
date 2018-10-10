using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class SceneChooser : MonoBehaviour
{
    private void Awake()
    {
        SceneManager.LoadScene(getSceneArg(), LoadSceneMode.Single);
    }

    private string getSceneArg()
    {
        var args = System.Environment.GetCommandLineArgs();
        var scene = "";
        for (var i = 0; i < args.Length; i++)
        {
            if (args[i] == "--scene")
            {
                scene = args[i + 1];
            }
        }

        return scene;
    }
}