using System.IO;
using UnityEngine;
using System.Text;

public static class DataEncryptionUtility
{
    private static readonly string EncryptionKey = "ThisIsIntegrityKey"; // 고정된 암호화 키
    private static readonly string IntegrityKey = "ThisIsEncryptionKey";   // 해시용 고정 키

    // 데이터 암호화
    public static string Encrypt(string plainText)
    {
        byte[] data = Encoding.UTF8.GetBytes(plainText);
        Debug.Log(plainText);
        byte[] key = Encoding.UTF8.GetBytes(EncryptionKey.PadRight(32));
        return System.Convert.ToBase64String(XOROperation(data, key));
    }

    // 데이터 복호화
    public static string Decrypt(string encryptedText)
    {
        byte[] data = System.Convert.FromBase64String(encryptedText);
        byte[] key = Encoding.UTF8.GetBytes(EncryptionKey.PadRight(32));
        return Encoding.UTF8.GetString(XOROperation(data, key));
    }

    // XOR 연산
    private static byte[] XOROperation(byte[] data, byte[] key)
    {
        byte[] result = new byte[data.Length];
        for (int i = 0; i < data.Length; i++)
        {
            result[i] = (byte)(data[i] ^ key[i % key.Length]);
        }
        return result;
    }

    // 데이터의 해시 생성 (무결성 검증)
    private static string GenerateHash(string data)
    {
        string combined = data + IntegrityKey;
        using (var sha = System.Security.Cryptography.SHA256.Create())
        {
            byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(combined));
            return System.Convert.ToBase64String(hashBytes);
        }
    }

    // 해시 검증
    private static bool VerifyHash(string data, string hash)
    {
        string newHash = GenerateHash(data);
        return newHash == hash;
    }

    // 암호화된 데이터 저장 + 무결성 해시
    public static void SaveEncryptedDataWithIntegrity<T>(string path, T data)
    {
        string jsonData = JsonUtility.ToJson(data, true);     // JSON 직렬화
        string hash = GenerateHash(jsonData);                // 무결성 해시 생성
        string encryptedData = Encrypt(jsonData);            // 암호화

        var wrapper = new EncryptedDataWrapper
        {
            EncryptedData = encryptedData,
            IntegrityHash = hash
        };

        string finalJson = JsonUtility.ToJson(wrapper, true); // Wrapper를 JSON으로 변환
        File.WriteAllText(path, finalJson);                  // 파일 저장
        Debug.Log($"Encrypted data with integrity saved to: {path}");
    }

    // 암호화된 데이터 로드 + 무결성 검증
    public static T LoadEncryptedDataWithIntegrity<T>(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"File not found: {path}");
            return default;
        }

        string fileContent = File.ReadAllText(path);            // 파일 읽기
        var wrapper = JsonUtility.FromJson<EncryptedDataWrapper>(fileContent);

        string decryptedData = Decrypt(wrapper.EncryptedData);  // 복호화

        // 무결성 검증
        if (!VerifyHash(decryptedData, wrapper.IntegrityHash))
        {
            Debug.LogError("Data integrity check failed! Data might have been tampered with.");
            return default;
        }

        return JsonUtility.FromJson<T>(decryptedData);          // JSON 역직렬화
    }

    // 암호화된 데이터와 해시를 함께 저장하는 Wrapper 클래스
    [System.Serializable]
    private class EncryptedDataWrapper
    {
        public string EncryptedData;
        public string IntegrityHash;
    }
}