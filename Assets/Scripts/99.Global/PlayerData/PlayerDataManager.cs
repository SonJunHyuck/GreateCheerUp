public static class PlayerDataManager
{
    #region Currency
        public static int GetCurrency() => CurrencyManager.GetCurrency();
        public static void AddCurrency(int amount) => CurrencyManager.AddCurrency(amount);
        public static bool SpendCurrency(int amount) => CurrencyManager.SpendCurrency(amount);
    #endregion

    #region Stage
        public static int GetStageClearGrade(int stageId) => StageProgressManager.GetStageClearGrade(stageId);
        public static void SaveStageClear(int stageId, int clearGrade) => StageProgressManager.SaveStageClear(stageId, clearGrade);
        public static bool IsOpenStage(int stageId) => StageProgressManager.IsOpenStage(stageId);
    #endregion

    #region UnitGrade
        public static int GetUnitGrade(string unitKey) => UnitUpgradeManager.GetUnitGrade(unitKey);
        public static void UpgradeUnit(string unitKey) => UnitUpgradeManager.UpgradeUnit(unitKey);
    #endregion

    
}