using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Net;
using System.Threading;

namespace PrayerTiming
{

    public partial class frmSettings : Form
    {
        //prayerTimingSettings prayerTimingSettings;
        Thread mapeSearching;//Declaring outside enables us to abort this thread via some button.
        public frmSettings()//prayerTimingSettings prayerTimingSettings)
        {
            InitializeComponent();
            //this.prayerTimingSettings = prayerTimingSettings;
        }
        //public prayerTimingSettings PrayerTimingSettings
        //{
        //    get { return prayerTimingSettings; }
        //}
        PrayerTimes PT = new PrayerTimes();
        private void frmSettings_Load(object sender, EventArgs e)
        {
            //we have to download settings from registry
            //if settings not found in registry

            ///Default settings

            //Tab1 General 
            try
            {
                pbMape.Load(prayerTimingSettings.MapeName);
                //mape.load
                mape = (Bitmap)pbMape.Image;
                if (pbMape.Image != null)
                {
                    NumUDZoom.Visible = true;
                    lblMapeStatus.Visible = false;
                }
                else
                    lblMapeStatus.Visible = true;
                    
            }
            catch (Exception exception)
            {

            }



            //string tz = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).ToString().Substring(0, 5);
            //setZone(tz);
            txtBoxCityName.Text = prayerTimingSettings.CityName;
            setZone(prayerTimingSettings.TimeZone.ToString());
            txtSunRise.Text = PT.GetPrayerTimes()[1].ToShortTimeString();
            txtSunSet.Text = PT.GetPrayerTimes()[4].ToShortTimeString();

            //Tab2 ClockDimension
            //clockSize
            switch (prayerTimingSettings.ClockSize)
            {
                case ClockSizeType.Small:
                    rdoBtnSmall.Checked = true;
                    break;
                case ClockSizeType.Medium:
                    rdoBtnMedium.Checked = true;
                    break;
                case ClockSizeType.Large:
                    rdoBtnLarge.Checked = true;
                    break;
                case ClockSizeType.Manual:
                    rdoBtnManually.Checked = true;
                    break;
                default:
                    rdoBtnMedium.Checked = true;
                    break;
            }
            rdoBtnClockSizeChanged(null, e);//This method is called to update the fields related to rdos of clockSize.
            //Drawing Ref
            switch (prayerTimingSettings.DrawingRef)
            {
                case DrawingRefType.Pies:
                    rdoPies.Checked = true;
                    break;
                case DrawingRefType.Hour00:
                    rdoHour00.Checked = true;
                    break;
                default:
                    rdoPies.Checked = true;
                    break;
            }
            rdoBtnDrawingRefChanged(null, e);//This method is called to update the fields related to rdos of DrawingRef size.

            cmbTrayHoveType.SelectedItem = prayerTimingSettings.TrayHover.ToString();
            cmbPiesHoverText.SelectedItem = prayerTimingSettings.PiesHover.ToString();
            /*Setting Items
            //int index = getIndex(cmbStartAngle, (int)prayerTimingSettings.StartAngle);
            //cmbStartAngle.SelectedIndex = index == -1 ? 1 : index;// (object)(int)prayerTimingSettings.StartAngle;
            //Above method is to get the desired item selected using a method which gets index given a cmb and item text.
            */
            cmbStartAngle.SelectedItem = prayerTimingSettings.StartAngle.ToString();
            cmbSunHandWidth.SelectedItem = prayerTimingSettings.SunHandWidth.ToString();
            cmbSunPointTo.SelectedItem = prayerTimingSettings.SunHandPointTo.ToString();
            cmbSunPointAt.SelectedItem = prayerTimingSettings.SunHandPointAT.ToString();

            //Setting colors
            btnColorFajir.BackColor = new Pen(prayerTimingSettings.NamazPieBrushes[0]).Color;
            btnColorDay.BackColor = new Pen(prayerTimingSettings.NamazPieBrushes[1]).Color;
            btnColorZohar.BackColor = new Pen(prayerTimingSettings.NamazPieBrushes[2]).Color;
            btnColorAser.BackColor = new Pen(prayerTimingSettings.NamazPieBrushes[3]).Color;
            btnColorMaghrib.BackColor = new Pen(prayerTimingSettings.NamazPieBrushes[4]).Color;
            btnColorIsha.BackColor = new Pen(prayerTimingSettings.NamazPieBrushes[5]).Color;
            btnColorNight.BackColor = new Pen(prayerTimingSettings.NamazPieBrushes[6]).Color;

            btnColorDayTime.BackColor = new Pen(prayerTimingSettings.DaynightBrushes[0]).Color;
            btnColorNightTime.BackColor = new Pen(prayerTimingSettings.DaynightBrushes[1]).Color;

            btnColorSunHand.BackColor = prayerTimingSettings.SunHandColor;
        }
        private Bitmap mape;

