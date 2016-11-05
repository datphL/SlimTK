﻿//
//  ModelPortalReferences.cs
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
	public class ModelPortalReferences : IRIFFChunk, IBinarySerializable
	{
		public const string Signature = "MOPR";

		public readonly List<PortalReference> PortalReferences = new List<PortalReference>();

		public ModelPortalReferences()
		{

		}

		public ModelPortalReferences(byte[] inData)
		{
			LoadBinaryData(inData);
		}

		public void LoadBinaryData(byte[] inData)
        {
        	using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					int portalReferenceCount = inData.Length / PortalReference.GetSize();
					for (int i = 0; i < portalReferenceCount; ++i)
					{
						PortalReferences.Add(new PortalReference(br.ReadBytes(PortalReference.GetSize())));
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
		            foreach (PortalReference portalReference in this.PortalReferences)
		            {
			            bw.Write(portalReference.Serialize());
		            }
            	}

            	return ms.ToArray();
            }
		}
	}

	public class PortalReference : IBinarySerializable
	{
		public ushort PortalIndex;
		public ushort GroupIndex;
		public short Side;
		public ushort Unknown;

		public PortalReference(byte[] inData)
		{
			using (MemoryStream ms = new MemoryStream(inData))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.PortalIndex = br.ReadUInt16();
					this.GroupIndex = br.ReadUInt16();
					this.Side = br.ReadInt16();
					this.Unknown = br.ReadUInt16();
				}
			}
		}

		public static int GetSize()
		{
			return 8;
		}

		public byte[] Serialize()
		{
			using (MemoryStream ms = new MemoryStream())
            {
            	using (BinaryWriter bw = new BinaryWriter(ms))
            	{
            		bw.Write(this.PortalIndex);
		            bw.Write(this.GroupIndex);
		            bw.Write(this.Side);
		            bw.Write(this.Unknown);
            	}

            	return ms.ToArray();
            }
		}
	}
}

