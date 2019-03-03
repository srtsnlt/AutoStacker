using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;

namespace AutoStacker.Projectiles
{
	public class OreEater : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ore Eater");
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			ProjectileID.Sets.LightPet[projectile.type] = true;
		}
		
		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.timeLeft *= 5;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.scale = 0.8f;
			projectile.tileCollide = false;
		}
		
		const int fadeInTicks = 30;
		const int fullBrightTicks = 200;
		const int fadeOutTicks = 30;
		const int range = 500;
		int rangeHypoteneus = (int)Math.Sqrt(range * range + range * range);
		
		const int serchMax= 50;
		int serchMax_cur= 0;
		
		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			Players.OreEater modPlayer = player.GetModPlayer<Players.OreEater>(mod);
			Pet pet=modPlayer.pet;
			if (!player.active)
			{
				projectile.active = false;
				return;
			}
			if (player.dead)
			{
				modPlayer.oreEater = false;
			}
			
			if (modPlayer.oreEater)
			{
				projectile.timeLeft = 2;
				//projectile.ai[0]++;
			}
			else
			{
				pet.initList();
			}
			
			//scan pick
			int pickPower = Main.LocalPlayer.inventory.Max(item => item.pick);
			if( pickPower == 0)
			{
				modPlayer.oreEater = false;
			}
			
			//light
			Lighting.AddLight(projectile.position, 3 * 0.9f, 3 * 0.1f, 3 * 0.3f);
			
			
			//ore scan & move & pick 
			int distanceX;
			int distanceY;
			int distanceSum;
			
			double velocity = 8;
			velocity = velocity * ( 1 - 0.5 * serchMax_cur / serchMax);
			
			if(pet.tileType.Count ==0)
			{
				serchMax_cur = serchMax_cur < serchMax ? serchMax_cur + 1 : serchMax;
				
				//int startRadius = pet.latestRadius >= serchMax_cur ? 1 : pet.latestRadius -1;
				//pet.serch((int)projectile.Center.X/16, (int)projectile.Center.Y/16, startRadius, serchMax_cur);
				pet.serch((int)projectile.Center.X/16, (int)projectile.Center.Y/16, 1, serchMax_cur);
				
				distanceX   = (int)player.Center.X - (int)projectile.Center.X;
				distanceY   = (int)player.Center.Y - (int)projectile.Center.Y;
				distanceSum = Math.Abs(distanceX + distanceY);
				if( distanceSum >= 2 )
				{
					
					projectile.velocity.X = (float)(velocity * (double)distanceX / (double)distanceSum);
					projectile.velocity.Y = (float)(velocity * (double)distanceY / (double)distanceSum);
				}
			}
			else
			{
				serchMax_cur = serchMax_cur > 0 ? serchMax_cur - 1 : 0;
				
				distanceX   = (int)pet.X[0]*16 - (int)projectile.Center.X;
				distanceY   = (int)pet.Y[0]*16 - (int)projectile.Center.Y;
				distanceSum = Math.Abs(distanceX + distanceY);
				
				if( distanceSum >= 2 )
				{
					projectile.velocity.X = (float)(velocity * (double)distanceX / (double)distanceSum);
					projectile.velocity.Y = (float)(velocity * (double)distanceY / (double)distanceSum);
				}
				
				if(distanceSum <= 16*4)
				{
					
					
					if(
						TileID.Sets.Ore[pet.tileType[0]] 
						|| TileID.Sets.JungleSpecial[pet.tileType[0]] 
						|| pet.tileType[0] == TileID.Amethyst
						|| pet.tileType[0] == TileID.AmethystGemspark
						|| pet.tileType[0] == TileID.AmethystGemsparkOff
						|| pet.tileType[0] == TileID.Topaz
						|| pet.tileType[0] == TileID.TopazGemspark
						|| pet.tileType[0] == TileID.TopazGemsparkOff
						|| pet.tileType[0] == TileID.Sapphire
						|| pet.tileType[0] == TileID.SapphireGemspark
						|| pet.tileType[0] == TileID.SapphireGemsparkOff
						|| pet.tileType[0] == TileID.Emerald
						|| pet.tileType[0] == TileID.EmeraldGemspark
						|| pet.tileType[0] == TileID.EmeraldGemsparkOff
						|| pet.tileType[0] == TileID.Ruby
						|| pet.tileType[0] == TileID.RubyGemspark
						|| pet.tileType[0] == TileID.RubyGemsparkOff
						|| pet.tileType[0] == TileID.Diamond
						|| pet.tileType[0] == TileID.DiamondGemspark
						|| pet.tileType[0] == TileID.DiamondGemsparkOff
						|| pet.tileType[0] == TileID.GemLocks
						|| pet.tileType[0] == TileID.LifeFruit
						|| pet.tileType[0] == TileID.Crystals
						
					)
					{
						modPlayer.player.PickTile((int)pet.X[0], (int)pet.Y[0], pickPower);
						pet.removeList(0);
						
					}	
				}
				
			}
			
			
		}
	}
	
	public class Pet
	{
		//serch results
		//~~~~~~~~~~~~~~~~~~~~~
		private List<int> _tileType = new List<int>();
		private List<int> _X        = new List<int>();
		private List<int> _Y        = new List<int>();
		private int _latestRadius = 0;
		
		public List<int> tileType
		{
			get
			{
				return _tileType;
			}
		}
		
		public List<int> X
		{
			get
			{
				return _X;
			}
		}
		
		public List<int> Y
		{
			get
			{
				return _Y;
			}
		}
		
		public int latestRadius
		{
			get
			{
				return _latestRadius;
			}
		}
		
		public  Dictionary<int,Dictionary<int,int>>  petDictionary    = new Dictionary<int,Dictionary<int,int>>();
		private List<List<int>>                     _petDictionaryInv = new List<List<int>>();
		
		/*
		public void serch(int originX, int originY, int startRadius, int maxRadius )
		{
			_serch((int)originX/16, (int)originY/16, startRadius, maxRadius );
		}
		*/
		
		public void serch(int originX, int originY, int startRadius, int maxRadius )
		{
			if(startRadius > maxRadius)
			{
				return;
			}
			
			int x=0;
			int y=0;
			int dx=0;
			int dy=0;
			
			for(int side =0;side <= 3; side++)
			{
				switch(side)
				{
					case 0: //upper side
						x = (int)(originX - startRadius);
						y = (int)(originY - startRadius);
						dx = 1;
						dy = 0;
						if(y < 0)
						{
							continue;
						}
						break;
					
					case 1: //right side
						x = (int)(originX + startRadius);
						y = (int)(originY - startRadius);
						dx = 0;
						dy = 1;
						if(x >= Main.Map.MaxWidth)
						{
							continue;
						}
						break;
					
					case 2: //lower side
						x = (int)(originX + startRadius);
						y = (int)(originY + startRadius);
						dx = -1;
						dy = 0;
						if(y >= Main.Map.MaxHeight) 
						{
							continue;
						}
						break;
					
					case 3: //left side
						x = (int)(originX - startRadius);
						y = (int)(originY + startRadius);
						dx = 0;
						dy = -1;
						if(x < 0)
						{
							continue;
						}
						break;
				}
				
				for(int tileNo=0; tileNo<= startRadius*2; tileNo++, x += dx, y += dy )
				{
					if(dy==0 && ( x < 0 || x >= Main.Map.MaxWidth ) )
					{
						continue;
					}
					
					if(dx==0 && ( y < 0 || y >= Main.Map.MaxHeight) )
					{
						continue;
					}
					
					Tile tile = Main.tile[x,y];
					if (tile == null || !tile.active())
					{
						continue;
					}
					
					if( TileID.Sets.Ore[tile.type] ) //|| TileID.Sets.BasicChest[tile.type] )
					{
						addList((int)x, (int)y, tile.type);
					}
					
				}
			}
			_latestRadius=startRadius;
			serch(originX, originY, startRadius + 1, maxRadius);
		}
		
		public Point16 GetOrigin(int x, int y)
		{
			Tile tile = Main.tile[x, y];
			if (tile == null || !tile.active())
				return new Point16(x, y);
			
			TileObjectData tileObjectData = TileObjectData.GetTileData(tile.type, 0);
			if (tileObjectData == null)
				return new Point16(x, y);
			
			//OneByOne
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			if (tileObjectData.Width == 1 && tileObjectData.Height == 1)
				return new Point16(x, y);
			
			//xOffset
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			int xOffset = tile.frameX % tileObjectData.CoordinateFullWidth / tileObjectData.CoordinateWidth ;
			
			//yOffset
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
			//Rectangle(single)
			int yOffset;
			if (tileObjectData.CoordinateHeights.Distinct().Count() == 1)
			{
				yOffset = tile.frameY % tileObjectData.CoordinateFullHeight / tileObjectData.CoordinateHeights[0] ;
			}
			
			//Rectangle(complex)
			else
			{
				yOffset = 0;
				int FullY = tile.frameY % tileObjectData.CoordinateFullHeight;
				for (int i = 0; i < tileObjectData.CoordinateHeights.Length && FullY >= tileObjectData.CoordinateHeights[i]; i++)
				{
					FullY -= tileObjectData.CoordinateHeights[i];
					yOffset++;
				}
			}
			return new Point16(x - xOffset, y - yOffset);
		}
		
		private void addList(int x, int y, int tileType)
		{
			if(!petDictionary.ContainsKey(x))
			{
				petDictionary.Add(x, new Dictionary<int,int>() );
			}
			
			if(!petDictionary[x].ContainsKey(y))
			{
				_petDictionaryInv.Insert( _tileType.Count, new List<int>() );
				_petDictionaryInv[_tileType.Count].Add(x);
				_petDictionaryInv[_tileType.Count].Add(y);
				
				petDictionary[x].Add( y ,_tileType.Count );
				_tileType.Add(tileType);
				_X.Add(x);
				_Y.Add(y);
			}
			
		}
		
		public void initList()
		{
			_tileType.Clear();
			_X.Clear();
			_Y.Clear();
			
			 petDictionary.Clear();
			_petDictionaryInv.Clear();
		}
		
		public void removeList(int x, int y)
		{
			if(!petDictionary.ContainsKey(x))
			{
				return;
			}
			
			if(!petDictionary[x].ContainsKey(y))
			{
				return;
			}
			
			int removeIndexNo = petDictionary[x][y];
			int rowNumber = _tileType.Count;
			
			_petDictionaryInv.RemoveAt(removeIndexNo);
			petDictionary[x].Remove( y );
			for(int indexNo=removeIndexNo; indexNo < rowNumber -1; indexNo++)
			{
				petDictionary[_petDictionaryInv[indexNo][0]][_petDictionaryInv[indexNo][1]]--;
			}
			
			_tileType.RemoveAt(removeIndexNo);
			_X.RemoveAt(removeIndexNo);
			_Y.RemoveAt(removeIndexNo);
		}
		
		public void removeList(int index)
		{
			if( index >= _petDictionaryInv.Count )
			{
				return;
			}
			
			int x = _petDictionaryInv[index][0];
			int y = _petDictionaryInv[index][1];
			
			removeList(x, y);
		}
	}
}
