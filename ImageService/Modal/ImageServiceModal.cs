using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {
        #region Members
        // The output folder
        private string m_OutputFolder;
        // The size of the thumbnail
        private int m_thumbnailSize;
        // will use us for GetDateTakenFromImage function
        private static Regex r;
        #endregion

        public ImageServiceModal(string outputFolder, int thumbnailSize)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;
            r = new Regex(":");
        }

        /// <summary>
        /// The Function Addes A file to the system
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <returns>Indication if the Addition Was Successful</returns>
        public string AddFile(string path, out bool result)
        {
            string success = "Success";
            string failure = "Failure";
            string nameOfImage = Path.GetFileName(path);
            // check if outputDir exists, if not – create it
            try
            {
                if (!Directory.Exists(this.m_OutputFolder))
                {
                    Directory.CreateDirectory(this.m_OutputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
                result = false;
                return failure + ex.ToString();
            }

            // get file creation time, year and month
            DateTime date = GetDateTakenFromImage/*GetFileDate*/(path);
            string year = date.Year.ToString();
            string month = date.Month.ToString();

            // Create dirs "year" and "month" (unless they already exist), and move the image to its new dir
            string newPathToImage = CreateDirsAndMoveImage(path, "", year, month, nameOfImage);

            // create thumbnail image from path
            Image image = Image.FromFile(newPathToImage);
            Image thumb = image.GetThumbnailImage(this.m_thumbnailSize, this.m_thumbnailSize, () => false, IntPtr.Zero);

            // check if Thumbnails dir exists, if not – create it
            try
            {
                if (!Directory.Exists(this.m_OutputFolder + "\\" + "Thumbnails"))
                {
                    Directory.CreateDirectory(this.m_OutputFolder + "\\" + "Thumbnails");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
                result = false;
                return failure + ex.ToString();
            }

            // Create dirs "year" and "month" within Thumbnails dir (unless they already exist)
            string newPathToThumbnailImage = CreateDirsAndMoveImage(path, "\\Thumbnails", year, month, nameOfImage);
            // Save the Thumbnail Image into its new path
            thumb.Save(Path.ChangeExtension(/*this.m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month*/newPathToThumbnailImage, nameOfImage));
            image.Dispose();
            thumb.Dispose();
            result = true;
            return success + ".The new path is: " + newPathToImage;
        }

        /// <summary>
        /// The Function create dirs and moves the Image to its new dir
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <param name="Thumbnails">In case we want to create the thumbnail dir</param>
        /// <param name="year">The name of the year dir</param>
        /// <param name="month">The name of the month dir</param> 
        /// <returns>The new path to the image</returns>
        public string CreateDirsAndMoveImage(string path, string Thumbnails, string year, string month, string nameOfImage)
        {
            if (Directory.Exists(this.m_OutputFolder + Thumbnails + "\\" + year))
            {
                if (Directory.Exists(this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month))
                {
                    // Check if there is already file with this name in the dir
                    int i = 1;
                    string uniqueNameOfImage = nameOfImage;
                    while (File.Exists(this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month + "\\" + uniqueNameOfImage))
                    {
                        uniqueNameOfImage = nameOfImage + i.ToString();
                        i++;
                    }    
                    if (Thumbnails == "")
                        File.Move(path, this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month + "\\" + uniqueNameOfImage);
                }
                else
                {
                    Directory.CreateDirectory(this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month);
                    if (Thumbnails == "")
                        File.Move(path, this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month + "\\" + nameOfImage);
                }
            }
            else
            {
                Directory.CreateDirectory(this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month);
                if (Thumbnails == "")
                    File.Move(path, this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month + "\\" + nameOfImage);
            }
            return this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month + "\\" + nameOfImage;
        }

        /// <summary>
        /// The function gets the file date
        /// </summary>
        /// <param name="filename">The Path of the Image from the file</param>
        /// <returns>returns DateTime</returns>
        public static DateTime GetFileDate(string filename)
        {
            DateTime now = DateTime.Now;
            TimeSpan localOffset = now - now.ToUniversalTime();
            return File.GetLastWriteTimeUtc(filename) + localOffset;
        }

        /// <summary>
        /// Retrieves the datetime WITHOUT loading the whole image
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <returns>DateTime</returns>
        public static DateTime GetDateTakenFromImage(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (Image myImage = Image.FromStream(fs, false, false))
            {
                PropertyItem propItem = myImage.GetPropertyItem(36867);
                string dateTaken = r.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                return DateTime.Parse(dateTaken);
            }
        }
    }
}
