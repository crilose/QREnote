using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Unsubscribe : MonoBehaviour
{

  //string rootURL = "http://192.168.1.129/"; //Path where php files are located
  public SC_LoginSystem loginSystem;
  public FetchEvents fetch;
  public string errorMessage;
  public int myEventId;

  public AlertManager alert;

  Button delete;
  public Button yesDelete;
  public Button noDelete;

    // Start is called before the first frame update
    void Start()
    {
      loginSystem = FindObjectOfType<SC_LoginSystem>();
      fetch = FindObjectOfType<FetchEvents>();
      alert = FindObjectOfType<AlertManager>();
      delete = GetComponentInChildren<Button>();
      delete.onClick.AddListener(delegate () { alert.appearChoice("Eliminare evento?"); });
      yesDelete = GameObject.FindGameObjectWithTag("yesdelete").GetComponent<Button>();
      noDelete = GameObject.FindGameObjectWithTag("nodelete").GetComponent<Button>();
      
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void activateMyAction()
    {
            yesDelete.onClick.AddListener(delegate () { unsub(); });
            Debug.Log("Abilitata cancellazione evento");
    }

    public void unsub()
    {
      StartCoroutine(UnSubscribeFromEvent());
      Destroy(this.gameObject);
      fetch.fetchNow();
    }




    
    public void setId(int i)
    {
        myEventId = i;
    }

    IEnumerator UnSubscribeFromEvent()
    {
      WWWForm form = new WWWForm();
      form.AddField("userId", loginSystem.loggedId);
      form.AddField("eventId", myEventId);
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
