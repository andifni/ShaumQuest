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
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace ShaumQuest
{
    public partial class KotakHadiah : PhoneApplicationPage
    {
        String[] jumlahPuasa = new String[10];

        public KotakHadiah()
        {
            InitializeComponent();

            #region READ THE DATA

            IsolatedStorageFile fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
            StreamReader Reader = null;
            try
            {
                Reader = new StreamReader(new IsolatedStorageFileStream("DataShaum.txt", FileMode.Open, fileStorage));
                string textFile = Reader.ReadToEnd();
                String[] jt = textFile.Split('#');

                for (int i = 0; i < jt.Length; i++)
                    jumlahPuasa[i] = jt[i];
            }
            catch (Exception ex)
            {
                for (int i = 0; i < 7; i++)
                    jumlahPuasa[i] = "0";
            }
            if (Reader != null)
                Reader.Close();

            for (int i = 0; i < 7; i++)
                if (jumlahPuasa[i] == null || jumlahPuasa[i].Equals(""))
                    jumlahPuasa[i] = "0";
            #endregion

            setQuest();
        }

        #region SET THE QUEST
        private void setQuest()
        {
            int[] jp = new int[10];
            try
            {
                for (int i = 0; i < 7; i++)
                    jp[i] = Convert.ToInt32(jumlahPuasa[i]);
            }
            catch (Exception e)
            {
            }

            #region 1. Puasa Ayyamul Bidh 1x
            if (jp[2] >= 1)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgeab1.png", UriKind.Relative));
                _1.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-ab1t.png", UriKind.Relative));
                _1text.Background = myBrush2;
            }
            #endregion

            #region 2.	Puasa Ayyamul Bidh 3x
            if (jp[0] >= 3)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgeab3.png", UriKind.Relative));
                _2.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-ab3t.png", UriKind.Relative));
                _2text.Background = myBrush2;
            }
            #endregion
            
            #region 3.	Puasa Ayyamul Bidh 6x
            if (jp[2] >= 6)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgeab6.png", UriKind.Relative));
                _3.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-ab6t.png", UriKind.Relative));
                _3text.Background = myBrush2;
            }
            #endregion
            
            #region 4.	Puasa Arafah
            if (jp[4] >= 1)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgear.png", UriKind.Relative));
                _4.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-at.png", UriKind.Relative));
                _4text.Background = myBrush2;
            }
            #endregion
            
            #region 5.	Puasa Kamis 1x
            if (jp[1] >= 1)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgek1.png", UriKind.Relative));
                _5.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-k1t.png", UriKind.Relative));
                _5text.Background = myBrush2;
            }
            #endregion
            
            #region 6.	Puasa Muharram Asyura
            if (jp[6] >= 1)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgema.png", UriKind.Relative));
                _6.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-mat.png", UriKind.Relative));
                _6text.Background = myBrush2;
            }
            #endregion
            
            #region 7.	Puasa Muharam Full
            if (jp[5] >= 1 && jp[6] >= 1)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgemf.png", UriKind.Relative));
                _7.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-mt.png", UriKind.Relative));
                _7text.Background = myBrush2;
            }
            #endregion
            
            #region 8.	Puasa Muharram Tasu'a
            if (jp[5] >= 1)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgemt.png", UriKind.Relative));
                _8.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-mtt.png", UriKind.Relative));
                _8text.Background = myBrush2;
            }
            #endregion
            
            #region 9.	Puasa Ramadhan Full
            if (jp[3] >= 30)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badger.png", UriKind.Relative));
                _9.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-rt.png", UriKind.Relative));
                _9text.Background = myBrush2;
            }
            #endregion
            
            #region 10.	Puasa Senin 1x
            if (jp[0] >= 1)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badges1.png", UriKind.Relative));
                _10.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-s1t.png", UriKind.Relative));
                _10text.Background = myBrush2;
            }
            #endregion
            
            #region 11.	Puasa Senin Kamis 1x
            if (jp[0] >= 1 && jp[1] >= 1)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgesk1.png", UriKind.Relative));
                _11.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-sk1t.png", UriKind.Relative));
                _11text.Background = myBrush2;
            }
            #endregion
            
            #region 12.	Puasa Senin Kamis 3x
            if (jp[0] >= 3 && jp[1] >= 3)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgesk3.png", UriKind.Relative));
                _12.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-sk3t.png", UriKind.Relative));
                _12text.Background = myBrush2;
            }
            #endregion

            #region 13.	Puasa Senin Kamis 5x
            if (jp[0] >= 5 && jp[1] >= 5)
            {
                ImageBrush myBrush, myBrush2;
                myBrush = new ImageBrush();
                myBrush2 = new ImageBrush();
                myBrush.ImageSource = new BitmapImage(new Uri("/badgesk5.png", UriKind.Relative));
                _13.Background = myBrush;
                myBrush2.ImageSource = new BitmapImage(new Uri("/kh-sk5t.png", UriKind.Relative));
                _13text.Background = myBrush2;
            }
            #endregion


        }
        #endregion
    }
}