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

namespace Spine.Unity {
	public enum UpdateMode {
		Nothing = 0,
		OnlyAnimationStatus,
		OnlyEventTimelines = 4, // added as index 4 to keep scene behavior unchanged.
		EverythingExceptMesh = 2,
		FullUpdate,
		//Reserved 4 for OnlyEventTimelines
	};

	public enum UpdateTiming {
		ManualUpdate = 0,
		InUpdate,
		InFixedUpdate,
		InLateUpdate
	}

	public delegate void ISkeletonAnimationDelegate (ISkeletonAnimation animated);
	public delegate void UpdateBonesDelegate (ISkeletonAnimation animated);

	public interface ISpineComponent { }
	public static class ISpineComponentExtensions {
		public static bool IsNullOrDestroyed (this ISpineComponent component) {
			if (component == null) return true;
			return (UnityEngine.Object)component == null;
		}
	}

	/// <summary>A Spine-Unity Component that animates a Skeleton but not necessarily with a Spine.AnimationState.</summary>
	public interface ISkeletonAnimation : ISpineComponent {
		event ISkeletonAnimationDelegate OnAnimationRebuild;
		event UpdateBonesDelegate UpdateLocal;
		event UpdateBonesDelegate UpdateWorld;
		event UpdateBonesDelegate UpdateComplete;
		Skeleton Skeleton { get; }
		UpdateTiming UpdateTiming { get; set; }
	}

	/// <summary>Holds a reference to a SkeletonDataAsset.</summary>
	public interface IHasSkeletonDataAsset : ISpineComponent {
		/// <summary>Gets the SkeletonDataAsset of the Spine Component.</summary>
		SkeletonDataAsset SkeletonDataAsset { get; }
	}

	/// <summary>A Spine-Unity Component that manages a Spine.Skeleton instance, instantiated from a SkeletonDataAsset.</summary>
	public interface ISkeletonComponent : ISpineComponent {
		/// <summary>Gets the SkeletonDataAsset of the Spine Component.</summary>
		//[System.Obsolete]
		SkeletonDataAsset SkeletonDataAsset { get; }

		/// <summary>Gets the Spine.Skeleton instance of the Spine Component. This is equivalent to SkeletonRenderer's .skeleton.</summary>
		Skeleton Skeleton { get; }
	}

	/// <summary>A Spine-Unity Component that uses a Spine.AnimationState to animate its skeleton.</summary>
	public interface IAnimationStateComponent : ISpineComponent {
		/// <summary>Gets the Spine.AnimationState of the animated Spine Component. This is equivalent to SkeletonAnimation.state.</summary>
		AnimationState AnimationState { get; }
		/// <summary>If enabled, AnimationState time is advanced by Unscaled Game Time
		/// (<c>Time.unscaledDeltaTime</c> instead of the default Game Time(<c>Time.deltaTime</c>).
		/// to animate independent of game <c>Time.timeScale</c>.
		/// Instance SkeletonGraphic.timeScale and SkeletonAnimation.timeScale will still be applied.</summary>
		bool UnscaledTime { get; set; }
	}

	/// <summary>A Spine-Unity Component that holds a reference to a SkeletonRenderer.</summary>
	public interface IHasSkeletonRenderer : ISpineComponent {
		SkeletonRenderer SkeletonRenderer { get; }
	}

	/// <summary>A Spine-Unity Component that holds a reference to an ISkeletonComponent.</summary>
	public interface IHasSkeletonComponent : ISpineComponent {
		ISkeletonComponent SkeletonComponent { get; }
	}
}
