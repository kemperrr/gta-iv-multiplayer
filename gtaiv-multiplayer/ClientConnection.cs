﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace gtaiv_multiplayer
{
    class ClientConnection
    {
        TcpClient connection;
        //NetworkStream stream;
        byte[] buffer;

        public delegate void onUpdateDataDelegate(multiplayer_sdk.UpdateDataStruct data);
        public delegate void onDisconnectDelegate();
        public delegate void onConnectDelegate(string nick);

        public event onUpdateDataDelegate onUpdateData;
        public event onDisconnectDelegate onDisconnect;
        public event onConnectDelegate onConnect;

        public ClientConnection(TcpClient client)
        {
            connection = client;
            buffer = new byte[512];
            streamBegin();
            //stream = connection.GetStream();
        }

        public void startReceiving()
        {
            connection.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
        }

        byte[] tempbuf;

        public void streamBegin()
        {
            tempbuf = new byte[0];
        }

        public void streamFlush()
        {
            connection.Client.Send(tempbuf, tempbuf.Length, SocketFlags.None);
            streamBegin();
        }


        public byte[] appendBytes(byte[] byte1, byte[] byte2)
        {
            var list = byte1.ToList();
            list.AddRange(byte2);
            return list.ToArray();
        }


        public void streamReadString(byte[] buffer)
        {
            //streamWrite(buffer.Length);
            //stream.Write(buffer, 0, buffer.Length);
        }

        public void streamWrite(byte[] buffer)
        {
            tempbuf = appendBytes(tempbuf, buffer);
        }
        public void streamWrite(List<byte> buffer)
        {
            tempbuf = appendBytes(tempbuf, buffer.ToArray());
        }
        public void streamWrite(string buffer)
        {
            byte[] buf = Encoding.UTF8.GetBytes(buffer);
            tempbuf = appendBytes(tempbuf, buf);
        }
        public void streamWrite(multiplayer_sdk.UpdateDataStruct buffer)
        {
            byte[] buf = buffer.serialize();
            tempbuf = appendBytes(tempbuf, buf);
        }
        public void streamWrite(multiplayer_sdk.Commands command)
        {
            byte[] buf = BitConverter.GetBytes((ushort)command);
            tempbuf = appendBytes(tempbuf, buf);
        }
        public void streamWrite(int integer)
        {
            byte[] buf = BitConverter.GetBytes(integer);
            tempbuf = appendBytes(tempbuf, buf);
        }

        private void onReceive(IAsyncResult iar)
        {
            //try
            //{
            Console.WriteLine("receiving...");
            int count = connection.Client.EndReceive(iar);
            if (iar.IsCompleted)
            {
                if (buffer.Length > 0)
                {
                    //foreach (byte by in buffer) Console.Write(by.ToString("X") + " ");
                    switch ((multiplayer_sdk.Commands)BitConverter.ToUInt16(buffer, 0))
                    {
                        case multiplayer_sdk.Commands.Disconnect:
                            {
                                if (onDisconnect != null) onDisconnect.Invoke();
                            }
                            break;
                        case multiplayer_sdk.Commands.Connect:
                            {
                                var list = buffer.ToList();
                                int nickLength = BitConverter.ToInt32(buffer, 2);
                                if (onConnect != null) onConnect.Invoke(Encoding.UTF8.GetString(list.Skip(2 + 4).Take(nickLength).ToArray()));
                            }
                            break;
                        case multiplayer_sdk.Commands.UpdateData:
                            {
                                multiplayer_sdk.UpdateDataStruct data = multiplayer_sdk.UpdateDataStruct.unserialize(buffer, 2);
                                if (onUpdateData != null) onUpdateData.Invoke(data);
                            }
                            break;

                    }
                }
            }
            connection.Client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, onReceive, null);
            /*}
            catch (Exception e)
            {
                Console.WriteLine("Failed receive with message " + e.Message);
                throw e;
            }*/
        }

    }
}