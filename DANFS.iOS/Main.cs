using UIKit;
using Xamarin.Auth;
using System.Linq;

namespace DANFS.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main (string[] args)
		{

			UnpackStoredUserCredentials();

			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main (args, null, "AppDelegate");
		}

		private static void PackStoredUserCredentials()
		{
			//if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
			{
				Account account = new Account
				{
					Username = "TestUsername3"
				};

account.Properties.Add("Password", "TestPassword3");

				AccountStore.Create().Save(account, "MegaDude");


		  	}
		}

		private static void UnpackStoredUserCredentials()
		{



			//Now that we have saved, see if we can retrieve it!

			CommonCredentialStore.StoredUsername = Application.UserName;
			CommonCredentialStore.StoredPassword = Application.Password;
		}

		public static string UserName
		{
			get
			{
				var account = AccountStore.Create().FindAccountsForService("MegaDude").LastOrDefault();
				return (account != null) ? account.Username : null;
			}
		}

		public static string Password
		{
			get
			{
				var account = AccountStore.Create().FindAccountsForService("MegaDude").LastOrDefault();
				return (account != null) ? account.Properties["Password"] : null;
			}
		}
	}

	public class CommonCredentialStore
	{	
		public static string StoredUsername { get; set; }
		public static string StoredPassword { get; set; }
	}

}
