using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Microsoft.Win32;
using System.Globalization;

namespace PrayerTiming
{
    /// <summary>
    /// the type of pies going to show in drawing
    /// </summary>
    public enum ClockPieType
    {
        both = 0, namaz, daynight
    }
    public enum DrawingRefType
    {
        Pies = 0, Hour00
    }
    public enum PiesHoverType
    {
        RemainingTime = 0, NextNamazTime, PassedPercent
    }
    public enum TrayHoverType
    {
        HijriDateTime = 0,
        NextEventTime,
        NextNamazTime,
        TimetoNextNamaz,
        TimetoNextEvent
    }
    public enum ClockSizeType
    {
        Small = 0, Medium, Large, Manual
    }
    public enum SunHandPointTo
    {
        NamazPies = 1, DayNightPies
    }
    public enum SunHandPointAt
    {
        Start = 0, Middle, End
    }

    public enum DigitalClockPosition
    {
        Bottom = 0, Top, Left, Right
    }
    public enum DigitalClockSize
    {
        Small = 0, Medium, Large
    }
    public partial class frmPrayerTiming : Form
    {
        public frmPrayerTiming()
        {
            InitializeComponent();
            //this.SetStyle(ControlStyles.CacheText, true);
            this.BackColor = Color.LightBlue;
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(620, 10);
            this.ShowInTaskbar = false;
            Timer.Start();
            notifyIcon.Visible = true;
            //this.SetStyle(ControlStyles.UserPaint, false);
            //this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //this.SetStyle(ControlStyles.Opaque, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        #region Initialization of objects and variables

        //Prayer Times Class from Islam Library for prayer timing
        PrayerTimes PT = new PrayerTimes();
        //prayerTimingSettings prayerTimingSettings = new prayerTimingSettings();

        double[,]
            namazPieDataArray,
            daynightPieDataArray;
        //Graphics Objects and their data
        GraphicsPath[]
            namazPies,
            daynightPies;
        //for saving prayerTimingSettings hand location for invalidation below
        GraphicsPath oldHand;

        //Tooltip for mouse hover on pies
        ToolTip hoverToolTip = new ToolTip();

        #endregion
        //==============================================================================
        private void Form1_Load(object sender, EventArgs e)
        {
            prayerTimingSettings.NamazPieBrushes = new Brush[8] { Brushes.DarkGray, Brushes.NavajoWhite, Brushes.Yellow, Brushes.DarkSeaGreen, Brushes.Black, Brushes.SlateGray, Brushes.DimGray, Brushes.Black };
            prayerTimingSettings.DaynightBrushes = new Brush[2] { Brushes.BurlyWood, Brushes.Black };
            prayerTimingSettings.SunHandColor = Color.Red;
            //Digital Clock Settings
            prayerTimingSettings.ShowDigitalClock = true;
            prayerTimingSettings.DigitalClockSize = DigitalClockSize.Large;
            prayerTimingSettings.DigitalClockPosition = DigitalClockPosition.Right;
            prayerTimingSettings.DigitalClockColor = Brushes.Wheat;
            prayerTimingSettings.DigitalClockBackColor = Brushes.Black;
            //PieTypeSettings

            prayerTimingSettings.ClockPies = ClockPieType.namaz;
            prayerTimingSettings.PiesHover = PiesHoverType.RemainingTime;
            prayerTimingSettings.ClockSize = ClockSizeType.Large;
            prayerTimingSettings.SunHandPointTo = SunHandPointTo.NamazPies;

            //Prayer timing class customization
            prayerTimingSettings.Latitude = 32.785;//51.25;//
            prayerTimingSettings.Longitude = 71.7803;//6.966667;//
            prayerTimingSettings.DateTime = DateTime.Now;
            prayerTimingSettings.TimeZone = 5.0;//2;//
            prayerTimingSettings.CityName = "Thamewali";

            //The Drawing(farm) size and coordinate settings
            prayerTimingSettings.ClockRadius = 400f;
            Width = Height = (int)prayerTimingSettings.ClockRadius;            //Pies            

            prayerTimingSettings.StartAngle = 270;
            prayerTimingSettings.StartPie = 1;
            prayerTimingSettings.DrawingRef = DrawingRefType.Pies;
            //Default Values....that should be loaded from a file 
            prayerTimingSettings.DayNightRadius = 50;
            prayerTimingSettings.DayNightWidth = 25;
            prayerTimingSettings.NamazRadius = 100;
            prayerTimingSettings.NamazWidth = 50;

            prayerTimingSettings.NumbersRadius = prayerTimingSettings.ClockPies == ClockPieType.namaz ? prayerTimingSettings.NamazRadius + prayerTimingSettings.NamazWidth : prayerTimingSettings.DayNightRadius + prayerTimingSettings.DayNightWidth;
            prayerTimingSettings.NumbersRadius -= 10;
            prayerTimingSettings.NumbersOffset = -7;

            prayerTimingSettings.BigLinesRadius = prayerTimingSettings.SmallLinesRadius = prayerTimingSettings.NamazRadius + 1;
            prayerTimingSettings.BigLinesWidth = prayerTimingSettings.NamazWidth * 0.5;
            prayerTimingSettings.SmallLinesWidth = prayerTimingSettings.BigLinesWidth * 0.5;
            prayerTimingSettings.SunHandRadius = prayerTimingSettings.NamazRadius * 0.99;
            prayerTimingSettings.SunHandWidth = 2.5f;
            ///TrueFalls for different things to draw or not
            prayerTimingSettings.ShowHand = true;
            prayerTimingSettings.ShowBigLines = true;
            prayerTimingSettings.ShowSmallLines = true;
            prayerTimingSettings.ShowNumbers = true;
            prayerTimingSettings.NumbersSize = 8;

            //ToolTip Settings
            hoverToolTip.AutoPopDelay = 5000;
            hoverToolTip.InitialDelay = 500;
            hoverToolTip.ReshowDelay = 100;
            hoverToolTip.UseFading = false;
            hoverToolTip.ShowAlways = true;
            //TrayIcon Tooltip
            prayerTimingSettings.TrayHover = TrayHoverType.TimetoNextNamaz;
            //Custom trayIcon drawing below
            ////notifyIcon.Icon = DrawIcon(pt.GetTime(1).ToShortTimeString());
            //Custom Context Menu for trayIcon below
            System.Windows.Forms.ContextMenu cm = new ContextMenu();
            for (int i = 0; i <= 6; i++)
            {
                cm.MenuItems.Add(PT.GetPrayerTimes()[i].ToShortTimeString());
            }
            cm.MenuItems.Add("-");
            cm.MenuItems.Add("E&xit");
            //notifyIcon.ContextMenu = cm;

            //Now Setting the region of farm
            this.Region = GetRegion();
            //Loading Settings from Registry
            registryKey();
            // registryLocation("Thamewali", "123", "321");

            if (!locationFound())
            {
                //MessageBox.Show("No location set, Please set your location.", "No Location", MessageBoxButtons.OK);
                this.settingsToolStripMenuItem_Click(sender, e);
            }
        }
        #region GraphicsMethods
        /// <summary>
        /// The main drawing method which gives graphics object to all subdrawing methods.
        /// </summary>
        /// <param name="graphics">The main graphic context object</param>
        private void Draw(Graphics graphics)
        {
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            drawPies(graphics);
            drawRulers(graphics, prayerTimingSettings.ClockPies);
            drawNumbers(graphics);
            drawHand(graphics);
            drawDigitalClock(graphics);
            //Disposing
            graphics.Dispose();
        }
        void drawPies(Graphics graphics)
        {
            //===Times Pie
            int[] prayerMinutes = this.PT.GetPrayerMinutes();
            namazPieDataArray = this.GetPieData(prayerMinutes, prayerTimingSettings.StartAngle, prayerTimingSettings.StartPie);
            daynightPieDataArray = this.GetPieData(new int[] { PT.TotalDayMinutes, PT.TotalNightMinutes }, prayerTimingSettings.StartAngle, 0);

            switch (prayerTimingSettings.ClockPies)
            {
                case ClockPieType.both:
                    this.Region = GetRegion();
                    namazPies = new GraphicsPath[namazPieDataArray.Length / 2];
                    daynightPies = new GraphicsPath[daynightPieDataArray.Length / 2];
                    for (int i = 0; i < 8; i++)//drawing the 7 pies from above data
                    {
                        namazPies[i] = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.NamazRadius, prayerTimingSettings.NamazWidth, namazPieDataArray[i, 0], namazPieDataArray[i, 1]);
                        graphics.FillPath(prayerTimingSettings.NamazPieBrushes[i], namazPies[i]);
                    }
                    daynightPies[0] = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.DayNightRadius, prayerTimingSettings.DayNightWidth, prayerTimingSettings.StartAngle, daynightPieDataArray[0, 1]);
                    daynightPies[1] = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.DayNightRadius, prayerTimingSettings.DayNightWidth, daynightPieDataArray[1, 0], daynightPieDataArray[1, 1]);
                    graphics.FillPath(prayerTimingSettings.DaynightBrushes[0], daynightPies[0]);
                    graphics.FillPath(prayerTimingSettings.DaynightBrushes[1], daynightPies[1]);

