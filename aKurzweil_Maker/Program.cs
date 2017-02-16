/*
 * Created by SharpDevelop.
 * User: aZuZu
 * Date: 8.6.2016.
 * Time: 13:07
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Globalization;
using Microsoft.SqlServer.Server;

namespace aKurzweil_Maker
{
	class Program
	{
		struct KRZ_Data
		{
			private static byte[] Magic = new byte[4];
			private Int32 Header_Data_Length;
			private static byte[] KRZ_Data0 = new byte[24];
			private static byte[] Object_Info = new byte[6];
			private Int16 Data_Start;
			private Int16 Number_Of_Objects;
			private static byte[] Collection_Name = new byte[8];
			private Int16 Collection_Size;
			private Int16 Collection_First_Object;
			private static byte[] UnKnow_Data = new byte[4];
			
			
			public byte[] kd_Magic
			{
				get {
					return Magic;
				}
				set {
					Magic = value;
				}
			}
			public Int32 kd_Header_Data_Length
			{
				get {
					return Header_Data_Length;
				}
				set {
					Header_Data_Length = value;
				}
				
			}
			public byte[] kd_KRZ_Data0
			{
				get {
					return KRZ_Data0;
				}
				set {
					KRZ_Data0 = value;
				}
			}
			public byte[] kd_Object_Info
			{
				get {
					return Object_Info;
				}
				set {
					Object_Info = value;
				}
			}
			public Int16 kd_Data_Start
			{
				get {
					return Data_Start;
				}
				set {
					Data_Start = value;
				}
			}
			public Int16 kd_Number_Of_Objects
			{
				get {
					return Number_Of_Objects;
				}
				set {
					Number_Of_Objects = value;
				}
			}
			public byte[] kd_Collection_Name
			{
				get {
					return Collection_Name;
				}
				set {
					Collection_Name = value;
				}
			}
			public Int16 kd_Collection_Size
			{
				get {
					return Collection_Size;
				}
				set {
					Collection_Size = value;
				}

			}
			public Int16 kd_Collection_First_Object
			{
				get {
					return Collection_First_Object;
				}
				set {
					Collection_First_Object = value;
				}
			}
			public byte[] kd_UnKnow_Data
			{
				get {
					return UnKnow_Data;
				}
				set {
					UnKnow_Data = value;
				}
			}
			
		}
		
		public static BinaryReader br;
		public static BinaryWriter bw;
		private static string ByteToHex(byte[] In_Bytes)
		{
			return BitConverter.ToString(In_Bytes).Replace("-", string.Empty);
		}
		private static byte[] HexToByte(string In_String)
		{
			byte[] Bytes = new byte[In_String.Length / 2];
			for (int index = 0; index < Bytes.Length; index++)
			{
				Bytes[index] = byte.Parse(In_String.Substring(index * 2, 2).ToString(), NumberStyles.HexNumber);
			}
			return Bytes;
		}
		private static byte[] String2ByteArray( string In_String )
		{
			Encoding ASCII = new ASCIIEncoding();
			return ASCII.GetBytes(In_String);
		}
		private static string ByteArray2String( byte[] In_Bytes )
		{
			Encoding ASCII = new ASCIIEncoding();
			return ASCII.GetString(In_Bytes);
		}
		private static int ByteIndexOf(byte[] Where, byte[] What, int Start)
		{
			bool matched = false;
			for (int index = Start; index <= Where.Length - What.Length; ++index)
			{
				matched = true;
				for (int subIndex = 0; subIndex < What.Length; ++subIndex)
				{
					if (What[subIndex] != Where[index + subIndex])
					{
						matched = false;
						break;
					}
				}
				if (matched)
				{
					return index;
				}
			}
			return -1;
		}
		private static Int16 ByteArray2Integer16(byte[] In_Bytes)
		{
			Array.Reverse(In_Bytes);
			return BitConverter.ToInt16(In_Bytes, 0);
		}
		private static Int32 ByteArray2Integer32(byte[] In_Bytes)
		{
			Array.Reverse(In_Bytes);
			return BitConverter.ToInt32(In_Bytes, 0);
		}
		private static byte[] Int2ByteArray(int In_Number)
		{
			return BitConverter.GetBytes(In_Number);
		}
		
		public static void Main(string[] args)
		{
			KRZ_Data KD = new KRZ_Data();
			br = new BinaryReader(File.OpenRead("Alto_AB.KRZ"));
			//bw = new BinaryWriter(File.OpenWrite("out\\Alto_AB.KRZ"));
			KD.kd_Magic = br.ReadBytes(4);
			KD.kd_Header_Data_Length = ByteArray2Integer32(br.ReadBytes(4));
			KD.kd_KRZ_Data0 = br.ReadBytes(24);
			KD.kd_Object_Info = br.ReadBytes(6);
			KD.kd_Data_Start = ByteArray2Integer16(br.ReadBytes(2));
			KD.kd_Number_Of_Objects = ByteArray2Integer16(br.ReadBytes(2));
			KD.kd_Collection_Name = br.ReadBytes(8);
			KD.kd_Collection_Size = ByteArray2Integer16(br.ReadBytes(2));
			KD.kd_Collection_First_Object = ByteArray2Integer16(br.ReadBytes(2));
			KD.kd_UnKnow_Data = br.ReadBytes(4);
			                     
			Console.WriteLine("Magic: " + ByteArray2String(KD.kd_Magic));
			Console.WriteLine("Header data length: " + KD.kd_Header_Data_Length.ToString());
			Console.WriteLine("KRZ data0: " + ByteToHex(KD.kd_KRZ_Data0));
			Console.WriteLine("Object info: " + ByteToHex(KD.kd_Object_Info));
			Console.WriteLine("Data start: " + KD.kd_Data_Start.ToString());
			Console.WriteLine("Number of objects: " + KD.kd_Number_Of_Objects.ToString());
			Console.WriteLine("Collection name: " + ByteArray2String(KD.kd_Collection_Name.ToArray().Take(8).ToArray()));
			Console.WriteLine("Collection size: " + KD.kd_Collection_Size.ToString());
			Console.WriteLine("First object offset: " + KD.kd_Collection_First_Object.ToString());
			Console.WriteLine("UnKnow data: " + ByteToHex(KD.kd_UnKnow_Data));
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}