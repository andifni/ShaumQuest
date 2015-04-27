using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ShaumQuest.Resources;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.IO.IsolatedStorage;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading;

namespace ShaumQuest
{
    public partial class MainPage : PhoneApplicationPage
    {
        #region VARIABLE
        private String stringHijriah;
        private String jenisshaum;

        private String[] jumlahshaum = new String[10];
        String valueToStore;

        Popup my_popup_cs;
        #endregion

        #region Constructor
        public MainPage()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            InitializeComponent();
            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            my_popup_cs = new Popup();

            String wow = DateTime.Now.ToShortDateString();
            String[] temp = wow.Split('/');
            if (temp[1].Length == 1) temp[1] = "0" + temp[1];
            if (temp[0].Length == 1) temp[0] = "0" + temp[0];
            String stringTanggal = temp[1] + "/" + temp[0] + "/" + temp[2];

            int day = int.Parse(temp[1]);
            int month = int.Parse(temp[0]);
            int year = int.Parse(temp[2]);

            #region EDIT BAHASA INDONESIA
            String dDay = DateTime.Now.ToLongDateString();

            String[] temp2 = dDay.Split(',');

            if (temp2[0].Equals("Monday")) temp2[0] = "Senin";
            if (temp2[0].Equals("Tuesday")) temp2[0] = "Selasa";
            if (temp2[0].Equals("Wednesday")) temp2[0] = "Rabu";
            if (temp2[0].Equals("Thursday")) temp2[0] = "Kamis";
            if (temp2[0].Equals("Friday")) temp2[0] = "Jum'at";
            if (temp2[0].Equals("Saturday")) temp2[0] = "Sabtu";
            if (temp2[0].Equals("Sunday")) temp2[0] = "Minggu";

            String[] WowBulan = { "", "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
            keterangan.Text = temp2[0] + ", " + day + " " + WowBulan[month] + " " + year;
            #endregion

            stringHijriah = makeHijri(day, month, year);
            ketHij.Text = stringHijriah;
            jenisshaum = getJenisshaum();
            ketPuasa.Text = jenisshaum;

            readTheSettings();
            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }
        #endregion

        #region GET JENIS shaum
        private string getJenisshaum()
        {
            String[] temp = stringHijriah.Split(' ');
            int tgl = int.Parse(temp[0]);
            String bulan = temp[1];

            String[] temp2 = DateTime.Now.ToLongDateString().Split(',');

            if (tgl == 1 && bulan.Equals("Syawal"))
            {
                return "Haram shaum - Idul Fitri";
            }
            else if (tgl == 10 && bulan.Equals("Dzulhijah"))
            {
                return "Haram shaum - Idul Adha";
            }
            else if ((tgl == 11 || tgl == 12 || tgl == 13) && bulan.Equals("Dzulhijah"))
            {
                return "Haram shaum - Hari Tasyrik";
            }
            else if (bulan.Equals("Ramadhan"))
            {
                return "Shaum bulan Ramadhan";
            }
            else if (tgl == 13 || tgl == 14 || tgl == 15)
            {
                return "Shaum Ayyamul Bidh";
            }
            else if (bulan.Equals("Muharram"))
            {
                if (tgl == 9)
                    return "Shaum Muharram - Tasu'a";
                else if (tgl == 10)
                    return "Shaum Muharram - Asyura";
                else
                    return "Tidak ada shaum hari ini";
            }
            else if (tgl == 9 && bulan.Equals("Dzulhijah"))
            {
                return "Shaum Arafah";
            }
            else if (temp2[0].Equals("Monday") || temp2[0].Equals("Thursday"))
            {
                return "Shaum Senin - Kamis";
            }
            else
                return "Tidak ada shaum hari ini";
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

        #region POPUP
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Border border = new Border();                                                     // to create green color border
            border.BorderBrush = new SolidColorBrush(Color.FromArgb(120, 0, 0, 0));
            //border.Background = new SolidColorBrush(Color.FromArgb(100, 0, 0, 0));
            border.BorderThickness = new Thickness(20, 310, 30, 350);
            border.Margin = new Thickness(0, 0, 0, 0);


            StackPanel skt_pnl_outter = new StackPanel();                             // stack panel 
            skt_pnl_outter.Background = new SolidColorBrush(Color.FromArgb(255, 140, 157, 45));
            skt_pnl_outter.Orientation = System.Windows.Controls.Orientation.Vertical;



            TextBlock txt_blk1 = new TextBlock();                                         // Textblock 1
            txt_blk1.Text = "Alhamdulillah :)";
            txt_blk1.TextAlignment = TextAlignment.Center;
            txt_blk1.FontSize = 36;
            txt_blk1.Margin = new Thickness(10, 20, 0, 0);
            txt_blk1.Foreground = new SolidColorBrush(Colors.White);

            TextBlock txt_blk2 = new TextBlock();                                      // Textblock 2
            txt_blk2.Text = "Benarkah kamu shaum hari ini?";
            txt_blk2.TextAlignment = TextAlignment.Center;
            txt_blk2.FontSize = 21;
            txt_blk2.Margin = new Thickness(10, 0, 10, 0);
            txt_blk2.Foreground = new SolidColorBrush(Colors.White);


            //Adding control to stack panel
            skt_pnl_outter.Children.Add(txt_blk1);
            skt_pnl_outter.Children.Add(txt_blk2);

            StackPanel skt_pnl_inner = new StackPanel();
            skt_pnl_inner.Orientation = System.Windows.Controls.Orientation.Horizontal;

            Button btn_continue = new Button();                                         // Button continue
            btn_continue.Content = "Ya";
            btn_continue.Width = 215;
            btn_continue.Click += new RoutedEventHandler(btn_Ya);

            Button btn_cancel = new Button();                                           // Button cancel                                     
            btn_cancel.Content = "Tidak";
            btn_cancel.Width = 215;
            btn_cancel.Click += new RoutedEventHandler(btn_Tidak);

            skt_pnl_inner.Children.Add(btn_continue);
            skt_pnl_inner.Children.Add(btn_cancel);


            skt_pnl_outter.Children.Add(skt_pnl_inner);

            // Adding stackpanel  to border
            border.Child = skt_pnl_outter;

            // Adding border to pup-up
            my_popup_cs.Child = border;

            my_popup_cs.VerticalOffset = 0;
            my_popup_cs.HorizontalOffset = 0;

            my_popup_cs.IsOpen = true;
        }

