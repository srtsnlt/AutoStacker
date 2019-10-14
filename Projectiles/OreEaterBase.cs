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
	public class OreEaterBase : ModProjectile
	{
		string displayName="Ore Eater Base";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(displayName);
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
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
		
		
		public int maxSerchNum=60;
		System.Random rand = new System.Random();
		int prevLoop=0;
		
		int chestID = -1;
		int targetPrev=4;
		
		public int speed = 16*3;
		public float light = 0f;
		
		public override ModProjectile Clone()
		{
			OreEaterBase newInstance=(OreEaterBase)base.MemberwiseClone();
			newInstance.originX=this.originX;
			newInstance.originY=this.originY;
			newInstance.route_count=this.route_count;
			newInstance.route_count_shift=this.route_count_shift;
			newInstance.prevLoop=this.prevLoop;
			newInstance.targetPrev=this.targetPrev;
			
			return (ModProjectile)newInstance;
		}
		
		//public override void AI()
		//{
		//	if(!Terraria.Program.LoadedEverything )//&& Terraria.Main.tilesLoaded))
		//	{
		//		return;
		//	}
		//	
		//	Player player = Main.player[projectile.owner];
		//	Players.OreEater modPlayer = player.GetModPlayer<Players.OreEater>(mod);
		//	
		//	if(modPlayer.pet == null)
		//	{
		//		modPlayer.pet = new PetBase();
		//	}
		//	AI2(player, modPlayer, modPlayer.pet);
		//	
		//}
		
		public void AI2(Player player, Players.OreEater modPlayer, PetBase pet)
		{
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
				modPlayer.oreEaterEnable = false;
			}
			
			if (modPlayer.oreEaterEnable)
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
			Lighting.AddLight(projectile.position, 0.9f * light, 0.1f * light, 0.3f * light);
			
			
			//ore scan & move & pick 
			if(!pet.statusAIndex.ContainsKey(3) && !pet.statusAIndex.ContainsKey(4) )
			{
				if(pet.latestLoop >= maxSerchNum || prevLoop == pet.latestLoop || pet.statusA.Count == 0 )
				{
					if(pet.latestLoop >= maxSerchNum || prevLoop == pet.latestLoop)
					//if(prevLoop == pet.latestLoop)
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
				if(pet.statusAIndex.ContainsKey(3) )
				{
					target=3;
				}
				else if(pet.statusAIndex.ContainsKey(4) )
				{
					target=4;
				}
				else
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
				projectile.velocity.X = pet.routeListX[route_count]*16 - projectile.position.X -8;
				projectile.velocity.Y = pet.routeListY[route_count]*16 - projectile.position.Y -8;
				modPlayer.npc.position=projectile.position;
				
				//next cell
				if(route_count >= 0 && rand.Next( route_count * pickSpeed) <= speed )
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
						targetPrev=3;
					}
					else
					{
						if(targetPrev==3)
						{
							pet.initListA();
							pet.routeListX.Clear();
							pet.routeListY.Clear();
						}
						else
						{
							pet.statusA[pet.statusAIndex[4][0]]=5;
							pet.make_statusAIndex();
						}
						targetPrev=4;
					}
				}
			}
			modPlayer.npc.life=modPlayer.npc.lifeMax;
		}
	}
	
	public class PetBase
	{
		public PetBase()
		{
			initListA();
			make_statusAIndex();
			
		}
		
		private static Regex oreRegex =new Regex("Ore$|OreTile$",RegexOptions.Compiled);
		private static Regex gemRegex =new Regex("^Large",RegexOptions.Compiled);
		private static int _tileId = 0;
		private static Dictionary<int,bool> _oreTile = new Dictionary<int,bool>();
		Item _item = new Item();
		
		public void delayLoad()
		{
			if(_tileId == 0)
			{
				Main.NewText("AutoStacker[Ore Eater]:Item data loading...");
				_oreTile[TileID.ExposedGems] =true;
				_oreTile[TileID.Sapphire] =true;
				_oreTile[TileID.Ruby] =true;
				_oreTile[TileID.Emerald] =true;
				_oreTile[TileID.Topaz] =true;
				_oreTile[TileID.Amethyst] =true;
				_oreTile[TileID.Diamond] =true;
				_oreTile[TileID.Crystals] =true;
				_oreTile[TileID.Heart] =true;
				_oreTile[TileID.LifeFruit] =true;
				_oreTile[TileID.Pots] =true;
				_oreTile[TileID.Cobweb] =true;
				_oreTile[TileID.Heart] =true;
				_oreTile[TileID.LifeFruit] =true;
			}
			
			if(_tileId < Main.tileTexture.Length)
			{
				ModTile _tile = TileLoader.GetTile(_tileId);
				if(
					Terraria.ID.TileID.Sets.Ore[_tileId]
					||(_tile != null && _tile.Name != null && oreRegex.IsMatch(_tile.Name) )
				)
				{
					_oreTile[_tileId] =true;
				}
				_tileId += 1;
				if(_tileId == Main.tileTexture.Length)
				{
					//Main.recipe.Where( recipe => recipe.createItem.modItem != null && recipe.createItem.modItem.DisplayName != null && gemRegex.IsMatch( recipe.createItem.modItem.DisplayName.GetDefault() )).SelectMany( recipe => recipe.requiredItem ).Where(item => item.createTile != null && item.createTile != -1 ).Any(item => _oreTile[item.createTile] = true );
					Main.NewText("AutoStacker[Ore Eater]: Item data loading Complete!");
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
		
		public Dictionary<int,bool> oreTile
		{
			get
			{
				return _oreTile;
			}
			set
			{
				_oreTile=value;
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
			Tile tileUpper;
			
			//fined next tile
			if(_statusAIndex.ContainsKey(0))
			{
				foreach(int index in _statusAIndex[0])
				{
					int x=_AX[index];
					int y=_AY[index];
					for(int dX=-1; dX <= 1; dX++)
					{
						for(int dY=-1; dY <= 1; dY++ )
						{
							if(dY == 0 && dX == 0)
							{
								continue;
							}
							
							if(checkCanMove(index, dX, dY, pickPower)) 
							{
								addListA(x + dX, y + dY, 0, x, y, int.MaxValue );
							}
						}
					}
					_statusA[index] = 1;
				}
			}
			
			//find row cost route
			if(_statusAIndex.ContainsKey(1))
			{
				foreach(int index in _statusAIndex[1])
				{
					double cur = _nA[index];
					int x=_AX[index];
					int y=_AY[index];
					for(int dX=-1; dX <= 1; dX++)
					{
						for(int dY=-1; dY <= 1; dY++ )
						{
							if(
								(dY == 0 && dX == 0)
								|| !_petDictionaryA.ContainsKey(x + dX)
								|| !_petDictionaryA[x + dX].ContainsKey(y + dY)
								||
								(
									_statusA[_petDictionaryA[x + dX][y + dY]] != 0
									&& _statusA[_petDictionaryA[x + dX][y + dY]] != 1
									&& _statusA[_petDictionaryA[x + dX][y + dY]] != 2
								)
							)
							{
								continue;
							}
							double match;
							if(dX ==0 || dY ==0)
							{
								match = _nA[_petDictionaryA[x + dX][y + dY]] + 1d;
							}
							else
							{
								match = _nA[_petDictionaryA[x + dX][y + dY]] + root2;
							}
							
							if(match < cur)
							{
								_nA[index]=match;
								cur=match;
								_routeAX[index] = x + dX;
								_routeAY[index] = y + dY;
							}
						}
					}
					_statusA[index] = 2;
				}
			}
			
			//find player, ores...
			if(_statusAIndex.ContainsKey(2))
			{
				foreach(int index in _statusAIndex[2])
				{
					if(checkCanPick(index, pickPower))
					{
						_statusA[index] = 3;
					}
					else if
					(
						_AX[index] == (int)(Main.LocalPlayer.position.X / 16 )
						&& _AY[index] == (int)(Main.LocalPlayer.position.Y /16 ) //-4)
					)
					{
						_statusA[index] = 4;
					}
					else
					{
						_statusA[index] = 5;
					}
				}
			}
			
			make_statusAIndex();
			latestLoop += 1;
			serchA(originX, originY, serchTiles -1 ,resultMaxNum, resultMaxStatus, pickPower, false );
			
		}
		
		public virtual bool checkCanMove(int index, int dX, int dY,int pickPower)
		{
			Tile tile = Main.tile[_AX[index], _AY[index]];
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
								&& _oreTile.ContainsKey(tile.type)
							)
						)
					)
				)
			)
			{
			}
			else
			{
				return false;
			}

			if ((tile.type == 211 && pickPower < 200)
				|| ((tile.type == 25 || tile.type == 203) && pickPower < 65)
				|| (tile.type == 117 && pickPower < 65)
				|| (tile.type == 37 && pickPower < 50)
				|| (tile.type == 404 && pickPower < 65)
				|| ((tile.type == 22 || tile.type == 204) && (double)_AY[index] > Main.worldSurface && pickPower < 55)
				|| (tile.type == 56 && pickPower < 65)
				|| (tile.type == 58 && pickPower < 65)
				|| ((tile.type == 226 || tile.type == 237) && pickPower < 210)
				|| (Main.tileDungeon[tile.type] && pickPower < 65)
				|| ((double)_AX[index] < (double)Main.maxTilesX * 0.35 || (double)_AX[index] > (double)Main.maxTilesX * 0.65)
				|| (tile.type == 107 && pickPower < 100)
				|| (tile.type == 108 && pickPower < 110)
				|| (tile.type == 111 && pickPower < 150)
				|| (tile.type == 221 && pickPower < 100)
				|| (tile.type == 222 && pickPower < 110)
				|| (tile.type == 223 && pickPower < 150)
			)
			{
				return false;
			}

			int check=1;
			TileLoader.PickPowerCheck(tile, pickPower, ref check);
			if(check == 0)
			{
				return false;
			}
			
			return true;
		}
		
		private bool checkCanPick(int index, int pickPower)
		{
			if(index >= _AX.Count || index >= _AY.Count || index <= -1)
			{
				return false;
			}
			
			Tile tile = Main.tile[_AX[index], _AY[index]];
			Tile tileUpper = Main.tile[_AX[index], _AY[index]-1];

			if(tile == null )
			{
				return false;
			}

			//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/
			if ((tile.type == 211 && pickPower < 200)
				|| ((tile.type == 25 || tile.type == 203) && pickPower < 65)
				|| (tile.type == 117 && pickPower < 65)
				|| (tile.type == 37 && pickPower < 50)
				|| (tile.type == 404 && pickPower < 65)
//				|| ((tile.type == 22 || tile.type == 204) && (double)_AY[index] > Main.worldSurface && pickPower < 55)
				|| (tile.type == 56 && pickPower < 65)
				|| (tile.type == 58 && pickPower < 65)
				|| ((tile.type == 226 || tile.type == 237) && pickPower < 210)
				|| (Main.tileDungeon[tile.type] && pickPower < 65)
//				|| ((double)_AX[index] < (double)Main.maxTilesX * 0.35 || (double)_AX[index] > (double)Main.maxTilesX * 0.65)
				|| (tile.type == 107 && pickPower < 100)
				|| (tile.type == 108 && pickPower < 110)
				|| (tile.type == 111 && pickPower < 150)
				|| (tile.type == 221 && pickPower < 100)
				|| (tile.type == 222 && pickPower < 110)
				|| (tile.type == 223 && pickPower < 150)
			)
			{
				return false;
			}

			int check=1;
			TileLoader.PickPowerCheck(tile, pickPower, ref check);
			if(check == 0)
			{
				return false;
			}
			//_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/

			if (
				tile.active() 
				&& _oreTile.ContainsKey(tile.type) 
				&&
				(
					tileUpper ==  null
					||
					(
						tileUpper.type != TileID.Containers
						&&tileUpper.type != TileID.DemonAltar
					)
				)
			)
			{
				return true;
			}
			else
			{
				return false;
			}
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
					int x=routeListX[routeListX.Count-1];
					int y=routeListY[routeListY.Count-1];
					
					if(
						!_petDictionaryA.ContainsKey(x)
						|| !_petDictionaryA[x].ContainsKey(y)
					)
					{
						initListA();
						routeListX.Clear();
						routeListY.Clear();
						break;
					}
					
					{
						routeListX.Add( _routeAX[_petDictionaryA[x][y]] );
						routeListY.Add( _routeAY[_petDictionaryA[x][y]] );
					
						if(_nA[_petDictionaryA[routeListX[routeListX.Count-1]][routeListY[routeListY.Count-1]]] == 0)
						{
							break;
						}
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
