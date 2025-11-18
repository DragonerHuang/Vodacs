using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;

namespace Vodace.Core.Utilities
{
    /// <summary>
    /// 图片缩略图帮助类（使用 System.Drawing，免费可商用，Windows 环境原生支持）
    /// </summary>
    public static class ImageHelper
    {
        public enum ThumbnailMode
        {
            /// <summary>
            /// 等比缩放，最大不超过指定宽高（不裁剪）
            /// </summary>
            Fit,
            /// <summary>
            /// 填充指定宽高，居中裁剪（可能裁剪边缘）
            /// </summary>
            Crop
        }

        /// <summary>
        /// 从文件生成缩略图并保存到指定路径
        /// </summary>
        public static void CreateThumbnail(string inputPath, string outputPath, int width, int height, ThumbnailMode mode = ThumbnailMode.Fit, int jpegQuality = 85, bool keepFormat = true)
        {
            using (var fs = File.OpenRead(inputPath))
            {
                var bytes = CreateThumbnail(fs, width, height, mode, jpegQuality, keepFormat);
                var dir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllBytes(outputPath, bytes);
            }
        }

        /// <summary>
        /// 从字节生成缩略图（返回字节）
        /// </summary>
        public static byte[] CreateThumbnail(byte[] inputBytes, int width, int height, ThumbnailMode mode = ThumbnailMode.Fit, int jpegQuality = 85, bool keepFormat = true)
        {
            using (var ms = new MemoryStream(inputBytes))
            {
                return CreateThumbnail(ms, width, height, mode, jpegQuality, keepFormat);
            }
        }

        /// <summary>
        /// 从流生成缩略图（返回字节）
        /// </summary>
        public static byte[] CreateThumbnail(Stream inputStream, int width, int height, ThumbnailMode mode = ThumbnailMode.Fit, int jpegQuality = 85, bool keepFormat = true)
        {
            using (var img = Image.FromStream(inputStream, useEmbeddedColorManagement: true, validateImageData: true))
            {
                TryApplyOrientation(img);
                using (var thumb = mode == ThumbnailMode.Crop ? ResizeCrop(img, width, height) : ResizeFit(img, width, height))
                {
                    var format = GetSaveFormat(img, keepFormat);
                    using (var outMs = new MemoryStream())
                    {
                        if (format.Guid == ImageFormat.Jpeg.Guid)
                        {
                            var enc = GetEncoder(ImageFormat.Jpeg);
                            using (var ep = BuildJpegEncoderParams(jpegQuality))
                            {
                                thumb.Save(outMs, enc, ep);
                            }
                        }
                        else
                        {
                            thumb.Save(outMs, format);
                        }
                        return outMs.ToArray();
                    }
                }
            }
        }

        private static Bitmap ResizeFit(Image src, int maxWidth, int maxHeight)
        {
            if (maxWidth <= 0 || maxHeight <= 0) throw new ArgumentOutOfRangeException("width/height must be > 0");
            var ratio = Math.Min((double)maxWidth / src.Width, (double)maxHeight / src.Height);
            var w = Math.Max(1, (int)Math.Round(src.Width * ratio));
            var h = Math.Max(1, (int)Math.Round(src.Height * ratio));
            var bmp = new Bitmap(w, h);
            bmp.SetResolution(src.HorizontalResolution, src.VerticalResolution);
            using (var g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.DrawImage(src, new Rectangle(0, 0, w, h));
            }
            return bmp;
        }

        private static Bitmap ResizeCrop(Image src, int targetWidth, int targetHeight)
        {
            if (targetWidth <= 0 || targetHeight <= 0) throw new ArgumentOutOfRangeException("width/height must be > 0");
            var scale = Math.Max((double)targetWidth / src.Width, (double)targetHeight / src.Height);
            var srcW = Math.Max(1, (int)Math.Round(targetWidth / scale));
            var srcH = Math.Max(1, (int)Math.Round(targetHeight / scale));
            var srcX = (src.Width - srcW) / 2;
            var srcY = (src.Height - srcH) / 2;

            var bmp = new Bitmap(targetWidth, targetHeight);
            bmp.SetResolution(src.HorizontalResolution, src.VerticalResolution);
            using (var g = Graphics.FromImage(bmp))
            {
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                var destRect = new Rectangle(0, 0, targetWidth, targetHeight);
                var srcRect = new Rectangle(srcX, srcY, srcW, srcH);
                g.DrawImage(src, destRect, srcRect, GraphicsUnit.Pixel);
            }
            return bmp;
        }

