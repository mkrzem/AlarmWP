using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BackgroundTasks
{
    public sealed class AlarmTestTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {            
            XmlDocument document = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText01);

            XmlNodeList toastElements = document.GetElementsByTagName("text");
            toastElements[0].AppendChild(document.CreateTextNode("Akuku"));
            ToastNotification toast = new ToastNotification(document);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
