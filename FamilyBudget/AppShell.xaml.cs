namespace FamilyBudget
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();


            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ExpensePage), typeof(ExpensePage));
            Routing.RegisterRoute(nameof(IncomePage), typeof(IncomePage));
            Routing.RegisterRoute(nameof(StatisticPage), typeof(StatisticPage));
            Routing.RegisterRoute(nameof(OperationPage), typeof(OperationPage));
        }
    }
}
