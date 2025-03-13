using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ReLogic.Content;
using ReLogic.Content.Readers;
using ReLogic.Utilities;

using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Assets;

namespace Tomat.TML.Mod.NotQuiteNitrate.Patches;

internal sealed class FasterAssetLoading : ModSystem
{
    private sealed class FastRawimgReader(GraphicsDevice graphicsDevice) : IAssetReader
    {
        public async ValueTask<T> FromStream<T>(Stream stream, MainThreadCreationContext mainThreadCtx) where T : class
        {
            Debug.Assert(typeof(T) == typeof(Texture2D));

            var buf = (Span<byte>)stackalloc byte[12];
            {
                stream.ReadExactly(buf);
            }

            var width  = BinaryPrimitives.ReadInt32LittleEndian(buf[4..]);
            var height = BinaryPrimitives.ReadInt32LittleEndian(buf[8..]);

            var byteCount = width * height * 4;

            // if (byteCount < /* 256 * */ 1024)
            // {
            //     var data = (Span<byte>)stackalloc byte[byteCount];
            //     {
            //         stream.ReadExactly(data);
            //     }
            //     
            //     await mainThreadCtx;
            //
            //     var tex = new Texture2D(graphicsDevice, width, height);
            //     {
            //         fixed (byte* pData = data)
            //         {
            //             tex.SetDataPointerEXT(0, null, (nint)pData, byteCount);
            //         }
            //     }
            //
            //     return tex;
            // }

            var data = ArrayPool<byte>.Shared.Rent(byteCount);
            {
                await stream.ReadExactlyAsync(data, 0, byteCount);
            }

            await mainThreadCtx;

            var tex = new Texture2D(graphicsDevice, width, height);
            {
                tex.SetData(0, null, data, 0, byteCount);
            }

            ArrayPool<byte>.Shared.Return(data);
            return (tex as T)!;
        }
    }

    private IAssetReader? rawimgReader;

    public override void Load()
    {
        base.Load();

        // Disable thread checks.
        MonoModHooks.Add(
            typeof(ThreadCheck).GetMethod(nameof(ThreadCheck.CheckThread), BindingFlags.Public | BindingFlags.Static),
            () => { }
        );

        var readers = Main.instance.Services.Get<AssetReaderCollection>();
        {
            readers.TryGetReader(".rawimg", out rawimgReader);
            readers.RegisterReader(new FastRawimgReader(Main.instance.Services.Get<IGraphicsDeviceService>().GraphicsDevice), ".rawimg");
        }
    }

    public override void Unload()
    {
        base.Unload();

        var readers = Main.instance.Services.Get<AssetReaderCollection>();
        {
            readers.RegisterReader(rawimgReader ?? new RawImgReader(Main.instance.Services.Get<IGraphicsDeviceService>().GraphicsDevice), ".rawimg");
        }
    }
}