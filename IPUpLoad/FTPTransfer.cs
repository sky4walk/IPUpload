// André Betz 2004
// http://www.andrebetz.de
using System;

namespace IPUpLoad
{
	/// <summary>
	/// Summary description for FTPTransfer.
	/// </summary>
	public class FTPTransfer
	{
		private MySockets m_mySocks = null;
		private int m_FTPPort = 21;
		char[] m_SpaceSeperators = {' '};

		public FTPTransfer(string Adresse)
		{
			m_mySocks = new MySockets(Adresse,m_FTPPort);
		}

		public bool Connect()
		{
			m_mySocks.Connect();
			byte[] Data = m_mySocks.Read();			
			return FTPCode(Data,"220");
		}

		public string Byte2String(byte[] bArr)
		{
			string Text = "";
			try
			{
				for(int i=0;i<bArr.Length;i++)
				{
					Text += (char)bArr[i];
				}
			}
			catch
			{
				return null;
			}
			return Text;
		}

		public byte[] String2Byte(string Text)
		{
			Text += "\r\n";
			byte[] bArr = new byte[Text.Length];
			for(int i=0;i<Text.Length;i++)
			{
				bArr[i] = (byte)((Text.ToCharArray())[i]);
			}
			return bArr;
		}

		public bool FTPCode(byte[] Data,string Code)
		{
			try
			{
				string Text = Byte2String(Data);
				string[] substrings = Text.Split(m_SpaceSeperators);
				if(substrings!=null&&substrings[0].Equals(Code))
				{
					return true;
				}
			}
			catch
			{
				return false;
			}
			return false;
		}

		public bool EnterUser(string Username,string Password)
		{
			byte[] Data;
			bool res;

			res = m_mySocks.Send(String2Byte("USER " + Username));
			Data = m_mySocks.Read();
			res = FTPCode(Data,"331");

			res = m_mySocks.Send(String2Byte("PASS " + Password));
			Data = m_mySocks.Read();
			res = FTPCode(Data,"230");

			return res;
		}

		public bool Chdir(string dir)
		{			
			byte[] Data;
			bool res;

			res = m_mySocks.Send(String2Byte("CWD " + dir));
			Data = m_mySocks.Read();
			res = FTPCode(Data,"250");

			return res;
		}

		public bool Delete(string FileName)
		{	
			byte[] Data;
			bool res;

			res = m_mySocks.Send(String2Byte("DELE " + FileName));
			Data = m_mySocks.Read();
			res = FTPCode(Data,"250");

			return res;
		}

		public bool Quit()
		{	
			byte[] Data;
			bool res;

			res = m_mySocks.Send(String2Byte("QUIT"));
			Data = m_mySocks.Read();
			res = FTPCode(Data,"221");
			if(!res)
			{
				res = FTPCode(Data,"226");
			}

			m_mySocks.EndConnection();

			return res;
		}

		public bool Put(string FileName,byte[] FileData)
		{
			byte[] Data;
			bool res;

			res = SetTransTypeBin();

			MySockets TransConn = SetPassiveMode();
			if(TransConn==null)
			{
				return false;
			}

			res = TransConn.Connect();
			if(!res)
			{
				return false;
			}
			res = m_mySocks.Send(String2Byte("STOR " + FileName));
			Data = m_mySocks.Read();
			res = FTPCode(Data,"150");
			if(!res)
			{
				res = FTPCode(Data,"125");
				if(!res)
				{
					return false;
				}
			}

			res = TransConn.Send(FileData);
			TransConn.EndConnection();

			res = ValidateTransfer();

			return res;
		}

		public MySockets SetPassiveMode()
		{
			byte[] Data;
			bool res;

			res = m_mySocks.Send(String2Byte("PASV"));
			Data = m_mySocks.Read();
			res = FTPCode(Data,"227");

			try
			{
				string[] ConnDat = GetIPAdressFromPASVRes(Byte2String(Data));
				if(ConnDat==null)
				{
					return null;
				}

				int Port = Convert.ToInt32(ConnDat[1]);


				return new MySockets(ConnDat[0],Port);
			}
			catch
			{
				return null;
			};
		}

		private string[] GetIPAdressFromPASVRes(string Text)
		{
			string[] ConnectionData = new string[2];
			try
			{
				string IPAdress = "";
				string[] Results = Text.Split(m_SpaceSeperators);
				string RawIP = Results[Results.Length-1];
				char[] Seps = {',','(',')'};
				string[] Numbers = RawIP.Split(Seps);

				int Start = 0;			
				if(Numbers[0].Length==0)
				{
					Start++;
				}

				IPAdress = Numbers[Start]+"."+Numbers[Start+1]+"."+Numbers[Start+2]+"."+Numbers[Start+3];
				
				int pn1 = Convert.ToInt32(Numbers[Start+4]);
				int pn2 = Convert.ToInt32(Numbers[Start+5]);
				int Port = (pn1<<8)+pn2;
				ConnectionData[0] = IPAdress;
				ConnectionData[1] = Port.ToString();
			}
			catch
			{
				return null;
			}
			return ConnectionData;
		}

		public bool ValidateTransfer()
		{
			byte[] Data;
			bool res;

			Data = m_mySocks.Read();
			res = FTPCode(Data,"226");
			if(!res)
			{
				res = FTPCode(Data,"250");
			}
			return res;
		}

		public bool SetTransTypeBin()
		{
			byte[] Data;
			bool res;

			string TransTypeBinary = "I";
			res = m_mySocks.Send(String2Byte("TYPE " + TransTypeBinary));
			Data = m_mySocks.Read();
			res = FTPCode(Data,"200");
			return res;
		}

	}
}
