// André Betz 2004
// http://www.andrebetz.de
using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace IPUpLoad
{
	/// <summary>
	/// Summary description for MySockets.
	/// </summary>
	public class MySockets
	{
		private Socket m_SocketConn = null;
		private string m_HostName;
		private int m_Port;
		private int m_Timeout = 10000;
		private int BufferSize = 4096;
		private int m_MaxConnections = 5;
		private string m_LastError;

		public static string LocalIPAdress
		{
			get
			{
				string LocalName = Dns.GetHostName();
				IPHostEntry RemoteHost = Dns.Resolve(LocalName);
				IPAddress[] TCPAddress = RemoteHost.AddressList;
				return TCPAddress[0].ToString();
			}
		}

		public string LastError
		{
			get
			{
				string Error = m_LastError;
				m_LastError = null;
				return Error;
			}
		}

		public MySockets(string HostName, int Port)
		{
			m_HostName = HostName;
			m_Port = Port;
		}
		
		public MySockets(Socket socken)
		{
			m_SocketConn = socken;
		}

		public bool Connect()
		{
			try
			{
				IPHostEntry RemoteHost = Dns.Resolve(m_HostName);
				IPAddress[] TCPAddress = RemoteHost.AddressList;
				IPEndPoint EndPoint = new IPEndPoint(TCPAddress[0], m_Port);
				m_SocketConn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				m_SocketConn.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.ReceiveTimeout, m_Timeout);
				m_SocketConn.SetSocketOption(SocketOptionLevel.Socket,SocketOptionName.SendTimeout, m_Timeout);	
				m_SocketConn.Connect(EndPoint);
			}
			catch(Exception e)
			{
				m_LastError = e.ToString();
				return false;
			}
			return true;
		}

		public NetworkStream MakeStream(Socket socken)
		{
			try
			{
				NetworkStream NetStrm = new NetworkStream(socken, false);
				return NetStrm;
			}
			catch(Exception e)
			{
				m_LastError = e.ToString();
				return null;
			}
		}

		public void EndConnection()
		{
			m_SocketConn.Close();
		}

		public bool Listen()
		{			
			try
			{
				m_SocketConn = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
				IPHostEntry LocalHost = Dns.Resolve(Dns.GetHostName());
				IPEndPoint LocalPoint = new IPEndPoint(LocalHost.AddressList[0], 0);
				m_SocketConn.Bind(LocalPoint);
				m_SocketConn.Listen(m_MaxConnections);
				return true;
			}
			catch(Exception e)
			{
				m_LastError = e.ToString();
				return false;
			}
		}
		
		public MySockets WaitForConnection()
		{
			try
			{
				Socket socken = m_SocketConn.Accept();
				MySockets ConnSock = new MySockets(socken);
				return ConnSock;
			}
			catch(Exception e)
			{
				m_LastError = e.ToString();
				return null;
			}
		}

		public byte[] Read()
		{
			try
			{
				BinaryReader BinReader = new BinaryReader(MakeStream(m_SocketConn));
				MemoryStream memWriter = new MemoryStream();

				byte[] Buffer = new byte[BufferSize];
				int count;
				int Pos = 0;
				while ((count = BinReader.Read(Buffer, 0, Buffer.Length)) > 0)
				{
					memWriter.Write(Buffer,Pos,count);
					Pos += count;
					if(count<BufferSize)
					{
						break;
					}
				}
				memWriter.Flush();
				BinReader.Close();
				return memWriter.ToArray();
			}
			catch(Exception e)
			{
				m_LastError = e.ToString();
				return null;
			}
		}

		public bool Send(byte[] Data)
		{
			try
			{
				BinaryWriter BinWriter = new BinaryWriter(MakeStream(m_SocketConn));
				MemoryStream memReader = new MemoryStream(Data);

				byte[] Buffer = new byte[BufferSize];
				int count = 0;

				while ((count = memReader.Read(Buffer, 0, Buffer.Length)) > 0) 
				{
					BinWriter.Write(Buffer, 0, count);
					if(count<BufferSize)
					{
						break;
					}
				}
				BinWriter.Flush();
				BinWriter.Close();
				memReader.Close();
			}
			catch(Exception e)
			{
				m_LastError = e.ToString();
				return false;
			}
			return true;
		}

	}
}
