using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispatcherBase<T,P,X> : IDisposable 
    where T : new()
    where P : class
{
    #region ����
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }
    public void Dispose()
    {
        
    }
    #endregion
    /// <summary>
    /// �¼�ί��ԭ��
    /// </summary>
    /// <param name="param"></param>
    public delegate void OnActionHandler(P p);
    public Dictionary<X, List<OnActionHandler>> dic = new Dictionary<X, List<OnActionHandler>>();

    #region AddEventListener ����¼�����
    /// <summary>
    /// ����¼�����
    /// </summary>
    /// <param name="key"></param>
    /// <param name="handler"></param>
    public void AddEventListener(X key, OnActionHandler handler)
    {
        if (dic.ContainsKey(key))
        {
            dic[key].Add(handler);
        }
        else
        {
            List<OnActionHandler> lstHandler = new List<OnActionHandler>();
            lstHandler.Add(handler);
            dic[key] = lstHandler;
        }
    }
    #endregion

    #region RemoveEventListener �Ƴ��¼�����
    /// <summary>
    /// �Ƴ��¼�����
    /// </summary>
    /// <param name="btnKey"></param>
    /// <param name="handler"></param>
    public void RemoveEventListener(X key, OnActionHandler handler)
    {
        if (dic.ContainsKey(key))
        {
            List<OnActionHandler> lstHandler = new List<OnActionHandler>();
            lstHandler.Add(handler);
            if (lstHandler.Count == 0)
            {
                dic.Remove(key);
            }
        }
    }
    #endregion

    #region Dispatch �ɷ�
    /// <summary>
    /// �ɷ�
    /// </summary>
    /// <param name="btnKey"></param>
    /// <param name="param"></param>
    public void Dispatch(X key, P p)
    {
        if (dic.ContainsKey(key))
        {
            List<OnActionHandler> lstHandler = dic[key];
            for (int i = 0; i < lstHandler.Count; i++)
            {
                if (lstHandler[i] != null)
                {
                    lstHandler[i](p);
                }
            }
        }
    }

    public void Dispatch(X key)
    {
        Dispatch(key, null);
    }
    #endregion

}
