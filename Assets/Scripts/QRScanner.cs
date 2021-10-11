using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using System;

public class QRScanner : MonoBehaviour
{
    public WebCamTexture webcamTexture;
    public string QrCode = string.Empty;
    public AudioSource beepSound;
    public Text risultato;
    public GameObject pulsante;
    void Start()
    {
        var renderer = GetComponent<RawImage>();
        webcamTexture = new WebCamTexture(WebCamTexture.devices[0].name);
        renderer.material.mainTexture = webcamTexture;
        pulsante.SetActive(false);
        StartCoroutine(GetQRCode());
    }

    public void resetCam()
    {
      pulsante.SetActive(false);
      risultato.text = "";
    }

    IEnumerator GetQRCode()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
        webcamTexture.Play();
        var snap = new Texture2D(webcamTexture.width, webcamTexture.height, TextureFormat.ARGB32, false);
        while (true)
        {
            try
            {
                snap.SetPixels32(webcamTexture.GetPixels32());
                var Result= barCodeReader.Decode(snap.GetRawTextureData(), webcamTexture.width, webcamTexture.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                if (Result!= null)
                {
                    pulsante.SetActive(true);
                    beepSound.Play();
                    QrCode = Result.Text;
                    if (!string.IsNullOrEmpty(QrCode))
                    {
                         Debug.Log("DECODED TEXT FROM QR: " + QrCode);
                         risultato.text = "EVENTO TROVATO!";
                    }
                }
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
            yield return null;
        }
    }

    public void startScanning()
    {
      StartCoroutine(GetQRCode());
    }
}
