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

		protected virtual void Awake()
		{
			_progressBar = GetComponent<MMProgressBar>();
			SetOwner(owner);
		}
		void OnValueChanged(float oldCurValue, float newCurValue, float oldValue, float newValue)
        {
			_progressBar.UpdateBar(newCurValue, 0, property.BaseValue);
		}
		public void SetOwner(Character character)
        {
			if (_progressBar==null)
				_progressBar = GetComponent<MMProgressBar>();
			if (character!=null)
			{
				if (property!=null)	
					property.UnRegisterCallback(OnValueChanged);
				owner = character;
				property = owner.PropertyManager.GetPropertyByName(propertyName);
				property.RegisterChangeCallback(OnValueChanged);
				_progressBar.UpdateBar(property.Value, 0, property.BaseValue);
			}
		}
	}
}
