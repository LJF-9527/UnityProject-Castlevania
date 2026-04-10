using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";

    private bool encryptData=false;
    private string codeWord = "喊声爹听听";

    public FileDataHandler(string _dataDirPath,string _dataFileName,bool _encryptData)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encryptData = _encryptData;
    }
    public void Save(GameData _data)
    {
        string fullPath=Path.Combine(dataDirPath,dataFileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(_data, true);

            if(encryptData)
                dataToStore=EncryptDecrypt(dataToStore);
            
            using(FileStream stream=new FileStream(fullPath,FileMode.Create))
            {
                using(StreamWriter writer=new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }

        catch(Exception e)
        {
            Debug.LogError("尝试保存数据到路径时发生错误："+fullPath+"\n"+e);
        }

    }

    public GameData Load() 
    {
        string fullPath=Path.Combine(dataDirPath,dataFileName);
        GameData loadData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using(FileStream stream=new FileStream(fullPath,FileMode.Open))
                {
                    using(StreamReader reader=new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if(encryptData)
                    dataToLoad=EncryptDecrypt(dataToLoad);

                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("尝试从路径中加载存档时发生错误：" + fullPath + "\n" + e);
            }
        }
        return loadData;
    }
    public void Delet()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if(File.Exists(fullPath))File.Delete(fullPath);
    }
    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";
        for(int i=0;i<_data.Length;i++)
        {
            modifiedData += (char)(_data[i] ^ codeWord[i % codeWord.Length]);
        }
        return modifiedData;
    }
}
