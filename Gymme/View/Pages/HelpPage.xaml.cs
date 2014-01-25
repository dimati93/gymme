using System;
using System.Reflection;
using Gymme.Resources;

using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace Gymme.View.Pages
{
    public partial class HelpPage : PhoneApplicationPage
    {
        public HelpPage()
        {
            InitializeComponent();
            VersionBlock.Text = string.Format("{0} {1}", AppResources.About_Version, GetAppVersion());
        }

        private Version GetAppVersion()
        {
            var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
            return nameHelper.Version;
        }

        private void Email_To(object sender, GestureEventArgs e)
        {
            EmailComposeTask emailTask = new EmailComposeTask
            {
                Subject = AppResources.About_ReportSubject,
                To = AppResources.About_Email
            };
            emailTask.Show();
        }

        private void Goto_Facebook(object sender, GestureEventArgs e)
        {
            LaunchBrouser(AppResources.About_Facebook);
        }

        private void Goto_Vk(object sender, GestureEventArgs e)
        {
            LaunchBrouser(AppResources.About_Vk);
        }

        private void LaunchBrouser(string link)
        {
            WebBrowserTask browse = new WebBrowserTask {Uri = new Uri("http://" + link, UriKind.Absolute)};
            browse.Show();
        }
    }
}