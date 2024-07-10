using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * A UI component representing the phone screen of the phone equipment.
 */
public class PhoneUI : MonoBehaviour {

    [SerializeField] Button checkTemperatureButton;
    [SerializeField] Button checkHumidityButton;
    [SerializeField] Button checkWeatherConditionButton;

    [SerializeField] GameObject temperatureScreen;
    [SerializeField] GameObject humidityScreen;
    [SerializeField] GameObject weatherConditionScreen;

    [SerializeField] Button closeTemperatureScreenButton;
    [SerializeField] Button closeHumidityScreenButton;
    [SerializeField] Button closeWeatherConditionScreenButton;

    [SerializeField] TextMeshProUGUI temperatureText;
    [SerializeField] TextMeshProUGUI humidityText;
    [SerializeField] TextMeshProUGUI weatherText;

    private void Awake() {
        checkTemperatureButton.onClick.AddListener(() => {
            temperatureScreen.SetActive(true);
        });
        checkHumidityButton.onClick.AddListener(() => {
            humidityScreen.SetActive(true);
        });
        checkWeatherConditionButton.onClick.AddListener(() => {
            weatherConditionScreen.SetActive(true);
        });

        closeTemperatureScreenButton.onClick.AddListener(() => {
            temperatureScreen.SetActive(false);
        });
        closeHumidityScreenButton.onClick.AddListener(() => {
            humidityScreen.SetActive(false);
        });
        closeWeatherConditionScreenButton.onClick.AddListener(() => {
            weatherConditionScreen.SetActive(false);
        });
    }

    private void Start() {
        Hide();
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public bool IsShown() {
        return gameObject.activeSelf;
    }

    //Change weather details based on game scene
    public void UpdateText(EquipmentStorageManager.WeatherRecord weatherRecord) {
        temperatureText.text = weatherRecord.temperatureRecord.ToString() + "°C";
        humidityText.text = weatherRecord.humidityRecord.ToString() + "%";
        weatherText.text = weatherRecord.weatherConditionRecord.ToString();
    }
}
