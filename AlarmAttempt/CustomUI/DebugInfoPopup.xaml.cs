using System;
using System.Diagnostics;
using Windows.UI.Xaml;


// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace AlarmAttempt.CustomUI
{
    public sealed partial class DebugInfoPopup
    {
        public DebugInfoPopup()
        {
            InitializeComponent();
        }

        private readonly DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
        public void PopupControl_OnOpened(object sender, object e)
        {
            timer.Start();
            timer.Tick += UpdateMemoryInfo;                
        }

        private void UpdateMemoryInfo(object sender, object e)
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                // keep the popup open
                PopupControl.IsOpen = true;
            }
            catch (Exception ex)
            {
                // GC.Collect was throwing exceptions in some rare scenarios
                Debugger.Break();
            }
            
            long appMemoryUsage = (long)Windows.System.MemoryManager.AppMemoryUsage / 1024;
            MemoryBlock.Text = string.Format("Memory: {0:N} KB", appMemoryUsage);
        }
    }
}