        private static void TryApplyOrientation(Image img)
        {
            try
            {
                const int OrientationId = 0x0112;
                if (!img.PropertyIdList.Contains(OrientationId)) return;
                var prop = img.GetPropertyItem(OrientationId);
                if (prop?.Value == null || prop.Value.Length < 2) return;
                var orientation = prop.Value[0];
                // Handle common EXIF orientations
                switch (orientation)
                {
                    case 3: img.RotateFlip(RotateFlipType.Rotate180FlipNone); break;
                    case 6: img.RotateFlip(RotateFlipType.Rotate90FlipNone); break;
                    case 8: img.RotateFlip(RotateFlipType.Rotate270FlipNone); break;
                    default: break;
                }
                // Reset orientation to normal
                try { prop.Value[0] = 1; img.SetPropertyItem(prop); } catch { }
            }
            catch { }
        }

        private static ImageFormat GetSaveFormat(Image original, bool keepFormat)
        {
            if (!keepFormat) return ImageFormat.Jpeg;
            var fmt = original.RawFormat;
            if (fmt.Guid == ImageFormat.Jpeg.Guid) return ImageFormat.Jpeg;
            if (fmt.Guid == ImageFormat.Png.Guid) return ImageFormat.Png;
            if (fmt.Guid == ImageFormat.Gif.Guid) return ImageFormat.Gif;
            if (fmt.Guid == ImageFormat.Bmp.Guid) return ImageFormat.Bmp;
            if (fmt.Guid == ImageFormat.Tiff.Guid) return ImageFormat.Tiff;
            return ImageFormat.Jpeg;
        }

        // 根据输出路径扩展名选择保存格式
        private static ImageFormat GetSaveFormat(string outputPath)
        {
            var ext = (Path.GetExtension(outputPath) ?? "").ToLowerInvariant();
            return ext switch
            {
                ".jpg" => ImageFormat.Jpeg,
                ".jpeg" => ImageFormat.Jpeg,
                ".png" => ImageFormat.Png,
                ".bmp" => ImageFormat.Bmp,
                ".gif" => ImageFormat.Gif,
                ".tif" => ImageFormat.Tiff,
                ".tiff" => ImageFormat.Tiff,
                _ => ImageFormat.Jpeg
            };
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageEncoders();
            foreach (var c in codecs)
            {
                if (c.FormatID == format.Guid) return c;
            }
            return null;
        }