        private void btn_Ya(object sender, RoutedEventArgs e)
        {
            readData();
            GC.Collect();
            saveData();

            if (my_popup_cs.IsOpen)
                my_popup_cs.IsOpen = false;
        }

        private void btn_Tidak(object sender, RoutedEventArgs e)
        {
            if (my_popup_cs.IsOpen)
                my_popup_cs.IsOpen = false;
        }
        #endregion

        #region READ THE DATAS
        private void readData()
        {
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader Reader = null;
            try
            {
                Reader = new StreamReader(new IsolatedStorageFileStream("DataShaum.txt", FileMode.Open, fileStorage));
                string textFile = Reader.ReadToEnd();

                String[] jt = textFile.Split('#');
                for (int i = 0; i < jt.Length; i++)
                    jumlahshaum[i] = jt[i];
            }
            catch (Exception ex)
            {
                for (int i = 0; i < 7; i++)
                    jumlahshaum[i] = "0";
            }

            if (Reader != null)
                Reader.Close();

            for (int i = 0; i < 7; i++)
                if (jumlahshaum[i] == null || jumlahshaum[i].Equals(""))
                    jumlahshaum[i] = "0";

        }
        #endregion

        #region SAVE THE DATAS
        private void saveData()
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            isf.DeleteFile("DataShaum.txt");
            try
            {
                if (jumlahshaum[7].Equals(stringHijriah))
                    MessageBox.Show("Catatan shaum hari ini sudah tersimpan.");
                else
                    addshaum();
            }
            catch (Exception e)
            {
                addshaum();
            }

            while (isf.FileExists("DataShaum.txt"))
            { //do nothing 
            }
            StreamWriter Writer = new StreamWriter(new IsolatedStorageFileStream("DataShaum.txt", FileMode.OpenOrCreate, isf));
            valueToStore = "";
            for (int i = 0; i < 8; i++)
            {
                if (i != 7)
                    valueToStore = valueToStore + jumlahshaum[i] + "#";
                else
                    valueToStore = valueToStore + stringHijriah + "#";
            }
            Writer.WriteLine(valueToStore);
            Writer.Close();
        }
        #endregion

