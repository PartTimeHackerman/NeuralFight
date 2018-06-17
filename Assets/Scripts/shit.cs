using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shit : MonoBehaviour
{

    public int fps = 30;
    // Use this for initialization
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = fps;
        Debug.Log(fps);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
