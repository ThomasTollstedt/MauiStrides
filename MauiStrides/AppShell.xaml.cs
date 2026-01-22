namespace MauiStrides
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("ActivityDetailsPage", typeof(Views.ActivityDetailsPage));
        }
    }
}