                    break;
                case ClockPieType.namaz:
                    this.Region = GetRegion();
                    namazPies = new GraphicsPath[8];
                    for (int i = 0; i < namazPieDataArray.Length / 2; i++)//drawing the 8 pies from above data
                    {
                        namazPies[i] = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.NamazRadius, prayerTimingSettings.NamazWidth, namazPieDataArray[i, 0], namazPieDataArray[i, 1]);
                        graphics.FillPath(prayerTimingSettings.NamazPieBrushes[i], namazPies[i]);
                    }
                    daynightPies = null;
                    break;
                case ClockPieType.daynight:
                    this.Region = GetRegion();
                    daynightPies = new GraphicsPath[2];

                    //daynightPies[0] = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.DayNightRadius, prayerTimingSettings.DayNightWidth, prayerTimingSettings.StartAngle, daynightPieDataArray[0, 1]);
                    //daynightPies[1] = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.DayNightRadius, prayerTimingSettings.DayNightWidth, daynightPieDataArray[1, 0], daynightPieDataArray[1, 1]);

                    daynightPies[0] = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.NamazRadius, prayerTimingSettings.NamazWidth, prayerTimingSettings.StartAngle, daynightPieDataArray[0, 1]);
                    daynightPies[1] = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.NamazRadius, prayerTimingSettings.NamazWidth, daynightPieDataArray[1, 0], daynightPieDataArray[1, 1]);
                    graphics.FillPath(prayerTimingSettings.DaynightBrushes[0], daynightPies[0]);
                    graphics.FillPath(prayerTimingSettings.DaynightBrushes[1], daynightPies[1]);
                    namazPies = null;
                    break;
                default:
                    goto case ClockPieType.namaz;
            }

        }
        void drawRulers(Graphics graphics, ClockPieType PiesType)
        {
            //// The Ruler lines
            PointF[] linePoints;//which carries the start and end point of the line to be drawn
            double smallLinesRadius = 0, smallLinesWidth = 0, bigLinesRadius = 0, bigLinesWidth = 0;
            switch (PiesType)
            {
                case ClockPieType.both:
                    smallLinesRadius = bigLinesRadius = prayerTimingSettings.SmallLinesRadius;
                    bigLinesWidth = prayerTimingSettings.NamazWidth / 2;
                    smallLinesWidth = bigLinesWidth / 2;
                    drawRulers(graphics, ClockPieType.daynight);
                    break;
                case ClockPieType.namaz:
                    smallLinesRadius = bigLinesRadius = prayerTimingSettings.SmallLinesRadius;
                    bigLinesWidth = prayerTimingSettings.NamazWidth / 2;
                    smallLinesWidth = bigLinesWidth / 2;
                    break;
                case ClockPieType.daynight:
                    if (prayerTimingSettings.ClockPies != ClockPieType.both)
                        goto case ClockPieType.namaz;
                    else
                    {
                        smallLinesRadius = bigLinesRadius = prayerTimingSettings.DayNightRadius;
                        bigLinesWidth = prayerTimingSettings.DayNightWidth / 2;
                        smallLinesWidth = bigLinesWidth / 2;
                    }
                    break;
                default:
                    goto case ClockPieType.namaz;
            }
            if (prayerTimingSettings.ShowSmallLines && prayerTimingSettings.ShowBigLines)
            {
                for (int i = 0; i <= 95; i++)
                {
                    linePoints = getSizeLinePoints(prayerTimingSettings.ClockCenterPoint, smallLinesRadius, smallLinesWidth, halfNightAngle() + i * 3.75);//the first ruler line angle starts from HalfNightAnlge+number of line *3.75 for 96 lines as 96*3.75=360
                    if (i % 4 != 0)
                    {
                        graphics.DrawLine(Pens.Red, linePoints[0], linePoints[1]);
                    }
                    else
                    {
                        linePoints = getSizeLinePoints(prayerTimingSettings.ClockCenterPoint, bigLinesRadius, bigLinesWidth, halfNightAngle() + i * 3.75);
                        graphics.DrawLine(Pens.Red, linePoints[0], linePoints[1]);
                    }
                }
            }
            else
            {
                if (prayerTimingSettings.ShowBigLines)
                {
                    for (int i = 0; i <= 96; i += 6)
                    {
                        linePoints = getSizeLinePoints(prayerTimingSettings.ClockCenterPoint, bigLinesRadius, bigLinesWidth, halfNightAngle() + i * 3.75);
                        graphics.DrawLine(Pens.Red, linePoints[0], linePoints[1]);
                    }
                }
                if (prayerTimingSettings.ShowSmallLines)
                {
                    for (int i = 0; i <= 95; i++)
                    {
                        linePoints = getSizeLinePoints(prayerTimingSettings.ClockCenterPoint, smallLinesRadius, smallLinesWidth, halfNightAngle() + i * 3.75);
                        graphics.DrawLine(Pens.Red, linePoints[0], linePoints[1]);
                    }
                }
            }
        }
        void drawNumbers(Graphics graphics)
        {
            PointF textPoint = prayerTimingSettings.ClockCenterPoint;//which carries the start point of text printed on each hour line
            textPoint.X += prayerTimingSettings.NumbersOffset; textPoint.Y += prayerTimingSettings.NumbersOffset;//offset for making numbers equal distance from center
            if (prayerTimingSettings.ShowNumbers)
            {
                for (int i = 0; i <= 95; i += 4)
                { graphics.DrawString((i / 4).ToString(), new Font(new FontFamily("Arial"), prayerTimingSettings.NumbersSize), Brushes.LightBlue, getEndLinePoint(textPoint, prayerTimingSettings.NumbersRadius, halfNightAngle() + i * 3.75)); }
            }
        }
        void drawHand(Graphics graphics)
        {
            //The Sun Hand
            double sunHandRadius = 0;
            if (prayerTimingSettings.ClockPies != ClockPieType.both)
                prayerTimingSettings.SunHandPointTo = SunHandPointTo.NamazPies;//because we are taking daynight and namaz of same radious.
            switch (prayerTimingSettings.SunHandPointTo)
            {
                case SunHandPointTo.NamazPies:
                    switch (prayerTimingSettings.SunHandPointAT)
                    {
                        case SunHandPointAt.Start:
                            sunHandRadius = prayerTimingSettings.NamazRadius;
                            break;
                        case SunHandPointAt.Middle:
                            sunHandRadius = prayerTimingSettings.NamazRadius + prayerTimingSettings.NamazWidth / 2;
                            break;
                        case SunHandPointAt.End:
                            sunHandRadius = prayerTimingSettings.NamazRadius + prayerTimingSettings.NamazWidth;
                            break;
                        default:
                            sunHandRadius = prayerTimingSettings.NamazRadius;
                            break;
                    }
                    break;
                case SunHandPointTo.DayNightPies:
                    switch (prayerTimingSettings.SunHandPointAT)
                    {
                        case SunHandPointAt.Start:
                            sunHandRadius = prayerTimingSettings.DayNightRadius;
                            break;
                        case SunHandPointAt.Middle:
                            sunHandRadius = prayerTimingSettings.DayNightRadius + prayerTimingSettings.DayNightWidth / 2;
                            break;
                        case SunHandPointAt.End:
                            sunHandRadius = prayerTimingSettings.DayNightRadius + prayerTimingSettings.DayNightWidth;
                            break;
                        default:
                            sunHandRadius = prayerTimingSettings.DayNightRadius;
                            break;
                    }
                    break;
                default:
                    goto case SunHandPointTo.NamazPies;
            }
            prayerTimingSettings.SunHandRadius = sunHandRadius;
            graphics.DrawPath(sunPen(prayerTimingSettings.SunHandWidth), SunHand(prayerTimingSettings.ClockCenterPoint, sunHandRadius));
        }
        void drawDigitalClock(Graphics graphics)
        {
            if (prayerTimingSettings.ClockPies == ClockPieType.both)
                return;
            graphics.FillPath(prayerTimingSettings.DigitalClockBackColor, DrawBoxSimple(digitalClockPoint(prayerTimingSettings.DigitalClockPosition), prayerTimingSettings.DigitalClockHeight, prayerTimingSettings.DigitalClockWidth));
            //graphics.DrawString(DateTime.Now.ToShortTimeString(), new Font(new FontFamily("Arial"), 20), Brushes.Black, digitalClockPoint(DigitalClockPosition.Right));
            graphics.FillPath(prayerTimingSettings.DigitalClockColor, PrintString(digitalClockPoint(prayerTimingSettings.DigitalClockPosition), DateTime.Now.ToShortTimeString(), 20));
        }
        private void refresh()
        {
            System.Drawing.Region reg = new Region(oldHand);

            this.Invalidate(reg);
            this.Invalidate(new Region(DrawBoxSimple(digitalClockPoint(prayerTimingSettings.DigitalClockPosition), 25, 86)));
            this.Region = GetRegion();
            Graphics g = this.CreateGraphics();
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawPath(sunPen(prayerTimingSettings.SunHandWidth), SunHand(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.SunHandRadius));
        }

        System.Drawing.Pen sunPen(float width)
        {
            Pen sunPen = new Pen(prayerTimingSettings.SunHandColor, width);
            sunPen.EndCap = LineCap.ArrowAnchor;
            sunPen.StartCap = LineCap.RoundAnchor;
            return sunPen;
        }
        System.Drawing.Region GetRegion()
        {
            GraphicsPath gp = new GraphicsPath();
            //adding a graphic path shape just like containing all graphic elements

            switch (prayerTimingSettings.ClockPies)
            {
                case ClockPieType.both:
                    gp = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.NamazRadius, prayerTimingSettings.NamazWidth, 0, 360);
                    gp.AddPath(PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.DayNightRadius, prayerTimingSettings.DayNightWidth, 0, 360), false);
                    break;
                case ClockPieType.namaz:
                    gp = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.NamazRadius, prayerTimingSettings.NamazWidth, 0, 360);
                    break;
                case ClockPieType.daynight:
                    gp = PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.NamazRadius, prayerTimingSettings.NamazWidth, 0, 360);
                    //                    gp.AddPath(PieGraph(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.DayNightRadius, prayerTimingSettings.DayNightWidth, 0, 360), false);
                    break;
                default:
                    break;
            }
            if (prayerTimingSettings.ShowHand)
            {
                GraphicsPath sun = SunHand(prayerTimingSettings.ClockCenterPoint, prayerTimingSettings.SunHandRadius);
                sun.Widen(sunPen(prayerTimingSettings.SunHandWidth));
                gp.AddPath(sun, false);
            }
            if (prayerTimingSettings.ShowDigitalClock && prayerTimingSettings.ClockPies != ClockPieType.both)
            {
                gp.AddPath(DrawBoxSimple(digitalClockPoint(prayerTimingSettings.DigitalClockPosition), prayerTimingSettings.DigitalClockHeight, prayerTimingSettings.DigitalClockWidth), true);
            }
            //also set the width and height of the farm here. or make a new setRegion method etc
            this.Height = this.Width = (int)prayerTimingSettings.ClockRadius;
            return new Region(gp);//here you can through an exception if no bool is true.
            //giving the elements containing shape to region
        }
        private System.Drawing.Drawing2D.GraphicsPath SunHand(PointF center, double length)//which returns the path of sunhand and saves current location to oldhand GPath
        {
            GraphicsPath gp = new GraphicsPath();
            double drawAngle = this.TimeAngle();
            //center.Offset(-15,-15);length
            PointF sunEnd;
            sunEnd = this.getEndLinePoint(center, length, drawAngle);
            gp.AddLine(center, sunEnd);
            //gp.Widen(sunPen(prayerTimingSettings.SunHandWidth));
            #region Saving this location to oldhand to invalidate after hand moves on from here
            PointF point1 = getEndLinePoint(center, prayerTimingSettings.SunHandWidth, drawAngle - 90);
            PointF point2 = getEndLinePoint(point1, length + 5, drawAngle);
            PointF point3 = getEndLinePoint(point2, prayerTimingSettings.SunHandWidth * 2, drawAngle + 90);
            PointF point4 = getEndLinePoint(point3, length - 5, drawAngle + 180);
            PointF[] points = new PointF[] { point1, point2, point3, point4 };
            GraphicsPath gp1 = new GraphicsPath();
            gp1.AddPolygon(points);
            oldHand = (GraphicsPath)gp1.Clone();
            gp1.Dispose();
            #endregion
            return gp;
        }
        private System.Drawing.Drawing2D.GraphicsPath PieGraph(PointF center, double radius, double width, double startAngle, double sweepAngle)
        {
            GraphicsPath gp = new GraphicsPath();
            PointF[] PieCircle = this.DrawCircle(center.X, center.Y, radius);
            RectangleF PieRect = new RectangleF(PieCircle[0].X, PieCircle[0].Y, PieCircle[1].X, PieCircle[1].X);
            gp.AddLine(getEndLinePoint(center, radius, startAngle), getEndLinePoint(center, radius + width, startAngle));
            gp.AddArc(PieRect, (float)startAngle, (float)sweepAngle);
            gp.AddLine(getEndLinePoint(center, radius, startAngle + sweepAngle), getEndLinePoint(center, radius + width, startAngle + sweepAngle));
            PieCircle = this.DrawCircle(center.X, center.Y, radius + width);
            PieRect = new RectangleF(PieCircle[0].X, PieCircle[0].Y, PieCircle[1].X, PieCircle[1].X);
            gp.AddArc(PieRect, (float)(startAngle + sweepAngle), -(float)sweepAngle);
            return gp;
        }
        private System.Drawing.Drawing2D.GraphicsPath DrawBoxFromCenter(PointF center, float height, float width)
        {
            GraphicsPath gp = new GraphicsPath();
            RectangleF rcf = new RectangleF(center.X - width / 2, center.Y - height, width, height);
            gp.AddRectangle(rcf);
            return gp;
        }
        private System.Drawing.Drawing2D.GraphicsPath DrawBoxSimple(PointF center, float height, float width)
        {
            GraphicsPath gp = new GraphicsPath();
            RectangleF rcf = new RectangleF(center.X, center.Y, width, height);
            gp.AddRectangle(rcf);
            return gp;
        }
        private System.Drawing.Drawing2D.GraphicsPath PrintString(PointF center, string text, int size)
        {

            Font tmpFont = new Font("Arial", 8, FontStyle.Regular, GraphicsUnit.Pixel);
            PointF dayPoint = (PointF)center;
            GraphicsPath gp = new GraphicsPath();
            //center.Offset(-(text.Length * 4), -(size / 2 + 2));
            gp.AddString(text, tmpFont.FontFamily, 1, size, center, StringFormat.GenericDefault);
            return gp;
            //Brush BlackBrush = Brushes.Black;
            //graphics.DrawString("Day", tmpFont, BlackBrush, dayPoint);
            //graphics.DrawString("Night", tmpFont, BlackBrush, nightPoint);
            //graphics.DrawString("*Red line shows current time of day or night.", tmpFont, BlackBrush, notePoint);
        }
        #endregion
        #region MathTypeFunctions
        private System.Drawing.Icon DrawIcon(string IconMessage)
        {
            Icon oIcon = null;
            try
            {
                Bitmap bm = new Bitmap(16, 16);
                Graphics g = Graphics.FromImage((Image)bm);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                Font oFont = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);
                g.FillRectangle(Brushes.Tomato, new Rectangle(0, 0, bm.Width, bm.Height));
                //g.FillEllipse(Brushes.White, 0, 0, dimension,dimension);
                g.FillRectangle(Brushes.LightGreen, new Rectangle(0, 0, bm.Width, bm.Height - 10));
                g.DrawString(IconMessage, oFont, new SolidBrush(System.Drawing.Color.Black), 0, 5);
                oIcon = Icon.FromHandle(bm.GetHicon());
                oFont.Dispose();
                g.Dispose();
                bm.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
            return oIcon ;
        }
        string[ ] namazNames = { "Fajir", "Day", "Zohar", "Aser", "Sunset", "Maghrib", "Isha", "Night" };
        string[] daynightNames = { "Day", "Night" };
        /// <summary>
        /// Method getting center and radius and returns 2 points to be input to the Ellipse drawing or filling method.
        /// </summary>
        /// <param name="x">The x point of Center</param>
        /// <param name="y">The y point of Center</param>
        /// <param name="radius">The Radius of the cirle to be drawn Ellips of.</param>
        /// <returns>Return 2 points, 1st the left corner and 2nd contains width(as x) and height(as y) of the RectangleF.</returns>
        private PointF[] DrawCircle(double x, double y, double radius)
        {
            PointF[] pointArray = new PointF[2];
            //Left uper corner points
            pointArray[0].X = (float)(x - radius);
            pointArray[0].Y = (float)(y - radius);
            //The width and height in name of x and y.
            pointArray[1].X = (float)(radius * 2);
            pointArray[1].Y = pointArray[1].X;
            return pointArray;
        }
        private double[,] GetPieData(int[] Values, double startAngle)
        {
            double sum = 0; int length = Values.Length; double[] angleStarts = new double[length], angleSweeps = new double[length];
            for (int i = 0; i < length; i++)
            {
                sum += Values[i];
            }
            double mutiplier = 360 / sum; angleStarts[0] = startAngle;
            double[,] result = new double[length, 2];
            for (int i = 0; i < length; i++)
            {
                angleSweeps[i] = Values[i] * mutiplier;
                if ((i + 1) < length)
                    angleStarts[i + 1] = angleStarts[i] + angleSweeps[i];
                result[i, 0] = normalizeAngle(angleStarts[i]);
                result[i, 1] = normalizeAngle(angleSweeps[i]);
            }
            return result;
        }
        private double[,] GetPieData(int[] Values, double startAngle, int startAnglePie)
        {
            double sum = 0, mutiplier, MinusAngleValue = 0;
            int length = Values.Length;
            for (int i = 0; i < length; i++)
            {
                sum += Values[i];
            }
            mutiplier = 360 / sum;
            for (int i = 0; i <= startAnglePie - 1; i++)
            {
                MinusAngleValue += Values[i] * mutiplier;
            }
            startAngle = startAngle - MinusAngleValue;//the value of sweep angles minueses from startangle
            return GetPieData(Values, startAngle);
        }
        private PointF getEndLinePoint(PointF startPoint, double lengthPixels, double angleDegrees)//A method to return end point after adding lenght in a given angle to a point
        {
            double newPointx = lengthPixels * Math.Cos((angleDegrees * Math.PI) / 180.0);
            double newPointy = lengthPixels * Math.Sin((angleDegrees * Math.PI) / 180.0);
            return new PointF(((float)startPoint.X) + ((float)newPointx), ((float)startPoint.Y) + ((float)newPointy));
        }
        private PointF getEndLinePoint(double startX, double startY, double lengthPixels, double angleDegrees)//overload of getPoints for points input
        {
            PointF center = new PointF((float)startX, (float)startY);
            return getEndLinePoint(center, lengthPixels, angleDegrees);
        }
        private PointF[] getSizeLinePoints(PointF center, double radius, double length, double angleDegrees)
        {
            PointF start, end;
            start = getEndLinePoint(center, radius, angleDegrees);
            end = getEndLinePoint(start, length, angleDegrees);
            return new PointF[2] { start, end };
        }
        private PointF digitalClockPoint(DigitalClockPosition clockPosition)
        {
            switch (clockPosition)
            {
                case DigitalClockPosition.Bottom:
                    return new PointF(prayerTimingSettings.ClockCenterPoint.X - prayerTimingSettings.DigitalClockWidth / 2, prayerTimingSettings.ClockCenterPoint.Y + (int)(prayerTimingSettings.NamazRadius / 2));
                //return PointF.Subtract(prayerTimingSettings.ClockCenterPoint, new Size((int)(prayerTimingSettings.NamazRadius / 2), -(int)(prayerTimingSettings.NamazRadius / 2)));
                case DigitalClockPosition.Top:
                    return new PointF(prayerTimingSettings.ClockCenterPoint.X - prayerTimingSettings.DigitalClockWidth / 2, prayerTimingSettings.ClockCenterPoint.Y - (int)(prayerTimingSettings.NamazRadius / 2));
                case DigitalClockPosition.Left:
                    return new PointF(prayerTimingSettings.ClockCenterPoint.X - (int)(prayerTimingSettings.NamazRadius / 2) - prayerTimingSettings.DigitalClockHeight, prayerTimingSettings.ClockCenterPoint.Y - prayerTimingSettings.DigitalClockHeight / 2);
                case DigitalClockPosition.Right:
                    return new PointF(prayerTimingSettings.ClockCenterPoint.X + (int)(prayerTimingSettings.NamazRadius / 2) - prayerTimingSettings.DigitalClockHeight, prayerTimingSettings.ClockCenterPoint.Y - prayerTimingSettings.DigitalClockHeight / 2);
                default:
                    return PointF.Empty;
            }
        }
        /// <summary>
        /// Tells if mouse is inside the bounds of specified graphics path(GP).
        /// </summary>
        /// <param name="gp">The graphics path to be checked for mouse pointer.</param>
        /// <returns></returns>
        private bool mouseInGP(GraphicsPath gp)
        {
            PointF mousePoint = new PointF(Control.MousePosition.X - this.Bounds.Location.X, Control.MousePosition.Y - this.Location.Y);
            return pointInGP(gp, mousePoint);
        }
        private int? getMouseGPIndex(GraphicsPath[] gps)
        {
            for (int i = 0; i < gps.Length; i++)
            {
                if (mouseInGP(gps[i]))
                {
                    return i;
                }
            }
            return null;
        }
        private bool pointInGP(GraphicsPath gp, PointF point)
        {
            return gp.IsVisible(point);
        }
        //rich math functions
        private bool angleInRange(double startAngle, double sweepAngle, double angle)
        {
            startAngle = normalizeAngle(startAngle);
            angle = normalizeAngle(angle);
            double endAngle = normalizeAngle(startAngle + sweepAngle);
            //Making two arcs to check whether the angle arc, starting from start angle to angle, lies in that of big arc of the start and end angles.
            double angleArc = normalizeAngle(angle - startAngle);
            double totalArc = normalizeAngle(endAngle - startAngle);
            return angleArc <= totalArc ? true : false;
        }
        private double normalizeAngle(double angle)
        {
            return angle < 0 ? angle % 360 + 360 : angle % 360;
        }
        //This version is obseleted as pies can't be drawn currectly when we take 0 as 360.ok.
        //private double normalizeAngle(double angle)
        //{
        //    return angle <= 0 ? angle % 360 + 360 : angle % 360 != 0 ? angle % 360 : 360;
        //}
        double halfNightAngle()
        {
            DateTime dt = DateTime.Now;
            return halfNightAngle(dt);

        }
        double halfNightAngle(DateTime Date)
        {
            return TimeAngle(new DateTime(Date.Year, Date.Month, Date.Day, 23, 59, 59));
        }
        private double LineLength(double Minutes, float deviser)
        {
            return (Minutes / deviser);
        }
        private double TimeAngle()
        {
            return TimeAngle(DateTime.Now, prayerTimingSettings.StartAngle);
        }
        private double TimeAngle(DateTime time)
        {
            return TimeAngle(time, prayerTimingSettings.StartAngle);
        }
        private double TimeAngle(DateTime time, double startAngle)
        {
            double totalMinutes;
            if (prayerTimingSettings.DrawingRef == DrawingRefType.Hour00)
                totalMinutes = time.TimeOfDay.TotalMinutes;//This is general time angle
            else//this is special timeangle which fits the hand in reference to pies.
            {
                totalMinutes = time.TimeOfDay.TotalMinutes;
                if (prayerTimingSettings.StartPie != 7)
                {
                    totalMinutes -= PT.GetPrayerMinutes()[7];
                    for (int i = 1; i < prayerTimingSettings.StartPie + 1; i++)
                    {
                        totalMinutes -= this.PT.GetPrayerMinutes()[i - 1];
                    }
                }
            }
            totalMinutes /= 4.0;//so 60/4=15 hence. 15 deg for 1 hour. and 360 for 24 hours. 
            //totalMinutes += 90.0;//because C# starts angle from +horizontal line but above formula starts 0 degree from +vertical line
            totalMinutes += startAngle;
            totalMinutes = normalizeAngle(totalMinutes);
            return totalMinutes;
        }

        //Times functions
        private DateTime NextPrayerTime()
        {
            return NextPrayerTime(DateTime.Now);
        }
        private DateTime NextPrayerTime(DateTime dateTime)
        {
            DateTime[] dt = PT.GetPrayerTimes();
            //Now = dt[0];//the default next prayer is first.
            if (dateTime > dt[dt.Length - 1])
                dateTime = dt[0];
            else
                for (int i = 0; i < dt.Length; i++)
                {
                    //it starts checking from 0 to all prayertime so the first that is greater than current given time is the next prayer time.
                    if (dt[i] > dateTime)
                    {
                        dateTime = dt[i];
                        break;
                    }
                }
            return dateTime;
        }
        private string TimeToNextPrayer()
        {
            return TimeToNextPrayer(DateTime.Now);
        }
        private string TimeToNextPrayer(DateTime Now)
        {
            DateTime nextPrayerTime = NextPrayerTime();
            TimeSpan ts;
            if (nextPrayerTime > Now)
                ts = nextPrayerTime - Now;
            else
            {
                ts = Now - nextPrayerTime;
                ts = new TimeSpan(24, 0, 0) - ts;//ok
            }
            return getTimeString(ts);
        }
        private string getTimeString(TimeSpan ts)
        {
            int hours = ts.Hours;//Math.Abs((int)ts.TotalHours);
            int minutes = ts.Minutes;//Math.Abs((int)ts.TotalMinutes);
            return (hours < 10 ? "0" + hours.ToString() : hours.ToString()) + ":" + (minutes < 10 ? "0" + minutes.ToString() : minutes.ToString());
        }
        private string[] GetPrayerTimesWithNames()
        {
            string[] str = new string[7];
            for (int i = 0; i <= 6; i++)
            {
                str[i] = PT.GetPrayerTimes()[i].ToShortTimeString() + ": " + namazNames[i] + " time.";
            }
            return str;
        }
        private string ThisPrayerTime()
        {
            string temp = NextPrayerTime().ToShortTimeString();
            for (int i = 0; i < 7; i++)
            {
                if (GetPrayerTimesWithNames()[i].Contains(temp))
                { temp = GetPrayerTimesWithNames()[i].ToString(); break; }
            }
            return temp;
        }
        #endregion
        #region RegistryData
        private void registryKey()
        {
            RegistryKey rk;
            //rk = ;
            if ((rk = Registry.LocalMachine.OpenSubKey("Software//PrayerTiming")) == null)
            {
                rk = Registry.LocalMachine.OpenSubKey("Software", true);
                rk.CreateSubKey("PrayerTiming\\Location");
                rk.CreateSubKey("PrayerTiming\\Dimensions");
            }
        }
        private void registryLocation(string name, string lat, string lng)
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("Software\\PrayerTiming\\location", true);
            //rk.OpenSubKey("PrayerTiming\\Location");
            rk.SetValue("name", name);
            rk.SetValue("lat", lat);
            rk.SetValue("lng", lng);
            rk.SetValue("zone", lng);
        }
        private bool locationFound()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("Software\\PrayerTiming\\location");
            return rk.ValueCount > 2 ? true : false;
        }
        #endregion


        private void timer1_Tick(object sender, EventArgs e)
        {
            notifyIcon.BalloonTipText = TimeToNextPrayer();
            refresh();
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Draw(e.Graphics);
            e.Graphics.Dispose();
        }
        private string getHoverText(PiesHoverType hp, int gpIndex, bool daynight)
        {
            switch (hp)
            {
                case PiesHoverType.RemainingTime:
                    return TimeToNextPrayer();
                case PiesHoverType.PassedPercent:
                    double PassedAngle = 0, passedPercent = 0;//, totalPercent = 0;
                    //totalPercent = PiesData[gpIndex, 1] / 360 * 100;
                    PassedAngle = !daynight ? normalizeAngle(TimeAngle() - namazPieDataArray[gpIndex, 0]) : normalizeAngle(TimeAngle() - daynightPieDataArray[gpIndex, 0]);
                    passedPercent = PassedAngle / (!daynight ? namazPieDataArray[gpIndex, 1] : daynightPieDataArray[gpIndex, 1]) * 100;
                    return Convert.ToString((int)passedPercent);// +NamazNames[gpIndex] + " : " + (int)totalPercent + "%";
                default:
                    break;
            }
            return "";
        }
        private string getHoverString(GraphicsPath[] Pies, double[,] PiesData, bool daynight)
        {
            int? gpIndexFound;
            if ((gpIndexFound = getMouseGPIndex(Pies)) != null)
            {
                int gpIndex = (int)gpIndexFound;//made to not repeat the conversion process below
                if (!daynight ? angleInRange(namazPieDataArray[gpIndex, 0], namazPieDataArray[gpIndex, 1], TimeAngle()) : angleInRange(daynightPieDataArray[gpIndex, 0], daynightPieDataArray[gpIndex, 1], TimeAngle()))//it checks if the sun angle is in range of this pie
                {
                    //after making and implementinga an angle in range method in mouse.hover method then above method will be merged down.
                    if (!daynight)
                    {
                        return getHoverText(prayerTimingSettings.PiesHover, gpIndex, false) + " to next prayer.";// +NamazNames[gpIndex == 7 ? 0 : gpIndex + 1];
                    }
                    else
                        return getHoverText(prayerTimingSettings.PiesHover, gpIndex, true) + "% " + daynightNames[gpIndex] + " passed";
                }
                return !daynight ? namazNames[gpIndex] : daynightNames[gpIndex];
            }
            return "";
        }
        private void Form1_MouseHover(object sender, EventArgs e)
        {
            string Message = "";
            hoverToolTip.Hide(this);
            //select which type of types or shown then get the required message.
            if (namazPies != null && daynightPies != null)
            {
                Message = getHoverString(namazPies, namazPieDataArray, false);
                if (Message == "")//the message is empty only if mouse is not in namaz pies(then surely its in daynight area.
                    Message = getHoverString(daynightPies, daynightPieDataArray, true);
            }
            else if (namazPies != null)
                Message = getHoverString(namazPies, namazPieDataArray, false);
            else
                Message = getHoverString(daynightPies, daynightPieDataArray, true);
            //Now show the tooltip aquired by above selection
            Message = Message.ToString();
            //hoverToolTip.ToolTipTitle = Message.Substring(0, Message.IndexOf("d") + 1);
            //Message = Message.Substring(Message.IndexOf("d") + 1, Message.Length - Message.IndexOf("d") - 1);
            hoverToolTip.Show(Message, this);
        }
        private void frmPrayerTiming_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                contextMenuStripRightClick.Show(this.PointToScreen(e.Location));
            // MessageBox.Show(e.Location.ToString());
        }
        private void PrayerTiming_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int selected = (int)prayerTimingSettings.ClockPies;
            if (selected == 2)
                selected = 0;
            else
                selected++;
            prayerTimingSettings.ClockPies = (ClockPieType)selected;
            if (selected != 0)
                prayerTimingSettings.SunHandPointTo = SunHandPointTo.NamazPies;//=selected;(not this)Because we are using namaz radius as daynight in single mode.
            //Draw(this.CreateGraphics());
            Graphics g = this.CreateGraphics();
            g.SmoothingMode = SmoothingMode.HighQuality;
            drawPies(g);
            drawRulers(g, prayerTimingSettings.ClockPies);
            drawHand(g);

            refresh();
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.Visible)
                this.Hide();
            else
                this.Show();
        }
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                if (this.Visible)
                    this.Hide();
                else
                    this.Show();

        }
        private void notifyIcon_MouseMove(object sender, MouseEventArgs e)
        {
            setTooltipHover(prayerTimingSettings.TrayHover);

        }

        private void setTooltipHover(TrayHoverType thp)
        {
            switch (thp)
            {
                case TrayHoverType.HijriDateTime:
                    notifyIcon.Text = "HijriDate from hijri.dll";
                    break;
                case TrayHoverType.TimetoNextNamaz:
                    notifyIcon.Text = TimeToNextPrayer() + " to next Prayer.";
                    break;
                case TrayHoverType.TimetoNextEvent:
                    notifyIcon.Text = "Next Event has to be programmed";
                    break;
                default:
                    notifyIcon.Text = TimeToNextPrayer() + " to next Prayer.";
                    break;
            }
        }
        #region Draging
        private Point ClickedPoint;
        private bool IsDragging = false;
        private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            this.IsDragging = true;
            this.ClickedPoint = new Point(e.X, e.Y);

        }
        private void Form1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {

            this.IsDragging = false;

        }
        private void Form1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.IsDragging)
            {
                Point NewPoint;
                NewPoint = this.PointToScreen(new Point(e.X, e.Y));
                NewPoint.Offset(this.ClickedPoint.X * -1, this.ClickedPoint.Y * -1);
                this.Location = NewPoint;
            }

        }
        #endregion

        //Settings tab interaction
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings frmset = new frmSettings();
            frmset.FormClosed += new FormClosedEventHandler(frmset_FormClosed);
            frmset.Show();
        }

        void frmset_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.IO.File.Delete("mape.jpg");
            frmSettings newSettingsForm = (frmSettings)sender;
            //prayerTimingSettings = newSettingsForm.PrayerTimingSettings;
            PT.Lattitude = prayerTimingSettings.Latitude;
            PT.Longitude = prayerTimingSettings.Longitude;
            PT.Zone = prayerTimingSettings.TimeZone;
            PT.Date = prayerTimingSettings.DateTime;
            try
            {
                prayerTimingSettings.MapeName = "mape" + DateTime.Now.Second.ToString() + ".jpg";
                newSettingsForm.Mape.Save(prayerTimingSettings.MapeName, System.Drawing.Imaging.ImageFormat.Jpeg);
                newSettingsForm.Mape = null;
            }
            catch (Exception)
            {
            }

            Draw(this.CreateGraphics());
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Clock Type
            if (sender.Equals(namazToolStripMenuItem))
                prayerTimingSettings.ClockPies = ClockPieType.namaz;
            if (sender.Equals(dayNightToolStripMenuItem))
                prayerTimingSettings.ClockPies = ClockPieType.daynight;
            if (sender.Equals(bothAboveToolStripMenuItem))
                prayerTimingSettings.ClockPies = ClockPieType.both;

            //Clock Size
            if (sender.Equals(smallToolStripMenuItem))
                prayerTimingSettings.ClockSize = ClockSizeType.Small;
            if (sender.Equals(mediumToolStripMenuItem))
                prayerTimingSettings.ClockSize = ClockSizeType.Medium;
            if (sender.Equals(largeToolStripMenuItem))
                prayerTimingSettings.ClockSize = ClockSizeType.Large;
            //Main Settings
            if (sender.Equals(sendToTrayToolStripMenuItem))
                this.Hide();
            if (sender.Equals(exitToolStripMenuItem1))
                Application.Exit();
            //PieColor

            //if (sender.Equals(redToolStripMenuItem))
            //
            //Graphics g = this.CreateGraphics();
            //g.SmoothingMode = SmoothingMode.HighQuality;
            //drawPies(g);
            //drawRulers(g, prayerTimingSettings.ClockPies);
            //drawHand(g);
            //refresh();

            //Draw(this.CreateGraphics());
        }




    }
    public static class prayerTimingSettings
    {

        //Property Variables to hold the data to be transported among forms

        #region PrayerTimingClassData

        //Types Settings
        private static DrawingRefType drawingRef;
        public static DrawingRefType DrawingRef
        {
            get { return drawingRef; }
            set { drawingRef = value; }
        }

        private static TrayHoverType trayHover;
        public static TrayHoverType TrayHover
        {
            get { return trayHover; }
            set { trayHover = value; }
        }

        private static ClockSizeType clockSize;
        public static ClockSizeType ClockSize
        {
            get { return clockSize; }
            set { SetClockSize(value); }
        }
        private static void SetClockSize(ClockSizeType size)
        {
            clockSize = size;
            switch (size)
            {
                case ClockSizeType.Small:
                    prayerTimingSettings.ClockRadius = 400;
                    prayerTimingSettings.DayNightRadius = 50;
                    prayerTimingSettings.DayNightWidth = 25;
                    prayerTimingSettings.NamazRadius = 100;
                    prayerTimingSettings.NamazWidth = 50;
                    break;
                case ClockSizeType.Medium:
                    prayerTimingSettings.ClockRadius = 500;
                    prayerTimingSettings.DayNightRadius = 50;
                    prayerTimingSettings.DayNightWidth = 30;
                    prayerTimingSettings.NamazRadius = 120;
                    prayerTimingSettings.NamazWidth = 50;
                    break;
                case ClockSizeType.Large:
                    prayerTimingSettings.ClockRadius = 500;
                    prayerTimingSettings.DayNightRadius = 75;
                    prayerTimingSettings.DayNightWidth = 32;
                    prayerTimingSettings.NamazRadius = 150;
                    prayerTimingSettings.NamazWidth = 75;
                    break;
                case ClockSizeType.Manual:
                    break;
                default:
                    break;
            }
            prayerTimingSettings.smallLinesR = prayerTimingSettings.bigLinesR = namazR;
            prayerTimingSettings.bigLinesW = namazW / 2;
            prayerTimingSettings.smallLinesW = bigLinesW / 2;



        }

        private static ClockPieType piesType;
        public static ClockPieType ClockPies
        {
            get { return piesType; }
            set { piesType = value; }
        }

        private static DigitalClockPosition digitalClockPosition;
        public static DigitalClockPosition DigitalClockPosition
        {
            get { return digitalClockPosition; }
            set { digitalClockPosition = value; }
        }

        private static PiesHoverType piesHover;
        public static PiesHoverType PiesHover
        {
            get { return piesHover; }
            set { piesHover = value; }
        }

        private static SunHandPointTo sunHandPointTo;
        public static SunHandPointTo SunHandPointTo
        {
            get { return sunHandPointTo; }
            set { sunHandPointTo = value; }
        }

        private static SunHandPointAt sunHandPointAt;
        public static SunHandPointAt SunHandPointAT
        {
            get { return sunHandPointAt; }
            set { sunHandPointAt = value; }
        }

        //Coordinates Settings
        private static int startPie;
        public static int StartPie
        {
            get { return startPie; }
            set { startPie = value; }
        }

        private static float startAngle;
        public static float StartAngle
        {
            get { return startAngle; }
            set { startAngle = value; }
        }

        private static float clockRadius;
        public static float ClockRadius
        {
            get { return clockRadius; }
            set { clockRadius = value; clockCenterVectorLength = clockRadius * 0.5f; clockCenterPoint = new PointF(ClockCenterVecter, ClockCenterVecter); }
        }
        private static float clockCenterVectorLength;
        public static float ClockCenterVecter
        {
            get { return clockCenterVectorLength; }
        }

        private static PointF clockCenterPoint;
        public static PointF ClockCenterPoint
        {
            get { return clockCenterPoint; }
            set { clockCenterPoint = value; }
        }


        //Clock Pies Settings
        private static double dayNightR;
        public static double DayNightRadius
        {
            get { return dayNightR; }
            set { dayNightR = value; }
        }

        private static double dayNightW;
        public static double DayNightWidth
        {
            get { return dayNightW; }
            set { dayNightW = value; }
        }

        private static double namazR;
        public static double NamazRadius
        {
            get { return namazR; }
            set { namazR = value; }
        }

        private static double namazW;
        public static double NamazWidth
        {
            get { return namazW; }
            set { namazW = value; }
        }
        //Lines
        private static double bigLinesR;
        public static double BigLinesRadius
        {
            get { return bigLinesR; }
            set { bigLinesR = value; }
        }

        private static double bigLinesW;
        public static double BigLinesWidth
        {
            get { return bigLinesW; }
            set { bigLinesW = value; }
        }

        private static double smallLinesR;
        public static double SmallLinesRadius
        {
            get { return smallLinesR; }
            set { smallLinesR = value; }
        }

        private static double smallLinesW;
        public static double SmallLinesWidth
        {
            get { return smallLinesW; }
            set { smallLinesW = value; }
        }

        private static int numbersSize;
        public static int NumbersSize
        {
            get { return numbersSize; }
            set { numbersSize = value; }
        }

        private static double numbersR;
        public static double NumbersRadius
        {
            get { return numbersR; }
            set { numbersR = value; }
        }

        private static int numbersOffset;
        public static int NumbersOffset
        {
            get { return numbersOffset; }
            set { numbersOffset = value; }
        }

        //Clock Sun Hand Settings
        private static double sunHandR;
        public static double SunHandRadius
        {
            get { return sunHandR; }
            set { sunHandR = value; }
        }

        private static float sunHandW;
        public static float SunHandWidth
        {
            get { return sunHandW; }
            set { sunHandW = value; }
        }

        private static int digitalClockWidth;

        public static int DigitalClockWidth
        {
            get { return digitalClockWidth; }
            set { digitalClockWidth = value; }
        }
        private static int digitalClockHeight;

        public static int DigitalClockHeight
        {
            get { return digitalClockHeight; }
            set { digitalClockHeight = value; }
        }
        private static DigitalClockSize digitalClockSize;
        public static DigitalClockSize DigitalClockSize
        {
            get { return digitalClockSize; }
            set { setDigitalClockSize(value); }
        }



        private static void setDigitalClockSize(DigitalClockSize size)
        {
            digitalClockSize = size;
            switch (size)
            {
                case DigitalClockSize.Small:
                    break;
                case DigitalClockSize.Medium:
                    break;
                case DigitalClockSize.Large:
                    prayerTimingSettings.DigitalClockHeight = 25;
                    prayerTimingSettings.DigitalClockWidth = 86;
                    break;
                default:
                    break;
            }
        }

        //Colors
        private static Brush[] namazPieBrushes;
        public static Brush[] NamazPieBrushes
        {
            get { return prayerTimingSettings.namazPieBrushes; }
            set { prayerTimingSettings.namazPieBrushes = value; }
        }

        private static Brush[] daynightBrushes;
        public static Brush[] DaynightBrushes
        {
            get { return daynightBrushes; }
            set { daynightBrushes = value; }
        }


        private static Color sunHandColor;
        public static Color SunHandColor
        {
            get { return sunHandColor; }
            set { sunHandColor = value; }
        }

        private static Brush digitalClockColor;
        public static Brush DigitalClockColor
        {
            get { return digitalClockColor; }
            set { digitalClockColor = value; }
        }

        private static Brush digitalClockBackColor;
        public static Brush DigitalClockBackColor
        {
            get { return digitalClockBackColor; }
            set { digitalClockBackColor = value; }
        }


        //Clock Items Boolean Settings

        private static bool showHand;
        public static bool ShowHand
        {
            get { return showHand; }
            set { showHand = value; }
        }

        private static bool showBigLines;
        public static bool ShowBigLines
        {
            get { return showBigLines; }
            set { showBigLines = value; }
        }

        private static bool showSmallLines;
        public static bool ShowSmallLines
        {
            get { return showSmallLines; }
            set { showSmallLines = value; }
        }

        private static bool showNumbers;
        public static bool ShowNumbers
        {
            get { return showNumbers; }
            set { showNumbers = value; }
        }

        private static bool showDigitalClock;
        public static bool ShowDigitalClock
        {
            get { return showDigitalClock; }
            set { showDigitalClock = value; }
        }

        #endregion

        #region General
        private static string mapeName;

        public static string MapeName
        {
            get { return mapeName; }
            set { mapeName = value; }
        }

        #endregion

        #region PrayerTimesClassData
        private static double timeZone;

        public static double TimeZone
        {
            get { return timeZone; }
            set { timeZone = value; }
        }

        private static double latitude;

        public static double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        private static double longitude;

        public static double Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        private static string cityName;

        public static string CityName
        {
            get { return cityName; }
            set { cityName = value; }
        }

        private static DateTime dateTime;

        public static DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

        #endregion
    }

    #region PrayerTimesSourceClass

    public enum CalculationMehod
    {
        Jafari, Karachi, ISNA, MWL, Makkah, Egypt, Custom, Tehran
    }
    public enum JuristicMethod
    {
        Shafii = 1, Hanafi
    }
    public enum HigherLattitude
    {
        None, MidNight, OneSeventh, AngleBased
    }
    public enum TimeFormat
    {
        Time24, Time12
    }

    public class PrayerTimes
    {
        private double lattitude;
        private double longitude;
        private double timeZone;
        private DateTime dateTime;

        private double JDate;
        private double[][] methodParams;


        public PrayerTimes()
        {
            this.calcMethod = CalculationMehod.Karachi;
            this.highLats = HigherLattitude.None;
            this.timeFormat = TimeFormat.Time12;
            this.juristicMethod = JuristicMethod.Hanafi;

            methodParams = new double[][]
            {
                new double[]{16, 0, 4, 0, 14},
                new double[]{18, 1, 0, 0, 18},
                new double[]{15, 1, 0, 0, 15},
                new double[]{18, 1, 0, 0, 17},
                new double[]{19, 1, 0, 1, 90},
                new double[]{19.5, 1, 0, 0, 17.5},
                new double[]{17.7, 0, 4.5, 0, 15},
                new double[]{18, 1, 0, 0, 17}
            };
        }

        public DateTime[] GetPrayerTimes()
        {
            return GetPrayerTimes(this.dateTime, this.lattitude, this.longitude, this.timeZone);
        }
        public int[] GetPrayerMinutes()
        {
            int[] times = new int[8];
            DateTime[] dt = GetPrayerTimes();
            TimeSpan ts = new TimeSpan();
            for (int i = 0; i <= 5; i++)
            {
                ts = dt[i + 1].TimeOfDay - dt[i].TimeOfDay;
                times[i] = (int)ts.TotalMinutes;
            }

            //ts = dt[2].TimeOfDay - dt[1].TimeOfDay;
            //times[1] = (int)ts.TotalMinutes;
            //ts = dt[3].TimeOfDay - dt[2].TimeOfDay;
            //times[2] = (int)ts.TotalMinutes;
            //ts = dt[4].TimeOfDay - dt[3].TimeOfDay;
            //times[3] = (int)ts.TotalMinutes;
            //ts = dt[5].TimeOfDay - dt[4].TimeOfDay;
            //times[4] = (int)ts.TotalMinutes;
            //ts = dt[6].TimeOfDay - dt[5].TimeOfDay;
            //times[5] = (int)ts.TotalMinutes;

            //isha to 12 am minutes are calculated below
            ts = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59) - DateTime.Parse(GetPrayerTimes()[6].ToShortTimeString());
            times[6] = (int)ts.TotalMinutes;//TotalNightMinutes - (times[0] + times[5]);
            times[7] = (int)GetPrayerTimes()[0].TimeOfDay.TotalMinutes;
            return times;
        }
        public DateTime[] GetPrayerTimes(DateTime date, double lattitude, double longitude, double timeZone)
        {
            this.lattitude = lattitude;
            this.longitude = longitude;
            this.timeZone = timeZone;
            this.JDate = this.JulianDate(date) - longitude / (15 * 24);
            double[] times = this.ComputeTimes();
            DateTime[] dt = new DateTime[7];
            times = this.AdjustTimes(times);
            string[] tempStr = this.AdjustTimesFormat(times);
            //SalaahTimeCalculator stc = new SalaahTimeCalculator();
            //string[] tempStr = stc.getPrayerTimes(date, lattitude, longitude, (int)timeZone);
            for (int i = 0; i <= 6; i++)
            {
                dt[i] = DateTime.Parse(tempStr[i]);
            }
            return dt;
        }

        public DateTime GetTime(int TimeNumber)
        {
            //string outStr="";
            DateTime[] Times = GetPrayerTimes(this.dateTime, this.lattitude, this.longitude, this.timeZone);
            switch (TimeNumber)
            {
                case 0:
                    return Times[0];
                case 1:
                    return Times[1];

                case 2:
                    return Times[2];

                case 3:
                    return Times[3];

                case 4:
                    return Times[4];

                case 5:
                    return Times[5];

                case 6:
                    return Times[6];
                default:
                    return Times[0];

            }

            //return DateTime.Parse(outStr);
        }
        public double JulianDate(DateTime date)
        {
            int year = date.Year, month = date.Month, day = date.Day;

            if (date.Month <= 2)
            {
                year -= 1;
                month += 12;
            }

            double a = Math.Floor((double)year / 100);
            double b = 2 - a + Math.Floor(a / 4);
            return Math.Floor(365.25 * (year + 4716)) + Math.Floor(30.6001 * (month + 1)) + day + b - 1524.5;
        }

        double[] ComputeTimes()
        {
            double[] t = new double[] { 5, 6, 12, 13, 18, 18, 18 };
            for (int i = 0; i < 7; i++)
                t[i] /= 24;
            double Fajr = this.ComputeTime(180 - this.methodParams[(int)calcMethod][0], t[0]);
            double Sunrise = this.ComputeTime(180 - 0.833, t[1]);
            double Dhuhr = this.ComputeMidDay(t[2]);
            double Asr = this.ComputeAsr(this.juristicMethod, t[3]);
            double Sunset = this.ComputeTime(0.833, t[4]); ;
            double Maghrib = this.ComputeTime(this.methodParams[(int)calcMethod][2], t[5]);
            double Isha = this.ComputeTime(this.methodParams[(int)calcMethod][4], t[6]);

            return new double[] { Fajr, Sunrise, Dhuhr, Asr, Sunset, Maghrib, Isha };
        }

        double ComputeTime(double angle, double dayPortion)
        {
            double d = this.SunDeclination(this.JDate + dayPortion);
            double z = this.ComputeMidDay(dayPortion);
            double a = (-this.Dsin(angle) - this.Dsin(d) * this.Dsin(this.lattitude)) /
                    (this.Dcos(d) * this.Dcos(this.lattitude));
            double v = 1d / 15 * this.Darccos(a);
            return z + (angle > 90 ? -v : v);
        }

        public double SunDeclination(double julianDate)
        {
            double d = julianDate - 2451545.0;
            double g = this.Fixangle(357.529 + 0.98560028 * d);
            double q = this.Fixangle(280.459 + 0.98564736 * d);
            double l = this.Fixangle(q + 1.915 * this.Dsin(g) + 0.020 * this.Dsin(2 * g));
            double e = 23.439 - 0.00000036 * d;
            return this.Darcsin(this.Dsin(e) * this.Dsin(l));
        }

        public double EquationOfTime(double julianDate)
        {
            double d = julianDate - 2451545.0;
            double g = this.Fixangle(357.529 + 0.98560028 * d);
            double q = this.Fixangle(280.459 + 0.98564736 * d);
            double l = this.Fixangle(q + 1.915 * this.Dsin(g) + 0.020 * this.Dsin(2 * g));
            double e = 23.439 - 0.00000036 * d;
            double ra = this.Darctan2(this.Dcos(e) * this.Dsin(l), this.Dcos(l)) / 15;
            ra = this.Fixhour(ra);
            return q / 15 - ra;
        }

        double ComputeMidDay(double dayPortion)
        {
            double t = this.EquationOfTime(this.JDate + dayPortion);
            return this.Fixhour(12 - t);
        }
        double ComputeAsr(JuristicMethod juristicMethod, double dayPortion)  // Shafii: step=1, Hanafi: step=2
        {
            double d = this.SunDeclination(this.JDate + dayPortion);
            double g = -this.Darccot(((double)juristicMethod) + this.Dtan(Math.Abs(this.lattitude - d)));
            return this.ComputeTime(g, dayPortion);
        }

        double[] AdjustTimes(double[] times)
        {
            for (int i = 0; i < 7; i++)
                times[i] += this.timeZone - this.longitude / 15;
            times[2] += this.dhuhrMinutes / 60; //Dhuhr
            if (this.methodParams[(int)this.calcMethod][1] == 1) // Maghrib
                times[5] = times[4] + this.methodParams[(int)this.calcMethod][2] / 60;
            if (this.methodParams[(int)this.calcMethod][3] == 1) // Isha
                times[6] = times[5] + this.methodParams[(int)this.calcMethod][4] / 60;

            if (this.highLats != HigherLattitude.None)
                times = this.AdjustHighLatTimes(times);
            return times;
        }

        string[] AdjustTimesFormat(double[] times)
        {
            string[] temp = new string[times.Length];

            for (int i = 0; i < 7; i++)
            {
                if (this.timeFormat == TimeFormat.Time12)
                    temp[i] = this.FloatToTime12(times[i], false);
                else if (this.timeFormat == TimeFormat.Time24)
                    temp[i] = this.FloatToTime24(times[i]);
            }
            return temp;
        }

        double[] AdjustHighLatTimes(double[] times)
        {
            double nightTime = this.TimeDiff(times[4], times[1]); // sunset to sunrise

            // Adjust Fajr
            double FajrDiff = this.NightPortion(this.methodParams[(int)this.calcMethod][0]) * nightTime;
            if (times[0] == 0d || this.TimeDiff(times[0], times[1]) > FajrDiff)
                times[0] = times[1] - FajrDiff;

            // Adjust Isha
            double IshaAngle = (this.methodParams[(int)this.calcMethod][3] == 0) ? this.methodParams[(int)this.calcMethod][4] : 18;
            double IshaDiff = this.NightPortion(IshaAngle) * nightTime;
            if (times[6] == 0d || this.TimeDiff(times[4], times[6]) > IshaDiff)
                times[6] = times[4] + IshaDiff;

            // Adjust Maghrib
            double MaghribAngle = (this.methodParams[(int)this.calcMethod][1] == 0) ? this.methodParams[(int)this.calcMethod][2] : 4;
            double MaghribDiff = this.NightPortion(MaghribAngle) * nightTime;
            if (times[5] == 0d || this.TimeDiff(times[4], times[5]) > MaghribDiff)
                times[5] = times[4] + MaghribDiff;

            return times;
        }

        double NightPortion(double angle)
        {
            double r = 0d;
            if (this.highLats == HigherLattitude.AngleBased)
                r = 1d / 60 * angle;
            if (this.highLats == HigherLattitude.MidNight)
                r = 1d / 2;
            if (this.highLats == HigherLattitude.OneSeventh)
                r = 1d / 7;
            return r;
        }

        double TimeDiff(double time1, double time2)
        {
            return this.Fixhour(time2 - time1);
        }


        string FloatToTime12(double time, bool noSuffix)
        {
            if (time == 0d)
                throw new Exception("Invalid Time");
            time = this.Fixhour(time + 0.5 / 60);  // add 0.5 minutes to round
            double hours = Math.Floor(time);
            double minutes = Math.Floor((time - hours) * 60);
            string suffix = hours >= 12 ? " pm" : " am";
            hours = (hours + 12 - 1) % 12 + 1;
            return hours + ":" + this.TwoDigitsFormat(minutes) + (noSuffix ? "" : suffix.ToString());
        }

        string FloatToTime24(double time)
        {
            if (time == 0d)
                throw new Exception("Invalid Time");
            time = this.Fixhour(time + 0.5 / 60);  // add 0.5 minutes to round
            double hours = Math.Floor(time);
            double minutes = Math.Floor((time - hours) * 60);
            return this.TwoDigitsFormat(hours) + ":" + this.TwoDigitsFormat(minutes);
        }

        string TwoDigitsFormat(double num)
        {
            return (num < 10) ? "0" + num : num.ToString(); ;
        }

        double Darctan2(double y, double x)
        {
            return this.Dtd(Math.Atan2(y, x));
        }

        double Fixhour(double a)
        {
            a = a - 24.0 * (Math.Floor(a / 24.0));
            return a < 0 ? a + 24.0 : a;
        }

        double Fixangle(double a)
        {
            a = a - 360.0 * (Math.Floor(a / 360.0));
            return a < 0 ? a + 360.0 : a;
        }
        double Darccos(double x)
        {
            return this.Dtd(Math.Acos(x));
        }
        double Darcsin(double x)
        {
            return this.Dtd(Math.Asin(x));
        }
        double Darccot(double x)
        {
            return this.Dtd(Math.Atan(1d / x));
        }

        double Dsin(double d)
        {
            return Math.Sin(this.Dtr(d));
        }
        double Dcos(double d)
        {
            return Math.Cos(this.Dtr(d));
        }
        double Dtan(double d)
        {
            return Math.Tan(this.Dtr(d));
        }
        double Dtd(double r)
        {
            return (r * 180.0) / Math.PI;
        }

        double Dtr(double r)
        {
            return (r * Math.PI) / 180.0;
        }

        int GetMinutes(DateTime time1, DateTime time2, bool complement)
        {
            TimeSpan ts = new TimeSpan();
            TimeSpan ts24 = new TimeSpan(24, 0, 0);
            if (complement)
                ts = ts24 + (time2 - time1);
            else
                ts = time2 - time1;
            return Math.Abs((int)ts.TotalMinutes);
        }
        int TDayMin(string sunrise, string sunset)
        {
            DateTime Sunrise = DateTime.Parse(sunrise);
            DateTime Sunset = DateTime.Parse(sunset);
            TimeSpan ts = new TimeSpan();
            ts = Sunset - Sunrise;
            return Math.Abs((int)ts.TotalMinutes);
        }
        int PDayMin(string sunset, string sunrise)
        {
            DateTime Sunset = DateTime.Parse(sunset);
            DateTime Sunrise = DateTime.Parse(sunrise);
            DateTime Nowtime = DateTime.Parse(DateTime.Now.ToShortTimeString());
            TimeSpan ts = new TimeSpan();
            if (Nowtime > Sunrise && Nowtime < Sunset)
            {
                ts = Nowtime.TimeOfDay - Sunrise.TimeOfDay;
                return Math.Abs((int)ts.TotalMinutes);
            }
            else
                return TDayMin(sunrise, sunset);


        }
        int TNightMin(string sunrise, string sunset)
        {

            DateTime Sunrise = DateTime.Parse(sunrise);
            DateTime Sunset = DateTime.Parse(sunset);
            TimeSpan ts = new TimeSpan();
            ts = new TimeSpan(24, 0, 0) - Sunset.TimeOfDay + Sunrise.TimeOfDay;
            return Math.Abs((int)ts.TotalMinutes);
        }
        int PNightMin(string sunset, string sunrise)
        {
            DateTime Nowtime = DateTime.Parse(DateTime.Now.ToShortTimeString());
            DateTime Sunset = DateTime.Parse(sunset);
            DateTime Sunrise = DateTime.Parse(sunrise);
            TimeSpan ts = new TimeSpan();
            DateTime zero00 = DateTime.Parse("23:59:59 PM");

            if (Nowtime < zero00 && Nowtime > Sunset)
            {
                ts = Nowtime - Sunset;
                return Math.Abs((int)ts.TotalMinutes);
            }
            else if (Nowtime < Sunrise)
            {
                ts = zero00 - Sunset;
                ts += Nowtime.TimeOfDay;
                return (int)ts.TotalMinutes;
            }
            else
                return 0;
        }

        public int TotalDayMinutes
        {
            get { return TDayMin(GetTime(1).ToShortTimeString(), GetTime(4).ToShortTimeString()); }
        }

        public int PassedDayMintues
        {
            get { return PDayMin(GetTime(4).ToShortTimeString(), GetTime(1).ToShortTimeString()); }
        }
        public int TotalNightMinutes
        {
            get { return TNightMin(GetTime(1).ToShortTimeString(), GetTime(4).ToShortTimeString()); }
        }
        public int PassedNightMinutes
        {
            get { return PNightMin(GetTime(4).ToShortTimeString(), GetTime(1).ToShortTimeString()); }
        }


        private TimeFormat timeFormat;

        public TimeFormat TimeFormat
        {
            get { return timeFormat; }
            set { timeFormat = value; }
        }

        public double Lattitude
        {
            get { return this.lattitude; }
            set { this.lattitude = value; }
        }
        public double Longitude
        {
            get { return this.longitude; }
            set { this.longitude = value; }
        }
        public double Zone
        {
            get { return this.timeZone; }
            set { this.timeZone = value; }
        }

        public DateTime Date
        {
            get { return this.dateTime; }
            set { this.dateTime = value; }
        }





        private CalculationMehod calcMethod;

        public CalculationMehod CalcMethod
        {
            get { return calcMethod; }
            set { calcMethod = value; }
        }

        private JuristicMethod juristicMethod;

        public JuristicMethod JuristicMethod
        {
            get { return juristicMethod; }
            set { juristicMethod = value; }
        }

        private double dhuhrMinutes;

        public double DhuhrMinutes
        {
            get { return dhuhrMinutes; }
            set { dhuhrMinutes = value; }
        }

        private HigherLattitude highLats;

        public HigherLattitude HigherLattitude
        {
            get { return highLats; }
            set { highLats = value; }
        }

        public string ToString(DateTime date, double lattitude, double longitude, double timeZone)
        {
            DateTime[] strs = GetPrayerTimes(date, lattitude, longitude, timeZone);
            string result = "Date : " + date.ToShortDateString() + "\n";

            result += "Fajr : " + strs[0].ToShortTimeString() + "\n";
            result += "Sunrise : " + strs[1].ToShortTimeString() + "\n";
            result += "Dhuhr : " + strs[2].ToShortTimeString() + "\n";
            result += "Asr : " + strs[3].ToShortTimeString() + "\n";
            result += "Sunset : " + strs[4].ToShortTimeString() + "\n";
            result += "Maghrib : " + strs[5].ToShortTimeString() + "\n";
            result += "Isha : " + strs[6].ToShortTimeString() + "\n";

            return result;
        }
    }
    #endregion
}