using EEMod.Config;
using EEMod.Effects;
using EEMod.Extensions;
using EEMod.ID;
using EEMod.Projectiles;
using EEMod.Tiles;
using EEMod.Tiles.Furniture;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Reflection;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.Social;
using Terraria.UI;
using EEMod.Prim;
using EEMod.Seamap.Core;
using MonoMod.RuntimeDetour.HookGen;
using EEMod.Systems;
using Terraria.GameContent.Liquid;
using EEMod.Seamap;
using MonoMod.Utils;
using Mono.Cecil;
using Terraria.UI.Chat;

namespace EEMod
{
    public partial class EEMod : Mod
    {
        private delegate void ModifyWaterColorDelegate(ref VertexColors colors);

        private Vector2 _sunPos;
        private float _globalAlpha;
        private float _intensityFunction;
        private Vector2 _sunShaderPos;
        private float _nightHarshness = 1f;
        private Color _baseColor;
        public float alpha;

        /// <summary>
        /// Instance for adding and handling il hooks
        /// </summary>
        internal ILHookList hooklist;

        private void LoadIL()
        {
            IL.Terraria.IO.WorldFile.SaveWorldTiles += WorldFile_SaveWorldTiles;
            //IL.Terraria.GameContent.Drawing.TileDrawing.DrawTile_LiquidBehindTile += TileDrawing_DrawTile_LiquidBehindTile;
            //IL.Terraria.GameContent.Liquid.LiquidRenderer.InternalPrepareDraw += LiquidRenderer_InternalPrepareDraw1;

            hooklist = new ILHookList();

            //WorldGenBeaches();
        }

        private void UnloadIL()
        {
            IL.Terraria.IO.WorldFile.SaveWorldTiles -= WorldFile_SaveWorldTiles;
            //IL.Terraria.GameContent.Drawing.TileDrawing.DrawTile_LiquidBehindTile -= TileDrawing_DrawTile_LiquidBehindTile;
            //IL.Terraria.GameContent.Liquid.LiquidRenderer.InternalPrepareDraw -= LiquidRenderer_InternalPrepareDraw1;

            hooklist?.UnloadAll();
            hooklist?.Dispose();
            hooklist = null;
        }

        /*private void TileDrawing_DrawTile_LiquidBehindTile(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (c.TryGotoNext(i => i.MatchLdsfld<Main>("worldSurface")))
            {
                var cLdfld = c.Clone();
                if (c.Previous.MatchLdfld(out _))
                {
                    cLdfld.Emit(OpCodes.Pop);
                    cLdfld.Emit(OpCodes.Ldc_R4, 0f);
                    c.Emit(OpCodes.Pop);
                    c.Emit(OpCodes.Ldc_R4, 0f);
                }
                else
                {
                    throw new Exception("didn't match bixh");
                }
            }
        }*/

        private void WorldFile_SaveWorldTiles(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!c.TryGotoNext(i => i.MatchLdsfld<Main>("tile")))
                return;
            if (!c.TryGotoNext(i => i.MatchCall(out _))) // lazy solution but hueh
                return;
            //c.Index++;
            c.Remove();
            c.EmitDelegate<Func<Tile[,], int, int, Tile>>((arrae, i, j) => Framing.GetTileSafely(i, j));
        }

        /*private void LiquidRenderer_InternalPrepareDraw1(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            if (!c.TryGotoNext(i => i.MatchLdloc(41), j => j.MatchLdloc(4), k => k.MatchLdfld(typeof(LiquidRenderer).GetNestedType("LiquidCache").GetField("HasWall")), l => l.MatchBrtrue(out ILLabel _)))
                return;

            c.Index++;

            c.Emit(OpCodes.Box, );

            c.EmitDelegate<Func<object, bool>>((ptr2) =>
            {
                //ptr4->IsVisible = 

                return (ptr2->HasWall || !ptr2->IsHalfBrick || !ptr2->HasLiquid || !(ptr2->LiquidLevel < 1f) || !(!solidLayer || drawData.tileCache.inActive() || _tileSolidTop[drawData.typeCache] || (drawData.tileCache.halfBrick() && (height.liquid > 160 || num.liquid > 160) && Main.instance.waterfallManager.CheckForWaterfall(tileX, tileY)) || (TileID.Sets.BlocksWaterDrawingBehindSelf[drawData.tileCache.type] && drawData.tileCache.slope() == 0)));
            });
        }*/

        /*private void WorldGenBeaches()
        {
            /*
                MethodInfo genWorld = typeof(WorldGen).GetMethod(nameof(WorldGen.GenerateWorld));
                using (var dmd = new DynamicMethodDefinition(genWorld))
                {
                    ILCursor c = new ILCursor(new ILContext(dmd.Definition));
                    MethodReference methodReference = null;

                    if (!c.TryGotoNext(i => i.MatchLdstr("Beaches"),
                        i => i.MatchLdloc(out _),
                        i => i.MatchLdftn(out methodReference)
                        ))
                        throw new Exception("Could not match beaches generation delegate");

                    if (methodReference == null)
                        throw new Exception("Method reference for the delegate was null");

                    MethodBase delegateMethodBase = methodReference.ResolveReflection();
                    if (delegateMethodBase == null)
                        throw new Exception("Resolved method base for beaches generation delegate was null");

                    hooklist.Add((MethodInfo)delegateMethodBase, new ILContext.Manipulator(IL_WorldgenPass_Beaches));
                }
             
        }*/

