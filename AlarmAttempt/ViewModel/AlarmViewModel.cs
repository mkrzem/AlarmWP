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
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;

namespace AlarmAttempt.ViewModel
{
    public class AlarmViewModel : ViewModelBase
    {        
        private Alarm newAlarm;
        private INavigationService navigationService;
        private StorageFile sound;
        private IReadOnlyList<StorageFile> sounds;
        private string pageTitle;

        public AlarmViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            RegisterMessages();
            ReadSounds();
        }

        private void RegisterMessages()
        {
            Messenger.Default.Register<AlarmMessage>(this, Tokens.Edit, EditAlarm);
            Messenger.Default.Register<AlarmMessage>(this, Tokens.CreateNew, CreateNewAlarm);
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
                    var oldValue = newAlarm.Name;   
                    newAlarm.Name = value;
                    RaisePropertyChanged(nameof(Name));
                    Broadcast(oldValue, newAlarm.Name, nameof(Name));
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
                Set(ref sound, value, broadcast: true);
                newAlarm.Sound = new Uri(sound.Path);
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
                var command = new AutoRelayCommand((param) => Save());
                command.DependsOn(() => Name);
                command.DependsOn(() => Sound);
                return command;
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
            if (!Validate())
            {
                var dialogService = new DialogService();
                dialogService.ShowMessage("Wysztkie wartości alarmu muszą być wypełnione", "Dane Alarmu");
                return;
            }            
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
        private bool Validate()
        {            
            return !(string.IsNullOrEmpty(newAlarm.Name) || sound == null);
        }
        private void CreateNewAlarm(AlarmMessage message)
        {
            pageTitle = "Nowy";
            newAlarm = new Alarm()
            {
                Name = "",
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
