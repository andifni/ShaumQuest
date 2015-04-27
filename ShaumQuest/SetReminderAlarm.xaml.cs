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
using Microsoft.Phone.Scheduler;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;
using System.Threading;

namespace ShaumQuest
{
    public partial class SetReminderAlarm : PhoneApplicationPage
    {
        #region Private Variable
        private DateTime tanggalnya;

        private String stringTanggal;
        private String stringHijriah;
        private String jenisPuasa;
        #endregion

        #region CONSTRUCTOR
        public SetReminderAlarm()
        {
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

            int day= 0, month = 0, year = 0;

            InitializeComponent();
            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader Reader = null;
            try
            {
                Reader = new StreamReader(new IsolatedStorageFileStream("DateTemp.txt", FileMode.Open, fileStorage));
                string textFile = Reader.ReadToEnd();
                tanggalnya = DateTime.Parse(textFile);

                String wow = tanggalnya.ToShortDateString();
                String[] temp = wow.Split('/');
                if (temp[1].Length == 1) temp[1] = "0" + temp[1];
                if (temp[0].Length == 1) temp[0] = "0" + temp[0];
                stringTanggal = temp[1] +"/" + temp[0] + "/" + temp[2];
                tanggalnyah.Header = stringTanggal;

                Reader.Close();
                day = int.Parse(temp[1]);
                month = int.Parse(temp[0]);
                year = int.Parse(temp[2]);

            }
            catch
            {
                MessageBox.Show("File is not created");
            }
            stringHijriah = makeHijri(day, month, year);
            jenisPuasa = getJenisPuasa();
            pembuka.Text = stringHijriah + "\n" + jenisPuasa;

            Debug.WriteLine(tanggalnya);
            Debug.WriteLine(DateTime.Now);
            getInfo();
        }
        #endregion

