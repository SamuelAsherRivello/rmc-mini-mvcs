using UnityEngine;

namespace RMC.Core.Components
{
	/// <summary>
	/// Repeatedly scrolls the camera background
	/// </summary>
	public class SkyboxMaterialScroller : MonoBehaviour
	{
		// Properties -------------------------------------
		
		
		// Fields -----------------------------------------
		[SerializeField]
		private Material _skyboxMaterial = null;
		
		[SerializeField]
		private float _skyboxRotationSpeed = 2;
		
		[SerializeField]
		private bool _isSkyboxRotating = true;
		
		
		// Unity Methods -----------------------------------
		protected void OnValidate()
		{
			if (RenderSettings.skybox != _skyboxMaterial)
			{
				RenderSettings.skybox = _skyboxMaterial;
			}
		}

		
		protected virtual void Update()
		{
			SetSkyboxRotation(Time.time * _skyboxRotationSpeed);
		}

		
		protected void OnDestroy()
		{
			// Prevents dirtying git version control on the material
			SetSkyboxRotation(0);
		}
		
		
		// General Methods --------------------------------
		private void SetSkyboxRotation(float rotation)
		{
			if (!_isSkyboxRotating || RenderSettings.skybox == null)
			{
				return;
			}

			RenderSettings.skybox.SetFloat("_Rotation", rotation);
		}
		
		// Event Handlers ---------------------------------
	}
}
