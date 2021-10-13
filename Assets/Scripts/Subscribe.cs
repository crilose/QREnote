using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Subscribe : MonoBehaviour
{

  //string rootURL = "http://ceccaserver.zapto.org/"; //Path where php files are located
  public QRScanner qrscan;
  public SC_LoginSystem loginSystem;
  public FetchEvents fetch;
  public string errorMessage;

  public Canvas mycanvas;

  public AlertManager alert;

    // Start is called before the first frame update
    void Start()
    {
      fetch = FindObjectOfType<FetchEvents>();
      alert = FindObjectOfType<AlertManager>();
    }

    // Update is called once per frame
    void Update()
    {
      if(mycanvas.enabled)
      {
        qrscan.enabled = true;
      }
      else
      {
        qrscan.enabled = false;
      }
    }

    public void sub()
    {
      StartCoroutine(SubscribetoEvent());
      fetch.fetchNow();
    }
    IEnumerator SubscribetoEvent()
    {
      WWWForm form = new WWWForm();
      form.AddField("userId", loginSystem.loggedId);
      print("Sending link" + qrscan.QrCode);
      string scannedId = qrscan.QrCode.Substring(qrscan.QrCode.Length - 1);
      if(fetch.checkAlreadyBooked(int.Parse(scannedId)) == false)
      {
        using (UnityWebRequest www = UnityWebRequest.Post(qrscan.QrCode, form))
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
              if(string.Equals(responseText, "pieno"))
              {
                alert.appearNotifica();
                alert.setNotificaText("Siamo spiacenti, l'evento è pieno!", "Al completo!");
              }

              if(string.Equals(responseText, "inesistente"))
              {
                alert.appearNotifica();
                alert.setNotificaText("Siamo spiacenti, l'evento non risulta nel database!", "Non esiste!");
              }

              else
              {
                alert.appearNotifica();
                alert.setNotificaText("Premi per tornare indietro!", "Iscritto!");
              }

          }
      }
        
      }
      else
      {
        Debug.Log("Evento già prenotato");
        alert.appearNotifica();
        alert.setNotificaText("Hai già prenotato l'evento", "Già prenotato!");
      }
      
    }
}
