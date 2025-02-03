using UnityEngine;

public static class CurrencyManager
{
    private static readonly string SavePath = $"{Application.persistentDataPath}/currency_data.json";
    private static CurrencyData currencyData;

    static CurrencyManager()
    {
        LoadCurrency();
    }

    public static int GetCurrency()
    {
        return currencyData.amount;
    }

    public static void AddCurrency(int amount)
    {
        currencyData.amount += amount;
        SaveCurrency();
    }

    public static bool SpendCurrency(int amount)
    {
        if (currencyData.amount < amount)
        {
            Debug.LogWarning("Not enough currency!");
            return false;
        }

        currencyData.amount -= amount;
        SaveCurrency();
        return true;
    }

    private static void SaveCurrency()
    {
        DataEncryptionUtility.SaveEncryptedDataWithIntegrity(SavePath, currencyData);
        // File.WriteAllText(SavePath, JsonUtility.ToJson(currencyData, true));
    }

    private static void LoadCurrency()
    {
        var data = DataEncryptionUtility.LoadEncryptedDataWithIntegrity<CurrencyData>(SavePath);

        if(data == null)
        {
            data = new CurrencyData();
            currencyData = data;
            SaveCurrency();
        }
        else
        {
            currencyData = data;
        }
    }
}

[System.Serializable]
public class CurrencyData
{
    public int amount;
}