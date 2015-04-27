using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.IO.IsolatedStorage;


namespace ShaumQuest
{
    public partial class ViewCalendar : PhoneApplicationPage
    {
        private DateTime selDate;
        private DateTime toBeChecked;

        private String stringHijriah;

        private bool SET_DAUD;
        private bool SET_SYAWAL;
        private String START_DAUD;
        private String START_SYAWAL;

        public ViewCalendar()
        {
            InitializeComponent();
            readTheSettings();

            MonthCalendar.ClearEvents();

            setAllEvents();
            //MonthCalendar.AddNewEvent(now, new CalendarControl.LCalendar.CalendarEvent("PUASA AWAL BULAN", now, null));
        }

        #region ADD EVENTS
        private void setAllEvents()
        {
            DateTime start = DateTime.Now.AddYears(-5);
            DateTime fin = DateTime.Now.AddYears(15);

            toBeChecked = DateTime.Now;
            MonthCalendar.AddNewEvent(toBeChecked, new CalendarControl.LCalendar.CalendarEvent("TandaToday", toBeChecked, null));

            if (SET_DAUD)
                setDaud();

            int len = (DateTime.Now - start).Days;
            for (int i = -len; i < len; i++)
            {
                toBeChecked = DateTime.Now.AddDays(i);

                String wow = toBeChecked.ToShortDateString();
                String[] temp = wow.Split('/');
                int day = int.Parse(temp[1]);
                int month = int.Parse(temp[0]);
                int year = int.Parse(temp[2]);

                stringHijriah = makeHijri(day, month, year);
                setJenisPuasa();
            }
        }
        #endregion

        #region JENIS PUASA
        private void setJenisPuasa()
        {
            String[] temp = stringHijriah.Split(' ');
            int tgl = int.Parse(temp[0]);
            String bulan = temp[1];

            String[] temp2 = toBeChecked.ToLongDateString().Split(',');

            if (tgl == 1 && bulan.Equals("Syawal"))
            {
                //return "Haram Puasa - Idul Fitri";
                MonthCalendar.AddNewEvent(toBeChecked, new CalendarControl.LCalendar.CalendarEvent("Haram", toBeChecked, null));
            }
            else if (tgl == 10 && bulan.Equals("Dzulhijah"))
            {
                //return "Haram Puasa - Idul Adha";
                MonthCalendar.AddNewEvent(toBeChecked, new CalendarControl.LCalendar.CalendarEvent("Haram", toBeChecked, null));
            }
            else if ((tgl == 11 || tgl == 12 || tgl == 13) && bulan.Equals("Dzulhijah"))
            {
                //return "Haram Puasa - Hari Tasyrik";
                MonthCalendar.AddNewEvent(toBeChecked, new CalendarControl.LCalendar.CalendarEvent("Haram", toBeChecked, null));
            }
            else if (bulan.Equals("Ramadhan"))
            {
                //return "Puasa bulan Ramadhan";
                MonthCalendar.AddNewEvent(toBeChecked, new CalendarControl.LCalendar.CalendarEvent("Ramadhan", toBeChecked, null));
            }
            else if (tgl == 13 || tgl == 14 || tgl == 15)
            {
                //return "Puasa Ayyamul Bidh";
                MonthCalendar.AddNewEvent(toBeChecked, new CalendarControl.LCalendar.CalendarEvent("Ayyamul", toBeChecked, null));
            }
            else if ((tgl == 9 || tgl == 10) && bulan.Equals("Muharram"))
            {
                //return "Puasa Muharram";
                MonthCalendar.AddNewEvent(toBeChecked, new CalendarControl.LCalendar.CalendarEvent("Muharram", toBeChecked, null));
            }
            else if (tgl == 9 && bulan.Equals("Dzulhijah"))
            {
                //return "Puasa Arafah";
                MonthCalendar.AddNewEvent(toBeChecked, new CalendarControl.LCalendar.CalendarEvent("Arafah", toBeChecked, null));
            }
        }
        #endregion

