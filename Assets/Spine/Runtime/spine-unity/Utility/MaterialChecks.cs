/******************************************************************************
 * Spine Runtimes License Agreement
 * Last updated April 5, 2025. Replaces all prior versions.
 *
 * Copyright (c) 2013-2025, Esoteric Software LLC
 *
 * Integration of the Spine Runtimes into software or otherwise creating
 * derivative works of the Spine Runtimes is permitted under the terms and
 * conditions of Section 2 of the Spine Editor License Agreement:
 * http://esotericsoftware.com/spine-editor-license
 *
 * Otherwise, it is permitted to integrate the Spine Runtimes into software
 * or otherwise create derivative works of the Spine Runtimes (collectively,
 * "Products"), provided that each user of the Products must obtain their own
 * Spine Editor license and redistribution of the Products in any form must
 * include this license and copyright notice.
 *
 * THE SPINE RUNTIMES ARE PROVIDED BY ESOTERIC SOFTWARE LLC "AS IS" AND ANY
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL ESOTERIC SOFTWARE LLC BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES,
 * BUSINESS INTERRUPTION, OR LOSS OF USE, DATA, OR PROFITS) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 * THE SPINE RUNTIMES, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *****************************************************************************/

using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

namespace Spine.Unity {
	/// <summary>Utility class providing methods to check material settings for incorrect combinations.</summary>
	public class MaterialChecks {

		static readonly int STRAIGHT_ALPHA_PARAM_ID = Shader.PropertyToID("_StraightAlphaInput");
		static readonly string ALPHAPREMULTIPLY_ON_KEYWORD = "_ALPHAPREMULTIPLY_ON";
		static readonly string ALPHAPREMULTIPLY_VERTEX_ONLY_ON_KEYWORD = "_ALPHAPREMULTIPLY_VERTEX_ONLY";
		static readonly string ALPHABLEND_ON_KEYWORD = "_ALPHABLEND_ON";
		static readonly string STRAIGHT_ALPHA_KEYWORD = "_STRAIGHT_ALPHA_INPUT";
		static readonly string[] FIXED_NORMALS_KEYWORDS = {
			"_FIXED_NORMALS_VIEWSPACE",
			"_FIXED_NORMALS_VIEWSPACE_BACKFACE",
			"_FIXED_NORMALS_MODELSPACE",
			"_FIXED_NORMALS_MODELSPACE_BACKFACE",
			"_FIXED_NORMALS_WORLDSPACE"
			};
		static readonly string NORMALMAP_KEYWORD = "_NORMALMAP";
		static readonly string CANVAS_GROUP_COMPATIBLE_KEYWORD = "_CANVAS_GROUP_COMPATIBLE";

		public static readonly string kPMANotSupportedLinearMessage =
			"\nWarning: Premultiply-alpha atlas textures not supported in Linear color space!"
			+ "You can use a straight alpha texture with 'PMA Vertex Color' by choosing blend mode 'PMA Vertex, Straight Texture'.\n\n"
			+ "If you have a PMA Texture, please\n"
			+ "a) re-export atlas as straight alpha texture with 'premultiply alpha' unchecked\n"
			+ "   (if you have already done this, please set the 'Straight Alpha Texture' Material parameter to 'true') or\n"
			+ "b) switch to Gamma color space via\nProject Settings - Player - Other Settings - Color Space.\n";
		public static readonly string kZSpacingRequiredMessage =
			"\nWarning: Z Spacing required on selected shader! Otherwise you will receive incorrect results.\n\nPlease\n"
			+ "1) make sure at least minimal 'Z Spacing' is set at the SkeletonRenderer/SkeletonAnimation component under 'Advanced' and\n"
			+ "2) ensure that the skeleton has overlapping parts on different Z depth. You can adjust this in Spine via draw order.\n";
		public static readonly string kZSpacingRecommendedMessage =
			"\nWarning: Z Spacing recommended on selected shader configuration!\n\nPlease\n"
			+ "1) make sure at least minimal 'Z Spacing' is set at the SkeletonRenderer/SkeletonAnimation component under 'Advanced' and\n"
			+ "2) ensure that the skeleton has overlapping parts on different Z depth. You can adjust this in Spine via draw order.\n";
		public static readonly string kAddNormalsMessage =
			"\nWarning: 'Add Normals' required when not using 'Fixed Normals'!\n\nPlease\n"
			+ "a) enable 'Add Normals' at the SkeletonRenderer/SkeletonAnimation component under 'Advanced' or\n"
			+ "b) enable 'Fixed Normals' at the Material.\n";
		public static readonly string kSolveTangentsMessage =
			"\nWarning: 'Solve Tangents' required when using a Normal Map!\n\nPlease\n"
			+ "a) enable 'Solve Tangents' at the SkeletonRenderer/SkeletonAnimation component under 'Advanced' or\n"
			+ "b) clear the 'Normal Map' parameter at the Material.\n";
		public static readonly string kNoSkeletonGraphicMaterialMessage =
			"\nWarning: Normal non-UI shaders other than 'Spine/SkeletonGraphic *' are not compatible with 'SkeletonGraphic' components! "
			+ "This will lead to incorrect rendering on some devices.\n\n"
			+ "Please change the assigned Material to e.g. 'SkeletonGraphicDefault' or change the used shader to one of the 'Spine/SkeletonGraphic *' shaders.\n\n"
			+ "Note that 'Spine/SkeletonGraphic *' shall still be used when using URP.\n";
		public static readonly string kSkeletonGraphicTintBlackMaterialRequiredMessage =
			"\nWarning: Only enable 'Tint Black' when using a 'SkeletonGraphic Tint Black' shader!\n"
			+ "Otherwise this will lead to incorrect rendering.\n\nPlease\n"
			+ "a) disable 'Tint Black' under 'Advanced' or\n"
			+ "b) use a 'SkeletonGraphic Tint Black' Material if you need Tint Black on a CanvasGroup.\n";

