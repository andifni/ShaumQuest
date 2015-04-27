using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Globalization;

namespace CalendarControl
{
    public partial class LCalendar : UserControl
    {
        #region Private Variable
        private Dictionary<ShortDateString, List<CalendarEvent>> mEvents;
        private DateTime mCurrentDate;
        private event EventHandler<DayClickedEventArgs> dayClicked;
        #endregion

        #region Costructor

        public LCalendar()
        {
            InitializeComponent();
            InitializeObjects();
            PopulateDaysTextBlock();
        }

        #endregion

        #region Method Public
        
        public void Refresh()
        {
            InizializeCalendar(mCurrentDate);
        }
        
        public event EventHandler<DayClickedEventArgs> OnDayClicked
        {
            add
            {
                dayClicked += new EventHandler<DayClickedEventArgs>(value);
            }
            remove
            {
                dayClicked -= new EventHandler<DayClickedEventArgs>(value);
            }
        }
        
        public void AddNewEvent(DateTime date, CalendarEvent _event)
        {
            ShortDateString sDate = new ShortDateString(date);

            if (!mEvents.ContainsKey(sDate))
                mEvents.Add(sDate, new List<CalendarEvent>() { _event });
            else
            {
                List<CalendarEvent> list = mEvents[sDate];

                if (list == null)
                    list = new List<CalendarEvent>();

                list.Add(_event);
            }

        }
        
        public bool RemoveEvent(DateTime date, CalendarEvent _event)
        {
            ShortDateString sDate = new ShortDateString(date);
            if (!mEvents.ContainsKey(sDate))
                return false;
            else
            {
                List<CalendarEvent> list = mEvents[sDate];

                if (list == null)
                    return false;
                else return list.Remove(_event);
            }
        }
        
        public bool RemoveEvent(DateTime date, Func<CalendarEvent, bool> predicate)
        {

            ShortDateString sDate = new ShortDateString(date);
            if (!mEvents.ContainsKey(sDate))
                return false;
            else
            {
                List<CalendarEvent> list = mEvents[sDate];

                if (list == null)
                    return false;
                else
                {
                    CalendarEvent ev = list.First(predicate);
                    if (ev != null)
                        return list.Remove(ev);
                    else return false;
                }
            }
        }
       
        public bool RemoveDateEvents(DateTime date)
        {
            ShortDateString sDate = new ShortDateString(date);
            return mEvents.Remove(sDate);

        }
        
        public void ClearEvents()
        {
            mEvents.Clear();
        }

        private List<CalendarEvent> FindEventsByDate(DateTime dateTime)
        {
            ShortDateString sDate = new ShortDateString(dateTime);

            if (mEvents.ContainsKey(sDate))
                return mEvents[sDate];
            else return null;
        }
        #endregion

        #region Event
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

            InizializeCalendar(DateTime.Now);
        }

