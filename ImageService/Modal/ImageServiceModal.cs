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
        #endregion

        public ImageServiceModal(string outputFolder, int thumbnailSize)
        {
            this.m_OutputFolder = outputFolder;
            this.m_thumbnailSize = thumbnailSize;
        }

        /// <summary>
        /// The Function Addes A file to the system
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <returns>Indication if the Addition Was Successful</returns>
        public string AddFile(string path, out bool result)
        {
            string success = "success";
            string failure = "failure";
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
                return failure;
            }

            // get file creation time, year and month
            DateTime date = GetFileDate(path);
            string year = date.Year.ToString();
            string month = date.Month.ToString();

            // Create dirs "year" and "month" (unless they already exist), and move the image to its new dir
            CreateDirsAndMoveImage(path, "", year, month);

            // create thumbnail image from path
            Image image = Image.FromFile(path);
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
                return failure;
            }

            // Create dirs "year" and "month" within Thumbnails dir (unless they already exist), and move the image to its new dir
            CreateDirsAndMoveImage(path, "\\Thumbnails", year, month);
            
            thumb.Save(Path.ChangeExtension(this.m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month, nameOfImage));
            result = true;
            return success;
        }

        /// <summary>
        /// The Function Addes A file to the system
        /// </summary>
        /// <param name="path">The Path of the Image from the file</param>
        /// <param name="Thumbnails">In case we want to create the thumbnail dir</param>
        /// <param name="year">The name of the year dir</param>
        /// <param name="month">The name of the month dir</param> 
        /// <returns>none</returns>
        public void CreateDirsAndMoveImage(string path, string Thumbnails, string year, string month)
        {
            if (Directory.Exists(this.m_OutputFolder + Thumbnails + "\\" + year))
            {
                if (Directory.Exists(this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month))
                {
                    File.Move(path, this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month);
                }
                else
                {
                    Directory.CreateDirectory(this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month);
                    File.Move(path, this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month);
                }
            }
            else
            {
                Directory.CreateDirectory(this.m_OutputFolder + Thumbnails + "\\" + year);
                Directory.CreateDirectory(this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month);
                File.Move(path, this.m_OutputFolder + Thumbnails + "\\" + year + "\\" + month);
            }
        }

        /// <summary>
        /// The function gets the file date
        /// </summary>
        /// <param name="filename">The Path of the Image from the file</param>
        /// <returns>returns DateTime</returns>
        public DateTime GetFileDate(string filename)
        {
            DateTime now = DateTime.Now;
            TimeSpan localOffset = now - now.ToUniversalTime();
            return File.GetLastWriteTimeUtc(filename) + localOffset;
        }
        /*
            if (Directory.Exists(this.m_OutputFolder + "\\" + "Thumbnails" + "\\" + year))
            {
                if (Directory.Exists(this.m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month))
                {
                    File.Move(path, this.m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                }
                else
                {
                    Directory.CreateDirectory(this.m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                    File.Move(path, this.m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                }
            }
            else
            {
                Directory.CreateDirectory(this.m_OutputFolder + "\\" + "Thumbnails" + "\\" + year);
                Directory.CreateDirectory(this.m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                File.Move(path, this.m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
            }
            */
    }
}
