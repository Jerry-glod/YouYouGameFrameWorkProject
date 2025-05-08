using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AssetBundleʵ��
/// </summary>
public class AssetBundleEntity
{
    /// <summary>
    /// ���ڴ��ʱ��ѡ�� Ψһ��Key
    /// </summary>
    public string Key;

    /// <summary>
    /// ����
    /// </summary>
    public string Name;
    /// <summary>
    /// ���
    /// </summary>
    public string Tag;
    /// <summary>
    /// �汾��
    /// </summary>
    public int Version;
    /// <summary>
    /// ��С��K��
    /// </summary>
    public long Size;
    /// <summary>
    /// �����·��
    /// </summary>
    public string ToPath;

    private List<string> m_PathList = new List<string>();
    /// <summary>
    /// ·������
    /// </summary>
    public List<string> PathList
    {
        get { return m_PathList;}
    }
}
