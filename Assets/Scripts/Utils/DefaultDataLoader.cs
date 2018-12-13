using System;
using System.IO;
using UnityEngine;

public class DefaultDataLoader : MonoBehaviour
{
    private void Awake()
    {
        BetterStreamingAssets.Initialize();
        if (!PlayerPrefs.HasKey("Played"))
        {
            PlayerPrefs.SetInt("Played", 0);
            PlayerPrefs.Save();
        }

        int hasPlayed = PlayerPrefs.GetInt("Played");
        if (hasPlayed == 0)
        {
            //PlayerPrefs.SetInt("Played", 1);
            PlayerPrefs.Save();
            foreach (string file in  BetterStreamingAssets.GetFiles("\\QuickSave\\", "*.json", SearchOption.AllDirectories))
            {
                InitFile(file.Substring(1));
                //Debug.Log(file);
            }
        }
    }


    void InitFile(string name)
    {
        bool CanExQuery = false;
        string path = System.IO.Path.Combine(Application.persistentDataPath, name);
        string sourcePath = System.IO.Path.Combine(Application.streamingAssetsPath, name);
        if (!System.IO.File.Exists(path)) //||(System.IO.File.GetLastWriteTimeUtc(sourcePath) > System.IO.File.GetLastWriteTimeUtc(path)))
        {
            if (sourcePath.Contains("://"))
            {
                WWW www = new WWW(sourcePath);
                while (!www.isDone)
                {
                    ;
                }

                if (String.IsNullOrEmpty(www.error))
                {
                    System.IO.File.WriteAllBytes(path, www.bytes);
                }
                else
                {
                    CanExQuery = false;
                }
            }
            else
            {
                if (System.IO.File.Exists(sourcePath))
                {
                    System.IO.File.Copy(sourcePath, path, true);
                }
                else
                {
                    CanExQuery = false;
                    Debug.Log("ERROR: the file DB named " + name +
                              " doesn't exist in the StreamingAssets Folder, please copy it there.");
                }
            }
        }
    }
}