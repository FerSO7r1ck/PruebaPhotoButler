// (c) Copyright Andrey Torchinskiy, 2019. All rights reserved.

using UnityEngine;

namespace ScreenTransitionsPro
{
	[ExecuteInEditMode]
	[AddComponentMenu("Screen Transitions Pro/Fade")]
	public class ScreenTransitionFade : MonoBehaviour, IScreenTransition
	{
		#region Variables

		/// <summary>
		/// Material that will be applied to rendered image during transition.
		/// </summary>
		[Tooltip("Material that will be applied to rendered image during transition.")]
		public Material transitionMaterial;

		/// <summary>
		/// Background color that will be used during transition.
		/// </summary>
		[Tooltip("Background color that will be used during transition.")]
		public Color backgroundColor = Color.black;

		/// <summary>
		/// Texture that will be used as background during transition.
		/// Render Texture also allowed.
		/// </summary>
		[Tooltip("Texture that will be used as a background during transition. Render Texture also allowed.")]
		public Texture backgroundTexture;

		public enum BackgroundType
		{
			COLOR,
			TEXTURE
		}
		/// <summary>
		/// Defines what type background will be used during transition.
		/// </summary>
		[Tooltip("Defines what type background will be used during transition.")]
		public BackgroundType backgroundType;

		/// <summary>
		/// Represents current progress of the transition.
		/// 0 - no transition
		/// 1 - full transition to background color.
		/// </summary>
		[Range(0, 1f), Tooltip("Represents current progress of the transition.")]
		public float cutoff = 0f;

		/// <summary>
		/// Defines if transition color should be progressively added to the rendered image during transition.
		/// Works best with white background color.
		/// </summary>
		[Tooltip("Defines if transition color should be progressively added to the rendered image during transition. Works best with white background color.")]
		public bool additive = false;

		/// <summary>
		/// Flag that tells Unity to process transition. 
		/// Set this flag at the beginning of the transition and unset at the end 
		/// to avoid unnecessary calculations and save some performance.
		/// </summary>
		[Tooltip("Flag that tells Unity to process transition. Set this flag at the beginning of the transition and unset it at the end to avoid unnecessary calculations and to save some performance.")]
		public bool transitioning;

		#endregion

		#region Unity Callbacks

		private void Start()
		{
			if (transitionMaterial)
			{
				switch (backgroundType)
				{
					case BackgroundType.COLOR:
						transitionMaterial.DisableKeyword("USE_TEXTURE");
						break;
					case BackgroundType.TEXTURE:
						transitionMaterial.EnableKeyword("USE_TEXTURE");
						break;
				}
			}
		}

		private void LateUpdate()
		{
			if (!SystemInfo.supportsImageEffects)
			{
				transitioning = false;
				return;
			}

			if (transitioning && transitionMaterial)
			{
				transitionMaterial.SetFloat("_Cutoff", cutoff);
				transitionMaterial.SetInt("_Additive", additive ? 1 : 0);
				switch (backgroundType)
				{
					case BackgroundType.COLOR:
						transitionMaterial.SetColor("_Color", backgroundColor);
						break;
					case BackgroundType.TEXTURE:
						transitionMaterial.SetTexture("_Texture", backgroundTexture);
						break;
				}
			}
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (transitioning && transitionMaterial)
			{
				Graphics.Blit(source, destination, transitionMaterial);
			}
			else
			{
				Graphics.Blit(source, destination);
			}
		}

		#endregion

		#region Interface Implementation

		public void SetTransitioning(bool t)
		{
			transitioning = t;
		}

		public void SetMaterial(Material m)
		{
			transitionMaterial = m;
		}

		public void SetCutoff(float c)
		{
			cutoff = Mathf.Clamp(c, -1f, 1f);
		}

		public void SetFalloff(float f)
		{
			Debug.LogWarning("Current screen transition doesn't support falloff. Value will be ignored.");
		}

		public void SetBackgroundColor(Color bc)
		{
			backgroundColor = bc;
		}

		public void SetBackgroundTexture(Texture tex)
		{
			backgroundTexture = tex;
		}

		public void SetFitToScreen(bool fts)
		{
			Debug.LogWarning("Current screen transition doesn't support fit to screen. Value will be ignored.");
		}

		public void SetHorizontalFlip(bool hf)
		{
			Debug.LogWarning("Current screen transition doesn't support horizontal flip. Value will be ignored.");
		}

		public void SetVerticalFlip(bool vf)
		{
			Debug.LogWarning("Current screen transition doesn't support vertical flip. Value will be ignored.");
		}

		public void SetInvert(bool i)
		{
			Debug.LogWarning("Current screen transition doesn't support invert. Value will be ignored.");
		}

		public void AddFocus(Transform f)
		{
			Debug.LogWarning("Current screen transition doesn't support adding focus. Value will be ignored.");
		}

		public void RemoveFocus(Transform f)
		{
			Debug.LogWarning("Current screen transition doesn't support removing focus. Value will be ignored.");
		}

		public void SetNoiseScale(float s)
		{
			Debug.LogWarning("Current screen transition doesn't support noise scale. Value will be ignored.");
		}

		public void SetNoiseVelocity(Vector2 v)
		{
			Debug.LogWarning("Current screen transition doesn't support noise velocity. Value will be ignored.");
		}

		#endregion
	}
}
