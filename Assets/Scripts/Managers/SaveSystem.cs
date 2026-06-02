using System.IO;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;
using System;

[Serializable]
public struct SaveData
{
    public PlayerSaveData playerSaveData;
    public GameSaveData gameSaveData;
}

public class SaveSystem
{
    private static SaveData saveData = new SaveData();
    private static readonly string encryptionKey = "TyqefIuNP2peLJp7H44+bE4tGntcLomdLp54HS6Nbg==";

    public static string FileName { get => Application.persistentDataPath + "/save" + ".dat"; }


    public static void SaveGame()
    {
        GameManager.Instance.Save(ref saveData);
        string json =  JsonUtility.ToJson(saveData);
        string encrypted = EncryptString(json);

        File.WriteAllText(FileName, encrypted);
    }

    public static void LoadGame()
    {
        string saveContent = File.ReadAllText(FileName);
        string decrypted = DecryptString(saveContent);
        saveData = JsonUtility.FromJson<SaveData>(decrypted);

        GameManager.Instance.Load(saveData);
    }

    private static string EncryptString(string original)
    {
        byte[] key = Encoding.UTF8.GetBytes(encryptionKey[..32]);
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.GenerateIV();
            ICryptoTransform encyptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new())
            {
                ms.Write(aes.IV, 0, aes.IV.Length);
                using (CryptoStream cs = new(ms, encyptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new(cs))
                    {
                        sw.Write(original);
                    }
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private static string DecryptString(string encrypted)
    {
        byte[] fullCipher = Convert.FromBase64String(encrypted);
        byte[] iv = new byte[16];
        byte[] cipher = new byte[fullCipher.Length - 16];

        Array.Copy(fullCipher, iv, iv.Length);
        Array.Copy(fullCipher, 16, cipher, 0, cipher.Length);

        byte[] key = Encoding.UTF8.GetBytes(encryptionKey[..32]);
        
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream ms = new(cipher))
            {
                using (CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
