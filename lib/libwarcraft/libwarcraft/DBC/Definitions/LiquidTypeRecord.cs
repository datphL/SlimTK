﻿//
//  LiquidTypeRecord.cs
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
using Warcraft.DBC.SpecialFields;
using Warcraft.Core;

namespace Warcraft.DBC.Definitions
{
	public class LiquidTypeRecord : DBCRecord
	{
		public const string RecordName = "LiquidType";

		/// <summary>
		/// The name of the liquid.
		/// </summary>
		public StringReference Name;

		public LiquidType Type
		{
			get
			{
				return TranslateLiquidType();
			}
			set
			{
				this.Type = value;
			}
		}

		public UInt32ForeignKey SpellEffect;

		/// <summary>
		/// Loads and parses the provided data.
		/// </summary>
		/// <param name="data">Data.</param>
		public override void LoadData(byte[] data)
		{
			if (this.Version == WarcraftVersion.Unknown)
			{
				throw new InvalidOperationException("The record data cannot be loaded before SetVersion has been called.");
			}

			using (MemoryStream ms = new MemoryStream(data))
			{
				using (BinaryReader br = new BinaryReader(ms))
				{
					this.ID = br.ReadUInt32();
					this.Name = new StringReference(br.ReadUInt32());

					this.Type = (LiquidType)br.ReadInt32();

					this.SpellEffect = new UInt32ForeignKey(SpellRecord.RecordName, "ID", br.ReadUInt32());
				}
			}
		}

		public LiquidType TranslateLiquidType()
		{
			if (this.Version >= WarcraftVersion.Wrath)
			{
				return this.Type;
			}

			int baseValue = (int)this.Type;
			switch (baseValue)
			{
				case 0:
				{
					return LiquidType.Magma;
				}
				case 2:
				{
					return LiquidType.Slime;
				}
				case 3:
				{
					return LiquidType.Water;
				}
				default:
				{
					return LiquidType.Water;
				}
			}
		}

		// TODO: Implement records after Classic
		/// <summary>
		/// Gets the size of the record.
		/// </summary>
		/// <returns>The record size.</returns>
		public override int GetRecordSize()
		{
			if (this.Version == WarcraftVersion.Unknown)
			{
				throw new InvalidOperationException("The record information cannot be accessed before SetVersion has been called.");
			}

			switch (Version)
			{
				case WarcraftVersion.Classic:
					return 16;
				case WarcraftVersion.BurningCrusade:
					return 16;
				case WarcraftVersion.Wrath:
					return 180;
				case WarcraftVersion.Cataclysm:
					return 200;
				case WarcraftVersion.Mists:
					return 200;
				case WarcraftVersion.Warlords:
					return 200;
				case WarcraftVersion.Legion:
					return 200;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		/// <summary>
		/// Gets the field count for this record at.
		/// </summary>
		/// <returns>The field count.</returns>
		public override int GetFieldCount()
		{
			if (this.Version == WarcraftVersion.Unknown)
			{
				throw new InvalidOperationException("The record information cannot be accessed before SetVersion has been called.");
			}

			switch (Version)
			{
				case WarcraftVersion.Classic:
					return 4;
				case WarcraftVersion.BurningCrusade:
					return 4;
				case WarcraftVersion.Wrath:
					return 45;
				case WarcraftVersion.Cataclysm:
					return 50;
				case WarcraftVersion.Mists:
					return 50;
				case WarcraftVersion.Warlords:
					return 50;
				case WarcraftVersion.Legion:
					return 50;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}

	public enum LiquidType
	{
		// Originally 3, now 0
		Water = 0,
		// Originally 3 (same as water), now 1
		Ocean = 1,
		// Originally 0, now 2
		Magma = 2,
		// Originally 2, now 3
		Slime = 3
	}
}

