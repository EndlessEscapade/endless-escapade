using EEMod.Autoloading;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using EEMod.Extensions;
using System.Linq;
using System;
using EEMod.Effects;
using EEMod.Projectiles.Mage;
using static Terraria.ModLoader.ModContent;
using System.Reflection;
using EEMod.Projectiles.Ranged;
using EEMod.Projectiles.Melee;
using EEMod.NPCs.CoralReefs;
using EEMod.Projectiles;


namespace EEMod.Prim
{
	internal class WebPrimTrail : PrimTrail
	{
		private Vector2 start;

		private Vector2 mid;

		private Vector2 end;

		private int web;

		private Vector2 position;

		private bool isDoable;

		private Vector2[] pointCache = new Vector2[20];

		int down;
		int up;
		int down2;
		int up2;
		int down3;
		int up3;
		Vector2 p1;
		Vector2 p1Mid;
		Vector2 p1MidB;
		Vector2 p2;
		Vector2 p2Mid;
		Vector2 p2MidB;
		Vector2 p3;
		Vector2 p3Mid;
		Vector2 p3MidB;
		Vector2 p4;
		Vector2 p4Mid;
		Vector2 p4MidB;
		Vector2 p5;
		Vector2 p5Mid;
		Vector2 p5MidB;
		Vector2 p6;
		Vector2 p6Mid;
		Vector2 p6MidB;
		public WebPrimTrail(Projectile projectile, Vector2 position, int web)
			: base(projectile)
		{
			_projectile = projectile;
			this.web = web;
			this.position = position;
			behindTiles = false;
			for (int i = 0; i < 20; i++)
			{
				pointCache[i] = Helpers.TraverseBezier(end, start, mid, (float)_counter / 20f % 1f);
			}
		}

		public override void SetDefaults()
		{
			_alphaValue = 1f;
			_width = 3f;
			_cap = 20;
		}

		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			if (!(Vector2.DistanceSquared(Main.LocalPlayer.Center, position) < (float)(base.RENDERDISTANCE * base.RENDERDISTANCE)) || _noOfPoints <= 6)
				return;

			Math.Sin(_counter / 3f);
			for (int i = 0; i < _points.Count; i++)
			{
				Color _color = (isDoable ? Color.White : (Color.White * 0f));
				if (i == 0)
				{
					float widthVar = 0;
					if (web <= 5)
				    widthVar = _width;
					Vector2 normal2 = PrimTrail.CurveNormal(_points, i);
					Vector2 normalAhead2 = PrimTrail.CurveNormal(_points, i + 1);
					Vector2 firstUp2 = _points[i] - normal2 * widthVar;
					Vector2 firstDown2 = _points[i] + normal2 * widthVar;
					Vector2 secondUp2 = _points[i + 1] - normalAhead2 * widthVar;
					Vector2 secondDown2 = _points[i + 1] + normalAhead2 * widthVar;
					AddVertex(firstDown2, _color * _alphaValue, new Vector2(i / _cap, 1f));
					AddVertex(firstUp2, _color * _alphaValue, new Vector2(i / _cap, 0f));
					AddVertex(secondDown2, _color * _alphaValue, new Vector2((i + 1) / _cap, 1f));
					AddVertex(secondUp2, _color * _alphaValue, new Vector2((i + 1) / _cap, 0f));
					AddVertex(secondDown2, _color * _alphaValue, new Vector2((i + 1) / _cap, 1f));
					AddVertex(firstUp2, _color * _alphaValue, new Vector2(i / _cap, 0f));
				}
				else if (i != _points.Count - 1)
				{
					float widthVar = 0;
					if (web <= 5)
						widthVar = _width * (1 + ((i/ (float)_points.Count)* (i / (float)_points.Count) * (i / (float)_points.Count)) * 4f);
					else
                    {
						widthVar = _width;
					}
					Vector2 normal = CurveNormal(_points, i);
					Vector2 normalAhead = CurveNormal(_points, i + 1);
					Vector2 firstUp = _points[i] - normal * widthVar;
					Vector2 firstDown = _points[i] + normal * widthVar;
					Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;
					AddVertex(firstDown, _color * _alphaValue, new Vector2(i / (float)_cap, 1f));
					AddVertex(firstUp, _color * _alphaValue, new Vector2(i / (float)_cap, 0f));
					AddVertex(secondDown, _color * _alphaValue, new Vector2((i + 1) / (float)_cap, 1f));
					AddVertex(secondUp, _color * _alphaValue, new Vector2((i + 1) / (float)_cap, 0f));
					AddVertex(secondDown, _color * _alphaValue, new Vector2((i + 1) / (float)_cap, 1f));
					AddVertex(firstUp, _color * _alphaValue, new Vector2(i / (float)_cap, 0f));
				}
			}
		}

