#if (UNITY_EDITOR) 
using UnityEditor;

public static class HeadlessTest
{
    public static void Start100()
    {
        EditorApplication.OpenScene("Assets/100.unity");
        EditorApplication.isPlaying = true;
    }

    public static void Start2()
    {
        EditorApplication.OpenScene("Assets/2.unity");
        EditorApplication.isPlaying = true;
    }

    public static void Start1()
    {
        EditorApplication.OpenScene("Assets/1.unity");
        EditorApplication.isPlaying = true;
    }

    public static void StartSmallXY24()
    {
        EditorApplication.OpenScene("Assets/smallxy24.unity");
        EditorApplication.isPlaying = true;
    }

    public static void StartSmallXY100()
    {
        EditorApplication.OpenScene("Assets/smallxy100.unity");
        EditorApplication.isPlaying = true;
    }

    public static void StartSmallXY1()
    {
        EditorApplication.OpenScene("Assets/smallxy1.unity");
        EditorApplication.isPlaying = true;
    }

    public static void IPCTest()
    {
        var ipc = new IPCTest();
        ipc.Create();
    }
}
#endif