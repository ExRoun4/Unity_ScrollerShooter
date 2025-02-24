using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using com.cyborgAssets.inspectorButtonPro;
using UnityEditor;
using UnityEngine;

public class ObjectPoolsManager : MonoBehaviour
{
    public static ObjectPoolsManager instance;

    public GameRoot gameRoot;
    public List<ObjectPoolDecl> objectPoolDecls;
    public Dictionary<PoolableObject, ObjectPool> objectToPoolDict = new ();


    private void Awake(){
        instance = this;
    }


    #region POOLING

    public async Awaitable StartPooling(){
        await InitPools();

        foreach(ObjectPool pool in objectToPoolDict.Values){
            await pool.StartPool();
            print($"Pooled {pool.objectToPool.name}");
        }
    }

    public async Awaitable InitPools(){
        foreach(ObjectPoolDecl poolDecl in objectPoolDecls){
            await CreatePool(poolDecl.objectToPool);
        }
    }


    public async Awaitable CreatePool(PoolableObject fromObject){
        AsyncInstantiateOperation<ObjectPool> newPoolInstance = InstantiateAsync(GameRoot.instance.objectPoolPrefab, this.transform);
        while(!newPoolInstance.isDone) await Task.Yield();

        ObjectPool newPool = newPoolInstance.Result[0];

        newPool.transform.name = "[POOL] "+fromObject.name;
        newPool.objectToPool = fromObject;
        newPool.poolAmount = fromObject.poolAmount;

        objectToPoolDict.Add(fromObject, newPool);
    }

    #endregion


    #region EDITOR
    
    [ProButton]
    public void LoadPools(){
        objectPoolDecls.Clear();
        objectToPoolDict.Clear();

        List<string> allFolders = new ();
        allFolders.Add("Assets/Resources/Poolables");

        fetchFolderForSubFolders("Assets/Resources/Poolables", allFolders);

        foreach(string path in allFolders){
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();
            foreach(FileInfo file in files){
                if(file.Name.Substring(file.Name.Length - 5) != "asset") continue;
                string filePath = $"{path}/{file.Name.Substring(0, file.Name.Length).Substring(0, file.Name.Length - 6)}";
                filePath = filePath.Substring("Assets/Resources/".Length);

                RegisterPoolDecl((ObjectPoolDecl) Resources.Load(filePath));
            }
        }
    }

    private void fetchFolderForSubFolders(string path, List<string> resultList){
        String[] subFolders = AssetDatabase.GetSubFolders(path);
        foreach(string subFolderPath in subFolders){
            resultList.Add(subFolderPath);
            fetchFolderForSubFolders(subFolderPath, resultList);
        }
    }

    private void RegisterPoolDecl(ObjectPoolDecl decl){
        objectPoolDecls.Add(decl); 
    }

    #endregion


    public ObjectPool FindPoolByObject(PoolableObject byObject){
        if(!objectToPoolDict.ContainsKey(byObject)) return null;
        return objectToPoolDict[byObject];
    }
}
