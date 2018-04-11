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
                if (!Directory.Exists(m_OutputFolder))
                {
                    Directory.CreateDirectory(m_OutputFolder);
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
            //
            if (Directory.Exists(m_OutputFolder + "\\" +  year)) {
                if (Directory.Exists(m_OutputFolder + "\\" + year + "\\" + month)) {
                    File.Move(path, m_OutputFolder + "\\" + year + "\\" + month);
                }
                else {
                    Directory.CreateDirectory(m_OutputFolder + "\\" + year + "\\" + month);
                    File.Move(path, m_OutputFolder + "\\" + year + "\\" + month);
                }  
            }
            else {
                Directory.CreateDirectory(m_OutputFolder + "\\" + year);
                Directory.CreateDirectory(m_OutputFolder + "\\" + year + "\\" + month);
                File.Move(path, m_OutputFolder + "\\" + year + "\\" + month);
            }

            // create thumbnail image from path
            Image image = Image.FromFile(path);
            Image thumb = image.GetThumbnailImage(m_thumbnailSize, m_thumbnailSize, () => false, IntPtr.Zero);

            // check if Thumbnails dir exists, if not – create it
            try
            {
                if (!Directory.Exists(m_OutputFolder + "\\" + "Thumbnails"))
                {
                    Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception caught.", ex);
                result = false;
                return failure;
            }
            
            //
            if (Directory.Exists(m_OutputFolder + "\\" + "Thumbnails" + "\\" + year))
            {
                if (Directory.Exists(m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month))
                {
                    File.Move(path, m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                }
                else
                {
                    Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                    File.Move(path, m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                }
            }
            else
            {
                Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails" + "\\" + year);
                Directory.CreateDirectory(m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
                File.Move(path, m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month);
            }

            thumb.Save(Path.ChangeExtension(m_OutputFolder + "\\" + "Thumbnails" + "\\" + year + "\\" + month, nameOfImage));
            result = true;
            return success;
        }

        /// <summary>
        /// The function gets the file date
        /// </summary>
        /// <param name="filename">The Path of the Image from the file</param>
        /// <returns>returns DateTime</returns>
        static DateTime GetFileDate(string filename)
        {
            DateTime now = DateTime.Now;
            TimeSpan localOffset = now - now.ToUniversalTime();
            return File.GetLastWriteTimeUtc(filename) + localOffset;
        }
    }
}
