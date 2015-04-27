using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Scheduler;
using System.IO;
using System.IO.IsolatedStorage;

namespace ShaumQuest
{
    public partial class JadwalAlarm : PhoneApplicationPage
    {
        IEnumerable<ScheduledNotification> notifications;
        String[] jumlahPuasa = new String[10];
        String valueToStore;

        public JadwalAlarm()
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

            #region WRITE THE DATA
            PuasaSK.Content = "Kamu sudah " + (Convert.ToInt32(jumlahPuasa[0]) + Convert.ToInt32(jumlahPuasa[1])) + " kali shaum Senin - Kamis.";
            PuasaAB.Content = "Kamu sudah " + jumlahPuasa[2] +" kali shaum Ayyamul Bidh.";
            PuasaRMD.Content = "Kamu sudah " + jumlahPuasa[3] + " kali shaum Ramadhan.";
            PuasaAR.Content = "Kamu sudah " + jumlahPuasa[4] + " kali shaum Arafah.";
            PuasaMHR.Content = "Kamu sudah " + (Convert.ToInt32(jumlahPuasa[5]) + Convert.ToInt32(jumlahPuasa[6])) + " kali shaum Muharram.";
            #endregion
        }
        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
            //Reset the ReminderListBox items when the page is navigated to.
            ResetItemsList();
        }

        private void ResetItemsList()
        {
           
            notifications = ScheduledActionService.GetActions<ScheduledNotification>();

            if (notifications.Count<ScheduledNotification>() > 0)
            {
                EmptyTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                EmptyTextBlock.Visibility = Visibility.Visible;
            }


            // Update the ReminderListBox with the list of reminders.
            // A full MVVM implementation can automate this step.
            NotificationListBox.ItemsSource = notifications;
        }
        
        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            // The scheduled action name is stored in the Tag property
            // of the delete button for each reminder.
            string name = (string)((Button)sender).Tag;

            try
            {
                if (name.Substring(0, 11).Equals("LailaCantik"))
                    ScheduledActionService.Remove("WOW" + name);
            }
            catch (Exception exc)
            {
                       //do whatever you like dah
            }
            // Call Remove to unregister the scheduled action with the service.
            ScheduledActionService.Remove(name);

            // Reset the ReminderListBox items
            ResetItemsList();
        }

        private void time_loaded(object sender, RoutedEventArgs e)
        {
         

        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Kamu yakin akan menghapus semua data?", "Hapus Data", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                // Do this
                IsolatedStorageFile isf = IsolatedStorageFile.GetUserStoreForApplication();
                isf.DeleteFile("DataShaum.txt");
                while (isf.FileExists("DataShaum.txt"))
                { //do nothing 
                }
                StreamWriter Writer = new StreamWriter(new IsolatedStorageFileStream("DataShaum.txt", FileMode.OpenOrCreate, isf));
                valueToStore = "";
                for (int i = 0; i < 8; i++)
                {
                    if (i != 7)
                        valueToStore = valueToStore + 0 + "#";
                    else
                        valueToStore = valueToStore + "hihi" + "#";
                }
                Writer.WriteLine(valueToStore);
                Writer.Close();

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

                #region WRITE THE DATA
                PuasaSK.Content = "Kamu sudah " + (Convert.ToInt32(jumlahPuasa[0]) + Convert.ToInt32(jumlahPuasa[1])) + " kali shaum Senin - Kamis.";
                PuasaAB.Content = "Kamu sudah " + jumlahPuasa[2] + " kali shaum Ayyamul Bidh.";
                PuasaRMD.Content = "Kamu sudah " + jumlahPuasa[3] + " kali shaum Ramadhan.";
                PuasaAR.Content = "Kamu sudah " + jumlahPuasa[4] + " kali shaum Arafah.";
                PuasaMHR.Content = "Kamu sudah " + (Convert.ToInt32(jumlahPuasa[5]) + Convert.ToInt32(jumlahPuasa[6])) + " kali shaum Muharram.";
                #endregion
            }
        }

    }
}