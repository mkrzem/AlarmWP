using AlarmAttempt.Common;
using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AlarmAttempt.CustomUI
{
    public sealed partial class DaysMultiSelectBox : UserControl, INotifyPropertyChanged
    {
        private ObservableCollection<Day> items = new ObservableCollection<Day>();
        private HashSet<Day> selectedItems = new HashSet<Day>();
        private CommandBar viewOriginalCommandBar;
        private bool choiceAccepted;

        public event PropertyChangedEventHandler PropertyChanged;
        public DaysMultiSelectBox()
        {
            InitializeComponent();
            InitializeItems();
            SetDisplayText("Tylko raz");
        }

        private void InitializeItems()
        {
            items.Add(new Day() { Name = "Poniedziałek", Abbreviation = "Pn" });
            items.Add(new Day() { Name = "Wtorek", Abbreviation = "Wt" });
            items.Add(new Day() { Name = "Środa", Abbreviation = "Śr" });
            items.Add(new Day() { Name = "Czwartek", Abbreviation = "Czw" });
            items.Add(new Day() { Name = "Piątek", Abbreviation = "Pt" });
            items.Add(new Day() { Name = "Sobota", Abbreviation = "Sb" });
            items.Add(new Day() { Name = "Niedziela", Abbreviation = "Nd" });
        }

        #region Dependency Properties


        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Header.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(DaysMultiSelectBox), new PropertyMetadata(0));        

        #endregion

        #region Properties
        public ObservableCollection<Day> Items
        {
            get { return items; }
            set
            {
                items = value;
            }
        }
        #endregion

        #region Events
        private void Flyout_Opening(object sender, object e)
        {
            SwitchOrAddCommandBar();
            choiceAccepted = false;
        }       

        private void Flyout_Closed(object sender, object e)
        {
            ((Window.Current.Content as Frame).Content as Page).BottomAppBar = viewOriginalCommandBar;

            if (!choiceAccepted)
            {
                BackupCurrentItems();
                RaisePropertyChanged(nameof(Items));                
            }
        }
        #endregion

        #region Commands
        public ICommand AcceptChoice
        {
            get
            {
                return new RelayCommand((parameter) => Accept());
            }
        }
       
        public ICommand CancelChoice
        {
            get
            {
                return new RelayCommand((parameter) => Cancel());
            }
        }
        #endregion

        #region Private Methods
        private void BackupCurrentItems()
        {
            foreach (Day day in items)
            {
                if (!selectedItems.Contains(day))
                {
                    day.IsSelected = false;
                }
            }
        }

        private void SwitchOrAddCommandBar()
        {
            var page = (Window.Current.Content as Frame).Content as Page;
            viewOriginalCommandBar = page.BottomAppBar as CommandBar;

            var flyoutCommandBar = new CommandBar();
            flyoutCommandBar.PrimaryCommands.Add(new AppBarButton() { Icon = new SymbolIcon(Symbol.Accept), Label = "Gotowe", Command = AcceptChoice });
            flyoutCommandBar.PrimaryCommands.Add(new AppBarButton() { Icon = new SymbolIcon(Symbol.Cancel), Label = "Anuluj", Command = CancelChoice });
            ((Window.Current.Content as Frame).Content as Page).BottomAppBar = flyoutCommandBar;
        }
        #endregion

        #region Command Helper Methods
        private void Accept()
        {
            string displayText = "";
            choiceAccepted = true;
            selectedItems.Clear();

            foreach (Day day in items)
            {
                if (day.IsSelected)
                {
                    selectedItems.Add(day);
                    displayText += day.Abbreviation + ",";
                }
            }

            BackupCurrentItems();            
            SetDisplayText(displayText.TrimEnd(new char[] { ',' }));
            multiChoiceBox.Flyout.Hide();
        }
        
        private void Cancel()
        {
            multiChoiceBox.Flyout.Hide();
        }

        private void SetDisplayText(string displayText)
        {
            if ("Pn,Wt,Śr,Czw,Pt,Sb,Nd".Equals(displayText))
            {
                displayText = "Codziennie";
            }
            else if ("Pn,Wt,Śr,Czw,Pt".Equals(displayText))
            {
                displayText = "Dni robocze";
            }
            else if ("Sb,Nd".Equals(displayText))
            {
                displayText = "Weekend";
            }

            multiChoiceBox.Content = displayText;
        }
        #endregion

        #region INotifyPropertyChanged
        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public sealed class Day : ObservableObject
        {
            private bool isSelected;
            private string name;
            private string abbreviation;

            public Day()
            {

            }

            public bool IsSelected
            {
                get { return isSelected; }
                set
                {
                    isSelected = value;
                    RaisePropertyChanged(nameof(IsSelected));
                }
            }
            public string Name
            {
                get { return name; }
                set
                {
                    name = value;
                    RaisePropertyChanged(nameof(Name));
                }
            }
            public string Abbreviation
            {
                get { return abbreviation; }
                set
                {
                    abbreviation = value;
                    RaisePropertyChanged(nameof(Abbreviation));
                }
            }
        }        
        
    }
}