        #region GET INFO
        private void getInfo()
        {
            textHeader.Text = jenisPuasa;

            ImageBrush myBrush, myBrush2, myBrush3;

            #region Haram Shaum IDUL FITRI DAN ADHA
            if (jenisPuasa.Equals("Haram Shaum - Idul Fitri") || jenisPuasa.Equals("Haram Shaum - Idul Adha"))
            {
                myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/infopuasaHARAM-atas.png", UriKind.Relative));
                bgHeader.Background = myBrush;

                myBrush2 = new ImageBrush();
                myBrush2.ImageSource = new BitmapImage(new Uri("/infopuasaHARAM-tengah.png", UriKind.Relative));
                bgIsi.Background = myBrush2;
                bgIsi2.Background = myBrush2;

                myBrush3 = new ImageBrush();
                myBrush3.ImageSource = new BitmapImage(new Uri("/infopuasaHARAM-bawah.png", UriKind.Relative));
                bgFooter.Background = myBrush3;
                
                textIsi.Text = "Idul Fitri jatuh pada tanggal 1 Syawal dan merupakan hari kemenangan (kembali pada kesucian) bagi umat Islam. Sedangkan Idul Adha jatuh pada tanggal 10 Zulhijjah, hari ini diperingati peristiwa kurban, yaitu ketika Nabi Ibrahim harus menyembelih putranya, Ismail, atas perintah Allah, yang kemudian digantikan oleh-Nya dengan domba.\n\nDari Abu Said Al-Khudri RA, beliau mengatakan, \n“Nabi SAW melarang shaum pada saat Idul Fitri dan hari berkurban.” (HR. Bukhari 1991, Ibn Majah 1721)";
            }
            #endregion
            #region Haram Shaum - HARI TASYRIK
            else if (jenisPuasa.Equals("Haram Shaum - Hari Tasyrik"))
            {
                myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/infopuasaHARAM-atas.png", UriKind.Relative));
                bgHeader.Background = myBrush;

                myBrush2 = new ImageBrush();
                myBrush2.ImageSource = new BitmapImage(new Uri("/infopuasaHARAM-tengah.png", UriKind.Relative));
                bgIsi.Background = myBrush2;
                bgIsi2.Background = myBrush2;

                myBrush3 = new ImageBrush();
                myBrush3.ImageSource = new BitmapImage(new Uri("/infopuasaHARAM-bawah.png", UriKind.Relative));
                bgFooter.Background = myBrush3;

                textIsi.Text = "Hari Tasyriq jatuh pada tiga hari sesudah Idul Adha, yaitu tanggal 11, 12 dan 13 bulan Zulhijjah.\n\nDari Nubaisyah Al-Hudzali, Nabi SAW bersabda, \n“Hari-hari tasyriq adalah hari makan dan minum.” (HR. Muslim 1141)\n\nDiriwayatkan oleh Abu Daud (2418) dari 'Amr ibnul 'Ash RA, dia mengatakan, \n”Inilah hari-hari yang pernah diperintahkan oleh Rasulullah SAW kepada kita supaya berbuka padanya, dan melarang kita berpuasa padanya.” Imam Malik mengatakan, ”Yang dimaksud ialah hari-hari Tasyriq.”";
            }
            #endregion
            #region TIDAK ADA PUASA
            else if (jenisPuasa.Equals("Tidak ada puasa"))
            {
                myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/infopuasaHARAM-atas.png", UriKind.Relative));
                bgHeader.Background = myBrush;

                myBrush2 = new ImageBrush();
                myBrush2.ImageSource = new BitmapImage(new Uri("/infopuasaHARAM-tengah.png", UriKind.Relative));
                bgIsi.Background = myBrush2;
                bgIsi2.Background = myBrush2;

                myBrush3 = new ImageBrush();
                myBrush3.ImageSource = new BitmapImage(new Uri("/infopuasaHARAM-bawah.png", UriKind.Relative));
                bgFooter.Background = myBrush3;

                textIsi.Text = "Tidak ada informasi mengenai shaum hari ini.";
            }
            #endregion
            #region PUASA RAMADHAN
            else if (jenisPuasa.Equals("Shaum bulan Ramadhan"))
            {
                myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/infopuasaRMD-atas.png", UriKind.Relative));
                bgHeader.Background = myBrush;

                myBrush2 = new ImageBrush();
                myBrush2.ImageSource = new BitmapImage(new Uri("/infopuasaRMD-tengah.png", UriKind.Relative));
                bgIsi.Background = myBrush2;
                bgIsi2.Background = myBrush2;

                myBrush3 = new ImageBrush();
                myBrush3.ImageSource = new BitmapImage(new Uri("/infopuasaRMD-bawah.png", UriKind.Relative));
                bgFooter.Background = myBrush3;

                textIsi.Text = "Shaum Ramadhan adalah shaum wajib pada bulan Ramadhan yang juga merupakan salah satu rukun Islam. Puasa ini melatih kita untuk menahan hawa nafsu, mengajarkan untuk hidup sehat dan sederhana, juga menumbuhkan rasa peduli pada orang lain yang lemah.\n\nDalil-dalil tentang kewajiban shaum Ramadhan sangatlah banyak dalam nash-nash Al-Qur`an dan Sunnah. Di antaranya adalah firman Allah Ta’âla, \n“Wahai orang-orang yang beriman, diwajibkan atas kalian untuk berpuasa sebagaimana diwajibkan atas orang-orang sebelum kalian agar kalian bertakwa.” (Al-Baqarah: 183)\n\n”Barang siapa mendirikan shaum Ramadan dengan penuh keimanan dan kebaikan, maka akan diampunilah dosa-dosanya yang telah lalu” (HR. Bukhari – Muslim).";
            }
            #endregion
            #region PUASA MUHARRAM
            else if (jenisPuasa.Equals("Shaum Muharram"))
            {
                myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/infopuasaMHR-atas.png", UriKind.Relative));
                bgHeader.Background = myBrush;

                myBrush2 = new ImageBrush();
                myBrush2.ImageSource = new BitmapImage(new Uri("/infopuasaMHR-tengah.png", UriKind.Relative));
                bgIsi.Background = myBrush2;
                bgIsi2.Background = myBrush2;

                myBrush3 = new ImageBrush();
                myBrush3.ImageSource = new BitmapImage(new Uri("/infopuasaMHR-bawah.png", UriKind.Relative));
                bgFooter.Background = myBrush3;

                textIsi.Text = "Shaum Tasu’a adalah shaum sunnah yang dilaksanakan pada tanggal 9 Muharram. Sedangkan shaum Asyura adalah shaum sunnah yang dilaksanakan pada tanggal 10 Muharram. Hukum shaum Tasu’a dan shaum Asyura adalah sunnah muakkad, yaitu sunnah yang sangat dianjurkan. \n\nDari Abu Qatadah RA bahwasanya Nabi SAW bersabda, \n“Shaum hari ‘Asyura, aku mengharapkan pahalanya di sisi Allah dapat menghapuskan dosa-dosa kecil setahun sebelumnya.” (HR. Muslim dan Ibnu Majah)\n\nDari Ibnu Abbas RA berkata, \n“Ketika Rasulullah SAW melakukan shaum ‘Asyura dan memerintahkan para sahabat untuk mengerjakan shaum ‘Asyura, para sahabat berkata: “Wahai Rasulullah, hari ‘Asyura adalah hari yang diagungkan oleh orang-orang Yahudi dan Nasrani.” Maka Rasulullah SAW bersabda, “Jika tahun datang tiba, insya Allah, kita juga akan melakukan shaum pada tanggal Sembilan Muharram.” Tahun mendatang belum tiba, ternyata Rasulullah SAW sudah wafat. (HR. Muslim, ath-Thabari, dan al-Baihaqi)\n\nImam Al-Baihqi, Abdurrazzaq, dan ath-Thahawi meriwayatkan dengan sanad yang shahih dari jalur Ibnu Juraij dari Atha' dari Ibnu Abbas RA yang berkata, \n“Laksanakanlah shaum  tanggal 9 dan 10 Muharram, selisihilah orang-orang Yahudi.”";
            }
            #endregion
            #region PUASA ARAFAH 
            else if (jenisPuasa.Equals("Shaum Arafah"))
            {
                myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/infopuasaARF-atas.png", UriKind.Relative));
                bgHeader.Background = myBrush;

                myBrush2 = new ImageBrush();
                myBrush2.ImageSource = new BitmapImage(new Uri("/infopuasaARF-tengah.png", UriKind.Relative));
                bgIsi.Background = myBrush2;
                bgIsi2.Background = myBrush2;

                myBrush3 = new ImageBrush();
                myBrush3.ImageSource = new BitmapImage(new Uri("/infopuasaARF-bawah.png", UriKind.Relative));
                bgFooter.Background = myBrush3;

                textIsi.Text = "Shaum Arafah adalah shaum sunnah yang dilaksanakan pada hari Arafah yakni tanggal 9 Dzulhijah. Puasa ini sangat dianjurkan bagi orang-orang yang tidak menjalankan ibadah haji.\n\nDari Abi Qatadah RA, ia berkata Rasulullah SAW telah bersabda, \n“Puasa hari Arafah itu dapat menghapuskan dosa dua tahun, satu tahun yang telah lalu dan satu tahun yang akan datang.” (Riwayat Jama'ah kecuali Bukhari dan Tarmidzi).";
            }
            #endregion
            #region PUASA SENIN KAMIS 
            else if (jenisPuasa.Equals("Shaum Senin - Kamis"))
            {
                myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/infopuasaSENKAM-atas.png", UriKind.Relative));
                bgHeader.Background = myBrush;

                myBrush2 = new ImageBrush();
                myBrush2.ImageSource = new BitmapImage(new Uri("/infopuasaSENKAM-tengah.png", UriKind.Relative));
                bgIsi.Background = myBrush2;
                bgIsi2.Background = myBrush2;

                myBrush3 = new ImageBrush();
                myBrush3.ImageSource = new BitmapImage(new Uri("/infopuasaSENKAM-bawah.png", UriKind.Relative));
                bgFooter.Background = myBrush3;

                textIsi.Text = "Shaum ini dilakukan pada setiap pekan di dua hari tersebut. Keutamaannya bisa menghapus kesalahan dan meninggikan derajat, serta memang dua hari tersebut adalah saat amalan diangkat di hadapan Allah sehingga sangat baik untuk berpuasa saat itu.\n\nDari Abu Qotadah RA, sesungguhnya Rasulullah SAW ditanya tentang shaum pada hari Senin. Maka beliau menjawab, \n“Hari Senin adalah hari lahirku, hari aku mulai diutus atau hari mulai diturunkannya wahyu kepadaku.” (HR. Muslim)\n\nDari ‘Aisyah RA, beliau mengatakan, \n“Rasulullah SAW biasa menaruh pilihan berpuasa pada hari Senin dan Kamis.” (HR. An Nasai no. 2362 dan Ibnu Majah no. 1739. All Hafizh Abu Thohir mengatakan bahwa hadits ini hasan. Syaikh Al Albani mengatakan bahwa hadits ini shahih)\n\n“Dua hari tersebut adalah waktu dihadapkannya amalan pada Rabb semesta alam (pada Allah). Aku sangat suka ketika amalanku dihadapkan sedang aku dalam keadaan berpuasa.” (HR. An Nasai no. 2360 dan Ahmad 5: 201. Al Hafizh Abu Thohir mengatakan bahwa sanad hadits ini hasan).";

            }
            #endregion
            #region AYYAMUL BIDH
            else if (jenisPuasa.Equals("Shaum Ayyamul Bidh"))
            {
                myBrush = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/infopuasaAYBID-atas.png", UriKind.Relative));
                bgHeader.Background = myBrush;

                myBrush2 = new ImageBrush();
                myBrush2.ImageSource = new BitmapImage(new Uri("/infopuasaAYBID-tengah.png", UriKind.Relative));
                bgIsi.Background = myBrush2;
                bgIsi2.Background = myBrush2;

                myBrush3 = new ImageBrush();
                myBrush3.ImageSource = new BitmapImage(new Uri("/infopuasaAYBID-bawah.png", UriKind.Relative));
                bgFooter.Background = myBrush3;

                textIsi.Text = "Shaum ayyamul bidh adalah shaum pertengahan tiga hari pada bulan Hijriyah, yaitu pada hari ke-13, 14, dan 15. Puasa tersebut disebut ayyamul bidh (hari putih) karena pada malam-malam tersebut bersinar bulan purnama dengan sinar rembulannya yang putih.\n\nDari Abu Hurairah RA, ia berkata, \n“Kekasihku (yaitu Rasulullah SAW) mewasiatkan padaku tiga nasehat yang aku tidak meninggalkannya hingga aku mati: [1] berpuasa tiga hari setiap bulannya, [2] mengerjakan shalat Dhuha, [3] mengerjakan shalat witir sebelum tidur.” (HR. Bukhari no. 1178).\n\nDari Abdullah bin Amru bin Ash RA berkata, Rasulullah SAW bersabda, \n“(Pahala) Shaum sunah tiga hari setiap bulan adalah bagaikan (pahala) shaum satu tahun penuh.” (HR. Bukhari  Muslim)\n\nDari Ibnu ‘Abbas RA, beliau berkata, \n“Rasulullah SAW biasa berpuasa pada ayyamul biidh ketika tidak bepergian maupun ketika bersafar.” (HR. An Nasai no. 2345. Syaikh Al Albani mengatakan bahwa hadits ini hasan. Lihat Ash Shohihah no. 580).\n\nDari Abu Dzar, Rasulullah SAW bersabda padanya, \n“Jika engkau ingin berpuasa tiga hari setiap bulannya, maka berpuasalah pada tanggal 13, 14, dan 15 (dari bulan Hijriyah).” (HR. Tirmidzi no. 761 dan An Nasai no. 2424. Syaikh Al Albani mengatakan bahwa hadits ini hasan).";
            }
            #endregion
           
        }
        #endregion

