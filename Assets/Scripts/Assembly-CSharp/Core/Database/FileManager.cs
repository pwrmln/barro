using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using UnityEngine;

namespace Core.Database
{
	internal class FileManager
	{
		private Dictionary<string, List<SaveData>> data;

		private FileStream fileStream;

		private string savePath;

		private string dataFilePath;

		private byte[] key;

		private byte[] inicializationVector;

		public Dictionary<string, List<SaveData>> Data
		{
			get
			{
				return data;
			}
			private set
			{
				data = value;
			}
		}

		public FileManager()
		{
			key = new byte[16]
			{
				5, 2, 18, 4, 7, 6, 7, 8, 9, 17,
				17, 18, 19, 8, 21, 7
			};
			inicializationVector = new byte[16]
			{
				2, 2, 3, 5, 1, 9, 17, 0, 9, 16,
				17, 77, 19, 20, 5, 22
			};
			savePath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Save");
			dataFilePath = Path.ChangeExtension(Path.Combine(savePath, "{0}"), "scj");
			if (!Directory.Exists(savePath))
			{
				Directory.CreateDirectory(savePath);
			}
			data = new Dictionary<string, List<SaveData>>();
		}

		internal bool Load(string name)
		{
			string text = string.Format(dataFilePath, name);
			if (!data.ContainsKey(name))
			{
				data.Add(name, new List<SaveData>());
			}
			CryptoStream cryptoStream = null;
			try
			{
				if (!File.Exists(text))
				{
					Save(name);
				}
				fileStream = new FileStream(text, FileMode.OpenOrCreate);
				cryptoStream = new CryptoStream(fileStream, new RijndaelManaged().CreateDecryptor(key, inicializationVector), CryptoStreamMode.Read);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
				data[name] = (List<SaveData>)binaryFormatter.Deserialize(cryptoStream);
				return true;
			}
			catch
			{
				if (cryptoStream != null)
				{
					cryptoStream.Close();
				}
				if (fileStream != null)
				{
					fileStream.Close();
				}
				File.Move(text, text + "_" + DateTime.Now.ToString("MMddmmss") + ".scbkp");
				data[name] = new List<SaveData>();
				return false;
			}
			finally
			{
				if (cryptoStream != null)
				{
					cryptoStream.Close();
				}
				if (fileStream != null)
				{
					fileStream.Close();
				}
				if (data[name] == null)
				{
					data[name] = new List<SaveData>();
				}
			}
		}

		internal bool Save(string name)
		{
			string text = string.Format(dataFilePath, name);
			if (!data.ContainsKey(name))
			{
				data.Add(name, new List<SaveData>());
			}
			CryptoStream cryptoStream = null;
			try
			{
				fileStream = new FileStream(text, FileMode.Create);
				cryptoStream = new CryptoStream(fileStream, new RijndaelManaged().CreateEncryptor(key, inicializationVector), CryptoStreamMode.Write);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				binaryFormatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
				binaryFormatter.Serialize(cryptoStream, data[name]);
				return true;
			}
			catch
			{
				if (cryptoStream != null)
				{
					cryptoStream.Close();
				}
				if (fileStream != null)
				{
					fileStream.Close();
				}
				File.Move(text, text + "_" + DateTime.Now.ToString("MMddmmss") + ".scbkp");
				File.Delete(text);
				return false;
			}
			finally
			{
				if (cryptoStream != null)
				{
					cryptoStream.Close();
				}
				if (fileStream != null)
				{
					fileStream.Close();
				}
			}
		}
	}
}
