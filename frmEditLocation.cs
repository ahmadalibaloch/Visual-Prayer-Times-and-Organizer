using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.Threading;
namespace PrayerTiming
{
    public partial class frmEditLocation : Form
    {

        //when this form is closed, the calling form (settings form) access selected city by this public field
        //it should be in the form of Property not Public field. (do it yourself)
        private City selectedCity;
        public City SelectedCity
        {
            get
            {
                return selectedCity;
            }

        }

        public frmEditLocation()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            CitySearch citySearch = new CitySearch(textBoxPlace.Text);
            citySearch.SearchComplete += new CitySearch.SearchCompleteEventHandler(citySearch_SearchComplete);
            lblStatus.Text = "Searching place information. Please wait...";
            btnLocationOk.Enabled = false;
            btnSearch.Enabled = false;
            citySearch.BeginSearch();
        }

        void citySearch_SearchComplete(object sender, SearchCompleteEventArgs e)
        {
            lblStatus.Text = "Search completed Successfully.";
            btnLocationCancel.Enabled = true;
            btnLocationOk.Enabled = true;
            btnSearch.Enabled = true;
            List<City> cities = new List<City>(13);
            cities.Add(e.City);

            //now no need to use dataGridLocations.Invoke, Class CitySearch raises Thread safe event SearchComplete
            dataGridLocations.AutoGenerateColumns = false;
            dataGridLocations.DataSource = cities;

        }

        private void btnLocationOk_Click(object sender, EventArgs e)
        //show multiple cities if found, then by grid events selected on city
        {
            try
            //added for if grid is empty, it may raise error when accessing cell values
            {
                this.selectedCity = new City(dataGridLocations.Rows[0].Cells[0].Value.ToString(), dataGridLocations.Rows[0].Cells[1].Value.ToString(), dataGridLocations.Rows[0].Cells[2].Value.ToString(), dataGridLocations.Rows[0].Cells[3].Value.ToString());
            }
            catch { }

            //do not dispose the form. it will reopen faster.
            this.Close();
        }

        private void btnLocationCancel_Click(object sender, EventArgs e)
        {
            this.selectedCity = null;
            //do not dispose the form. it will reopen faster.
            this.Close();
        }

        private void frmEditLocation_Load(object sender, EventArgs e)
        {
            //Good
            textBoxPlace.Text = System.Globalization.RegionInfo.CurrentRegion.EnglishName;
            this.selectedCity = null;
        }
    }

    #region Helper Classes

    public class SearchCompleteEventArgs : System.EventArgs
    {

        private City city;
        // class constructor
        public SearchCompleteEventArgs(City city)
        {
            this.city = city;

        }

        public City City
        {
            get
            {
                return city;
            }
        }
    }

    // name of the class should be City
    public class City
    {
        public City(string name, string latitude, string longitude, string zone)
        {
            this.name = name;
            this.latitude = latitude;
            this.longitude = longitude;
            this.zone = zone;
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        private string latitude;
        public string Latitude
        {
            get
            {
                return latitude;
            }
        }

        private string longitude;
        public string Longitude
        {
            get
            {
                return longitude;
            }
        }
        private string zone;
        public string Zone
        {
            get
            {
                return zone;
            }
        }
    }

    public class CitySearch
    {
        private string address;

        public delegate void SearchCompleteEventHandler(object sender, SearchCompleteEventArgs e);

        public event SearchCompleteEventHandler SearchComplete;

        public CitySearch(string address)
        {
            this.address = address;
        }

        public void BeginSearch()
        {
            new Thread(new ThreadStart(delegate()
            {
                try
                {
                    //Sample URL=http://maps.googleapis.com/maps/api/geocode/xml?address=Thamewali&sensor=true
                    string urlStr = "http://maps.googleapis.com/maps/api/geocode/xml?address=" + this.address + "&sensor=true";
                    XmlDocument objXml = new XmlDocument();
                    objXml.Load(urlStr);
                    XmlNode locationNode = objXml.SelectSingleNode("GeocodeResponse/result/geometry/location");
                    string lat = locationNode.FirstChild.InnerText;
                    string lng = locationNode.LastChild.InnerText;
                    string cityName = objXml.SelectSingleNode("GeocodeResponse/result/address_component/short_name").FirstChild.InnerText;


                    ///////////////////////////////////
                    //Sample URL=http://ws.geonames.org/timezone?lat=47.01&lng=10.2&username=demo
                    urlStr = "http://ws.geonames.org/timezone?lat=" + lat + "&lng=" + lng + "&username=ahmadalibaloch";
                    objXml.Load(urlStr);
                    string zone = objXml.SelectSingleNode("geonames/timezone/gmtOffset").InnerText;
                    int firstDigit = Convert.ToInt32(zone.Substring(0, zone.IndexOf(".")));
                    int secondDigit = Convert.ToInt32(zone.Substring(zone.Length - 1, 1));
                    zone = (firstDigit < 10 ? (firstDigit > 0 ? "0" + firstDigit : "-0" + firstDigit.ToString().Substring(1)) : firstDigit.ToString()) + ":" + secondDigit * 6;
                    /////////////////////////////
                    City city = new City(cityName, lat, lng, zone);
                    OnSearchComplete(city);
                }
                catch (WebException ex)
                {
                    MessageBox.Show("Can't Access Network or no connection found. Or try again.\n Error:\n" + ex.Message);
                    
                }
            })).Start();
        }

        private void OnSearchComplete(City city)
        {
            /* simple method but not thread safe, you have to use control.Invoke on grid

            SearchCompleteEventHandler handler = SearchComplete;
            if (handler != null)
            {
                SearchComplete(this, new SearchCompleteEventArgs(city));
            }

             **********************************************************/

            /* so do this */
            SearchCompleteEventHandler handler = SearchComplete;

            if (handler != null)
            {
                foreach (SearchCompleteEventHandler searchCompleteHandler in handler.GetInvocationList())
                {
                    ISynchronizeInvoke syncInvoke = searchCompleteHandler.Target as ISynchronizeInvoke;
                    SearchCompleteEventArgs args = new SearchCompleteEventArgs(city);

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
                            searchCompleteHandler(this, args);
                        }
                    }
                    catch { }
                }
            }
        }
    }

    #endregion
}