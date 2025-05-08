using System.Collections;
using System.Collections.Generic;using UnityEngine;

public class TestMMoMemory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetWorkSocket.Instance.Content("192.168.0.111", 8080);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Send("���A");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Send("���S");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            for (int i = 0; i < 10; i++)
            {
                Send("���ѭ��" + i);
            }
        }
    }
    private void Send(string msg)
    {
        using (MMO_MemoryStrean ms = new MMO_MemoryStrean())
        {
            ms.WriteUTF8String(msg);
            NetWorkSocket.Instance.SendMsg(ms.ToArray());
        }
    }
}
