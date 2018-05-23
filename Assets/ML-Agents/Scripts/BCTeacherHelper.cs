using UnityEngine;

/// <summary>
///     Behavioral Cloning Helper script. Attach to teacher agent to enable
///     resetting the experience buffer, as well as toggling session recording.
/// </summary>
public class BCTeacherHelper : MonoBehaviour
{
    private float bufferResetTime;
    private Agent myAgent;

    private bool recordExperiences;

    public KeyCode recordKey = KeyCode.R;
    private bool resetBuffer;
    public KeyCode resetKey = KeyCode.C;

    // Use this for initialization
    private void Start()
    {
        recordExperiences = true;
        resetBuffer = false;
        myAgent = GetComponent<Agent>();
        bufferResetTime = Time.time;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(recordKey)) recordExperiences = !recordExperiences;
        if (Input.GetKeyDown(resetKey))
        {
            resetBuffer = true;
            bufferResetTime = Time.time;
        }
        else
        {
            resetBuffer = false;
        }

        Monitor.Log("Recording experiences " + recordKey, recordExperiences.ToString());
        var timeSinceBufferReset = Time.time - bufferResetTime;
        Monitor.Log("Seconds since buffer reset " + resetKey, Mathf.FloorToInt(timeSinceBufferReset));
    }

    private void FixedUpdate()
    {
        // Convert both bools into single comma separated string. Python makes
        // assumption that this structure is preserved. 
        myAgent.SetTextObs(recordExperiences + "," + resetBuffer);
    }
}