		public override void SetShaders()
		{
			if (Vector2.DistanceSquared(Main.LocalPlayer.Center, position) < (float)(base.RENDERDISTANCE * base.RENDERDISTANCE))
			{
				PrepareShader(EEMod.TrailPractice, "WebPass");
			}
		}

		public override void OnUpdate()
		{
			if (!(Vector2.DistanceSquared(Main.LocalPlayer.Center, position) < (float)(base.RENDERDISTANCE * base.RENDERDISTANCE)))
			{
				return;
			}
			float sineInt = (float)Main.GameUpdateCount / 100f;
			Vector2 tilePos = position / 16f;
			int spread = 13;
			float yield = 5f;
			if (Main.GameUpdateCount % 100 == 0)
			{
				down = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X + ((int)tilePos.Y % 5 - 2), (int)tilePos.Y, 1, 50);
				up = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X + ((int)tilePos.X % 5 - 2), (int)tilePos.Y, -1, 50);
				down2 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X - spread, (int)tilePos.Y, 1, 50);
				up2 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X - spread, (int)tilePos.Y, -1, 50);
				down3 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X + spread, (int)tilePos.Y, 1, 50);
				up3 = EEWorld.EEWorld.TileCheckVertical((int)tilePos.X + spread, (int)tilePos.Y, -1, 50);
				p1 = new Vector2(tilePos.X * 16f, down * 16);
				
			}
			p1Mid = Helpers.TraverseBezier(p1, position, Vector2.Lerp(p1, position, 0.5f) + new Vector2((float)Math.Cos(sineInt * 2f) * yield * 2f, 50f + (float)Math.Sin(sineInt * 2f) * 40f), 0.5f);
			p1MidB = Helpers.TraverseBezier(p1, position, Vector2.Lerp(p1, position, 0.5f) + new Vector2((float)Math.Cos(sineInt * 2f) * yield * 2f, 50f + (float)Math.Sin(sineInt * 2f) * 40f), 0.3f);
			p2 = new Vector2(tilePos.X * 16f, up * 16);
			p2Mid = Helpers.TraverseBezier(p2, position, Vector2.Lerp(p2, position, 0.5f) + new Vector2((float)Math.Cos(sineInt * 1.1f) * yield * 2f, 50f + (float)Math.Sin(sineInt * 2f) * 40f), 0.5f);
			p2MidB = Helpers.TraverseBezier(p2, position, Vector2.Lerp(p2, position, 0.5f) + new Vector2((float)Math.Cos(sineInt * 1.1f) * yield * 2f, 50f + (float)Math.Sin(sineInt * 2f) * 40f), 0.3f);
			p3 = new Vector2((tilePos.X - spread) * 16f, down2 * 16);
			p3Mid = Helpers.TraverseBezier(p3, position, Vector2.Lerp(p3, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 1.2f) * yield * 2f), 0.5f);
			p3MidB = Helpers.TraverseBezier(p3, position, Vector2.Lerp(p3, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 1.2f) * yield * 2f), 0.3f);
			p4 = new Vector2((tilePos.X - spread) * 16f, up2 * 16);
			p4Mid = Helpers.TraverseBezier(p4, position, Vector2.Lerp(p4, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 1.8f) * yield * 2f), 0.5f);
			p4MidB = Helpers.TraverseBezier(p4, position, Vector2.Lerp(p4, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 1.8f) * yield * 2f), 0.3f);
			p5 = new Vector2((tilePos.X + spread) * 16f, down3 * 16);
			p5Mid = Helpers.TraverseBezier(p5, position, Vector2.Lerp(p5, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 1.9f) * yield * 2f), 0.5f);
			p5MidB = Helpers.TraverseBezier(p5, position, Vector2.Lerp(p5, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 1.9f) * yield * 2f), 0.3f);
			p6 = new Vector2((tilePos.X + spread) * 16f, up3 * 16);
			p6Mid = Helpers.TraverseBezier(p6, position, Vector2.Lerp(p6, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 2.2f) * yield * 2f), 0.5f);
			p6MidB = Helpers.TraverseBezier(p6, position, Vector2.Lerp(p6, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 2.2f) * yield * 2f), 0.3f);
			switch (web)
			{
				case 0:
					if (p1.Y >= 1f)
					{
						end = p1;
						start = position;
						mid = Vector2.Lerp(p1, position, 0.5f) + new Vector2((float)Math.Cos(sineInt * 2f) * yield * 2f, 50f + (float)Math.Sin(sineInt * 2f) * 40f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 1:
					if (p2.Y >= 1f)
					{
						end = p2;
						start = position;
						mid = Vector2.Lerp(p2, position, 0.5f) + new Vector2((float)Math.Cos(sineInt * 1.1f) * yield * 2f, 50f + (float)Math.Sin(sineInt * 2f) * 40f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 2:
					if (p3.Y >= 1f)
					{
						end = p3;
						start = position;
						mid = Vector2.Lerp(p3, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 1.2f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 3:
					if (p4.Y >= 1f)
					{
						end = p4;
						start = position;
						mid = Vector2.Lerp(p4, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 1.8f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 4:
					if (p5.Y >= 1f)
					{
						end = p5;
						start = position;
						mid = Vector2.Lerp(p5, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 1.9f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 5:
					if (p6.Y >= 1f)
					{
						end = p6;
						start = position;
						mid = Vector2.Lerp(p6, position, 0.5f) + new Vector2(0f, 50f + (float)Math.Sin(sineInt * 2.2f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 6:
					if (p1.Y >= 1f && p5.Y >= 1f)
					{
						start = p1Mid;
						end = p5Mid;
						mid = Vector2.Lerp(p1Mid, p5Mid, 0.5f) + new Vector2(0f, -40f + (float)Math.Sin(sineInt * 3f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 7:
					if (p5.Y >= 1f && p6.Y >= 1f)
					{
						start = p5Mid;
						end = p6Mid;
						mid = Vector2.Lerp(p5Mid, p6Mid, 0.5f) + new Vector2(-40f + (float)Math.Sin(sineInt * 4f) * yield * 2f, 0f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 8:
					if (p6.Y >= 1f && p2.Y >= 1f)
					{
						start = p6Mid;
						end = p2Mid;
						mid = Vector2.Lerp(p6Mid, p2Mid, 0.5f) + new Vector2(0f, 40f + (float)Math.Sin(sineInt * 3f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 9:
					if (p2.Y >= 1f && p4.Y >= 1f)
					{
						start = p2Mid;
						end = p4Mid;
						mid = Vector2.Lerp(p2Mid, p4Mid, 0.5f) + new Vector2(0f, 40f + (float)Math.Sin(sineInt * 4f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 10:
					if (p4.Y >= 1f && p3.Y >= 1f)
					{
						start = p4Mid;
						end = p3Mid;
						mid = Vector2.Lerp(p4Mid, p3Mid, 0.5f) + new Vector2(40f + (float)Math.Sin(sineInt * 3f) * yield * 2f, 0f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 11:
					if (p3.Y >= 1f && p1.Y >= 1f)
					{
						start = p3Mid;
						end = p1Mid;
						mid = Vector2.Lerp(p3Mid, p1Mid, 0.5f) + new Vector2(0f, -40f + (float)Math.Sin(sineInt * 4f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 12:
					if (p1.Y >= 1f && p5.Y >= 1f)
					{
						start = p1MidB;
						end = p5MidB;
						mid = Vector2.Lerp(p1MidB, p5MidB, 0.5f) + new Vector2(0f, -40f + (float)Math.Sin(sineInt * 3f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 13:
					if (p5.Y >= 1f && p6.Y >= 1f)
					{
						start = p5MidB;
						end = p6MidB;
						mid = Vector2.Lerp(p5MidB, p6MidB, 0.5f) + new Vector2(-40f + (float)Math.Sin(sineInt * 4f) * yield * 2f, 0f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 14:
					if (p6.Y >= 1f && p2.Y >= 1f)
					{
						start = p6MidB;
						end = p2MidB;
						mid = Vector2.Lerp(p6MidB, p2MidB, 0.5f) + new Vector2(0f, 40f + (float)Math.Sin(sineInt * 3f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 15:
					if (p2.Y >= 1f && p4.Y >= 1f)
					{
						start = p2MidB;
						end = p4MidB;
						mid = Vector2.Lerp(p2MidB, p4MidB, 0.5f) + new Vector2(0f, 40f + (float)Math.Sin(sineInt * 4f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 16:
					if (p4.Y >= 1f && p3.Y >= 1f)
					{
						start = p4MidB;
						end = p3MidB;
						mid = Vector2.Lerp(p4MidB, p3MidB, 0.5f) + new Vector2(40f + (float)Math.Sin(sineInt * 3f) * yield * 2f, 0f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
				case 17:
					if (p3.Y >= 1f && p1.Y >= 1f)
					{
						start = p3MidB;
						end = p1MidB;
						mid = Vector2.Lerp(p3MidB, p1MidB, 0.5f) + new Vector2(0f, -40f + (float)Math.Sin(sineInt * 4f) * yield * 2f);
						isDoable = true;
					}
					else
					{
						isDoable = false;
					}
					break;
			}
			_counter++;
			for (int i = 0; i < _cap; i++)
			{
				pointCache[i] = Helpers.TraverseBezier(end, start, mid, i / ((float)_cap - 1f));
			}
			_points = pointCache.ToList();
			_noOfPoints = _points.Count() * 6;
			if (_cap < _noOfPoints / 6)
			{
				_points.RemoveAt(0);
			}
		}

		public override void OnDestroy()
		{
			_destroyed = true;
			_width *= 0.4f;
			if (_width < 0.05f)
			{
				Dispose();
			}
		}
	}
}