using AlarmAttempt.Common;
using AlarmAttempt.Messages;
using AlarmAttempt.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System.Collections.ObjectModel;
using Windows.Storage;
using System;
using Newtonsoft.Json;
using AlarmAttempt.DAL;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using AlarmAttempt.Constants;

namespace AlarmAttempt.ViewModel
{
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private ObservableCollection<Alarm> alarms;        
        public MainPageViewModel(INavigationService navigationService)
        {
            ReadAlarmsAsync();            
            this.navigationService = navigationService;
            Messenger.Default.Register<AlarmMessage>(this , Tokens.Save, (message) => SaveAlarm(message));
            Messenger.Default.Register<AlarmMessage>(this, Tokens.Delete, (message) => DeleteAlarm(message));
        }

        public ObservableCollection<Alarm> Alarms
        {
            get
            {
                return alarms;
            }
            private set
            {
                if (alarms != value)
                {
                    alarms = value;
                    RaisePropertyChanged(nameof(Alarms));
                }
            }
        }

        #region Private Methods

        private async void ReadAlarmsAsync()
        {
            Alarms = await StorageManager.ReadFromFile<ObservableCollection<Alarm>>("alarms.json", ApplicationData.Current.LocalCacheFolder);            
        }
        #endregion

        #region Commands
        public RelayCommand EditAlarm
        {
            get
            {
                return new RelayCommand((parameter) =>
                {                    
                    navigationService.NavigateTo(Pages.AlarmDetails);
                    Messenger.Default.Send(new AlarmMessage() { Alarm = (parameter as ItemClickEventArgs).ClickedItem as Alarm }, Tokens.Edit);
                });
            }
        }
        public RelayCommand NewAlarm
        {
            get
            {
                return new RelayCommand((parameter) =>
                {
                    navigationService.NavigateTo(Pages.AlarmDetails);
                });
            }
        }
        #endregion

        #region Command Helper Methods
        private async void SaveAlarm(AlarmMessage message)
        {
            if (!Alarms.Contains(message.Alarm))
            {                
                Alarms.Add(message.Alarm);
            }
            
            await StorageManager.WriteToFile(Alarms, "alarms.json", ApplicationData.Current.LocalCacheFolder).ConfigureAwait(false);            
        }

        private async void DeleteAlarm(AlarmMessage message)
        {
            Alarms.Remove(message.Alarm);
            await StorageManager.WriteToFile(Alarms, "alarms.json", ApplicationData.Current.LocalCacheFolder);
        }
        #endregion

    }    
}