        public Bitmap Mape
        {
            get { return mape; }
            set { mape = value; }
        }

        private int getIndex(ComboBox cmb, int value)
        {
            for (int i = 0; i < cmb.Items.Count; i++)
            {
                if (cmb.Items[i].ToString() == value.ToString())
                    return i;
            }
            return -1;
        }

        private void btnCitySearch_Click(object sender, EventArgs e)
        {


            /*********************** new code starts here ***********************/
            /*********************** new code starts here ***********************/


            //you can also use forms predefined events like this (your code was also fine for this purpose but this is only for your information)
            frmEditLocation newFrm = new frmEditLocation();
            newFrm.FormClosed += new FormClosedEventHandler(newFrm_FormClosed);
            newFrm.Show();
        }
        void newFrm_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmEditLocation frmLocation = (frmEditLocation)sender;
            City city = frmLocation.SelectedCity;
            if (city == null) return;// city may emtpy and can cause exceptions so return
            this.txtBoxCityName.Text = city.Name;
            prayerTimingSettings.Latitude = Convert.ToDouble(city.Latitude);
            prayerTimingSettings.Longitude = Convert.ToDouble(city.Longitude);
            prayerTimingSettings.TimeZone = Convert.ToDouble(getZone(city.Zone));
            prayerTimingSettings.CityName = city.Name;
            this.setZone(prayerTimingSettings.TimeZone.ToString());
            //Show Namaz Times

            DateTime[] dt = PT.GetPrayerTimes(DateTime.Now, prayerTimingSettings.Latitude, prayerTimingSettings.Longitude, prayerTimingSettings.TimeZone);
            txtSunRise.Text = dt[1].ToShortTimeString();
            txtSunSet.Text = dt[4].ToShortTimeString();
            //save location info to registry
            //DownLoading mape
            MapeSearch ms = new MapeSearch(prayerTimingSettings.Latitude, prayerTimingSettings.Longitude, prayerTimingSettings.CityName, (short)NumUDZoom.Value);
            ms.MapeSearchComplete += new MapeSearch.MapeSearchCompleteEventHandler(ms_MapeSearchComplete);
            mapeSearching = new Thread(new ThreadStart(ms.beginMapeSearch));
            mapeSearching.Start();
            lblMapeStatus.Text = "Downloading your place mape...";
            lblMapeStatus.Visible = true;
        }
        void ms_MapeSearchComplete(object sender, MapeSearchEventArgs e)
        {
            pbMape.Image = e.Mape;
            lblMapeStatus.Visible = false;
            btnIgnoreMape.Visible = false;
            NumUDZoom.Visible = true;
            lblZoom.Visible = true;
            mape = e.Mape;
            
            //lblLoadingMape.Dispose();
        }

        private void setZone(string timeZoneOffset)
        {
            txtBoxTimeZone.Text = "GMT " + (timeZoneOffset.Contains("-") ? "-" + timeZoneOffset : "+" + timeZoneOffset);
        }
        private float getZone(string timeZone)
        {
            //string zone = txtBoxTimeZone.Text[0]+txtBoxTimeZone.Text.Substring(
            return Convert.ToSingle(timeZone.Replace(':', '.'));
        }
        //Setting CMB Values
        //
        private object[] getCMBRange(int start, int end, int step)
        {
            int range = end - start;
            int totalSteps = range / step;
            object[] rList = new object[totalSteps + 1];
            for (int i = 0; i <= totalSteps; i++)
                rList[i] = start + i * step;

