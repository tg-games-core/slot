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

using System;

namespace Spine {
	/// <summary>
	/// An attachment which is a single point and a rotation. This can be used to spawn projectiles, particles, etc. A bone can be
	/// used in similar ways, but a PointAttachment is slightly less expensive to compute and can be hidden, shown, and placed in a
	/// skin.
	/// <p>
	/// See <a href="http://esotericsoftware.com/spine-point-attachments">Point Attachments</a> in the Spine User Guide.
	/// </summary>
	public class PointAttachment : Attachment {
		internal float x, y, rotation;
		public float X { get { return x; } set { x = value; } }
		public float Y { get { return y; } set { y = value; } }
		public float Rotation { get { return rotation; } set { rotation = value; } }

		public PointAttachment (string name)
			: base(name) {
		}

		/// <summary>Copy constructor.</summary>
		protected PointAttachment (PointAttachment other)
			: base(other) {
			x = other.x;
			y = other.y;
			rotation = other.rotation;
		}

		public void ComputeWorldPosition (Bone bone, out float ox, out float oy) {
			bone.LocalToWorld(this.x, this.y, out ox, out oy);
		}

		public float ComputeWorldRotation (Bone bone) {
			float r = rotation * MathUtils.DegRad, cos = (float)Math.Cos(r), sin = (float)Math.Sin(r);
			float x = cos * bone.a + sin * bone.b;
			float y = cos * bone.c + sin * bone.d;
			return MathUtils.Atan2Deg(y, x);
		}

		public override Attachment Copy () {
			return new PointAttachment(this);
		}
	}
}
