using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleMgr : Singleton<AssetBundleMgr>
{
    /// <summary>
    /// ���ؾ���
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject Load(string path, string name)
    {
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            return loader.LoadAsset<GameObject>(name); 
        }
    }
    /// <summary>
    /// ���ؿ�¡
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject LoadClone(string path, string name)
    {
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            GameObject obj = loader.LoadAsset<GameObject>(name);
            return Object.Instantiate(obj);
        }
    }
    /// <summary>
    /// �첽������Դ
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public AssetBundleLoaderAsync LoadAsyncObject(string path, string name)
    {
        GameObject obj = new GameObject("AssetBundleLoadAsync");
        AssetBundleLoaderAsync async = obj.GetOrCreatCompontent<AssetBundleLoaderAsync>();
        async.Init(path, name);
        return async;
    }
}
