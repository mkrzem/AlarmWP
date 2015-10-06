using AlarmAttempt.Common;
using AlarmAttempt.Constants;
using AlarmAttempt.DAL;
using AlarmAttempt.Messages;
using AlarmAttempt.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using Windows.UI.Notifications;
using Windows.UI.Xaml.Controls;

namespace AlarmAttempt.ViewModel
{
    public class AlarmViewModel : ViewModelBase
    {
        private MediaElement mediaElement = new MediaElement();
        private Alarm newAlarm;
        private INavigationService navigationService;
        private bool viewValid = true;
        private StorageFile sound;
        private IReadOnlyList<StorageFile> sounds;
        private string pageTitle;

        public AlarmViewModel(INavigationService navigationService)
        {            
            this.navigationService = navigationService;
            Messenger.Default.Register<AlarmMessage>(this, Tokens.Edit, EditAlarm);
            Messenger.Default.Register<AlarmMessage>(this, Tokens.CreateNew, CreateNewAlarm);
            ReadSounds(); 
        }
        
        private async void ReadSounds()
        {            
            StorageFolder soundsFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
            AvailableSounds = await StorageManager.GetFiles(soundsFolder);
        }

        #region Properties
        public string PageTitle
        {
            get { return pageTitle; }
        }
        public Type RepetitionType
        {
            get
            {
                Type enumType = typeof(Enums.StartDays);
                return enumType;
            }
        }

        public Uri Background
        {
            get { return newAlarm.Background; }
            set
            {
                newAlarm.Background = value;
                RaisePropertyChanged(nameof(Background));
            }
        }
        public bool IsOn
        {
            get { return newAlarm.IsOn; }
            set
            {
                newAlarm.IsOn = value;
                RaisePropertyChanged(nameof(IsOn));
            }
        }

        public string Name
        {
            get { return newAlarm.Name; }
            set
            {
                if (value != newAlarm.Name)
                {
                    newAlarm.Name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
        }
        public StorageFile Sound
        {
            get
            {
                if (sound == null)
                {
                    //sound = sounds[0];
                    //newAlarm.Sound = new Uri(sound.Path);
                    //RaisePropertyChanged(nameof(Sound));
                }
                return sound;
            }
            set
            {
                sound = value;
                newAlarm.Sound = new Uri(sound.Path);
                RaisePropertyChanged(nameof(Sound));
            }
        }

        public Enums.StartDays Repetition
        {
            get { return newAlarm.Repetition; }
            set
            {
                newAlarm.Repetition = value;
                RaisePropertyChanged(nameof(Repetition));
            }
        }

        public string RepetitionDisplay
        {
            get { return "SelectedItems so much"; }
        }

        public TimeSpan Time
        {
            get { return newAlarm.Time; }
            set
            {
                newAlarm.Time = value;
                RaisePropertyChanged(nameof(Time));
            }
        }

        public int Volume
        {
            get { return newAlarm.Volume; }
            set
            {
                newAlarm.Volume = value;
                RaisePropertyChanged(nameof(Volume));
            }
        }

        public bool Nap
        {
            get { return newAlarm.Nap; }
            set
            {
                newAlarm.Nap = value;
                RaisePropertyChanged(nameof(Nap));
            }
        }

        public int NapDuration
        {
            get { return newAlarm.NapDuration; }
            set
            {
                newAlarm.NapDuration = value;
                RaisePropertyChanged(nameof(NapDuration));
            }
        }

        public IReadOnlyList<StorageFile> AvailableSounds
        {
            get
            {
                return sounds;
            }
            private set
            {
                sounds = value;
                RaisePropertyChanged(nameof(AvailableSounds));
            }
        }
        #endregion

        #region Commands                
        public ICommand SaveAlarm
        {
            get
            {
                return new RelayCommand((parameter) => Save(), () => viewValid);
            }
        }

        public ICommand DeleteAlarm
        {
            get
            {
                return new RelayCommand((parameter) => Delete());
            }
        }
        #endregion

        #region Command Helper Methods
        private void Save()
        {
            
            Messenger.Default.Send(new AlarmMessage() { Alarm = newAlarm }, Tokens.Save);
            navigationService.GoBack();
        }

        private void Delete()
        {
            Messenger.Default.Send(new AlarmMessage() { Alarm = newAlarm }, Tokens.Delete);
            navigationService.GoBack();
        }
        #endregion
        #region Private Methods
        private void CreateNewAlarm(AlarmMessage message)
        {
            pageTitle = "Nowy";
            newAlarm = new Alarm()
            {
                IsOn = true,
                Repetition = Enums.StartDays.Monday,
                Nap = false
            };
        }
        private void EditAlarm(AlarmMessage message)
        {
            pageTitle = "Edytuj";
            newAlarm = message.Alarm;
        }
        #endregion
    }
}
