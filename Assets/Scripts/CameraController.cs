using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public CinemachineCamera cam;
    
    public float minZoom = 5;
    public float defaultZoom = 10;
    public float maxZoom = 20;

    public float zoomRate = 0.2f;
    
    public float currentZoom;

    public void Start()
    {
        currentZoom = defaultZoom;
    }

    void Update()
    {
        currentZoom += -Input.GetAxis("Mouse ScrollWheel") * zoomRate;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

        if (Input.GetButtonDown("ZoomReset"))
        {
            currentZoom = defaultZoom;
        }
        
        cam.Lens.OrthographicSize = currentZoom;
    }
}