		public static readonly string kTintBlackRequiredMessage =
			"\nWarning: 'Advanced - Tint Black' required when using any 'Tint Black' shader!\n\nPlease\n"
			+ "a) enable 'Tint Black' at the SkeletonRenderer/SkeletonGraphic component under 'Advanced' or\n"
			+ "b) use a different shader at the Material.\n";
		public static readonly string kCanvasTintBlackMessage =
			"\nWarning: Canvas 'Additional Shader Channels' 'uv1' and 'uv2' are required when 'Advanced - Tint Black' is enabled!\n\n"
			+ "Please enable both 'uv1' and 'uv2' channels at the parent Canvas component parameter 'Additional Shader Channels'.\n";
		public static readonly string kCanvasGroupCompatibleMaterialRequiredMessage =
			"\nWarning: 'CanvasGroup Compatible' is enabled at SkeletonGraphic but not at the Material!\n\nPlease\n"
			+ "a) use a Material with 'CanvasGroup Compatible' enabled or\n"
			+ "b) disable 'CanvasGroup Compatible' at the SkeletonGraphic component under 'Advanced'.\n"
			+ "You can find CanvasGroup Compatible 'SkeletonGraphicTintBlack' materials in the\n"
			+ "'CanvasGroupCompatible' subfolder of 'SkeletonGraphic-PMATexture' or 'SkeletonGraphic-StraightAlphaTexture'.";
		public static readonly string kCanvasGroupRequiredMessage =
			"\nWarning: 'CanvasGroup Compatible' is enabled at the Material but disabled at SkeletonGraphic!\n\nPlease\n"
			+ "a) disable 'CanvasGroup Compatible' at the Material or\n"
			+ "b) enable 'CanvasGroup Compatible' at the SkeletonGraphic component under 'Advanced'.\n"
			+ "You can find CanvasGroup Compatible 'SkeletonGraphicTintBlack' materials in the\n"
			+ "'CanvasGroupCompatible' subfolder of 'SkeletonGraphic-PMATexture' or 'SkeletonGraphic-StraightAlphaTexture'.";
		public static readonly string kCanvasGroupCompatiblePMAVertexMessage =
			"\nWarning: 'CanvasGroup Compatible' is enabled at the Material and 'PMA Vertex Colors' is enabled at SkeletonGraphic!\n\nPlease\n"
			+ "a) disable 'CanvasGroup Compatible' at the Material or\n"
			+ "b) disable 'PMA Vertex Colors' at the SkeletonGraphic component under 'Advanced'.";

		public static bool IsMaterialSetupProblematic (SkeletonRenderer renderer, ref string errorMessage) {
			Material[] materials = renderer.GetComponent<Renderer>().sharedMaterials;
			bool isProblematic = false;
			foreach (Material material in materials) {
				if (material == null) continue;
				isProblematic |= IsMaterialSetupProblematic(material, ref errorMessage);
				if (renderer.zSpacing == 0) {
					isProblematic |= IsZSpacingRequired(material, ref errorMessage);
				}
				if (renderer.addNormals == false && RequiresMeshNormals(material)) {
					isProblematic = true;
					errorMessage += kAddNormalsMessage;
				}
				if (renderer.calculateTangents == false && RequiresTangents(material)) {
					isProblematic = true;
					errorMessage += kSolveTangentsMessage;
				}
				if (renderer.tintBlack == false && RequiresTintBlack(material)) {
					isProblematic = true;
					errorMessage += kTintBlackRequiredMessage;
				}
			}
			return isProblematic;
		}

