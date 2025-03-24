using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class menuManager : MonoBehaviour
{
    public playerSettings playerSettings;
    public Slider sensX;
    public Slider sensY;
    public Slider volumePercent;
    [SerializeField] TMP_Text horizontalSensText;
    [SerializeField] TMP_Text verticalSensText;

    private int sensXNumber;
    private int sensYNumber;
    private int volumePercentNumber;

    // Start is called before the first frame update
    void Start()
    {
        sensX.value = playerSettings.horizontalSensitivity;
        sensY.value = playerSettings.verticalSensitivity;
        volumePercent.value = playerSettings.volumePercent;
    }

    // Update is called once per frame
    void Update()
    {
        sensXNumber = (int)sensX.value;
        sensYNumber = (int)sensY.value;
        volumePercentNumber = (int)volumePercent.value;

        playerSettings.horizontalSensitivity = sensXNumber;
        playerSettings.verticalSensitivity = sensYNumber;
        playerSettings.volumePercent = volumePercentNumber;

        horizontalSensText.text = sensXNumber.ToString();
        verticalSensText.text = sensYNumber.ToString();

    }
}
