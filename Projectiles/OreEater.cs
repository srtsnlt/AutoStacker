using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
			//ProjectileID.Sets.LightPet[projectile.type] = true;
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
		const int fullBrightTicks = 2;
		const int fadeOutTicks = 30;
		const int range = 10;
		int rangeHypoteneus = (int)Math.Sqrt(range * range + range * range);
		
		int originX=0;
		int originY=0;
		
		int route_count = -1;
		int route_count_shift = 0;
		
		
		int maxSerchNum=60;
		System.Random rand = new System.Random();
		int prevLoop=0;
		
		int chestID = -1;
		
		
		
		public override ModProjectile Clone()
		{
			OreEater newInstance=(OreEater)base.MemberwiseClone();
			newInstance.originX=this.originX;
			newInstance.originY=this.originY;
			newInstance.route_count=this.route_count;
			newInstance.route_count_shift=this.route_count_shift;
			newInstance.prevLoop=this.prevLoop;
			
			return (ModProjectile)newInstance;
		}

		
		
		public override void AI()
		{
			if(!Terraria.Program.LoadedEverything )//&& Terraria.Main.tilesLoaded))
			{
				return;
			}
			
			Player player = Main.player[projectile.owner];
			Players.OreEater modPlayer = player.GetModPlayer<Players.OreEater>(mod);
			
			if(modPlayer.pet == null)
			{
				modPlayer.pet = new Pet();
			}
			Pet pet=modPlayer.pet;
			pet.delayLoad();
			
			if (!player.active)
			{
				Main.npc[modPlayer.index].active=false;
				projectile.active = false;
				pet.initListA();
				pet.routeListX.Clear();
				pet.routeListY.Clear();
				projectile.position=player.position;
				return;
			}
			if (player.dead)
			{
				modPlayer.oreEater = false;
			}
			
			if (modPlayer.oreEater)
			{
				projectile.timeLeft = 2;
			}
			else
			{
				modPlayer.ResetEffects();
				Main.npc[modPlayer.index].active=false;
				pet.initListA();
				pet.routeListX.Clear();
				pet.routeListY.Clear();
				projectile.position=player.position;
			}
			
			//scan pickel
			int pickPower = Main.LocalPlayer.inventory.Max(item => item.pick);
			if( pickPower <= 1)
			{
				pickPower = 1;
			}
			
			int pickSpeed ;
			if(Main.LocalPlayer.inventory.Where(item => item.pick > 0).Count() == 0)
			{
				pickSpeed = 30;
			}
			else
			{
				pickSpeed = Main.LocalPlayer.inventory.Where(item => item.pick > 0).Min(item => item.useTime);
			}
			if( pickSpeed <= 1)
			{
				pickSpeed = 1;
			}
			
			
			//light
			Lighting.AddLight(projectile.position, 0.9f, 0.1f, 0.3f);
			
			
			//ore scan & move & pick 
			if(!pet.statusAIndex.ContainsKey(3) )// && !pet.statusAIndex.ContainsKey(4))
			{
				
				if(pet.latestLoop >= maxSerchNum || prevLoop == pet.latestLoop || pet.statusA.Count == 0 )
				{
					if(pet.latestLoop >= maxSerchNum || prevLoop == pet.latestLoop)
					{
						projectile.position=player.position;
						modPlayer.npc.position=projectile.position;
					}
					originX=(int)projectile.position.X/16;
					originY=(int)projectile.position.Y/16;
					prevLoop=0;
					route_count_shift=0;
					pet.serchA(originX,originY , 12, 2, 3, pickPower, true);
				}
				else
				{
					prevLoop=pet.latestLoop;
					pet.serchA(originX,originY , 1, 1, 3, pickPower, false);
					pet.serchA(originX,originY , 2, 1, 3, pickPower, false);
				}
			}
			else
			{
				int target=-1;
				if(pet.statusAIndex.ContainsKey(4) )
				{
					target=4;
				}
				if(pet.statusAIndex.ContainsKey(3) )
				{
					target=3;
				}
				if(target == -1)
				{
					return;
				}
				
				//makeRoute
				if(route_count == -1)
				{
					pet.makeRoute(target, 0, maxSerchNum);
					route_count=pet.routeListX.Count -1;
					if(target==4)
					{
						route_count = route_count - route_count_shift;
						route_count_shift=route_count;
					}
				}
				
				//set velocity
				projectile.velocity.X = pet.routeListX[route_count]*16 - projectile.position.X;;
				projectile.velocity.Y = pet.routeListY[route_count]*16 - projectile.position.Y;
				modPlayer.npc.position=projectile.position;
				
				//next cell
				if(route_count >= 0 && rand.Next( route_count * pickSpeed) <= 16*3 )
				{
					route_count -= 1;
				}
				
				
				//end route
				if(route_count == -1)
				{
					projectile.position.X=pet.routeListX[0]*16;
					projectile.position.Y=pet.routeListY[0]*16;
					projectile.velocity.X = 0;
					projectile.velocity.Y = 0;
					if(target == 3)
					{
						modPlayer.player.PickTile(pet.AX[pet.statusAIndex[3][0]], pet.AY[pet.statusAIndex[3][0]], pickPower);
						pet.initListA();
						pet.routeListX.Clear();
						pet.routeListY.Clear();
					}
					else
					{
						pet.statusA[pet.statusAIndex[4][0]]=5;
						pet.make_statusAIndex();
					}
				}
			}
		}
	}
	
	public class Pet
	{
		public Pet()
		{
			initListA();
			make_statusAIndex();
			
			//gemItemId =(List<int>)Main.recipe.Where( recipe => gemRegex.IsMatch( recipe.createItem.Name)).SelectMany( recipe=> recipe.requiredItem ).Select( item => item.type ).Cast<List<int>>();
			
		}
		
		private static Regex oreRegex=new Regex(" Ore$",RegexOptions.Compiled);
		private static Regex gemRegex=new Regex("^Large [a-zA-z]*$",RegexOptions.Compiled);
		private static List<int> gemItemId = new List<int>();
		private static Item _item = new Item();
		private static int itemId = 0;
		private static Dictionary<int,bool> _oreTile = new Dictionary<int,bool>();
		
		public void delayLoad()
		{
			if(itemId < Main.itemTexture.Length)
			{
				for(int count=0; count<50 && itemId < Main.itemTexture.Length; count++)
				{
					_item.SetDefaults(itemId);
					if(
						_item.createTile != -1 
						&& 
						(
							oreRegex.IsMatch(_item.Name) 
							|| gemItemId.Any( id => id == _item.type )
							|| _item.Name == "Cobweb"
						)
					)
					{
						_oreTile[_item.createTile] =true;
					}
					else
					{
						_oreTile[_item.createTile] =false;
					}
					itemId += 1;
				}
			}
		}
		
		
		private Dictionary<int,Dictionary<int,int>> _petDictionaryA    = new Dictionary<int,Dictionary<int,int>>();
		private List<List<int>>                     _petDictionaryAInv = new List<List<int>>();
		
		private List<int>                           _AX                = new List<int>();
		private List<int>                           _AY                = new List<int>();
		private List<int>                           _statusA           = new List<int>();
		private List<int>                           _routeAX           = new List<int>();
		private List<int>                           _routeAY           = new List<int>();
		private List<double>                        _nA                = new List<double>();
		
		
		private double                              root2              = Math.Sqrt(2);
		public  int                                 latestLoop         = 0;
		
		private Dictionary<int,List<int>>           _statusAIndex      = new Dictionary<int,List<int>>();
		
		
		public List<List<int>> petDictionaryAInv
		{
			get
			{
				return _petDictionaryAInv;
			}
		}
		
		public Dictionary<int,Dictionary<int,int>> petDictionaryA
		{
			get
			{
				return _petDictionaryA;
			}
		}
		
		public List<int> AX
		{
			get
			{
				return _AX;
			}
		}
		
		public List<int> AY
		{
			get
			{
				return _AY;
			}
		}
		
		public List<int> statusA
		{
			get
			{
				return _statusA;
			}
		}
		
		
		public List<int> routeAX
		{
			get
			{
				return _routeAX;
			}
		}
		
		public List<int> routeAY
		{
			get
			{
				return _routeAY;
			}
		}
		
		public List<double> nA
		{
			get
			{
				return _nA;
			}
		}
		
		public Dictionary<int,List<int>> statusAIndex
		{
			get
			{
				return _statusAIndex;
			}
			set
			{
				_statusAIndex=value;
			}
		}
		
		
		public void serchA(int originX, int originY, int serchTiles,int resultMaxNum, int resultMaxStatus, int pickPower, bool reset=false)
		{
			if(reset)
			{
				initListA();
				latestLoop = 0;
			}
			
			if(serchTiles <= 0)
			{
				return;
			}
			if( !_petDictionaryA.ContainsKey(originX) || !_petDictionaryA[originX].ContainsKey(originY) )
			{
				addListA(originX, originY, 0, originX, originY, 0);
				make_statusAIndex();
			}
			
			if( !_statusAIndex.ContainsKey(0) && !_statusAIndex.ContainsKey(1))
			{
				return;
			}
			
			if(_statusAIndex.ContainsKey(resultMaxStatus) && statusAIndex[resultMaxStatus].Count >= resultMaxNum)
			{
				return;
			}
			
			Tile tile;
			
			//fined next tile
			if(_statusAIndex.ContainsKey(0))
			{
				foreach(int index in _statusAIndex[0])
				{
					for(int dX=-1; dX <= 1; dX++)
					{
						for(int dY=-1; dY <= 1; dY++ )
						{
							if(dY == 0 && dX == 0)
							{
								continue;
							}
							
							tile = Main.tile[_AX[index], _AY[index]];
							
							if
							(
								(
									!_petDictionaryA.ContainsKey(_AX[index] + dX) 
									|| !_petDictionaryA[_AX[index] + dX].ContainsKey(_AY[index] + dY) 
								)
								&& _AX[index] + dX < Main.Map.MaxWidth
								&& _AX[index] + dX > 1
								&& _AY[index] + dY < Main.Map.MaxHeight
								&& _AY[index] + dY > 1
								&& Main.Map.IsRevealed(_AX[index] + dX,_AY[index] + dY)
								&&
								(
									tile == null 
									||
									(
										tile != null 
										&&
										(
											!tile.active()
											||
											(
												tile.active() 
												&& 
												(
													(
														_oreTile.ContainsKey(tile.type)
														&& _oreTile[tile.type]
													)
													|| tile.type == TileID.ExposedGems
													|| tile.type == TileID.Sapphire
													|| tile.type == TileID.Ruby
													|| tile.type == TileID.Emerald
													|| tile.type == TileID.Topaz
													|| tile.type == TileID.Amethyst
													|| tile.type == TileID.Diamond
													|| tile.type == TileID.Cobweb
													|| tile.type == TileID.Pots
													|| tile.type == TileID.Heart
													|| tile.type == TileID.LifeFruit
													
												)
											)
										)
									)
								)
							)
							{
								//add node
								addListA(_AX[index] + dX, _AY[index] + dY, 0, _AX[index], _AY[index], int.MaxValue );
							}
						}
					}
					_statusA[_petDictionaryA[_AX[index]][_AY[index]]] = 1;
				}
			}
			
			//find row cost route
			if(_statusAIndex.ContainsKey(1))
			{
				foreach(int index in _statusAIndex[1])
				{
					for(int dX=-1; dX <= 1; dX++)
					{
						for(int dY=-1; dY <= 1; dY++ )
						{
							if(
								dY == 0 && dX == 0
								|| !_petDictionaryA.ContainsKey(_AX[index] + dX)
								|| !_petDictionaryA[_AX[index] + dX].ContainsKey(_AY[index] + dY)
							)
							{
								continue;
							}
							double cur   = _nA[_petDictionaryA[_AX[index] + dX][_AY[index] + dY]];
							double match = _nA[_petDictionaryA[_AX[index]][_AY[index]]] + (double)( dX ==0 || dY ==0 ? 1 : root2);
							if(match <= cur)
							{
								_routeAX[_petDictionaryA[_AX[index]][_AY[index]]] = _AX[index];
								_routeAY[_petDictionaryA[_AX[index]][_AY[index]]] = _AY[index];
							}
						}
					}
					_statusA[_petDictionaryA[_AX[index]][_AY[index]]] = 2;
				}
			}
			
			//find player, ores...
			if(_statusAIndex.ContainsKey(2))
			{
				foreach(int index in _statusAIndex[2])
				{
					tile = Main.tile[_AX[index], _AY[index]];
					if (
						tile != null 
						&& tile.active() 
						&& 
						(
							(
								_oreTile.ContainsKey(tile.type) 
								&& _oreTile[tile.type]
							)
							|| tile.type == TileID.ExposedGems
							|| tile.type == TileID.Sapphire
							|| tile.type == TileID.Ruby
							|| tile.type == TileID.Emerald
							|| tile.type == TileID.Topaz
							|| tile.type == TileID.Amethyst
							|| tile.type == TileID.Diamond
							|| tile.type == TileID.Cobweb
							|| tile.type == TileID.Pots
							|| tile.type == TileID.Heart
							|| tile.type == TileID.LifeFruit
						)
						
					)
					{
						_statusA[_petDictionaryA[_AX[index]][_AY[index]]] = 3;
					}
					else if
					(
						_AX[index] == (int)(Main.LocalPlayer.position.X / 16 )
						&& _AY[index] == (int)(Main.LocalPlayer.position.Y /16 ) //-4)
					)
					{
						_statusA[_petDictionaryA[_AX[index]][_AY[index]]] = 4;
					}
					else
					{
						_statusA[_petDictionaryA[_AX[index]][_AY[index]]] = 5;
					}
				}
			}
			
			make_statusAIndex();
			latestLoop += 1;
			serchA(originX, originY, serchTiles -1 ,resultMaxNum, resultMaxStatus, pickPower, false );
			
		}
		
		
		public List<int> routeListX = new List<int>();
		public List<int> routeListY = new List<int>();
		public void makeRoute(int status,int routeNo, int maxSerchNum)
		{
			routeListX.Clear();
			routeListY.Clear();
			if(_statusAIndex.ContainsKey(status) && _statusAIndex[status].Count > routeNo)
			{
				routeListX.Add(_AX[_statusAIndex[status][routeNo]]);
				routeListY.Add(_AY[_statusAIndex[status][routeNo]]);
				for(int count=0; count <= maxSerchNum;count ++)
				{
					routeListX.Add( _routeAX[_petDictionaryA[routeListX[routeListX.Count-1]][routeListY[routeListY.Count-1]]]);
					routeListY.Add( _routeAY[_petDictionaryA[routeListX[routeListX.Count-1]][routeListY[routeListY.Count-1]]]);
					if(_nA[_petDictionaryA[routeListX[routeListX.Count-1]][routeListY[routeListY.Count-1]]] == 0)
					{
						break;
					}
				}
			}
		}
		
		
		
		private void addListA(int x, int y, int status,int routeX,int routeY, double n)
		{
			if(!_petDictionaryA.ContainsKey(x))
			{
				_petDictionaryA.Add(x, new Dictionary<int,int>() );
			}
			
			if(!_petDictionaryA[x].ContainsKey(y))
			{
				_petDictionaryAInv.Insert( _statusA.Count, new List<int>() );
				_petDictionaryAInv[_statusA.Count].Add(x);
				_petDictionaryAInv[_statusA.Count].Add(y);
				
				_petDictionaryA[x][y]=_statusA.Count;
				
				_AX.Add(x);
				_AY.Add(y);
				_statusA.Add(status);
				_routeAX.Add(routeX);
				_routeAY.Add(routeY);
				_nA.Add(n);
			}
			
		}
		
		public void initListA()
		{
			_AX.Clear();
			_AY.Clear();
			_statusA.Clear();
			_routeAX.Clear();
			_routeAY.Clear();
			_nA.Clear();
			
			_statusAIndex.Clear();
			
			_petDictionaryA.Clear();
			_petDictionaryAInv.Clear();
			
			make_statusAIndex();
		}
		
		public void removeListA(int x, int y)
		{
			if(!_petDictionaryA.ContainsKey(x))
			{
				return;
			}
			
			if(!_petDictionaryA[x].ContainsKey(y))
			{
				return;
			}
			
			int removeIndexNo = _petDictionaryA[x][y];
			int rowNumber = _statusA.Count;
			
			_petDictionaryAInv.RemoveAt(removeIndexNo);
			_petDictionaryA[x].Remove( y );
			for(int indexNo=removeIndexNo; indexNo < rowNumber -1; indexNo++)
			{
				_petDictionaryA[_petDictionaryAInv[indexNo][0]][_petDictionaryAInv[indexNo][1]]--;
			}
			
			_AX.RemoveAt(removeIndexNo);
			_AY.RemoveAt(removeIndexNo);
			_statusA.RemoveAt(removeIndexNo);
			_routeAX.RemoveAt(removeIndexNo);
			_routeAY.RemoveAt(removeIndexNo);
			_nA.RemoveAt(removeIndexNo);
			
		}
		
		public void removeListA(int index)
		{
			if( index >= _petDictionaryAInv.Count )
			{
				return;
			}
			
			int x = _petDictionaryAInv[index][0];
			int y = _petDictionaryAInv[index][1];
			
			removeListA(x, y);
		}
		
		public void make_statusAIndex()
		{
			_statusAIndex.Clear();
			int rowNo=0;
			foreach (int status in _statusA)
			{
				if(!_statusAIndex.ContainsKey(status))
				{
					_statusAIndex[status] = new List<int>();
				}
				_statusAIndex[status].Add(rowNo);
				rowNo += 1;
			}
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
	}
}
