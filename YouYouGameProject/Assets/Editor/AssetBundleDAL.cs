using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

public class AssetBundleDAL
{
    /// <summary>
    /// xml·��
    /// </summary>
    private string m_Path;
    /// <summary>
    /// ���ص����ݼ���
    /// </summary>
    private List<AssetBundleEntity> m_List = null;

    public AssetBundleDAL(string path)
    {
        m_Path = path;
        m_List = new List<AssetBundleEntity>();
    }
    /// <summary>
    /// ����XML ����
    /// </summary>
    /// <returns></returns>
    public List<AssetBundleEntity> GetList()
    {
        m_List.Clear();
        try
        {
            //��ȡXML ��������ӵ�m_List���
            XDocument xDoc = XDocument.Load(m_Path);
            XElement root = xDoc.Root;

            XElement assetBundleNode = root.Element("AssetBundle");

            IEnumerable<XElement> lst = assetBundleNode.Elements("Item");

            int index = 0;
            foreach (XElement item in lst)
            {
                AssetBundleEntity entity = new AssetBundleEntity();
                entity.Key = "Key" + ++index;
                entity.Name = item.Attribute("Name").Value;
                entity.Tag = item.Attribute("Tag").Value;
                entity.Version = item.Attribute("Version").Value.ToInt();
                entity.Size = item.Attribute("Size").Value.ToLong();
                entity.ToPath = item.Attribute("ToPath").Value;

                Debug.Log(entity.Name);
                IEnumerable<XElement> pathList = item.Elements("Path");

                foreach (XElement path in pathList)
                {
                    //entity.PathList.Add(string.Format("Assets/0" + path.Attribute("Value").Value));
                    entity.PathList.Add(Path.Combine("Assets", path.Attribute("Value").Value)); // ʹ��Path.Combine
                    //entity.PathList.Add(Path.Combine("Assets", path.Attribute("Value").Value.Replace('\\', '/')));

                }
                m_List.Add(entity);
            }
            Debug.Log($"�ɹ����� {m_List.Count} ��AssetBundle������");
        }
        catch (Exception ex)
        {
            Debug.LogError($"����XML�ļ�ʧ��: {ex.Message}");
        }

        return m_List;
    }
}
