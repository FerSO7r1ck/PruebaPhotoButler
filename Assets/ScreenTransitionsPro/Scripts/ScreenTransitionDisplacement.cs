// (c) Copyright Andrey Torchinskiy, 2019. All rights reserved.

using UnityEngine;

namespace ScreenTransitionsPro
{
	[ExecuteInEditMode]
	[AddComponentMenu("Screen Transitions Pro/Displacement")]
	public class ScreenTransitionDisplacement : MonoBehaviour, IScreenTransition
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
		[Tooltip("Texture that will be used as background during transition. Render Texture also allowed.")]
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
		[Range(0f, 1f), Tooltip("Represents current progress of the transition.")]
		public float cutoff = 0f;

		/// <summary>
		/// Flag that tells Unity to process transition. 
		/// Set this flag at the beginning of the transition and unset at the end 
		/// to avoid unnecessary calculations and save some performance.
		/// </summary>
		[Tooltip("Flag that tells Unity to process transition. Set this flag at the beginning of the transition and unset it at the end to avoid unnecessary calculations and to save some performance.")]
		public bool transitioning;

		/// <summary>
		/// Set this flag if you want to flip transition texture horizontally.
		/// </summary>
		[Tooltip("Set this flag if you want to flip transition texture horizontally.")]
		public bool flipHorizontally;

		/// <summary>
		/// Set this flag if you want to flip transition texture vertically.
		/// </summary>
		[Tooltip("Set this flag if you want to flip transition texture vertically.")]
		public bool flipVertically;

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
				transitionMaterial.SetInt("_FlipH", flipHorizontally ? 1 : 0);
				transitionMaterial.SetInt("_FlipV", flipVertically ? 1 : 0);
				transitionMaterial.SetFloat("_Cutoff", cutoff);
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
			cutoff = Mathf.Clamp01(c);
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
			flipHorizontally = hf;
		}

		public void SetVerticalFlip(bool vf)
		{
			flipVertically = vf;
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