        #region CONVERT TO HIJRI
        public String makeHijri(int day, int month, int year)
        {
            int JHH, JHM;
            int TH, XH, XM;
            String MH;

            day--;
            int[] nonkabisat = {0, 0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334};
            int[] kabisat = {0, 0, 31, 60, 91, 121, 152, 182, 213, 244, 274, 305, 335};

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
            
            JHM = (int)((year-1) * 365.25) + XM - 13;
            JHH = JHM - 227015;

            TH = (int) (JHH / 354.367) + 1;
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

        #region TimePicker Changed
        private void tpReminder_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {
            //tpReminder.
        }

        private void tpAlarm_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

        }
        #endregion

        #region JENIS PUASA
        private string getJenisPuasa()
        {
            String[] temp = stringHijriah.Split(' ');
            int tgl = int.Parse(temp[0]);
            String bulan = temp[1];

            String[] temp2 = tanggalnya.ToLongDateString().Split(',');

            if (tgl == 1 && bulan.Equals("Syawal"))
            {
                return "Haram Shaum - Idul Fitri";
            }
            else if (tgl == 10 && bulan.Equals("Dzulhijah"))
            {
                return "Haram Shaum - Idul Adha";
            }
            else if ((tgl == 11 || tgl == 12 || tgl == 13) && bulan.Equals("Dzulhijah"))
            {
                return "Haram Shaum - Hari Tasyrik";
            }
            else if (bulan.Equals("Ramadhan"))
            {
                return "Shaum bulan Ramadhan";
            }
            else if (tgl == 13 || tgl == 14 || tgl == 15)
            {
                return "Shaum Ayyamul Bidh";
            }
            else if ((tgl == 9 || tgl == 10) && bulan.Equals("Muharram"))
            {
                return "Shaum Muharram";
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
                return "Tidak ada puasa";
        }
        #endregion

        #region SET AND SAVE REMINDER
        // setAll Reminders and Alarms
        private void setAll_Click(object sender, RoutedEventArgs e)
        {
            // Generate a unique name for the new notification. You can choose a
            // name that is meaningful for your app, or just use a GUID.
            String name = System.Guid.NewGuid().ToString();
            Boolean bAlarm = false;
            Boolean bReminder = false;

            #region REMINDER H-1

            DateTime date = tanggalnya.AddDays(-1);
            DateTime time = (DateTime)tpReminder.Value;
            DateTime beginTime = date + time.TimeOfDay;

            // Make sure that the begin time has not already passed.
            
                String[] temp = tanggalnya.AddDays(-1).ToShortDateString().Split('/');
                int rday = int.Parse(temp[1]);
                int rmonth = int.Parse(temp[0]);
                int ryear = int.Parse(temp[2]);

                String dDay = tanggalnya.AddDays(-1).ToLongDateString();
                #region EDIT BAHASA INDONESIA
                    String[] temp2 = dDay.Split(',');

                    if (temp2[0].Equals("Monday")) temp2[0] = "Senin";
                    if (temp2[0].Equals("Tuesday")) temp2[0] = "Selasa";
                    if (temp2[0].Equals("Wednesday")) temp2[0] = "Rabu";
                    if (temp2[0].Equals("Thursday")) temp2[0] = "Kamis";
                    if (temp2[0].Equals("Friday")) temp2[0] = "Jum'at";
                    if (temp2[0].Equals("Saturday")) temp2[0] = "Sabtu";
                    if (temp2[0].Equals("Sunday")) temp2[0] = "Minggu";

                    String[] WowBulan = { "", "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
                    string param1Value = temp2[0] + ", " + rday + " " + WowBulan[rmonth] + " " + ryear;
                #endregion
            
            string param2Value = jenisPuasa + "\nPengingat " + time.ToShortTimeString();
            string queryString = "";
            if (param1Value != "" && param2Value != "")
            {
                queryString = "?param1=" + param1Value + "&param2=" + param2Value;
            }
            else if (param1Value != "" || param2Value != "")
            {
                queryString = (param1Value != null) ? "?param1=" + param1Value : "?param2=" + param2Value;
            }

            Uri navigationUri = new Uri("/MainPage.xaml" , UriKind.Relative);

            if ((bool)cbReminder.IsChecked && (beginTime < DateTime.Now))
            {
                MessageBox.Show("Tidak dapat memasang pengingat. Waktu sudah lewat.");
                return;
            }
            else if ((bool)cbReminder.IsChecked)
            {
                Reminder reminder = new Reminder(param1Value);
                //reminder.Title = param1Value;
                reminder.Content = param2Value;
                reminder.BeginTime = beginTime;
                reminder.NavigationUri = navigationUri;

                // Register the reminder with the system.
                try
                {
                    ScheduledActionService.Add(reminder);
                    bReminder = true;
                }
                catch (Exception exx)
                {
                    MessageBox.Show("Tidak bisa menyimpan. Pengingat sudah ada.");
                }
            }
            #endregion

            String Aname = System.Guid.NewGuid().ToString();

            #region ALARM SAHUR
            DateTime Adate = tanggalnya;
            DateTime Atime = (DateTime)tpAlarm.Value;
            DateTime AbeginTime = Adate + Atime.TimeOfDay;

            // Make sure that the begin time has not already passed.
            
            String[] Atemp = tanggalnya.ToShortDateString().Split('/');
            int arday = int.Parse(Atemp[1]);
            int armonth = int.Parse(Atemp[0]);
            int aryear = int.Parse(Atemp[2]);

            String adDay = tanggalnya.ToLongDateString();
            #region EDIT BAHASA INDONESIA
            String[] atemp2 = adDay.Split(',');

            if (atemp2[0].Equals("Monday")) atemp2[0] = "Senin";
            if (atemp2[0].Equals("Tuesday")) atemp2[0] = "Selasa";
            if (atemp2[0].Equals("Wednesday")) atemp2[0] = "Rabu";
            if (atemp2[0].Equals("Thursday")) atemp2[0] = "Kamis";
            if (atemp2[0].Equals("Friday")) atemp2[0] = "Jum'at";
            if (atemp2[0].Equals("Saturday")) atemp2[0] = "Sabtu";
            if (atemp2[0].Equals("Sunday")) atemp2[0] = "Minggu";

            String[] aWowBulan = { "", "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "November", "Desember" };
            string aparam1Value = atemp2[0] + ", " + arday + " " + aWowBulan[armonth] + " " + aryear;
            #endregion

            string aparam2Value = jenisPuasa + "\nAlarm Sahur " + Atime.ToShortTimeString();
            string aqueryString = "";
            if (aparam1Value != "" && aparam2Value != "")
            {
                aqueryString = "?param1=" + aparam1Value + "&param2=" + aparam2Value;
            }
            else if (aparam1Value != "" || aparam2Value != "")
            {
                aqueryString = (param1Value != null) ? "?param1=" + aparam1Value : "?param2=" + aparam2Value;
            }

            Uri anavigationUri = new Uri("/ShowParams.xaml", UriKind.Relative);

            if ((bool)cbAlarm.IsChecked && (AbeginTime < DateTime.Now))
            {
                //MessageBox.Show(AbeginTime + "wew" + DateTime.Now);
                MessageBox.Show("Tidak dapat memasang alarm. Waktu sudah lewat.");
                return;
            }
            else if ((bool)cbAlarm.IsChecked)
            {
                //Reminder reminder = new Reminder("LailaCantik"+ Aname);
                //reminder.Title = aparam1Value;
                //reminder.Content = aparam2Value;
                //reminder.BeginTime = AbeginTime;
                //reminder.NavigationUri = anavigationUri;

                var newAlaram = new Alarm(aparam1Value + "  ")
                {
                    Content = aparam2Value,
                    BeginTime = AbeginTime,
                    ExpirationTime = AbeginTime.AddHours(1),
                };

                // Register the reminder with the system.
               
                //ScheduledActionService.Add(reminder);
                try
                {
                    ScheduledActionService.Add(newAlaram);
                    bAlarm = true;
                }
                catch (Exception exx)
                {
                    MessageBox.Show("Tidak bisa menyimpan. Alarm sudah ada.");
                }
            }
            #endregion

            if (bAlarm && bReminder)
                MessageBox.Show("Pengingat dan Alarm berhasil disimpan.");
            else if (bAlarm)
                MessageBox.Show("Alarm berhasil disimpan.");
            else if (bReminder)
                MessageBox.Show("Pengingat berhasil disimpan.");

            // Navigate back to the main reminder list page.
            NavigationService.GoBack();
        }
    #endregion

       
    }
}