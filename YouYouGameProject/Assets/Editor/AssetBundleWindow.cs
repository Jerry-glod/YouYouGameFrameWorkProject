using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AssetBundleWindow : EditorWindow
{
    private AssetBundleDAL dal;
    private List<AssetBundleEntity> m_List;
    private Dictionary<string, bool> m_Dic;
    private string[] arrTag = { "All", "Scene", "Role", "Effect", "Audio", "None" };
    private int tagIndex = 0;
    private string[] arrBuildTarget = { "Windows", "Android", "iOS" };
    private BuildTarget target;
    private int buildTargetIndex = 0;
    private Vector2 pos;

    private void OnEnable()
    {
        // ��̬��ȡ��ǰƽ̨
        target = EditorUserBuildSettings.activeBuildTarget;
        // ���ݵ�ǰ BuildTarget ƥ�� arrBuildTarget �е�ƽ̨����
        switch (target)
        {
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                buildTargetIndex = 0; // ��Ӧ "Windows"
                break;
            case BuildTarget.Android:
                buildTargetIndex = 1; // ��Ӧ "Android"
                break;
            case BuildTarget.iOS:
                buildTargetIndex = 2; // ��Ӧ "iOS"
                break;
            default:
                buildTargetIndex = 0; // Ĭ����Ϊ Windows
                break;
        }
        buildTargetIndex = (int)target;

        // ��ʼ��·���ͼ�������
        //string xmlPath = Application.dataPath+ @"\Editor\AssetBundle\AssetBundleConfig.xml";

        string xmlPath = Path.Combine(Application.dataPath, "Editor", "AssetBundle", "AssetBundleConfig.xml");
        dal = new AssetBundleDAL(xmlPath);
        m_List = dal.GetList();
        m_Dic = new Dictionary<string, bool>();
        for (int i = 0; i < m_List.Count; i++)
        {
            m_Dic[m_List[i].Key] = true;
        }

    }

    /// <summary>
    /// ���ƴ���
    /// </summary>
    private void OnGUI()
    {
        if (m_List == null) return;

        #region ��ť��
        GUILayout.BeginHorizontal("box");
        tagIndex = EditorGUILayout.Popup(tagIndex, arrTag, GUILayout.Width(100));
        if (GUILayout.Button("ѡ��Tag", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnSelectTagCallBack;
        }

        buildTargetIndex = EditorGUILayout.Popup(buildTargetIndex, arrBuildTarget, GUILayout.Width(100));
        if (GUILayout.Button("ѡ��Target", GUILayout.Width(100)))
        {
            EditorApplication.delayCall = OnSelectTargetCallBack;
        }

        if (GUILayout.Button("��AssetBundle��", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnAssetBundleCallBack;
        }

        if (GUILayout.Button("���AssetBundle", GUILayout.Width(200)))
        {
            EditorApplication.delayCall = OnClearAssetBundleCallBack;
        }
        EditorGUILayout.Space();
        GUILayout.EndHorizontal();
        #endregion

        GUILayout.BeginHorizontal("box");
        GUILayout.Label("����");
        GUILayout.Label("���",GUILayout.Width(100));
        GUILayout.Label("����·��", GUILayout.Width(200));
        GUILayout.Label("�汾", GUILayout.Width(100));
        GUILayout.Label("��С", GUILayout.Width(100));
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical();

        pos = EditorGUILayout.BeginScrollView(pos);

        for (int i = 0; i < m_List.Count; i++)
        {
            AssetBundleEntity entity = m_List[i];
            GUILayout.BeginHorizontal("box");

            m_Dic[entity.Key] = GUILayout.Toggle(m_Dic[entity.Key], "", GUILayout.Width(20));
            GUILayout.Label(entity.Name);
            GUILayout.Label(entity.Tag, GUILayout.Width(100));
            GUILayout.Label(entity.ToPath, GUILayout.Width(200));
            GUILayout.Label(entity.Version.ToString(), GUILayout.Width(100));
            GUILayout.Label(entity.Size.ToString(), GUILayout.Width(100));
            GUILayout.EndHorizontal();
            foreach (string path in entity.PathList)
            {
                GUILayout.BeginHorizontal("box");
                GUILayout.Space(40);
                GUILayout.Label(path);
                GUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        GUILayout.EndVertical();


    }



    /// <summary>
    /// ѡ��Tag�ص�
    /// </summary>
    private void OnSelectTagCallBack()
    {
        switch (tagIndex)
        {
            case 0://ȫѡ
                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = true;
                }
                break;
            case 1://Scene
                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = entity.Tag.Equals("Scene", StringComparison.CurrentCultureIgnoreCase);
                }
                break;
            case 2://Role
                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = entity.Tag.Equals("Role", StringComparison.CurrentCultureIgnoreCase);
                }
                break;
            case 3://Effect
                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = entity.Tag.Equals("Effect", StringComparison.CurrentCultureIgnoreCase);
                }
                break;
            case 4://Audio
                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = entity.Tag.Equals("Audio", StringComparison.CurrentCultureIgnoreCase);
                }
                break;
            case 5://None
                foreach (AssetBundleEntity entity in m_List)
                {
                    m_Dic[entity.Key] = false;
                }
                break;
        }
        Debug.LogFormat("��ǰѡ�е�Tag��{0}", arrTag[tagIndex]);
    }
    /// <summary>
    /// ѡ��Target�ص�
    /// </summary>
    private void OnSelectTargetCallBack()
    {
        switch (buildTargetIndex)
        {
            case 0:
                target = BuildTarget.StandaloneWindows;
                break;
            case 1:
                target = BuildTarget.Android;
                break;
            case 2:
                target = BuildTarget.iOS;
                break;
        }
        Debug.LogFormat("��ǰѡ�е�buildTarget��{0}", arrBuildTarget[tagIndex]);

    }
    /// <summary>
    /// ����ص�
    /// </summary>
    private void OnAssetBundleCallBack()
    {
        List<AssetBundleEntity> lstNeedBuild = new List<AssetBundleEntity>();
        foreach (AssetBundleEntity entity in m_List)
        {
            if (m_Dic[entity.Key])
            {
                lstNeedBuild.Add(entity);
            }
        }
        for (int i = 0; i < lstNeedBuild.Count; i++)
        {
            Debug.LogFormat("���ڴ��{0}/{1}", i + 1, lstNeedBuild.Count);
            BuildAssetBundle(lstNeedBuild[i]);
        }
        Debug.Log("������");
    }
    /// <summary>
    /// �������
    /// </summary>
    /// <param name="entity"></param>
    private void BuildAssetBundle(AssetBundleEntity entity)
    {
        AssetBundleBuild[] arrBuild = new AssetBundleBuild[1];
        AssetBundleBuild build = new AssetBundleBuild();
        //����
        build.assetBundleName = $"{entity.Name}/{(entity.Tag.Equals("Scene", StringComparison.CurrentCultureIgnoreCase) ? "unity3d" : "assetbundle")}";
        //��Դ·��
        build.assetNames = entity.PathList.ToArray();
        arrBuild[0] = build;
        string platformSubDir = arrBuildTarget[buildTargetIndex];
        string basePath = Path.Combine(Application.dataPath, "..", "AssetBundle", platformSubDir);
        string toPath = Path.Combine(basePath, entity.ToPath.TrimStart('/'));
        //���Ŀ��·�������� �򴴽�
        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }
        BuildPipeline.BuildAssetBundles(toPath, arrBuild, BuildAssetBundleOptions.None, target);
    }
    /// <summary>
    /// ���AssetBundle�ص�
    /// </summary>
    private void OnClearAssetBundleCallBack()
    {
        if (buildTargetIndex < 0 || buildTargetIndex >= arrBuildTarget.Length)
        {
            Debug.LogError("ƽ̨����Խ�磡");
            return;
        }

        string platformSubDir = arrBuildTarget[buildTargetIndex];
        string path = Path.Combine(Application.dataPath, "..", "AssetBundle", platformSubDir);
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
        Debug.Log("�������");
    }
}
