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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spine.Unity.Examples {
	public class SpineboyBodyTilt : MonoBehaviour {

		[Header("Settings")]
		public SpineboyFootplanter planter;

		[SpineBone]
		public string hip = "hip", head = "head";
		public float hipTiltScale = 7;
		public float headTiltScale = 0.7f;
		public float hipRotationMoveScale = 60f;

		[Header("Debug")]
		public float hipRotationTarget;
		public float hipRotationSmoothed;
		public float baseHeadRotation;

		Bone hipBone, headBone;

		void Start () {
			SkeletonAnimation skeletonAnimation = GetComponent<SkeletonAnimation>();
			Skeleton skeleton = skeletonAnimation.Skeleton;

			hipBone = skeleton.FindBone(hip);
			headBone = skeleton.FindBone(head);
			baseHeadRotation = headBone.Rotation;

			skeletonAnimation.UpdateLocal += UpdateLocal;
		}

		private void UpdateLocal (ISkeletonAnimation animated) {
			hipRotationTarget = planter.Balance * hipTiltScale;
			hipRotationSmoothed = Mathf.MoveTowards(hipRotationSmoothed, hipRotationTarget, Time.deltaTime * hipRotationMoveScale * Mathf.Abs(2f * planter.Balance / planter.offBalanceThreshold));
			hipBone.Rotation = hipRotationSmoothed;
			headBone.Rotation = baseHeadRotation + (-hipRotationSmoothed * headTiltScale);
		}
	}

}
