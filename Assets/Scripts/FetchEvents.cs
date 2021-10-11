using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class FetchEvents : MonoBehaviour
{

  //string rootURL = "http://192.168.1.129/"; //Path where php files are located
  public Transform eventsGroup; //Il layout dove inseriamo gli eventi prenotati dall'utente
  //Insieme di variabili del contenuto dinamico
  Image backgroundColor;
  Image fotoEvento;
  Text eventText;
  public RawImage contentPrefab;

  public RawImage instantiatedContent;
  public SC_LoginSystem loginSystem;
  public string errorMessage;
  public string[] nomiEvento;
  public string[] dateEvento;
  public string[] orarioEvento;
  public string[] localitaEvento;
  public string[] tipologiaEvento;
  public int[] totposti;
  public int[] prenotato;
  public Text[] testiprecedenti;

  public Texture2D loadedTexture;
    // Start is called before the first frame update
    void Start()
    {
      fetchNow();
      
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void fetchNow()
    {
      StartCoroutine(FetchEnumerator());
    }

    public bool checkSameBox(string tocheck)
    {
      Text[] check = eventsGroup.GetComponentsInChildren<Text>();
      for(int i=0;i<check.Length;i++)
      {
        print(check[i].text);
        if(string.Equals(check[i].text,tocheck))
        {
          return true;
        }
      }
      return false;
    }

    public bool checkAlreadyBooked(int tocheck)
    {
      Unsubscribe[] presenti = eventsGroup.GetComponentsInChildren<Unsubscribe>();
      for(int i=0;i<presenti.Length;i++)
      {
        if(presenti[i].myEventId == tocheck)
          return true;
      }
      return false;
    }

    IEnumerator FetchEnumerator()
    {
      instantiatedContent = null;
      
      yield return new WaitForSeconds(2f);
      WWWForm form = new WWWForm();
      form.AddField("myid", loginSystem.loggedId);

      using (UnityWebRequest www = UnityWebRequest.Post(loginSystem.localURL + "myevents.php", form))
      {
          yield return www.SendWebRequest();

          if (www.result == UnityWebRequest.Result.ConnectionError)
          {
              errorMessage = www.error;
          }
          else
          {
              string responseText = www.downloadHandler.text;

                string[] dataChunks = responseText.Split('!');
                for(int i=0;i<dataChunks.Length-1;i++)
                {
                  string[] line = dataChunks[i].Split('|');
                  string formattedDesc = line[0] + " il " + line[1] + " ore " + line[2] + " a " + line[3] + ", " + line[4] + ", occupati " + line[6] + "/" + line[5] + ", evento n" + line[7] + ", prenotazione n. " + line[8];
                  
                  if(checkSameBox(formattedDesc) == false)
                  {
                    instantiatedContent = Instantiate(contentPrefab, eventsGroup);
                    instantiatedContent.GetComponent<Unsubscribe>().setId(int.Parse(line[7]));
                    
                    Image[] images = instantiatedContent.GetComponentsInChildren<Image>();
                    backgroundColor = images[1];
                    StartCoroutine(GetTexture(line[9], instantiatedContent));
                    Debug.Log(line[9]);
                    //instantiatedContent.texture = loadedTexture;
                    
                    //Ottengo componente testo
                    eventText = instantiatedContent.GetComponentInChildren<Text>();
                    //Impostazione colore sfondo e testo formattato
                    backgroundColor.color = new Color32((byte)Random.Range(0,255),(byte)Random.Range(0,255),(byte)Random.Range(0,255),255); //To be random
                    //fotoEvento.color = new Color32(255,255,255,255);
                    eventText.text = formattedDesc;
                  }




          }
      }
    }

    IEnumerator GetTexture(string imageurl, RawImage apply) {
        WWW www = new WWW(imageurl);
        yield return www;

        apply.texture = www.texture;
        
    }

    IEnumerator Wait()
    {
      yield return new WaitForSeconds(2f);
    }
}
}
