using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Core.Database
{
	public abstract class Data : MonoBehaviour
	{
		private static FileManager fileManager;

		[NonSerialized]
		private FieldInfo[] fields;

		protected abstract string Prefix { get; }

		protected abstract string FileName { get; }

		private void Start()
		{
			if (fileManager == null)
			{
				fileManager = new FileManager();
			}
			fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
			fileManager.Load(FileName);
			Load();
		}

		public void Load()
		{
			bool changed = false;
			LoadDefault();
			List<SaveData> list = fileManager.Data[FileName];
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo fieldInfo = fields[i];
				fieldInfo.SetValue(this, FindOrAdd(list, Prefix + fieldInfo.Name, fieldInfo.GetValue(this), ref changed));
			}
			if (changed)
			{
				fileManager.Save(FileName);
			}
		}

		public void Save()
		{
			ValidateBeforeSave();
			List<SaveData> list = fileManager.Data[FileName];
			for (int i = 0; i < fields.Length; i++)
			{
				FieldInfo fieldInfo = fields[i];
				for (int j = 0; j < list.Count; j++)
				{
					SaveData saveData = list[j];
					if (saveData.Key.Equals(Prefix + fieldInfo.Name))
					{
						saveData.Data = fieldInfo.GetValue(this);
						break;
					}
				}
			}
			fileManager.Save(FileName);
		}

		private T FindOrAdd<T>(List<SaveData> list, string key, T defaultValue, ref bool changed)
		{
			SaveData saveData = null;
			for (int i = 0; i < list.Count; i++)
			{
				saveData = list[i];
				if (saveData.Key == key)
				{
					break;
				}
				saveData = null;
			}
			if (saveData == null)
			{
				SaveData saveData2 = new SaveData();
				saveData2.Key = key;
				saveData2.Data = defaultValue;
				saveData = saveData2;
				fileManager.Data[FileName].Add(saveData);
				changed = true;
			}
			return (T)saveData.Data;
		}

		protected abstract void ValidateBeforeSave();

		public abstract void LoadDefault();
	}
}
