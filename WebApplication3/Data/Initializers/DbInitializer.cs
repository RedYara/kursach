namespace WebApplication3.Data.Initializers
{
    public partial class DbIdentityInitializer
    {
        public static void Initialize(IServiceScope serviceScope)
        {
            try
            {
                var context = serviceScope.ServiceProvider.GetService<_dbContext>();
                RolesInitialize(context);
            }
            catch (Exception E)
            {
            }
        }
    }
}
