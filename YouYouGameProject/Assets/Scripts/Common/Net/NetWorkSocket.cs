using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class NetWorkSocket : MonoBehaviour
{
    #region 单例
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
    /// 发送消息队列
    /// </summary>
    private Queue<byte[]> m_SendQueue = new Queue<byte[]>();
    /// <summary>
    /// 检查队列的委托
    /// </summary>
    private Action m_CheckSendueue;
    /// <summary>
    /// 客户端Socket
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
    #region Content 连接到socket服务器
    /// <summary>
    /// 连接到socket服务器
    /// </summary>
    /// <param name="ip">ip</param>
    /// <param name="port">端口</param>
    public void Content(string ip, int port)
    {
        ///如果socket 已经存在，并且处于连接中状态，则直接return
        if (m_Client != null && m_Client.Connected)
        {
            return;
        }
        m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        try
        {
            m_Client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            m_CheckSendueue = OnCheckSendQueueCallBack;
            Debug.Log("连接成功");
        }
        catch (Exception ex)
        {

            Debug.Log("连接失败=" + ex.Message);
        }
    }
    #endregion

    #region OnCheckSendQueueCallBack 检查队列的委托回调
    /// <summary>
    /// 检查队列的委托回调
    /// </summary>
    private void OnCheckSendQueueCallBack()
    {
        lock (m_SendQueue)
        {
            //如果队列中有数据包 则发送数据包
            if (m_SendQueue.Count > 0)
            {
                //发送数据包
                Send(m_SendQueue.Dequeue());
            }
        }
    }
    #endregion


    #region 封装数据包
    /// <summary>
    /// 封装数据包
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

    #region SendMsg 发送消息 把消息加入到队列
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="buffer"></param>
    public void SendMsg(byte[] buffer)
    {
        //得到封装后的数据包
        byte[] sendBuffer = MakeBuffer(buffer);
        lock (m_SendQueue)
        {
            ///把数据包加入队列
            m_SendQueue.Enqueue(sendBuffer);
            ///启动委托 (执行委托)
            m_CheckSendueue.BeginInvoke(null, null);
        }
    }
    #endregion

    #region Send 真正发送数据包到服务器
    /// <summary>
    /// 真正发送数据包到服务器
    /// </summary>
    /// <param name="buffer"></param>
    private void Send(byte[] buffer)
    {
        m_Client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, m_Client);
    }
    #endregion

    #region SendCallBack 发送数据包的回调
    /// <summary>
    /// 发送数据包的回调
    /// </summary>
    /// <param name="ar"></param>
    private void SendCallBack(IAsyncResult ar)
    {
        m_Client.EndSend(ar);
        //继续检查队列
        OnCheckSendQueueCallBack();
    }
    #endregion



}
