using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetWorkSocket : MonoBehaviour
{
    #region ����
    private static NetWorkSocket instance;
    public static NetWorkSocket Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject obj = new GameObject();
                DontDestroyOnLoad(obj);
                instance = obj.GetOrCreatCompontent<NetWorkSocket>();
            }
            return instance;
        }
    }
    #endregion

    //private byte[] buffer = new byte[1024];
    /// <summary>
    /// ������Ϣ����
    /// </summary>
    private Queue<byte[]> m_SendQueue = new Queue<byte[]>();
    /// <summary>
    /// �����е�ί��
    /// </summary>
    private Action m_CheckSendueue;
    /// <summary>
    /// �ͻ���Socket
    /// </summary>
    private Socket m_Client;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDestroy()
    {
        if (m_Client != null && m_Client.Connected)
        {
            m_Client.Shutdown(SocketShutdown.Both);
            m_Client.Close();
        }
    }
    #region Content ���ӵ�socket������
    /// <summary>
    /// ���ӵ�socket������
    /// </summary>
    /// <param name="ip">ip</param>
    /// <param name="port">�˿�</param>
    public void Content(string ip, int port)
    {
        ///���socket �Ѿ����ڣ����Ҵ���������״̬����ֱ��return
        if (m_Client != null && m_Client.Connected)
        {
            return;
        }
        m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            m_Client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            m_CheckSendueue = OnCheckSendQueueCallBack;
            Debug.Log("���ӳɹ�");
        }
        catch (Exception ex)
        {

            Debug.Log("����ʧ��=" + ex.Message);
        }
    }
    #endregion

    #region OnCheckSendQueueCallBack �����е�ί�лص�
    /// <summary>
    /// �����е�ί�лص�
    /// </summary>
    private void OnCheckSendQueueCallBack()
    {
        lock (m_SendQueue)
        {
            //��������������ݰ� �������ݰ�
            if (m_SendQueue.Count > 0)
            {
                //�������ݰ�
                Send(m_SendQueue.Dequeue());
            }
        }
    }
    #endregion


    #region ��װ���ݰ�
    /// <summary>
    /// ��װ���ݰ�
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private byte[] MakeBuffer(byte[] data)
    {
        byte[] retBuffer = null;
        using (MMO_MemoryStrean ms = new MMO_MemoryStrean())
        {
            ms.WriteUShort((short)data.Length);
            ms.Write(data, 0, data.Length);
            retBuffer = ms.ToArray();

        }
        return retBuffer;
    }
    #endregion

    #region SendMsg ������Ϣ ����Ϣ���뵽����
    /// <summary>
    /// ������Ϣ
    /// </summary>
    /// <param name="buffer"></param>
    public void SendMsg(byte[] buffer)
    {
        //�õ���װ������ݰ�
        byte[] sendBuffer = MakeBuffer(buffer);
        lock (m_SendQueue)
        {
            ///�����ݰ��������
            m_SendQueue.Enqueue(sendBuffer);
            ///����ί�� (ִ��ί��)
            m_CheckSendueue.BeginInvoke(null, null);
        }
    }
    #endregion

    #region Send �����������ݰ���������
    /// <summary>
    /// �����������ݰ���������
    /// </summary>
    /// <param name="buffer"></param>
    private void Send(byte[] buffer)
    {
        m_Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Client);
    }
    #endregion

    #region SendCallBack �������ݰ��Ļص�
    /// <summary>
    /// �������ݰ��Ļص�
    /// </summary>
    /// <param name="ar"></param>
    private void SendCallBack(IAsyncResult ar)
    {
        m_Client.EndSend(ar);
        //����������
        OnCheckSendQueueCallBack();
    }
    #endregion



}
