using AlarmAttempt.Common;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AlarmAttempt.CustomUI
{
    public sealed partial class DaysMultiSelectBox : UserControl
    {
        private ObservableCollection<Day> items = new ObservableCollection<Day>();
        private ObservableCollection<Day> selectedItems = new ObservableCollection<Day>();
        private CommandBar viewOriginalCommandBar;
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
            var page = (Window.Current.Content as Frame).Content as Page;
            viewOriginalCommandBar = page.BottomAppBar as CommandBar;

            var flyoutCommandBar = new CommandBar();
            flyoutCommandBar.PrimaryCommands.Add(new AppBarButton() { Icon = new SymbolIcon(Symbol.Accept), Label = "Gotowe", Command = AcceptChoice });
            flyoutCommandBar.PrimaryCommands.Add(new AppBarButton() { Icon = new SymbolIcon(Symbol.Cancel), Label = "Anuluj" });
            ((Window.Current.Content as Frame).Content as Page).BottomAppBar = flyoutCommandBar;
        }

        private void Flyout_Closed(object sender, object e)
        {
            ((Window.Current.Content as Frame).Content as Page).BottomAppBar = viewOriginalCommandBar;
        }
        #endregion

        #region Commands
        public ICommand AcceptChoice
        {
            get
            {
                return new RelayCommand((parameter) =>
                {
                    string displayText = "";
                    foreach (Day day in items)
                    {
                        if (day.IsSelected)
                        {
                            selectedItems.Add(day);
                            displayText += day.Abbreviation + ",";
                        }
                    }

                    SetDisplayText(displayText.TrimEnd(new char[] { ',' }));
                    multiChoiceBox.Flyout.Hide();
                });
            }
        }
        #endregion

        #region Helper Methods
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