        // NOTE: the indexes could break after updating
        private static void IL_WorldgenPass_Beaches(ILContext il)
        {
            ILCursor c = new(il);

            /*    
                IL_002f: brtrue.s IL_0035

                // floridaStyle = true;
                IL_0031: ldc.i4.1
                IL_0032: stloc.0
                // floridaStyle2 = true;
                IL_0033: br.s IL_0037

                IL_0035: ldc.i4.1
                IL_0036: stloc.1


            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdcI4(1),
                i => i.MatchStloc(0)
                ))
                throw new Exception("Could not find Ldc.i4 1 and Stloc 0 instructions");

            // after the stloc
            c.Emit(OpCodes.Ldc_I4_0);
            c.Emit(OpCodes.Stloc_S, (byte)0); // (_S means the operand is 1 byte)
                                              // the statement would look like floridaStyle = false;
        }*/
        }
        

        public class ILHook : IDisposable
        {
            MethodInfo _to;
            Delegate _manipulator;
            bool _applied;
            private bool disposed;
            public ILHook(MethodInfo to, ILContext.Manipulator manipulator, bool apply = true)
            {
                _to = to;
                _manipulator = manipulator;
                if (apply)
                    Apply();
            }
            public void Apply()
            {
                if (_applied)
                    return;
                if (disposed)
                    throw new ObjectDisposedException("Cannot apply a disposed ILHook");

                HookEndpointManager.Modify(_to, _manipulator);
                _applied = true;
                OnApply?.Invoke();
            }
            public void Unapply()
            {
                if (!_applied)
                    return;
                HookEndpointManager.Unmodify(_to, _manipulator);
                _applied = false;
                OnUnapply?.Invoke();
            }
            public event Action OnDispose;
            public event Action OnApply;
            public event Action OnUnapply;
            public void Dispose() => Dispose(true);
            protected virtual void Dispose(bool disposing)
            {
                if (disposed)
                    return;

                if (disposing)
                {
                    OnDispose?.Invoke();
                }
                Unapply();
                _to = null;
                _manipulator = null;
                OnDispose = null;
                OnApply = null;
                OnUnapply = null;

                disposed = true;
            }
            ~ILHook() => Dispose(false);
        }

        public class ILHookList : IDisposable
        {
            public IList<ILHook> HookList = new List<ILHook>();
            private bool disposed;
            public ILHook Add(MethodInfo to, ILContext.Manipulator manipulator, bool apply = true)
            {
                var ilhook = new ILHook(to, manipulator, apply);
                HookList.Add(ilhook);
                return ilhook;
            }
            public ILHook Add(ILContext.Manipulator manipulator, Type targetType, string methodname, BindingFlags flags = Helpers.FlagsALL, params Type[] paramTypes) => Add(targetType.GetMethod(methodname, flags, null, paramTypes, null), manipulator);
            public ILHook Add<TType>(ILContext.Manipulator manipulator, string methodname, BindingFlags flags = Helpers.FlagsALL) => Add(typeof(TType).GetMethod(methodname, flags), manipulator);
            public ILHook Add<T0>(ILContext.Manipulator manipulator, Type targetType, string methodname, BindingFlags flags = Helpers.FlagsALL) => Add(manipulator, targetType, methodname, flags, typeof(T0));
            public ILHook Add<T0, T1>(ILContext.Manipulator manipulator, Type targetType, string methodname, BindingFlags flags = Helpers.FlagsALL) => Add(manipulator, targetType, methodname, flags, typeof(T0), typeof(T1));
            public ILHook Add<T0, T1, T2>(ILContext.Manipulator manipulator, Type targetType, string methodname, BindingFlags flags = Helpers.FlagsALL) => Add(manipulator, targetType, methodname, flags, typeof(T0), typeof(T1), typeof(T2));
            public ILHook Add<T0, T1, T2, T3>(ILContext.Manipulator manipulator, Type targetType, string methodname, BindingFlags flags = Helpers.FlagsALL) => Add(manipulator, targetType, methodname, flags, typeof(T0), typeof(T1), typeof(T2), typeof(T3));
            public ILHook Add<T0, T1, T2, T3, T4>(ILContext.Manipulator manipulator, Type targetType, string methodname, BindingFlags flags = Helpers.FlagsALL) => Add(manipulator, targetType, methodname, flags, typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));
            public ILHook Add<T0, T1, T2, T3, T4, T5>(ILContext.Manipulator manipulator, Type targetType, string methodname, BindingFlags flags = Helpers.FlagsALL) => Add(manipulator, targetType, methodname, flags, typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
            public ILHook Add<T0, T1, T2, T3, T4, T5, T6>(ILContext.Manipulator manipulator, Type targetType, string methodname, BindingFlags flags = Helpers.FlagsALL) => Add(manipulator, targetType, methodname, flags, typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));
            public ILHook Add<T0, T1, T2, T3, T4, T5, T6, T7>(ILContext.Manipulator manipulator, Type targetType, string methodname, BindingFlags flags = Helpers.FlagsALL) => Add(manipulator, targetType, methodname, flags, typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));
            public ILHook Add<T0, T1, T2, T3, T4, T5, T6, T7, T8>(ILContext.Manipulator manipulator, Type targetType, string methodname, BindingFlags flags = Helpers.FlagsALL) => Add(manipulator, targetType, methodname, flags, typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));
            public void UnloadAll()
            {
                if (HookList == null)
                    return;
                foreach (var hook in HookList)
                    hook?.Dispose();
                HookList.Clear();
            }
            public void Dispose() => Dispose(true);
            protected virtual void Dispose(bool disposing)
            {
                if (disposed) return;
                if (disposing)
                {

                }
                UnloadAll();
                HookList?.Clear();
                HookList = null;
                disposed = true;
            }
            ~ILHookList() => Dispose(false);
        }
    }
}
