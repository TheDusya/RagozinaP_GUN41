using System;
using UnityEngine;

namespace DefaultNamespace
{
	[RequireComponent(typeof(PositionSaver))]
	public class ReplayMover : MonoBehaviour
	{
		private PositionSaver _save;

		private int _index;
		private PositionSaver.Data _prev;
		private float _duration;

		private void Start()
		{
				////todo comment: зачем нужны эти проверки?
            //чтобы обнаружить ситуацию, когда нет ссылки на PositionSaver или в нём нет записей
            if (!TryGetComponent(out _save) || _save.Records.Count == 0)
			{
				Debug.LogError("Records incorrect value", this);
					//todo comment: Для чего выключается этот компонент?
                //чтобы всё не сломалось на Update, когда функция ссылается на _save и _save.Records[_index] (+все равно нечего воспризводить)
                enabled = false;
			}
		}

		private void Update()
		{
			var curr = _save.Records[_index];
				//todo comment: Что проверяет это условие (с какой целью)? 
			//проверяет, пришло ли время переходить на следующую точку
			if (Time.time > curr.Time)
			{
				_prev = curr;
				_index++;
					//todo comment: Для чего нужна эта проверка?
                //проверка на достижение конца списка _save.Records
                if (_index >= _save.Records.Count)
				{
					enabled = false;
					Debug.Log($"<b>{name}</b> finished", this);
				}
			}
				//todo comment: Для чего производятся эти вычисления (как в дальнейшем они применяются)?
            //вычисляется отношение промежутков "текущее время - начало текущего отрезка времени (между точками)" и
            //"начало текущего отрезка времени - конец текущего отрезка времени".
			//В дальнейшем в соответствии с этим соотношением высчитывается перемещение.
            var delta = (Time.time - _prev.Time) / (curr.Time - _prev.Time);
				//todo comment: Зачем нужна эта проверка?
            //на случай, если curr.Time - _prev.Time == 0 == Time.time - _prev.Time
            if (float.IsNaN(delta)) delta = 0f;
				//todo comment: Опишите, что происходит в этой строчке так подробно, насколько это возможно
            //вычисляется точка между _prev.Position и curr.Position, соотношение расстояний от которой
            //до _prev.Position и до curr.Position пропорционально соотношению промежутков во времени от момента
            //вызова "update" до _prev.Time и до curr.Time, чтобы обеспечить правильное перемещение.
            //используется (в векторном виде) формула prev + (curr - prev) * delta
            transform.position = Vector3.Lerp(_prev.Position, curr.Position, delta);
		}
	}
}