using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SC_LoginSystem : MonoBehaviour
{
    public enum CurrentWindow { Login, Register }
    public CurrentWindow currentWindow = CurrentWindow.Login;
    public InterfaceManager interfacemanager;
    
    public GameObject loginstuff;
    public GameObject regstuff;

    public Text loginmail;
    public InputField passwordtext;

    public Text regmailtext;
    public InputField regpswtext;
    public InputField regpswtext2;
    public Text regusername;
    public Text errortext;


    public string loginEmail = "";
    public string loginPassword = "";
    public string registerEmail = "";
    public string registerPassword1 = "";
    public string registerPassword2 = "";
    public string registerUsername = "";
    public string errorMessage = "";
    public int loggedId = 0;

    bool isWorking = false;
    bool registrationCompleted = false;
    public bool isLoggedIn = false;

    //Logged-in user data
    string userName = "";
    string userEmail = "";

    public static string rootURL = "http://ceccaserv.zapto.org/"; //Path where php files are located
    public string localURL = "http://ceccaserv.zapto.org/"; //Path where php files are located

    void Start()
    {

    }

    void Update()
    {
      loginEmail = loginmail.text;
      loginPassword = passwordtext.text;

      registerEmail = regmailtext.text;
      registerPassword1 = regpswtext.text;
      registerPassword2 = regpswtext2.text;
      registerUsername = regusername.text;

      if(isLoggedIn)
      {
        loginstuff.SetActive(false);
      }

      if(registrationCompleted)
      {
        switchToLogin();
      }

      errortext.text = errorMessage;
    }

    public void switchToLogin()
    {
      loginstuff.SetActive(true);
      regstuff.SetActive(false);
    }

    public void switchToRegistration()
    {
      loginstuff.SetActive(false);
      regstuff.SetActive(true);
    }

    public void registrazione()
    {
      StartCoroutine(RegisterEnumerator());
    }

    public void login()
    {
      StartCoroutine(LoginEnumerator());
    }

    IEnumerator RegisterEnumerator()
    {
        isWorking = true;
        registrationCompleted = false;
        errorMessage = "";

        WWWForm form = new WWWForm();
        form.AddField("email", registerEmail);
        form.AddField("username", registerUsername);
        form.AddField("password1", registerPassword1);
        form.AddField("password2", registerPassword2);

        using (UnityWebRequest www = UnityWebRequest.Post(rootURL + "register.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                errorMessage = www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;

                if (responseText.StartsWith("Success"))
                {
                    ResetValues();
                    registrationCompleted = true;
                    currentWindow = CurrentWindow.Login;
                }
                else
                {
                    errorMessage = responseText;
                }
            }
        }

        isWorking = false;
    }

    IEnumerator LoginEnumerator()
    {
        isWorking = true;
        registrationCompleted = false;
        errorMessage = "";

        WWWForm form = new WWWForm();
        form.AddField("email", loginEmail);
        form.AddField("password", loginPassword);

        using (UnityWebRequest www = UnityWebRequest.Post(rootURL + "login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                errorMessage = www.error;
            }
            else
            {
                string responseText = www.downloadHandler.text;
                print(responseText);
                if (responseText.StartsWith("Success"))
                {
                    string[] dataChunks = responseText.Split('|');
                    userName = dataChunks[1];
                    userEmail = dataChunks[2];
                    loggedId = int.Parse(dataChunks[3]);
                    isLoggedIn = true;

                    ResetValues();
                    interfacemanager.enableDefaultView();
                }
                else
                {
                    errorMessage = responseText;
                }
            }
        }

        isWorking = false;
    }

    void ResetValues()
    {
        errorMessage = "";
        loginEmail = "";
        loginPassword = "";
        registerEmail = "";
        registerPassword1 = "";
        registerPassword2 = "";
        registerUsername = "";
    }


}
