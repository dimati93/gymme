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
    }
}