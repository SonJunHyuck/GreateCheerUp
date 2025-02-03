using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class UnitUpgradeManager
{
    private static readonly string SavePath = $"{Application.persistentDataPath}/unit_upgrade.json";
    private static UnitUpgradeCollection upgradeData;

    static UnitUpgradeManager()
    {
        LoadUpgrades();
    }

    public static int GetUnitGrade(string unitKey)
    {
        var unit = upgradeData.units.Find(u => u.unitKey == unitKey);
        return unit != null ? unit.grade : 1;
    }

    public static void UpgradeUnit(string unitKey)
    {
        var unit = upgradeData.units.Find(u => u.unitKey == unitKey);
        if (unit == null)
        {
            unit = new UnitUpgrade { unitKey = unitKey, grade = 1 };
            upgradeData.units.Add(unit);
        }
        else
        {
            unit.grade++;
        }

        SaveUpgrades();
    }

    private static void SaveUpgrades()
    {
        DataEncryptionUtility.SaveEncryptedDataWithIntegrity(SavePath, upgradeData);
    }

    private static void LoadUpgrades()
    {
        var data = DataEncryptionUtility.LoadEncryptedDataWithIntegrity<UnitUpgradeCollection>(SavePath);
        
        if(data == null)
        {
            data = new UnitUpgradeCollection();

            foreach (var unitInfo in DataManager.Instance.GetAllUnitKey()) // 가상의 유닛 키 가져오기
            {
                data.units.Add(new UnitUpgrade { unitKey = unitInfo, grade = 1 });
            }

            upgradeData = data;

            SaveUpgrades();
        }
        else
        {
            upgradeData = data;
        }
    }
}

[System.Serializable]
public class UnitUpgrade
{
    public string unitKey;
    public int grade;
}

[System.Serializable]
public class UnitUpgradeCollection
{
    public List<UnitUpgrade> units = new List<UnitUpgrade>();
}