using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MoreMountains.Tools;

namespace MoreMountains.Tools
{
	public class PropertyProgressBar : MonoBehaviour
	{
		protected MMProgressBar _progressBar;
		[SerializeField]
		protected Character owner;
		[SerializeField]
		protected string propertyName;
		protected ObjectProperty property;

		protected virtual void Start()
		{
			_progressBar = GetComponent<MMProgressBar>();
			property = owner.PropertyManager.GetPropertyByName(propertyName);
			property.RegisterChangeCallback(OnEnduranceChanged);
		}
		void OnEnduranceChanged(float oldCurValue, float newCurValue, float oldValue, float newValue)
        {
			_progressBar.UpdateBar(newCurValue, 0, property.BaseValue);
		}
	}
}