        #region CONVERT TO HIJRI
        public String makeHijri(int day, int month, int year)
        {
            int JHH, JHM;
            int TH, XH, XM;
            String MH;
            // day--;

            int[] nonkabisat = { 0, 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334 };
            int[] kabisat = { 0, 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335 };

            Boolean kab = false;
            if (year % 4 == 0)
            {
                if (year % 100 == 0)
                {
                    if (year % 400 == 0)
                        kab = true;
                }
                else
                {
                    kab = true;
                }
            }

            if (kab)
            {
                XM = day + kabisat[month];
            }
            else
            {
                XM = day + nonkabisat[month];
            }

            JHM = (int)((year - 1) * 365.25) + XM - 13;
            JHH = JHM - 227015;

            TH = (int)(JHH / 354.367) + 1;
            XH = JHH - (int)((TH - 1) * 354.367);

            #region BULAN HIJRIAH
            if (XH > 325)
            {
                MH = "Dzulhijah";
                XH = XH - 325;
            }
            else if (XH > 295)
            {
                MH = "Dzulqa’dah";
                XH = XH - 295;
            }
            else if (XH > 266)
            {
                MH = "Syawal";
                XH = XH - 266;
            }
            else if (XH > 236)
            {
                MH = "Ramadhan";
                XH = XH - 236;
            }
            else if (XH > 207)
            {
                MH = "Sya'ban";
                XH = XH - 207;
            }
            else if (XH > 177)
            {
                MH = "Rajab";
                XH = XH - 177;
            }
            else if (XH > 148)
            {
                MH = "Jumada Tsani";
                XH = XH - 148;
            }
            else if (XH > 118)
            {
                MH = "Jumada Awal";
                XH = XH - 118;
            }
            else if (XH > 89)
            {
                MH = "Rabi’ul Tsani";
                XH = XH - 89;
            }
            else if (XH > 59)
            {
                MH = "Rabi’ul Awal";
                XH = XH - 59;
            }
            else if (XH > 30)
            {
                MH = "Safar";
                XH = XH - 30;
            }
            else
            {
                MH = "Muharram";
            }
            #endregion

            return XH + " " + MH + " " + TH;
        }
        #endregion

        #region KALO DIKLIK
        private void MonthCalendar_OnDayClicked(object sender, CalendarControl.LCalendar.DayClickedEventArgs e)
        {
            //ClickedDate.Text = "" + e.SelectedDate.ToString().Substring(0,10);
            //var valueToStore = e.SelectedDate.Ticks;
            string valueToStore = e.SelectedDate.ToLongDateString();

                //save to isolated storage
                IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
                isf.DeleteFile("DateTemp.txt");
                StreamWriter Writer = new StreamWriter(new IsolatedStorageFileStream("DateTemp.txt", FileMode.OpenOrCreate, isf));
                Writer.WriteLine(valueToStore);
                Writer.Close();
            
            this.NavigationService.Navigate(new Uri("/SetReminderAlarm.xaml", UriKind.RelativeOrAbsolute));
            selDate = e.SelectedDate;
        }
        #endregion

        #region HELPER FOR READ SETTINGS
        private void readTheSettings()
        {
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader Reader = null;
            try
            {
                Reader = new StreamReader(new IsolatedStorageFileStream("SettingsShaum.txt", FileMode.Open, fileStorage));
                string textFile = Reader.ReadToEnd();

                String[] setting = textFile.Split('#');
                if (setting[0].Equals("1"))
                {
                    SET_DAUD = true;
                    START_DAUD = setting[2];
                }
                if (setting[1].Equals("1"))
                {
                    SET_SYAWAL = true;
                    START_SYAWAL = setting[3];
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error reading the settings");
            }

            if (Reader != null)
                Reader.Close();
        }
        #endregion

        #region SET DAUD-SYAWAL
        private void setDaud()
        {
            DateTime st = DateTime.Parse(START_DAUD);
            int len = 365;
            for (int i = 0; i < len; i++)
            {
                if (i % 2 == 0)
                {
                    DateTime toBeChecked2 = st.AddDays(i);
                    MonthCalendar.AddNewEvent(toBeChecked2, new CalendarControl.LCalendar.CalendarEvent("PuasaDaud", toBeChecked, null));
                }
            }
        }

        #endregion
    }
}