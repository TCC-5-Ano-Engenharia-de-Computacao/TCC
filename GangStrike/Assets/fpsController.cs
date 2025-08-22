using UnityEngine;

public class fpsController : MonoBehaviour
{
    public int fps = 60;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 0; // Set vSyncCount to 0 so that using .targetFrameRate is enabled.
        Application.targetFrameRate = fps;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