        private static EncoderParameters BuildJpegEncoderParams(int quality)
        {
            var q = Math.Min(100, Math.Max(1, quality));
            var ep = new EncoderParameters(1);
            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)q);
            return ep;
        }

        // ========================
        // 图片马赛克（分块平均）
        // ========================

        public static void ApplyMosaic(string inputPath, string outputPath, int blockSize, bool keepFormat = true)
        {
            using (var fs = File.OpenRead(inputPath))
            {
                var bytes = ApplyMosaic(fs, blockSize, keepFormat);
                var dir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllBytes(outputPath, bytes);
            }
        }

        public static byte[] ApplyMosaic(byte[] inputBytes, int blockSize, bool keepFormat = true)
        {
            using (var ms = new MemoryStream(inputBytes))
            {
                return ApplyMosaic(ms, blockSize, keepFormat);
            }
        }

        public static byte[] ApplyMosaic(Stream inputStream, int blockSize, bool keepFormat = true)
        {
            using (var img = Image.FromStream(inputStream, useEmbeddedColorManagement: true, validateImageData: true))
            {
                TryApplyOrientation(img);
                using (var bmp = new Bitmap(img))
                {
                    using (var mosaiced = MosaicBlocks(bmp, blockSize))
                    {
                        var format = GetSaveFormat(img, keepFormat);
                        using (var outMs = new MemoryStream())
                        {
                            if (format.Guid == ImageFormat.Jpeg.Guid)
                            {
                                var enc = GetEncoder(ImageFormat.Jpeg);
                                using (var ep = BuildJpegEncoderParams(90))
                                {
                                    mosaiced.Save(outMs, enc, ep);
                                }
                            }
                            else
                            {
                                mosaiced.Save(outMs, format);
                            }
                            return outMs.ToArray();
                        }
                    }
                }
            }
        }

        // 马赛克（像素化）处理：输入图片文件流，块大小，保存到指定路径（根据扩展名选择格式，JPEG 支持质量）
        public static void MosaicImageToFile(Stream inputStream, int blockSize, string savePath, int jpegQuality = 90)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (string.IsNullOrWhiteSpace(savePath)) throw new ArgumentNullException(nameof(savePath));
            if (blockSize < 2) blockSize = 2;

            using (var ms = new MemoryStream())
            {
                if (inputStream.CanSeek) inputStream.Position = 0;
                inputStream.CopyTo(ms);
                ms.Position = 0;
                using (var img = Image.FromStream(ms, useEmbeddedColorManagement: true, validateImageData: true))
                {
                    TryApplyOrientation(img);
                    using (var bmp = new Bitmap(img))
                    using (var mosaiced = MosaicBlocks(bmp, blockSize))
                    {
                        var fmt = GetSaveFormat(savePath);
                        if (fmt.Guid == ImageFormat.Jpeg.Guid)
                        {
                            var enc = GetEncoder(ImageFormat.Jpeg);
                            using (var ep = BuildJpegEncoderParams(jpegQuality))
                            {
                                mosaiced.Save(savePath, enc, ep);
                            }
                        }
                        else
                        {
                            mosaiced.Save(savePath, fmt);
                        }
                    }
                }
            }
        }

        private static Bitmap MosaicBlocks(Bitmap src, int blockSize)
        {
            var b = Math.Max(2, blockSize);
            var width = src.Width;
            var height = src.Height;

            // 统一到 24bpp 以简化处理
            var work = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            work.SetResolution(src.HorizontalResolution, src.VerticalResolution);
            using (var g = Graphics.FromImage(work)) g.DrawImage(src, new Rectangle(0, 0, width, height));

            var dest = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            dest.SetResolution(src.HorizontalResolution, src.VerticalResolution);

            var rect = new Rectangle(0, 0, width, height);
            var srcData = work.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            var destData = dest.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            try
            {
                int strideSrc = srcData.Stride;
                int strideDest = destData.Stride;
                int lenSrc = strideSrc * height;
                int lenDest = strideDest * height;
                var srcBytes = new byte[lenSrc];
                var destBytes = new byte[lenDest];
                Marshal.Copy(srcData.Scan0, srcBytes, 0, lenSrc);

                for (int by = 0; by < height; by += b)
                {
                    int blockH = Math.Min(b, height - by);
                    for (int bx = 0; bx < width; bx += b)
                    {
                        int blockW = Math.Min(b, width - bx);

                        long sumR = 0, sumG = 0, sumB = 0;
                        int count = blockW * blockH;

                        for (int y = 0; y < blockH; y++)
                        {
                            int rowOffset = (by + y) * strideSrc + bx * 3;
                            for (int x = 0; x < blockW; x++)
                            {
                                int idx = rowOffset + x * 3;
                                byte b0 = srcBytes[idx + 0];
                                byte g0 = srcBytes[idx + 1];
                                byte r0 = srcBytes[idx + 2];
                                sumB += b0; sumG += g0; sumR += r0;
                            }
                        }

                        byte avgB = (byte)(sumB / count);
                        byte avgG = (byte)(sumG / count);
                        byte avgR = (byte)(sumR / count);

                        for (int y = 0; y < blockH; y++)
                        {
                            int rowOffset = (by + y) * strideDest + bx * 3;
                            for (int x = 0; x < blockW; x++)
                            {
                                int idx = rowOffset + x * 3;
                                destBytes[idx + 0] = avgB;
                                destBytes[idx + 1] = avgG;
                                destBytes[idx + 2] = avgR;
                            }
                        }
                    }
                }

                Marshal.Copy(destBytes, 0, destData.Scan0, lenDest);
            }
            finally
            {
                work.UnlockBits(srcData);
                dest.UnlockBits(destData);
                work.Dispose();
            }
            return dest;
        }

        // ========================
        // 图片模糊（5x5 高斯卷积）
        // ========================

        public static void ApplyGaussianBlur(string inputPath, string outputPath, int strength = 1, bool keepFormat = true)
        {
            using (var fs = File.OpenRead(inputPath))
            {
                var bytes = ApplyGaussianBlur(fs, strength, keepFormat);
                var dir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllBytes(outputPath, bytes);
            }
        }

        public static byte[] ApplyGaussianBlur(byte[] inputBytes, int strength = 1, bool keepFormat = true)
        {
            using (var ms = new MemoryStream(inputBytes))
            {
                return ApplyGaussianBlur(ms, strength, keepFormat);
            }
        }

        public static byte[] ApplyGaussianBlur(Stream inputStream, int strength = 1, bool keepFormat = true)
        {
            using (var img = Image.FromStream(inputStream, useEmbeddedColorManagement: true, validateImageData: true))
            {
                TryApplyOrientation(img);
                using (var bmp = new Bitmap(img))
                using (var blurred = Gaussian5x5(bmp, Math.Max(1, strength)))
                {
                    var format = GetSaveFormat(img, keepFormat);
                    using (var outMs = new MemoryStream())
                    {
                        if (format.Guid == ImageFormat.Jpeg.Guid)
                        {
                            var enc = GetEncoder(ImageFormat.Jpeg);
                            using (var ep = BuildJpegEncoderParams(90))
                            {
                                blurred.Save(outMs, enc, ep);
                            }
                        }
                        else
                        {
                            blurred.Save(outMs, format);
                        }
                        return outMs.ToArray();
                    }
                }
            }
        }

        private static Bitmap Gaussian5x5(Bitmap src, int iterations)
        {
            var kernel = new int[] { 1, 4, 6, 4, 1 }; // separable kernel; normalization 16 per pass
            var width = src.Width;
            var height = src.Height;

            // 统一到 24bpp
            var work = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            work.SetResolution(src.HorizontalResolution, src.VerticalResolution);
            using (var g = Graphics.FromImage(work)) g.DrawImage(src, new Rectangle(0, 0, width, height));

            var rect = new Rectangle(0, 0, width, height);
            var data = work.LockBits(rect, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            try
            {
                int stride = data.Stride;
                int len = stride * height;
                var srcBytes = new byte[len];
                Marshal.Copy(data.Scan0, srcBytes, 0, len);

                var tmpBytes = new byte[len];
                var outBytes = new byte[len];
                int iters = Math.Max(1, iterations);

                for (int it = 0; it < iters; it++)
                {
                    // Horizontal pass: srcBytes -> tmpBytes
                    for (int y = 0; y < height; y++)
                    {
                        int rowOff = y * stride;
                        for (int x = 0; x < width; x++)
                        {
                            int sumB = 0, sumG = 0, sumR = 0;
                            for (int k = -2; k <= 2; k++)
                            {
                                int xi = Math.Min(width - 1, Math.Max(0, x + k));
                                int w = kernel[k + 2];
                                int idx = rowOff + xi * 3;
                                sumB += srcBytes[idx + 0] * w;
                                sumG += srcBytes[idx + 1] * w;
                                sumR += srcBytes[idx + 2] * w;
                            }
                            int tIdx = rowOff + x * 3;
                            tmpBytes[tIdx + 0] = (byte)(sumB / 16);
                            tmpBytes[tIdx + 1] = (byte)(sumG / 16);
                            tmpBytes[tIdx + 2] = (byte)(sumR / 16);
                        }
                    }

                    // Vertical pass: tmpBytes -> outBytes
                    for (int x = 0; x < width; x++)
                    {
                        for (int y = 0; y < height; y++)
                        {
                            int sumB = 0, sumG = 0, sumR = 0;
                            for (int k = -2; k <= 2; k++)
                            {
                                int yi = Math.Min(height - 1, Math.Max(0, y + k));
                                int w = kernel[k + 2];
                                int idx = yi * stride + x * 3;
                                sumB += tmpBytes[idx + 0] * w;
                                sumG += tmpBytes[idx + 1] * w;
                                sumR += tmpBytes[idx + 2] * w;
                            }
                            int oIdx = y * stride + x * 3;
                            outBytes[oIdx + 0] = (byte)(sumB / 16);
                            outBytes[oIdx + 1] = (byte)(sumG / 16);
                            outBytes[oIdx + 2] = (byte)(sumR / 16);
                        }
                    }

                    // 下一次迭代以 outBytes 作为输入
                    Buffer.BlockCopy(outBytes, 0, srcBytes, 0, len);
                }

                // 将最终结果写回
                Marshal.Copy(outBytes, 0, data.Scan0, len);
            }
            finally
            {
                work.UnlockBits(data);
            }
            return work;
        }

        // 将图片进行高斯模糊（5x5，可迭代增强效果），输入文件流，保存到指定路径
        public static void BlurImageToFile(Stream inputStream, int iterations, string savePath, int jpegQuality = 90)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (string.IsNullOrWhiteSpace(savePath)) throw new ArgumentNullException(nameof(savePath));

            // 确保可读取为 Image
            using (var ms = new MemoryStream())
            {
                inputStream.CopyTo(ms);
                ms.Position = 0;
                using (var img = Image.FromStream(ms, useEmbeddedColorManagement: true, validateImageData: true))
                using (var bmp = new Bitmap(img))
                {
                    var blurred = Gaussian5x5(bmp, iterations);
                    var fmt = GetSaveFormat(savePath);
                    if (fmt.Guid == ImageFormat.Jpeg.Guid)
                    {
                        var enc = GetEncoder(ImageFormat.Jpeg);
                        var ep = BuildJpegEncoderParams(jpegQuality);
                        blurred.Save(savePath, enc, ep);
                    }
                    else
                    {
                        blurred.Save(savePath, fmt);
                    }
                    blurred.Dispose();
                }
            }
        }

        // ========================
        // 图片加密/解密（AES-CBC，前置IV）
        // ========================

        public static byte[] EncryptImageBytes(byte[] inputBytes, byte[] key, byte[] iv = null)
        {
            if (inputBytes == null || inputBytes.Length == 0) throw new ArgumentException("inputBytes empty");
            if (key == null || (key.Length != 16 && key.Length != 24 && key.Length != 32)) throw new ArgumentException("key length must be 16/24/32");
            using var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            if (iv == null || iv.Length != aes.BlockSize / 8)
            {
                iv = new byte[aes.BlockSize / 8];
                using var rng = RandomNumberGenerator.Create();
                rng.GetBytes(iv);
            }
            aes.IV = iv;
            using var ms = new MemoryStream();
            ms.Write(iv, 0, iv.Length); // prefix IV
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(inputBytes, 0, inputBytes.Length);
                cs.FlushFinalBlock();
            }
            return ms.ToArray();
        }

        public static byte[] DecryptImageBytes(byte[] encryptedBytes, byte[] key)
        {
            if (encryptedBytes == null || encryptedBytes.Length < 16) throw new ArgumentException("encryptedBytes invalid");
            if (key == null || (key.Length != 16 && key.Length != 24 && key.Length != 32)) throw new ArgumentException("key length must be 16/24/32");
            using var aes = Aes.Create();
            aes.Key = key;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            int block = aes.BlockSize / 8;
            var iv = new byte[block];
            Array.Copy(encryptedBytes, 0, iv, 0, block);
            aes.IV = iv;
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(new MemoryStream(encryptedBytes, block, encryptedBytes.Length - block), aes.CreateDecryptor(), CryptoStreamMode.Read))
            {
                cs.CopyTo(ms);
            }
            return ms.ToArray();
        }

        public static void EncryptImageFile(string inputPath, string outputPath, byte[] key)
        {
            var bytes = File.ReadAllBytes(inputPath);
            var enc = EncryptImageBytes(bytes, key);
            var dir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.WriteAllBytes(outputPath, enc);
        }

        public static void DecryptImageFile(string inputPath, string outputPath, byte[] key)
        {
            var bytes = File.ReadAllBytes(inputPath);
            var dec = DecryptImageBytes(bytes, key);
            var dir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) Directory.CreateDirectory(dir);
            File.WriteAllBytes(outputPath, dec);
        }

        // AES 加密：输入图片文件流，字符串 key，保存到指定路径（密文文件）
        public static void EncryptImageStreamToFile(Stream inputStream, string key, string savePath)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(savePath)) throw new ArgumentNullException(nameof(savePath));

            byte[] plain;
            using (var ms = new MemoryStream())
            {
                inputStream.CopyTo(ms);
                plain = ms.ToArray();
            }

            var cipher = EncryptBytesAes(plain, key);
            File.WriteAllBytes(savePath, cipher);
        }

        // AES 解密：输入密文文件流，字符串 key，保存到指定路径（还原后的图片原始字节）
        public static void DecryptImageStreamToFile(Stream inputStream, string key, string savePath)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(savePath)) throw new ArgumentNullException(nameof(savePath));

            byte[] cipher;
            using (var ms = new MemoryStream())
            {
                inputStream.CopyTo(ms);
                cipher = ms.ToArray();
            }

            var plain = DecryptBytesAes(cipher, key);
            File.WriteAllBytes(savePath, plain);
        }

        // 解密并校验图片有效性：解密后尝试用 System.Drawing 读取并按保存路径的扩展名重新保存（避免扩展名与内容不匹配导致无法打开）
        public static void DecryptImageStreamToFileValidate(Stream inputStream, string key, string savePath, int jpegQuality = 90)
        {
            if (inputStream == null) throw new ArgumentNullException(nameof(inputStream));
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrWhiteSpace(savePath)) throw new ArgumentNullException(nameof(savePath));

            byte[] cipher;
            using (var ms = new MemoryStream())
            {
                if (inputStream.CanSeek) inputStream.Position = 0;
                inputStream.CopyTo(ms);
                cipher = ms.ToArray();
            }
            if (cipher == null || cipher.Length < 16)
            {
                throw new ArgumentException("输入流不是有效的密文：总长度不足 16 字节。请确认传入的是由 EncryptImageStreamToFile/EncryptImageFile 生成的密文文件，并且从流起始位置读取。", nameof(inputStream));
            }

            var plain = DecryptBytesAes(cipher, key);

            try
            {
                using (var imgMs = new MemoryStream(plain))
                using (var img = Image.FromStream(imgMs, useEmbeddedColorManagement: true, validateImageData: true))
                {
                    TryApplyOrientation(img);
                    var fmt = GetSaveFormat(savePath);
                    if (fmt.Guid == ImageFormat.Jpeg.Guid)
                    {
                        var enc = GetEncoder(ImageFormat.Jpeg);
                        using (var ep = BuildJpegEncoderParams(jpegQuality))
                        {
                            img.Save(savePath, enc, ep);
                        }
                    }
                    else
                    {
                        img.Save(savePath, fmt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("解密后数据不是有效图片或与文件扩展名不匹配。请确认密文来源与密钥一致，或改为直接保存原始字节：DecryptImageStreamToFile。", ex);
            }
        }

        // 内部：AES 加密（CBC/PKCS7），key 通过 SHA256 派生；输出格式：IV(16字节)+密文
        private static byte[] EncryptBytesAes(byte[] data, string key)
        {
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));
            aes.GenerateIV();

            using var ms = new MemoryStream();
            // 先写入 IV
            ms.Write(aes.IV, 0, aes.IV.Length);
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
            }
            return ms.ToArray();
        }

        // 内部：AES 解密（CBC/PKCS7），输入格式：IV(16字节)+密文
        private static byte[] DecryptBytesAes(byte[] data, string key)
        {
            using var aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(key));

            if (data.Length < 16) throw new ArgumentException("密文长度无效", nameof(data));
            var iv = new byte[16];
            Buffer.BlockCopy(data, 0, iv, 0, 16);
            aes.IV = iv;

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(data, 16, data.Length - 16);
                cs.FlushFinalBlock();
            }
            return ms.ToArray();
        }
    }
}
