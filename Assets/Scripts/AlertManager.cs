using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertManager : MonoBehaviour
{

    public Canvas NotifierObj;
    public Animator contenitore;
    public Animator sceltaContenitore;

    public Text notificaText;
    public Text sceltaText;
    public Text notificaTitle;
    public Text sceltaTitle;



    // Start is called before the first frame update
    void Start()
    {
      NotifierObj.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void disappear(Animator anim)
    {
      StartCoroutine(disappRoutine(anim));
    }

    public void disappearChoice()
    {
      StartCoroutine(disappRoutine(sceltaContenitore));
    }

    public void disappearNotifica()
    {
      StartCoroutine(disappRoutine(contenitore));
    }

    public void appearNotifica()
    {
      NotifierObj.enabled = true;
      contenitore.SetTrigger("Appari");
    }

    public void appearChoice(string message)
    {
      setSceltaText(message, "Sicuro?");
      NotifierObj.enabled = true;
      sceltaContenitore.SetTrigger("Appari");
    }

    public IEnumerator disappRoutine(Animator anim)
    {
      anim.SetTrigger("Scompari");
      yield return new WaitForSeconds(1);
      NotifierObj.enabled = false;
    }

    public void setNotificaText(string content, string title)
    {
      notificaText.text = content;
      notificaTitle.text = title;
    }

    public void setSceltaText(string content, string title)
    {
      sceltaText.text = content;
      sceltaTitle.text = title;
    }
}
