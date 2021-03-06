﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Diagnostics;

namespace imgyazur
{
    public partial class formImgyazur : Form
    {
        #region Data

        private static string apiUriString = "http://api.imgur.com/2/upload.json";
        private static Uri apiUri = new Uri(apiUriString);

        private Form captureForm = new formCaptureArea();
        private Point captureStartPoint;
        private Rectangle captureArea = new Rectangle();

        #endregion

        #region Imports for showing the window without stealing focus

        private const int SW_SHOWNOACTIVATE = 4;
        private const int HWND_TOPMOST = -1;
        private const uint SWP_NOACTIVATE = 0x0010;

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool SetWindowPos(
             int hWnd,            // window handle
             int hWndInsertAfter, // placement-order handle
             int X,               // horizontal position
             int Y,               // vertical position
             int cx,              // width
             int cy,              // height
             uint uFlags);        // window positioning flags

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        static void ShowInactiveTopmost(Form frm)
        {
            ShowWindow(frm.Handle, SW_SHOWNOACTIVATE);
            SetWindowPos(frm.Handle.ToInt32(), HWND_TOPMOST, Cursor.Position.X, Cursor.Position.Y, frm.Width, frm.Height, SWP_NOACTIVATE);
        }

        #endregion

        private bool capturing = false;

        public formImgyazur()
        {
            InitializeComponent();

            Bounds = Screen.AllScreens
                          .Select(x => x.Bounds)
                          .Aggregate(Rectangle.Union);
        }

        private void formImgyazur_Load(object sender, EventArgs e)
        {
            BringToFront();
        }

        private void Die()
        {
            captureForm.Dispose();
            Dispose();
        }

        void formImgyazur_KeyPress(object sender, KeyPressEventArgs e)
        {
            Die();
        }

        private void formImgyazur_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                Die();
                return;
            }

            captureStartPoint = e.Location;

            capturing = true;

            ShowInactiveTopmost(captureForm);
        }

        private void formImgyazur_MouseMove(object sender, MouseEventArgs e)
        {
            if (!capturing || captureStartPoint == null)
                return;

            captureArea.X = Math.Min(e.X, captureStartPoint.X);
            captureArea.Width = Math.Abs(e.X - captureStartPoint.X);
            captureArea.Y = Math.Min(e.Y, captureStartPoint.Y);
            captureArea.Height = Math.Abs(e.Y - captureStartPoint.Y);

            SetWindowPos(captureForm.Handle.ToInt32(), HWND_TOPMOST, captureArea.X, captureArea.Y, captureArea.Width, captureArea.Height, SWP_NOACTIVATE);
        }

        private void formImgyazur_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            try
            {
                uploadImage();
            }
            finally
            {
                Cursor = Cursors.Default;
            }

            Die();
        }

        private void uploadImage()
        {
            if (captureArea.Width == 0 && captureArea.Height == 0)
                return;

            // if we don't do this, the image turns out ugly and grey... duh
            captureForm.Opacity = 0;
            Opacity = 0;
            Hide();

            Bitmap captureBitmap = new Bitmap(captureArea.Width, captureArea.Height);
            Graphics captureGraphics = Graphics.FromImage(captureBitmap);

            // copy from the screen, using our captureArea for the start point,
            // a new 0,0 point for the destination, and the captureArea size for the size
            captureGraphics.CopyFromScreen(captureArea.Location, new Point(), captureArea.Size);

            // get the base64 encoding of the image bytes, encoded in PNG
            String base64Image;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                captureBitmap.Save(memoryStream, ImageFormat.Png);
                base64Image = Convert.ToBase64String(memoryStream.GetBuffer());
            }

            captureBitmap.Dispose();
            captureGraphics.Dispose();

            using (WebClient webClient = new WebClient())
            {
                webClient.UploadProgressChanged += new UploadProgressChangedEventHandler(webClient_UploadProgressChanged);
                webClient.UploadValuesCompleted += new UploadValuesCompletedEventHandler(webClient_UploadValuesCompleted);

                NameValueCollection parameters = new NameValueCollection();
                //if this doesn't compile, add an apikey.cs:
                /* 
                    namespace imgyazur
                    {
                        class apikey
                        {
                            public static string apikey = "xxx"
                        }
                    }
                 */
                parameters.Add("key", apikey.key);
                parameters.Add("type", "base64");
                parameters.Add("image", base64Image);

                webClient.UploadValuesAsync(apiUri, parameters);

                parameters = null;
            }
        }

        void webClient_UploadValuesCompleted(object sender, UploadValuesCompletedEventArgs e)
        {
            ImgyazurIcon.Instance.notifyIcon.Text = ImgyazurIcon.defaultText;

            if (e.Error != null)
            {
                ImgyazurIcon.Instance.notifyIcon.ShowBalloonTip(10000, "Error Uploading Image", "An error occurred while uploading your image. Check your connection and try again, or give up. I don't care.", ToolTipIcon.Error);
                return;
            }

            byte[] responseArray = e.Result;

#if DEBUG
            // this might help debugging in case the frickin API ever breaks the responses
            String responseString = Encoding.UTF8.GetString(responseArray);
#endif

            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(ImgurUploadResponse));

            ImgurUploadResponse photos = null;
            try
            {
                photos = (ImgurUploadResponse)jsonSerializer.ReadObject(new MemoryStream(responseArray));
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while processing the response from imgur.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (photos != null)
            {
                if (Properties.Settings.Default.openAfterUpload)
                    Process.Start(photos.upload.links.original);
                if (Properties.Settings.Default.copyToClipboard)
                {
                    Clipboard.SetText(photos.upload.links.original);
                    ImgyazurIcon.Instance.notifyIcon.ShowBalloonTip(10000, "Upload Complete", photos.upload.links.original + " copied to clipboard.", ToolTipIcon.Info);
                }
            }
        }

        void webClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            ImgyazurIcon.Instance.notifyIcon.Text = "Uploading... " + e.ProgressPercentage.ToString() + "% done.";
        }
    }
}

#region ImgureUploadResponse

public class ImgurUploadResponse
{
    public Upload upload { get; set; }
}

public class Upload
{
    public Image image { get; set; }
    public Links links { get; set; }
}

public class Image
{
    public bool? name { get; set; }
    public string title { get; set; }
    public string caption { get; set; }
    public string hash { get; set; }
    public string deletehash { get; set; }
    public string datetime { get; set; }
    public string type { get; set; }
    public string animated { get; set; }
    public int width { get; set; }
    public int height { get; set; }
    public int size { get; set; }
    public int views { get; set; }
    public int bandwidth { get; set; }
}

public class Links
{
    public string original { get; set; }
    public string imgur_page { get; set; }
    public string delete_page { get; set; }
    public string small_square { get; set; }
    public string large_thumbnail { get; set; }
}

#endregion