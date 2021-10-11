using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{

  public Canvas loggingmenu;
  public Canvas defaultview;
  public Canvas cameraview;
  public GameObject scannercam;
  public FetchEvents eventsUpdate;
    // Start is called before the first frame update
    void Start()
    {
      scannercam.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void enableDefaultView()
    {
      cameraview.enabled = false;
      loggingmenu.enabled = false;
      defaultview.enabled = true;
      scannercam.SetActive(false);
      eventsUpdate.fetchNow();
    }

    public void enableScanner()
    {
      defaultview.enabled = false;
      cameraview.enabled = true;
      scannercam.SetActive(true);
    }
}
