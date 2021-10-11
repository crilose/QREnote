using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class DeleteAccount : MonoBehaviour
{

  //string rootURL = "http://192.168.1.129/"; //Path where php files are located
  public SC_LoginSystem loginSystem;
  public string errorMessage;

  public Canvas appCanvas;
  public Canvas QRcanvas;
  public Canvas loginCanvas;
  public Canvas settingsCanvas;

  public AlertManager alert;

  Button delete;
  public Button yesDelete;
  public Button noDelete;

  public Unsubscribe[] myEvents;

    // Start is called before the first frame update
    void Start()
    {
      loginSystem = FindObjectOfType<SC_LoginSystem>();
      alert = FindObjectOfType<AlertManager>();
      delete = GetComponentInChildren<Button>();
      delete.onClick.AddListener(delegate () { alert.appearChoice("Eliminare account?"); });
      yesDelete = GameObject.FindGameObjectWithTag("yesdelete").GetComponent<Button>();
      noDelete = GameObject.FindGameObjectWithTag("nodelete").GetComponent<Button>();
      
        
      myEvents = FindObjectsOfType<Unsubscribe>();
    }

    // Update is called once per frame
    void Update()
    {
        myEvents = FindObjectsOfType<Unsubscribe>();
    }

    public void activateMyAction()
    {
            yesDelete.onClick.AddListener(delegate () { deleteAcc(); });
            Debug.Log("Abilitata cancellazione account");
    }

    public void deleteAcc()
    {
      for(int i=0;i<myEvents.Length;i++)
      {
        StartCoroutine(UnSubscribeFromEvents(myEvents[i].myEventId));
        Debug.Log("Deleting event " + myEvents[i].myEventId + " as my " + i + " iteration out of " + myEvents.Length);
        Destroy(myEvents[i].gameObject);
      }
      //StartCoroutine(Deleteaccount());
      appCanvas.enabled = false;
      QRcanvas.enabled = false;
      loginCanvas.enabled = true;
      loginSystem.isLoggedIn = false;
      loginSystem.loginstuff.SetActive(true);
      settingsCanvas.enabled = false;
    }

    IEnumerator Deleteaccount()
    {
      WWWForm form = new WWWForm();
      form.AddField("userId", loginSystem.loggedId);
      using (UnityWebRequest www = UnityWebRequest.Post(SC_LoginSystem.rootURL + "deleteaccount.php", form))
      {
          yield return www.SendWebRequest();

          if (www.result == UnityWebRequest.Result.ConnectionError)
          {
              errorMessage = www.error;
          }
          else
          {
              string responseText = www.downloadHandler.text;
              print (responseText);

          }
      }
    }

    IEnumerator UnSubscribeFromEvents(int eventId)
    {
      WWWForm form = new WWWForm();
      form.AddField("userId", loginSystem.loggedId);
      form.AddField("eventId", eventId);
      using (UnityWebRequest www = UnityWebRequest.Post(SC_LoginSystem.rootURL + "unsubscribe.php", form))
      {
          yield return www.SendWebRequest();

          if (www.result == UnityWebRequest.Result.ConnectionError)
          {
              errorMessage = www.error;
          }
          else
          {
              string responseText = www.downloadHandler.text;
              print (responseText);

          }
      }
    }
}
