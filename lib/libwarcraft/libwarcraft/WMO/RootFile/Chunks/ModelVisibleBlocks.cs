﻿//
//  ModelVisibleBlocks.cs
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
using System.Collections.Generic;
using System.IO;
using Warcraft.Core.Interfaces;

namespace Warcraft.WMO.RootFile.Chunks
{
	public class ModelVisibleBlocks : IRIFFChunk, IBinarySerializable
	{
		public const string Signature = "MOVB";

		public readonly List<VisibleBlock> VisibleBlocks = new List<VisibleBlock>();

		public ModelVisibleBlocks()
		{

		}

		public ModelVisibleBlocks(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int visibleBlockCount = inData.Length / VisibleBlock.GetSize();
					for (int i = 0; i < visibleBlockCount; ++i)
					{
						VisibleBlocks.Add(new VisibleBlock(br.ReadBytes(VisibleBlock.GetSize())));
					}
				}
			}
        }

        public string GetSignature()
        {
        	return Signature;
        }

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
		            foreach (VisibleBlock visibleBlock in this.VisibleBlocks)
		            {
						bw.Write(visibleBlock.Serialize());
		            }
            	}

	            return ms.ToArray();
            }
		}
	}

	public class VisibleBlock : IBinarySerializable
	{
		public ushort FirstVertexIndex;
		public ushort VertexCount;

		public VisibleBlock(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.FirstVertexIndex = br.ReadUInt16();
					this.VertexCount = br.ReadUInt16();
				}
			}
		}

		public static int GetSize()
		{
			return 4;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
	            {
		            bw.Write(this.FirstVertexIndex);
		            bw.Write(this.VertexCount);
	            }

	            return ms.ToArray();
            }
		}
	}
}