		public static bool IsMaterialSetupProblematic (SkeletonGraphic skeletonGraphic, ref string errorMessage) {
			Material material = skeletonGraphic.material;
			bool isProblematic = false;
			if (material) {
				isProblematic |= IsMaterialSetupProblematic(material, ref errorMessage);
				MeshGenerator.Settings settings = skeletonGraphic.MeshGenerator.settings;
				if (settings.zSpacing == 0) {
					isProblematic |= IsZSpacingRequired(material, ref errorMessage);
				}
				if (IsSpineNonSkeletonGraphicMaterial(material)) {
					isProblematic = true;
					errorMessage += kNoSkeletonGraphicMaterialMessage;
				}
				bool isTintBlackMaterial = IsSkeletonGraphicTintBlackMaterial(material);
				if (settings.tintBlack != isTintBlackMaterial) {
					isProblematic = true;
					errorMessage += (settings.tintBlack == false) ?
						kTintBlackRequiredMessage : kSkeletonGraphicTintBlackMaterialRequiredMessage;
				}

				if (settings.tintBlack == true && CanvasNotSetupForTintBlack(skeletonGraphic)) {
					isProblematic = true;
					errorMessage += kCanvasTintBlackMessage;
				}

				bool isCanvasGroupCompatible = IsCanvasGroupCompatible(material);
				if (settings.canvasGroupCompatible != isCanvasGroupCompatible) {
					isProblematic = true;
					errorMessage += (settings.canvasGroupCompatible == false) ?
						kCanvasGroupRequiredMessage : kCanvasGroupCompatibleMaterialRequiredMessage;
				}

				if (settings.pmaVertexColors == true && settings.canvasGroupCompatible == true && settings.tintBlack == false) {
					isProblematic = true;
					errorMessage += kCanvasGroupCompatiblePMAVertexMessage;
				}
			}
			return isProblematic;
		}

		public static bool IsMaterialSetupProblematic (Material material, ref string errorMessage) {
			return !IsColorSpaceSupported(material, ref errorMessage);
		}

		public static bool IsZSpacingRequired (Material material, ref string errorMessage) {
			bool hasForwardAddPass = material.FindPass("FORWARD_DELTA") >= 0;
			if (hasForwardAddPass) {
				errorMessage += kZSpacingRequiredMessage;
				return true;
			}
			bool zWrite = material.HasProperty("_ZWrite") && material.GetFloat("_ZWrite") > 0.0f;
			if (zWrite) {
				errorMessage += kZSpacingRecommendedMessage;
				return true;
			}
			return false;
		}

		public static bool IsColorSpaceSupported (Material material, ref string errorMessage) {
			if (QualitySettings.activeColorSpace == ColorSpace.Linear) {
				if (IsPMATextureMaterial(material)) {
					errorMessage += kPMANotSupportedLinearMessage;
					return false;
				}
			}
			return true;
		}

		public static bool UsesSpineShader (Material material) {
			return material.shader.name.Contains("Spine/");
		}

		public static bool IsTextureSetupProblematic (Material material, ColorSpace colorSpace,
			bool sRGBTexture, bool mipmapEnabled, bool alphaIsTransparency,
			string texturePath, string materialPath,
			ref string errorMessage) {

			if (material == null || !UsesSpineShader(material)) {
				return false;
			}

			bool isProblematic = false;
			if (IsPMATextureMaterial(material)) {
				// 'sRGBTexture = true' generates incorrectly weighted mipmaps at PMA textures,
				// causing white borders due to undesired custom weighting.
				if (sRGBTexture && mipmapEnabled && colorSpace == ColorSpace.Gamma) {
					errorMessage += string.Format("`{0}` : Problematic Texture Settings found: " +
						"When enabling `Generate Mip Maps` in Gamma color space, it is recommended " +
						"to disable `sRGB (Color Texture)` on `Premultiply alpha` textures. Otherwise " +
						"you will receive white border artifacts on an atlas exported with default " +
						"`Premultiply alpha` settings.\n" +
						"(You can disable this warning in `Edit - Preferences - Spine`)\n", texturePath);
					isProblematic = true;
				}
				if (alphaIsTransparency) {
					string materialName = System.IO.Path.GetFileName(materialPath);
					errorMessage += string.Format("`{0}` and material `{1}` : Problematic " +
						"Texture / Material Settings found: It is recommended to disable " +
						"`Alpha Is Transparency` on `Premultiply alpha` textures.\n" +
						"Assuming `Premultiply alpha` texture because `Straight Alpha Texture` " +
						"is disabled at material). " +
						"(You can disable this warning in `Edit - Preferences - Spine`)\n", texturePath, materialName);
					isProblematic = true;
				}
			} else { // straight alpha texture
				if (!alphaIsTransparency) {
					string materialName = System.IO.Path.GetFileName(materialPath);
					errorMessage += string.Format("`{0}` and material `{1}` : Incorrect" +
						"Texture / Material Settings found: It is strongly recommended " +
						"to enable `Alpha Is Transparency` on `Straight alpha` textures.\n" +
						"Assuming `Straight alpha` texture because `Straight Alpha Texture` " +
						"is enabled at material). " +
						"(You can disable this warning in `Edit - Preferences - Spine`)\n", texturePath, materialName);
					isProblematic = true;
				}
			}
			return isProblematic;
		}

