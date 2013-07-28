namespace GMATechProject.Web
{
	#region Using Directives
	
	using System;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Drawing.Imaging;
	using System.IO;

	#endregion

	public static class ImageResizing
	{
		//Overload for crop that default starts top left of the image.
		public static Image CropImage(Image Image, int Height, int Width)
		{
			return CropImage(Image, Height, Width, 0,0);
		}

		//The crop image sub
		public static Image CropImage(Image Image, int Height, int Width, int StartAtX, int StartAtY)
		{
			Image outimage;
			MemoryStream mm = null;
			try
			{
				//check the image height against our desired image height
				if (Image.Height < Height) {
					Height = Image.Height;
				}

				if (Image.Width < Width) {
					Width = Image.Width;
				}

				//create a bitmap window for cropping
				Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
				bmPhoto.SetResolution(72, 72);

				//create a new graphics object from our image and set properties
				Graphics grPhoto = Graphics.FromImage(bmPhoto);
				grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
				grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
				grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;

				//now do the crop
				grPhoto.DrawImage(Image, new Rectangle(0, 0, Width, Height), StartAtX, StartAtY, Width, Height, GraphicsUnit.Pixel);

				// Save out to memory and get an image from it to send back out the method.
				mm = new MemoryStream();
				bmPhoto.Save(mm, ImageFormat.Jpeg);
				Image.Dispose();
				bmPhoto.Dispose();
				grPhoto.Dispose();
				outimage = Image.FromStream(mm);

				return outimage;
			}
			catch (Exception ex)
			{
				throw new Exception("Error cropping image, the error was: " + ex.Message);
			}
		}

		//Hard resize attempts to resize as close as it can to the desired size and then crops the excess
		public static Image HardResizeImage(int Width, int Height, Image Image)
		{
			int width = Image.Width;
			int height = Image.Height;
			Image resized = null;
			if (Width > Height)
			{
				resized = ResizeImage(Width, Width, Image);
			}
			else
			{
				resized = ResizeImage(Height, Height, Image);
			}
			Image output = CropImage(resized, Height, Width);
			//return the original resized image
			return output;
		}

		//Hard resize attempts to resize as close as it can to the desired size and then crops the excess
		public static Image HardResizeImageExactSize(int width, int height, Image Image)
		{
			Image resized = ResizeImageExactSize(width, height, Image);

			Image output = CropImage(resized, height, width);
			//return the original resized image
			return output;
		}

		//Image resizing
		public static Image ResizeImageExactSize(int resultWidth, int resultHeight, Image Image)
		{
			int width = Image.Width;
			int height = Image.Height;

			//The flips are in here to prevent any embedded image thumbnails -- usually from cameras
			//from displaying as the thumbnail image later, in other words, we want a clean
			//resize, not a grainy one.
			Image.RotateFlip (RotateFlipType.Rotate180FlipX);
			Image.RotateFlip (RotateFlipType.Rotate180FlipX);

			float coefWidth = (float)resultWidth / (float)width;
			float coefHeight = (float)resultHeight / (float)height;

			if ( coefWidth > coefHeight) {
				width = resultWidth;
				height = Convert.ToInt32 (Math.Round((float)height * coefWidth));
			} else {
				height = resultHeight;
				width = Convert.ToInt32 (Math.Round((float)width * coefHeight));
			}

			//return the resized image
			return Image.GetThumbnailImage (width, height, null, IntPtr.Zero);
		}

		//Image resizing
		public static Image ResizeImage(int maxWidth, int maxHeight, Image Image)
		{
			int width = Image.Width;
			int height = Image.Height;

			//The flips are in here to prevent any embedded image thumbnails -- usually from cameras
			//from displaying as the thumbnail image later, in other words, we want a clean
			//resize, not a grainy one.
			Image.RotateFlip (RotateFlipType.Rotate180FlipX);
			Image.RotateFlip (RotateFlipType.Rotate180FlipX);

			float ratio = 0;
			if (width > height) {
				ratio = (float)width / (float)height;
				width = maxWidth;
				height = Convert.ToInt32 (Math.Round((float)width / ratio));
			} else {
				ratio = (float)height / (float)width;
				height = maxHeight;
				width = Convert.ToInt32 (Math.Round((float)height / ratio));
			}

			//return the resized image
			return Image.GetThumbnailImage (width, height, null, IntPtr.Zero);
		}
	}
}