            return rList;
        }
        private int getP(double total, double percent)
        {
            return (int)(total * percent / 100);
        }
        private int getS(ComboBox cmb)
        {
            if (cmb.Items.Count < 1)
                return 0;
            int sIndex = cmb.SelectedIndex;
            return (int)cmb.Items[sIndex];
        }
        private void cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender.Equals(cmbTrayHoveType))
            {
                prayerTimingSettings.TrayHover = (TrayHoverType)Enum.Parse(typeof(TrayHoverType), cmbTrayHoveType.Text);
            }
            if (sender.Equals(cmbStartPie))
            {
                prayerTimingSettings.StartPie = Convert.ToInt32(cmbStartPie.Text);
            }
            if (sender.Equals(cmbStartAngle))
            {
                prayerTimingSettings.StartAngle = Convert.ToInt32(cmbStartAngle.Text);
            }
            if (sender.Equals(cmbSunHandWidth))
            {
                prayerTimingSettings.SunHandWidth = Convert.ToSingle(cmbSunHandWidth.Text);
            }
            if (sender.Equals(cmbSunPointAt))
            {
                prayerTimingSettings.SunHandPointAT = (SunHandPointAt)Enum.Parse(typeof(SunHandPointAt), cmbSunPointAt.Text);
            }
            if (sender.Equals(cmbSunPointTo))
            {
                prayerTimingSettings.SunHandPointTo = (SunHandPointTo)Enum.Parse(typeof(SunHandPointTo), cmbSunPointTo.Text);
            }
            if (sender.Equals(cmbPiesHoverText))
            {
                prayerTimingSettings.PiesHover = (PiesHoverType)Enum.Parse(typeof(PiesHoverType), cmbPiesHoverText.Text);
            }
            if (sender.Equals(cmbClockRadius))
            {

            }
        }
        private void setCMB_Enables(bool value)
        {
            if (value)
            {
                cmbClockRadius.Enabled = true;
            }
            else
            {
                cmbClockRadius.Enabled = false;
            }
        }
        private void btnSaveDimensions_Click(object sender, EventArgs e)
        {
            ///Setting dimension values....
            //First mulitlied by 2 cause it is radius and not the width or height as it is input
            //prayerTimingSettings.ClockRadius = 2 * Convert.ToInt32(cmbClockRadius.Text);

            //btnSaveDimensions_Click(null, null);
            prayerTimingSettings.SmallLinesRadius = prayerTimingSettings.BigLinesRadius = prayerTimingSettings.NamazRadius + 1;
            prayerTimingSettings.BigLinesWidth = prayerTimingSettings.NamazWidth / 2;
            prayerTimingSettings.SmallLinesWidth = prayerTimingSettings.BigLinesWidth / 2;
            prayerTimingSettings.SunHandRadius = prayerTimingSettings.NamazRadius * 0.99;
        }

        private void rdoBtnClockSizeChanged(object sender, EventArgs e)
        {
            setCMB_Enables(false);
            if (rdoBtnSmall.Checked)
            //if (sender.Equals(rdoBtnSmall))
            {

                prayerTimingSettings.ClockSize = ClockSizeType.Small;
            }
            //else if (sender.Equals(rdoBtnMedium))
            else if (rdoBtnMedium.Checked)
            {
                prayerTimingSettings.ClockSize = ClockSizeType.Medium;
            }
            //else if (sender.Equals(rdoBtnLarge))
            else if (rdoBtnLarge.Checked)
            {
                prayerTimingSettings.ClockSize = ClockSizeType.Large;
            }
            //else if (sender.Equals(rdoBtnManually))
            else if (rdoBtnManually.Checked)
            {
                cmbClockRadius.Items.AddRange(getCMBRange(50, 400, 50));
                prayerTimingSettings.ClockSize = ClockSizeType.Manual;
                cmbClockRadius.SelectedIndex = cmbClockRadius.Items.Count - 1;

                setCMB_Enables(true);
            }
            btnSaveDimensions_Click(null, e);
        }
        private void rdoBtnDrawingRefChanged(object sender, EventArgs e)
        {
            if (rdoPies.Checked)
            {
                cmbStartPie.Enabled = true;
                prayerTimingSettings.StartPie = cmbStartPie.SelectedIndex = 1;
                prayerTimingSettings.DrawingRef = DrawingRefType.Pies;
            }
            if (rdoHour00.Checked)
            {
                cmbStartPie.Enabled = false;
                prayerTimingSettings.StartPie = cmbStartPie.SelectedIndex = 7;
                prayerTimingSettings.DrawingRef = DrawingRefType.Hour00;
            }
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            colorDialog.ShowDialog();
            Color color = colorDialog.Color;
            Brush brush = new Pen(color).Brush;
            if (sender.Equals(btnColorFajir))
            {
                prayerTimingSettings.NamazPieBrushes[0] = brush;
                btnColorFajir.BackColor = color;
            }
            if (sender.Equals(btnColorDay))
            {
                prayerTimingSettings.NamazPieBrushes[1] = brush;
                btnColorDay.BackColor = color;
            }
            if (sender.Equals(btnColorZohar))
            {
                prayerTimingSettings.NamazPieBrushes[2] = brush;
                btnColorZohar.BackColor = color;
            }
            if (sender.Equals(btnColorAser))
            {
                prayerTimingSettings.NamazPieBrushes[3] = brush;
                btnColorAser.BackColor = color;
            }
            if (sender.Equals(btnColorMaghrib))
            {
                prayerTimingSettings.NamazPieBrushes[4] = brush;
                btnColorMaghrib.BackColor = color;
            }
            if (sender.Equals(btnColorIsha))
            {
                prayerTimingSettings.NamazPieBrushes[5] = brush;
                btnColorIsha.BackColor = color;
            }
            if (sender.Equals(btnColorNight))
            {
                prayerTimingSettings.NamazPieBrushes[6] = brush;
                btnColorNight.BackColor = color;
            }
            if (sender.Equals(btnColorBlank))
            {
                //namazPieBrushes[0] = brush;
                btnColorBlank.BackColor = color;
            }
            //DayNight
            if (sender.Equals(btnColorDayTime))
            {
                prayerTimingSettings.NamazPieBrushes[0] = brush;
                btnColorDayTime.BackColor = color;
            }
            if (sender.Equals(btnColorNightTime))
            {
                prayerTimingSettings.NamazPieBrushes[0] = brush;
                btnColorNightTime.BackColor = color;
            }
            //SunHand
            if (sender.Equals(btnColorSunHand))
            {
                btnColorSunHand.BackColor = prayerTimingSettings.SunHandColor = color;
            }
        }

        private void NumUDZoom_ValueChanged(object sender, EventArgs e)
        {
            if (mapeSearching != null)
                mapeSearching.Abort();
            btnIgnoreMape.Visible = true;
            lblMapeStatus.Visible = true;
            lblMapeStatus.Text = "Refreshing Mape...";
            MapeSearch ms = new MapeSearch(prayerTimingSettings.Latitude, prayerTimingSettings.Longitude, prayerTimingSettings.CityName, (short)NumUDZoom.Value);
            ms.MapeSearchComplete += new MapeSearch.MapeSearchCompleteEventHandler(ms_MapeSearchComplete);
            mapeSearching = new Thread(new ThreadStart(ms.beginMapeSearch));
            mapeSearching.Start();
        }

        private void btnIgnoreMape_Click(object sender, EventArgs e)
        {

            mapeSearching.Abort();
            btnIgnoreMape.Visible = false;
            if (pbMape.Image == null)
                lblMapeStatus.Text = "Mape of place is displayed here.";
            else
                lblMapeStatus.Visible = false;
            NumUDZoom.Visible = true;
            lblZoom.Visible = true;
        }

    }
    #region MapeSearching Threading
    public class MapeSearchEventArgs
    {
        private Bitmap mape;
        public MapeSearchEventArgs(Bitmap mape)
        {
            this.mape = mape;
        }
        public Bitmap Mape
        {
            get { return mape; }
            set { mape = value; }
        }

    }
    public class MapeSearch
    {
        private double latitude;
        private double longitude;
        private string cityName;
        private short mapeZoom;
        public MapeSearch(double latitude, double longitude, string cityName, short mapeZoom)
        {
            this.mapeZoom = mapeZoom;
            this.latitude = latitude;
            this.longitude = longitude;
            this.cityName = cityName;
        }
        public delegate void MapeSearchCompleteEventHandler(object sender, MapeSearchEventArgs e);
        public event MapeSearchCompleteEventHandler MapeSearchComplete;
        public void beginMapeSearch()
        {
            //Now this thread is running in main class to make is possible to abort by a button there.
            //Thread mapeSearching = new Thread(new ThreadStart(delegate()
            //{
            try
            {
                //Download Image
                String url = "http://maps.google.com/maps/api/staticmap?center=" + latitude + "," + longitude + "&zoom=" + mapeZoom + "&size=464x170&markers=color:blue%7Clabel:?%7C" + latitude + "," + longitude + "&sensor=false";
                Uri uri = new Uri(url);
                HttpWebRequest httpRequest = (HttpWebRequest)HttpWebRequest.Create(uri);
                HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();
                Stream imageStream = httpResponse.GetResponseStream();
                Bitmap mape = new Bitmap(imageStream);
                httpResponse.Close();
                imageStream.Close();
                onMapeSearchComplete(mape);
            }
            catch (WebException ex)
            {
                MessageBox.Show("Can't Access Network or no connection found. Or try again.\n Error:\n" + ex.Message);
            }
            //}));
            //mapeSearching.Start();
        }
        public void onMapeSearchComplete(Bitmap mape)
        {
            MapeSearchCompleteEventHandler handler = MapeSearchComplete;

            if (handler != null)
            {
                foreach (MapeSearchCompleteEventHandler MapeSearchCompleteHandler in handler.GetInvocationList())
                {
                    ISynchronizeInvoke syncInvoke = MapeSearchCompleteHandler.Target as ISynchronizeInvoke;
                    MapeSearchEventArgs args = new MapeSearchEventArgs(mape);

                    try
                    {
                        if (syncInvoke != null && syncInvoke.InvokeRequired)
                        {
                            // Invokie SearchComplete on the main thread with safty
                            syncInvoke.Invoke(handler, new object[] { this, args });
                        }
                        else
                        {
                            // Raise SearchComplete event
                            MapeSearchCompleteHandler(this, args);
                        }
                    }
                    catch { }
                }
            }
        }
    }
    #endregion
}