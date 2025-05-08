using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SettingsWindow : EditorWindow
{
    private List<MacorItem> m_List = new List<MacorItem>();
    private Dictionary<string, bool> m_Dic = new Dictionary<string, bool>();
    private string m_Macro = null;

    private void OnEnable()
    {
        // ��ʼ���б���ֵ�
        m_List = new List<MacorItem>
        {
            new MacorItem { Name = "Debug_MODEL", DisPlayName = "����ģʽ", IsDebug = true, IsRelease = false },
            new MacorItem { Name = "Debug_LOG", DisPlayName = "��ӡ��־", IsDebug = true, IsRelease = false },
            new MacorItem { Name = "STAT_TD", DisPlayName = "����ͳ��", IsDebug = false, IsRelease = true }
        };
        m_Dic = new Dictionary<string, bool>();

        // ��ȫ��ȡ�궨��
        m_Macro = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
        // ��ʼ���ֵ�״̬
        foreach (var item in m_List)
        {
            bool isActive = !string.IsNullOrEmpty(m_Macro) && m_Macro.Contains(item.Name);
            m_Dic[item.Name] = isActive;
        }
    }
   
    private void OnGUI()
    {
        for (int i = 0; i < m_List.Count; i++)
        {
            EditorGUILayout.BeginHorizontal("box");
            m_Dic[m_List[i].Name] = GUILayout.Toggle(m_Dic[m_List[i].Name], m_List[i].DisPlayName);
            EditorGUILayout.EndHorizontal();

        }
        //����һ��
        EditorGUILayout.BeginHorizontal("box");
        if (GUILayout.Button("����", GUILayout.Width(100)))
        {
            SaveMacro();
        }
  

        if (GUILayout.Button("����ģʽ", GUILayout.Width(100)))
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                m_Dic[m_List[i].Name] = m_List[i].IsDebug;
            }
            SaveMacro();
        }
        if (GUILayout.Button("����ģʽ", GUILayout.Width(100)))
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                m_Dic[m_List[i].Name] = m_List[i].IsDebug;
            }
            SaveMacro();
        }
        EditorGUILayout.EndHorizontal();
    }

    private void SaveMacro()
    {
        m_Macro = string.Empty;
        foreach (var item in m_Dic)
        {
            if (item.Value)
            {
                m_Macro += string.Format("{0};",item.Key);
            }
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, m_Macro);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, m_Macro);
        PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, m_Macro);

    }

    public class MacorItem
    {
        /// <summary>
        /// ����
        /// </summary>
        public string Name;
        /// <summary>
        /// ��ʾ������
        /// </summary>
        public string DisPlayName;
        /// <summary>
        /// �Ƿ����
        /// </summary>
        public bool IsDebug;
        /// <summary>
        /// �Ƿ񷢲���
        /// </summary>
        public bool IsRelease;
    }
}
