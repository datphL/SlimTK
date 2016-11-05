//
//  MDXPlayableAnimationLookupTableEntry.cs
//
//  Author:
//       Jarl Gullberg <jarl.gullberg@gmail.com>
//
//  Copyright (c) 2016 Jarl Gullberg
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//
using System;
using System.IO;

namespace Warcraft.MDX.Animation
{
	public class MDXPlayableAnimationLookupTableEntry
	{
		public short FallbackAnimationID;
		public MDXPlayableAnimationFlags Flags;

		public MDXPlayableAnimationLookupTableEntry(byte[] data)
		{
			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.FallbackAnimationID = br.ReadInt16();
					this.Flags = (MDXPlayableAnimationFlags)br.ReadInt16();
				}
			}
		}
	}

	[Flags]
	public enum MDXPlayableAnimationFlags : short
	{
		PlayNormally = 0,
		PlayReversed = 1,
		Freeze = 3
	}
}