        #region SETTING DAUD & SYAWAL
        private void saveSettings_Click(object sender, RoutedEventArgs e)
        {
            String settingsData = "";
            if ((bool)cbDaud.IsChecked)
                settingsData += "1#";
            else 
                settingsData += "0#";

            if ((bool)cbSyawal.IsChecked)
                settingsData += "1#";
            else
                settingsData += "0#";

            if ((bool)cbDaud.IsChecked)
                settingsData += DateTime.Now.AddDays(1).ToLongDateString() + "#";
            else
                settingsData += "nonono#";

            if ((bool)cbSyawal.IsChecked)
                settingsData += DateTime.Now.AddDays(1).ToLongDateString() + "#";
            else
                settingsData += "nonono#";

            saveTheSettings(settingsData);
            MessageBox.Show("Pengaturan berhasil disimpan.");
        }

        private void saveTheSettings(String data)
        {
            IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
            isf.DeleteFile("SettingsShaum.txt");

            while (isf.FileExists("SettingsShaum.txt"))
            { //do nothing 
            }

            StreamWriter Writer = new StreamWriter(new IsolatedStorageFileStream("SettingsShaum.txt", FileMode.OpenOrCreate, isf));
            
            Writer.WriteLine(data);
            Writer.Close();
        }

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
                    cbDaud.IsChecked = true;
                }
                if (setting[1].Equals("1"))
                {
                    cbSyawal.IsChecked = true;
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

        #region ADD shaum
        private void addshaum()
        {
            #region shaum RAMADHAN
            if (jenisshaum.Equals("Shaum bulan Ramadhan"))
            {
                int temp;
                try
                {
                    temp = Convert.ToInt32(jumlahshaum[3]) + 1;
                }
                catch (Exception ex)
                {
                    temp = 1;
                }
                jumlahshaum[3] = "" + temp;
            }
            #endregion
            #region shaum MUHARRAM - TASU'A
            else if (jenisshaum.Equals("Shaum Muharram - Tasu'a"))
            {
                int temp;
                try
                {
                    temp = Convert.ToInt32(jumlahshaum[5]) + 1;
                }
                catch (Exception ex)
                {
                    temp = 1;
                }
                jumlahshaum[5] = "" + temp;
            }
            #endregion
            #region shaum MUHARRAM - ASYURA
            else if (jenisshaum.Equals("Shaum Muharram - Asyura"))
            {
                int temp;
                try
                {
                    temp = Convert.ToInt32(jumlahshaum[6]) + 1;
                }
                catch (Exception ex)
                {
                    temp = 1;
                }
                jumlahshaum[6] = "" + temp;
            }
            #endregion
            #region shaum ARAFAH
            else if (jenisshaum.Equals("Shaum Arafah"))
            {
                int temp;
                try
                {
                    temp = Convert.ToInt32(jumlahshaum[4]) + 1;
                }
                catch (Exception ex)
                {
                    temp = 1;
                }
                jumlahshaum[4] = "" + temp;
            }
            #endregion
            #region shaum SENIN KAMIS
            else if (jenisshaum.Equals("Shaum Senin - Kamis"))
            {
                String dDay = DateTime.Now.ToLongDateString();

                String[] temp2 = dDay.Split(',');
                if (temp2[0].Equals("Monday"))
                {
                    int temp;
                    try
                    {
                        temp = Convert.ToInt32(jumlahshaum[0]) + 1;
                    }
                    catch (Exception ex)
                    {
                        temp = 1;
                    }
                    jumlahshaum[0] = "" + temp;
                }
                else if (temp2[0].Equals("Thursday"))
                {
                    int temp;
                    try
                    {
                        temp = Convert.ToInt32(jumlahshaum[1]) + 1;
                    }
                    catch (Exception ex)
                    {
                        temp = 1;
                    }
                    jumlahshaum[1] = "" + temp;
                }
            }
            #endregion
            #region AYYAMUL BIDH
            else if (jenisshaum.Equals("Shaum Ayyamul Bidh"))
            {
                int temp;
                try
                {
                    temp = Convert.ToInt32(jumlahshaum[2]) + 1;
                }
                catch (Exception ex)
                {
                    temp = 1;
                }
                jumlahshaum[2] = "" + temp;
            }
            #endregion
            MessageBox.Show("Catatan shaum berhasil disimpan.");
        }
        #endregion

        #region ETC
        private void Pivot_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }
        #endregion
    }

}