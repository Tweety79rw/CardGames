using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;


namespace pokerGame
{
    public class SpriteSheet
    {
        private BitmapSource _spriteSheetSource;
        private WriteableBitmap _spriteSheetBitmap;
        private int _sheetWidth;
        private int _sheetHeight;

        public SpriteSheet(BitmapSource spriteSheetSource)
        {
            if (spriteSheetSource == null) throw new ArgumentNullException("spriteSheetSource");

            _spriteSheetSource = spriteSheetSource;
            _spriteSheetBitmap = new WriteableBitmap(_spriteSheetSource);
            _sheetWidth = _spriteSheetBitmap.PixelWidth;
            _sheetHeight = _spriteSheetBitmap.PixelHeight;
        }
        public int imageWidth()
        {
            return _spriteSheetBitmap.PixelWidth;
        }
        public int imageHeight()
        {
            return _spriteSheetBitmap.PixelHeight;
        }
        public CroppedBitmap GetBitmap(int x, int y, int width, int height)
        {
            CroppedBitmap destination = new CroppedBitmap(_spriteSheetBitmap, new Int32Rect(x, y, width, height));
            //GetBitmap(destination, x, y, width, height);
            return destination;
        }

        public void GetBitmap(CroppedBitmap targetBitmap, int x, int y, int width, int height)
        {
            // Validate incomming data
            if (targetBitmap == null) throw new ArgumentNullException("targetBitmap");
            if (x < 0 || x >= _sheetWidth) throw new ArgumentOutOfRangeException("x");
            if (y < 0 || y >= _sheetHeight) throw new ArgumentOutOfRangeException("y");
            if (width < 0 || (x + width > _sheetWidth)) throw new ArgumentOutOfRangeException("width");
            if (height < 0 || (y + height > _sheetHeight)) throw new ArgumentOutOfRangeException("height");

            // Get pixel buffers for the sprite sheet and the target bitmap
            int stride = _spriteSheetBitmap.PixelWidth * ((_spriteSheetBitmap.Format.BitsPerPixel + 7) / 8);
            byte[] sourcePixels = new byte[ stride * _spriteSheetBitmap.PixelHeight];
            _spriteSheetBitmap.CopyPixels(sourcePixels, stride, 0);
            //byte[] targetPixels = new byte[targetBitmap.PixelWidth * ((targetBitmap.Format.BitsPerPixel + 7) / 8) * targetBitmap.PixelHeight];
            //targetBitmap.CopyPixels(targetPixels, targetBitmap.PixelWidth * ((targetBitmap.Format.BitsPerPixel + 7) / 8), 0);

            // Calculate starting offsets into the pixel buffers      
            int sourceOffsetx = x;
            int sourceOffsety = (y * _sheetWidth);
            int targetOffset = 0;

            // Note that the offsets and widths are multiplied by 4, this is because Buffer.BlockCopy requires
            // byte offsets into the buffers and our buffers are integer buffers. To optimize this I have 
            // premultiplied to values so that the multiplication is removed from the loop
            int sourceByteOffsetx = sourceOffsetx << 2;
            int sourceByteOffsety = sourceOffsetx << 2;
            int sheetByteWidth = _sheetWidth << 2;
            int targetByteWidth = width << 2;
 //           for (int row = 0; row < height; ++row)
 //           {
                //Buffer.BlockCopy(sourcePixels, sourceByteOffset, targetPixels, targetOffset, targetByteWidth);
            targetBitmap = new CroppedBitmap(_spriteSheetBitmap, new Int32Rect(x, y, width, height));
                sourceByteOffsetx += sheetByteWidth;
                targetOffset += targetByteWidth;
//            }
        }

        public int Width
        {
            get { return _sheetWidth; }
        }

        public int Height
        {
            get { return _sheetHeight; }
        }
    }
}