		public static void EnablePMATextureAtMaterial (Material material, bool enablePMATexture) {
			if (material.HasProperty(STRAIGHT_ALPHA_PARAM_ID)) {
				material.SetInt(STRAIGHT_ALPHA_PARAM_ID, enablePMATexture ? 0 : 1);
				if (enablePMATexture)
					material.DisableKeyword(STRAIGHT_ALPHA_KEYWORD);
				else
					material.EnableKeyword(STRAIGHT_ALPHA_KEYWORD);
			} else {
				if (enablePMATexture) {
					material.DisableKeyword(ALPHAPREMULTIPLY_ON_KEYWORD);
					material.DisableKeyword(ALPHABLEND_ON_KEYWORD);
					material.EnableKeyword(ALPHAPREMULTIPLY_VERTEX_ONLY_ON_KEYWORD);
				} else {
					material.DisableKeyword(ALPHAPREMULTIPLY_ON_KEYWORD);
					material.DisableKeyword(ALPHAPREMULTIPLY_VERTEX_ONLY_ON_KEYWORD);
					material.EnableKeyword(ALPHABLEND_ON_KEYWORD);
				}
			}
		}

		public static bool IsPMATextureMaterial (Material material) {
			bool usesAlphaPremultiplyKeyword = IsSpriteShader(material);
			if (usesAlphaPremultiplyKeyword)
				return material.IsKeywordEnabled(ALPHAPREMULTIPLY_ON_KEYWORD);
			else
				return material.HasProperty(STRAIGHT_ALPHA_PARAM_ID) && material.GetInt(STRAIGHT_ALPHA_PARAM_ID) == 0;
		}

		static bool IsURP3DMaterial (Material material) {
			return material.shader.name.Contains("Universal Render Pipeline/Spine");
		}

		static bool IsSpineNonSkeletonGraphicMaterial (Material material) {
			return material.shader.name.Contains("Spine") && !material.shader.name.Contains("SkeletonGraphic");
		}

		static bool IsSkeletonGraphicTintBlackMaterial (Material material) {
			return material.shader.name.Contains("Spine") && material.shader.name.Contains("SkeletonGraphic")
				&& material.shader.name.Contains("Black");
		}

		static bool AreShadowsDisabled (Material material) {
			return material.IsKeywordEnabled("_RECEIVE_SHADOWS_OFF");
		}

		static bool RequiresMeshNormals (Material material) {
			bool anyFixedNormalSet = false;
			foreach (string fixedNormalKeyword in FIXED_NORMALS_KEYWORDS) {
				if (material.IsKeywordEnabled(fixedNormalKeyword)) {
					anyFixedNormalSet = true;
					break;
				}
			}
			bool isShaderWithMeshNormals = IsLitSpriteShader(material);
			return isShaderWithMeshNormals && !anyFixedNormalSet;
		}

		static bool IsLitSpriteShader (Material material) {
			string shaderName = material.shader.name;
			return shaderName.Contains("Spine/Sprite/Pixel Lit") ||
				shaderName.Contains("Spine/Sprite/Vertex Lit") ||
				shaderName.Contains("2D/Spine/Sprite") || // covers both URP and LWRP
				shaderName.Contains("Pipeline/Spine/Sprite"); // covers both URP and LWRP
		}

		static bool IsSpriteShader (Material material) {
			if (IsLitSpriteShader(material))
				return true;
			string shaderName = material.shader.name;
			return shaderName.Contains("Spine/Sprite/Unlit");
		}

		static bool RequiresTintBlack (Material material) {
			bool isTintBlackShader =
				material.shader.name.Contains("Spine") &&
				material.shader.name.Contains("Tint Black");
			return isTintBlackShader;
		}

		static bool RequiresTangents (Material material) {
			return material.IsKeywordEnabled(NORMALMAP_KEYWORD);
		}
		static bool IsCanvasGroupCompatible (Material material) {
			return material.IsKeywordEnabled(CANVAS_GROUP_COMPATIBLE_KEYWORD);
		}

		static bool CanvasNotSetupForTintBlack (SkeletonGraphic skeletonGraphic) {
			Canvas canvas = skeletonGraphic.canvas;
			if (!canvas)
				return false;
			var requiredChannels =
				AdditionalCanvasShaderChannels.TexCoord1 |
				AdditionalCanvasShaderChannels.TexCoord2;
			return (canvas.additionalShaderChannels & requiredChannels) != requiredChannels;
		}
	}
}

#endif // UNITY_EDITOR
