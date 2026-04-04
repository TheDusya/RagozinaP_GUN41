using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;

namespace DefaultNamespace
{
	public class PositionSaver : MonoBehaviour
	{
		[Serializable]
		public struct Data
		{
			public Vector3 Position;
			public float Time;
		}

		[SerializeField, ReadOnly, Tooltip("This field can be filled through a context menu in Inspector via a \"Create File\" command.")]
		private TextAsset _json;

		[field: SerializeField, HideInInspector]
		public List<Data> Records { get; private set; }

        private void Awake()
		{
				//todo comment: Что будет, если в теле этого условия не сделать выход из метода?
			//Null reference exception.
			if (_json == null)
			{
				gameObject.SetActive(false);
				Debug.LogError("Please, create TextAsset and add in field _json");
				return;
			}
			
			JsonUtility.FromJsonOverwrite(_json.text, this);
				//todo comment: Для чего нужна эта проверка (что она позволяет избежать)?
			//избежать переписывания Records, загруженных из json.
			if (Records == null)
				Records = new List<Data>(10);
		}

		private void OnDrawGizmos()
		{
				//todo comment: Зачем нужны эти проверки (что они позволляют избежать)?
            //избежать null reference exception и выхода за границы массива.
            if (Records == null || Records.Count == 0) return;
			var data = Records;
			var prev = data[0].Position;
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(prev, 0.3f);
				//todo comment: Почему итерация начинается не с нулевого элемента?
            //потому что сфера для data[0].Position нарисована вне цикла (чтобы всегда можно было соединить элемент с предыдущим элементом)
            for (int i = 1; i < data.Count; i++)
			{
				var curr = data[i].Position;
				Gizmos.DrawWireSphere(curr, 0.3f);
				Gizmos.DrawLine(prev, curr);
				prev = curr;
			}
		}
		
#if UNITY_EDITOR
		[ContextMenu("Create File")]
		private void CreateFile()
		{
				//todo comment: Что происходит в этой строке?
			//вычисляется нужный путь к файлу Path.txt в папке проекта, создаётся файл и FileStream
            var stream = File.Create(Path.Combine(Application.dataPath, "Path.txt"));
				//todo comment: Подумайте для чего нужна эта строка? (а потом проверьте догадку, закомментировав)
            //мы освобождаем FileStream, чтобы после создания другие функции смогли работать с файлом
            stream.Dispose();
			UnityEditor.AssetDatabase.Refresh();
			//В Unity можно искать объекты по их типу, для этого используется префикс "t:"
			//После нахождения, Юнити возвращает массив гуидов (которые в мета-файлах задаются, например)
			var guids = UnityEditor.AssetDatabase.FindAssets("t:TextAsset");
			foreach (var guid in guids)
			{
				//Этой командой можно получить путь к ассету через его гуид
				var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
				//Этой командой можно загрузить сам ассет
				var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<TextAsset>(path);
					//todo comment: Для чего нужны эти проверки?
				//мы ищем необходимый текстовый ассет.
				//Ответный вопрос: не является ли потенциальной проблемой то, что мы не рассматриваем другие форматы?
				//Если есть ассет Path.html, он перезапишется.
				if(asset != null && asset.name == "Path")
				{
					_json = asset;
					UnityEditor.EditorUtility.SetDirty(this);
					UnityEditor.AssetDatabase.SaveAssets();
					UnityEditor.AssetDatabase.Refresh();
						//todo comment: Почему мы здесь выходим, а не продолжаем итерироваться?
					//нашли то, что хотели
					return;
				}
			}
		}

		private void OnDestroy()
		{
			//todo logic...
			if (_json == null)
				return;
            string serializedThis = JsonUtility.ToJson(this);
			var path = UnityEditor.AssetDatabase.GetAssetPath(_json);
			path = Path.Combine(Application.dataPath.Replace("Assets", ""), path);
			File.WriteAllText(path, serializedThis);
            UnityEditor.EditorUtility.SetDirty(_json);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }
#endif
	}
}