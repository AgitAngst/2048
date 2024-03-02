using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;

public class SaveSytem : MonoBehaviour
{
    public GameManager GameManager;
    // Подписываемся на событие GetDataEvent в OnEnable
    private void OnEnable() => YandexGame.GetDataEvent += LoadSettings;

    // Отписываемся от события GetDataEvent в OnDisable
    private void OnDisable() => YandexGame.GetDataEvent -= LoadSettings;


    private void Awake()
    {
    }
    private void Start()
    {
        // Проверяем запустился ли плагин
        if (YandexGame.SDKEnabled == true)
        {
            // Если запустился, то выполняем Ваш метод для загрузки
            LoadSettings();

            // Если плагин еще не прогрузился, то метод не выполнится в методе Start,
            // но он запустится при вызове события GetDataEvent, после прогрузки плагина
        }
    }

    // Ваш метод для загрузки, который будет запускаться в старте
    public void LoadSettings()
    {
        // Получаем данные из плагина и делаем с ними что хотим
        // Например, мы хотил записать в компонент UI.Text сколько у игрока монет:
        GameManager._highScore = YandexGame.savesData.highScore;
        GameManager._textHighScore.text = GameManager._highScore.ToString();

    }

    // Допустим, это Ваш метод для сохранения
    public void SaveSettings()
    {
        // Записываем данные в плагин
        // Например, мы хотил сохранить количество монет игрока:
        YandexGame.savesData.highScore = GameManager._highScore;

        // Теперь остаётся сохранить данные
        YandexGame.SaveProgress();
    }
}
