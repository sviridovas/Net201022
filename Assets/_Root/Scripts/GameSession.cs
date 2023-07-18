using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int score = 0;
    [SerializeField] private TMP_Text scoreText;

    void Awake()
    {
        int count = FindObjectsOfType<GameSession>().Length;
        if (count > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        scoreText.text = score.ToString();

        LoginWithCustomID();
    }

    private const string AuthGuidKey = "authorization-guid";
    private const string VirtualCurrency = "AU";

    public void LoginWithCustomID()
    {
        // Here we need to check whether TitleId property is configured in settings or not
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            * If not we need to assign it to the appropriate variable manually
            * Otherwise we can just remove this if statement at all
            */
            PlayFabSettings.staticSettings.TitleId = "20C7F";
        }

        var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
        var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CustomId = id,
            CreateAccount = !needCreation
        }, success =>
        {
            PlayerPrefs.SetString(AuthGuidKey, id);

            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            result =>
            {
                Debug.Log($"Inventory was loaded successfully!");

                var amount = result.VirtualCurrency[VirtualCurrency];
                score = amount;
                scoreText.text = score.ToString();
            },
            error =>
            {
                var errorMessage = error.GenerateErrorReport();
                Debug.LogError($"Something went wrong: {errorMessage}");
            }
            );

            // PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), result =>
            // {
            //     HandleCatalog(result.Catalog);
            //     Debug.Log($"Catalog was loaded successfully!");
            // },
            // error =>
            // {
            //     var errorMessage = error.GenerateErrorReport();
            //     Debug.LogError($"Something went wrong: {errorMessage}");
            // });
        }, err =>
        {
            Debug.LogError($"Fail: {err.ErrorMessage}");
        });
    }

    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();

        PlayFabClientAPI.AddUserVirtualCurrency(new AddUserVirtualCurrencyRequest() {
            Amount = amount,
            VirtualCurrency = VirtualCurrency
        },
        result =>
        {
            Debug.Log($"VirtualCurrency was added successfully!");
        },
        error =>
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }
        );
    }

}
