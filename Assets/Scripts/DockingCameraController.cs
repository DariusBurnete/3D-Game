using Unity.Cinemachine;
using UnityEngine;

public class DockingCameraController : MonoBehaviour
{
    private string shipCamTag = "ShipCam";
    private string stationCamTag = "StationCam";
    private CinemachineVirtualCameraBase shipCam;
    private CinemachineVirtualCameraBase stationCam;

    private void RebindCameras()
    {
        GameObject shipCamObj = GameObject.FindGameObjectWithTag(shipCamTag);
        if (shipCamObj != null)
            shipCam = shipCamObj.GetComponent<CinemachineVirtualCameraBase>();

        GameObject stationCamObj = GameObject.FindGameObjectWithTag(stationCamTag);
        if (stationCamObj != null)
            stationCam = stationCamObj.GetComponent<CinemachineVirtualCameraBase>();
    }

    public void SwitchToStationView()
    {
        RebindCameras();
        if (stationCam == null || shipCam == null) return;
        stationCam.Priority = 20;
        shipCam.Priority = 10;
    }

    public void SwitchToShipView()
    {
        RebindCameras();
        if (stationCam == null || shipCam == null) return;
        shipCam.Priority = 20;
        stationCam.Priority = 10;
    }
}