        private void ba_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button && dayClicked != null)
            {
                DateTime date = new DateTime(mCurrentDate.Year, mCurrentDate.Month, (int)(sender as Button).Tag);
                dayClicked(sender, new DayClickedEventArgs(date, FindEventsByDate(date)));
            }
        }

        #endregion

        #region Method Private

        private void InitializeObjects()
        {
            mEvents = new Dictionary<ShortDateString, List<CalendarEvent>>();
            mDefaultHolidayBrush = new SolidColorBrush(Colors.Red);
            mDefaultEventsForegroundBrush = new SolidColorBrush(Colors.White);
            mDefaultEventsBackgroundBrush = (SolidColorBrush)Resources["PhoneAccentBrush"];
        }

        private DateTime BringToFirst(DateTime datetime)
        {
            return new DateTime(datetime.Year, datetime.Month, 1);
        }

        private void PopulateDaysTextBlock()
        {
          /**
            tLUN.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Monday).ToString().Substring(0, 3).ToUpper();
            tMAR.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Tuesday).ToString().Substring(0, 3).ToUpper();
            tMER.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Wednesday).ToString().Substring(0, 3).ToUpper();
            tGIO.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Thursday).ToString().Substring(0, 3).ToUpper();
            tVEN.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Friday).ToString().Substring(0, 3).ToUpper();
            tSAB.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Saturday).ToString().Substring(0, 3).ToUpper();
            tDOM.Text = DateTimeFormatInfo.CurrentInfo.GetDayName(DayOfWeek.Sunday).ToString().Substring(0, 3).ToUpper();
           */
            tLUN.Text = "SEN";
            tMAR.Text = "SEL";
            tMER.Text = "RAB";
            tGIO.Text = "KAM";
            tVEN.Text = "JUM";
            tSAB.Text = "SAB";
            tDOM.Text = "MIN";
        }

        private void InizializeCalendar(DateTime dateTime)
        {
            SetMonthYear(dateTime);

            int days = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
            DateTime firstOfMonth = BringToFirst(dateTime);
            DayOfWeek startingDay = firstOfMonth.DayOfWeek;


            ClearDays();

            int riga = 2;
            int colonna = GetDayNumber(startingDay);
            for (int i = 0; i < days; i++)
            {
                Border bord = new Border();
                bord.Name = "brd" + (i + 1).ToString();

                bord.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                bord.Margin = new Thickness(0);

                Button ba = new Button();
                ba.BorderBrush = new SolidColorBrush(Colors.Transparent);
                ba.Width = 99;
                ba.Height = 99;
                ba.Tag = (i + 1);
                ba.Margin = new Thickness(1);
                ba.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                ba.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                ba.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                ba.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
                ba.Content = (i + 1).ToString();
                ba.Click += new RoutedEventHandler(ba_Click);
                ba.Foreground = new SolidColorBrush(Colors.Black);

                //puasa Senin-Kamis
                if (colonna == 0 || colonna == 3) // giorno festivo
                {
                    ba.Background = new SolidColorBrush(Color.FromArgb(255, 28, 106, 129));
                    ba.Foreground = new SolidColorBrush(Colors.White);
                }

                //controllo sull'evento esistente
                #region JENIS PUASA
                if (HasEvents(new ShortDateString(firstOfMonth.AddDays(i))))
                {
                    ShortDateString hehe = new ShortDateString(firstOfMonth.AddDays(i));
                    List<CalendarEvent> wow = mEvents[hehe];
                    CalendarEvent jenisEvent = wow.First();
                    
                    if (jenisEvent.Description.Equals("TandaToday"))
                    {
                        ba.Margin = new Thickness(0);
                        bord.BorderThickness = new Thickness(5);
                        bord.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 16, 16, 16));
                        jenisEvent = wow.Last();
                    }

                    if (jenisEvent.Description.Equals("Muharram"))
                    {
                        ba.Background = new SolidColorBrush(Color.FromArgb(255, 221, 85, 123));
                        ba.Foreground = new SolidColorBrush(Colors.White);
                    }
                    else if (jenisEvent.Description.Equals("Arafah"))
                    {
                        ba.Background = new SolidColorBrush(Color.FromArgb(255, 129, 98, 139));
                        ba.Foreground = new SolidColorBrush(Colors.White);
                    }
                    else if (jenisEvent.Description.Equals("Ayyamul"))
                    {
                        ba.Background = new SolidColorBrush(Color.FromArgb(255, 22, 168, 158));
                        ba.Foreground = new SolidColorBrush(Colors.White);
                    }
                    
                    if (jenisEvent.Description.Equals("PuasaDaud"))
                    {
                        ba.Background = new SolidColorBrush(Color.FromArgb(255, 140, 157, 45));
                        ba.Foreground = new SolidColorBrush(Colors.White);
                    }

                    if (jenisEvent.Description.Equals("Haram"))
                    {
                        ba.Background = new SolidColorBrush(Color.FromArgb(255, 193, 17, 37));
                        ba.Foreground = new SolidColorBrush(Colors.White);
                    }

                    if (jenisEvent.Description.Equals("Ramadhan"))
                    {
                        ba.Background = new SolidColorBrush(Color.FromArgb(255, 176, 100, 25));
                        ba.Foreground = new SolidColorBrush(Colors.White);
                    }
                #endregion

                }

                bord.Child = ba;

                AddElementToGrid(riga, colonna++, bord);

                //countDay++;
                if (colonna % 7 == 0)
                {
                    riga++;
                    colonna = 0;
                }
            }

            mCurrentDate = dateTime;
            LayoutRoot.UpdateLayout();

        }

        private bool HasEvents(ShortDateString sDate)
        {
            if (mEvents.ContainsKey(sDate))
                return mEvents[sDate] != null && mEvents[sDate].Count > 0;
            else return false;
        }
        
        private void SetMonthYear(DateTime dateTime)
        {
            String bulan = dateTime.ToString("MMMM");

            #region BAHASA INDONESIA
            if (bulan.Equals("January")) bulan = "Januari";
            else if (bulan.Equals("February")) bulan = "Februari";
            else if (bulan.Equals("March")) bulan = "Maret";
            else if (bulan.Equals("April")) bulan = "April";
            else if (bulan.Equals("May")) bulan = "Mei";
            else if (bulan.Equals("June")) bulan = "Juni";
            else if (bulan.Equals("July")) bulan = "Juli";
            else if (bulan.Equals("August")) bulan = "Agustus";
            else if (bulan.Equals("September")) bulan = "September";
            else if (bulan.Equals("October")) bulan = "Oktober";
            else if (bulan.Equals("November")) bulan = "November";
            else if (bulan.Equals("December")) bulan = "Desember";
            #endregion

            tbMeseAnno.Text = String.Format("{0} {1}", bulan.ToUpper(), dateTime.Year);
        }

        private int GetDayNumber(DayOfWeek d)
        {
            if (d == DayOfWeek.Sunday)
            {
                return 6;
            }
            else return ((int)d) - 1;
        }

        private void ClearDays()
        {
            LayoutRoot.Children.ToList().ForEach(x =>
            {
                if (x is FrameworkElement && (x as FrameworkElement).Name.StartsWith("brd"))
                {
                    LayoutRoot.Children.Remove(x);
                }
            });
        }

        private void AddElementToGrid(int riga, int colonna, FrameworkElement el)
        {
            Grid.SetRow(el, riga);
            Grid.SetColumn(el, colonna);
            LayoutRoot.Children.Add(el);
        }

        private void OnChangeMonth(object sender, RoutedEventArgs e)
        {
            Button butSender = sender as Button;

            if (butSender.Name == NextBtn.Name)
            {
                mCurrentDate = mCurrentDate.AddMonths(+1);
            }
            else if (butSender.Name == BackBtn.Name)
            {
                mCurrentDate = mCurrentDate.AddMonths(-1);
            }

            InizializeCalendar(mCurrentDate);
        }
        #endregion

        #region Nested Classes
        public class DayClickedEventArgs : EventArgs
        {
            public DateTime SelectedDate { get; set; }
            public List<CalendarEvent> Events { get; set; }
            public DayClickedEventArgs(DateTime dt, List<CalendarEvent> evn)
            {
                SelectedDate = dt;
                Events = evn;
            }
        }
        public class ShortDateString
        {
            String Value { get; set; }
            DateTime DateTimeValue { get; set; }
            public ShortDateString(DateTime dt)
            {
                Value = dt.ToShortDateString();
                DateTimeValue = dt;
            }
            public override int GetHashCode()
            {
                return Value.GetHashCode();
            }
            public override bool Equals(object obj)
            {
                if (obj is ShortDateString)
                    return Value.Equals((obj as ShortDateString).Value);
                else return base.Equals(obj);
            }
        }
        public class CalendarEvent
        {
            public DateTime DateWhen { get; set; }
            public String Description { get; set; }
            public object Data { get; set; }
            public CalendarEvent(String _description, DateTime _when, object _data = null)
            {
                DateWhen = _when;
                Description = _description;
                Data = _data;
            }
        }
        #endregion

        #region Brushes and GUI
        private SolidColorBrush mDefaultHolidayBrush;
        private SolidColorBrush mDefaultEventsBackgroundBrush;
        private SolidColorBrush mDefaultEventsForegroundBrush;

        public SolidColorBrush HolidayBrush
        {
            get
            {
                return mDefaultHolidayBrush;
            }
            set
            {
                mDefaultHolidayBrush = value;
            }
        }

        public SolidColorBrush EventsBackgroundBrush
        {
            get
            {
                return mDefaultEventsBackgroundBrush;
            }
            set
            {
                mDefaultEventsBackgroundBrush = value;
            }
        }

        public SolidColorBrush EventsForegroundBrush
        {
            get
            {
                return mDefaultEventsForegroundBrush;
            }
            set
            {
                mDefaultEventsForegroundBrush = value;
            }
        }

        public Brush ButtonBorderBrush
        {
            get
            {
                return BackBtn.BorderBrush;
            }
            set
            {
                BackBtn.BorderBrush = value;
                NextBtn.BorderBrush = value;

            }
        }

        public Brush ButtonBackground
        {
            get
            {
                return BackBtn.Background;
            }
            set
            {
                //BackBtn.Background = value;
                //NextBtn.Background = value;

            }
        }
        #endregion
    }
}
