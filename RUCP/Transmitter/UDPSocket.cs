﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace RUCP.Transmitter
{
    internal class UDPSocket : ISocket
    {
        private Socket m_socket = null;
     //   private bool connection = false;
        private Object m_locker = new Object();

        public int AvailableBytes => m_socket.Available;



        //  private static UdpClient udpClient;

        private UDPSocket(int receiveBufferSize, int sendBufferSize, int localPort) 
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            if(receiveBufferSize > 0) m_socket.ReceiveBufferSize = 3_145_728;
            if(sendBufferSize > 0) m_socket.SendBufferSize = 3_145_728;
            IPEndPoint localIP = new IPEndPoint(IPAddress.Any, localPort);
            m_socket.Bind(localIP);
        }
        internal static UDPSocket CreateSocket(int receiveBufferSize = 0, int sendBufferSize = 0, int localPort = 0)//int ReceiveBufferSize, int SendBufferSize, 
        {
            UDPSocket udp = new UDPSocket(receiveBufferSize, sendBufferSize, localPort);
          

            return udp;
        }

        public void Connect(IPEndPoint iPEndPoint)
        {
           m_socket.Connect(iPEndPoint);
          //  connection = true;
        }

        public void SendTo(Packet packet, IPEndPoint remoteAdress)
        {
           // lock (m_locker)
            {
                if (m_socket.Connected) {
                   int sentBytes = m_socket.Send(packet.Data, packet.Length, SocketFlags.None);
                   if (sentBytes != packet.Length) throw new Exception("Failed to send packet");
                }
                else {
                    int sentBytes = m_socket.SendTo(packet.Data, packet.Length, SocketFlags.None, remoteAdress);
                    if (sentBytes != packet.Length) throw new Exception("Failed to send packet");
                }
            }
        }
        public void Send(Packet packet)
        {
           // lock (m_locker)
            {

              //  m_socket.Send(packet.Data, packet.Length, SocketFlags.None);
                SendTo(packet, packet.Client.RemoteAddress);
            }
        }
        public void SendTo(byte[] data, int size, IPEndPoint remoteAdress)
        {
           // lock (m_locker)
            {
                m_socket.SendTo(data, size, SocketFlags.None, remoteAdress);
            }
        }
        public int ReceiveFrom(byte[] buffer, ref EndPoint endPoint)
        { 
            return m_socket.ReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref endPoint);
        }


        public void Close()
        {
            m_socket?.Close();
            m_socket = null;
        }
    }
}
