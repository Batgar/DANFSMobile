using System;
using DANFS.Services;
using DANFS.Shared;
using Java.IO;
using Android.Content;

namespace DANFS.Android
{
	public class AppBootstrapper : IFolderProvider
	{
		public static void Initialize(Context context)
		{
			if (!_initialized) {
				CopySQLiteDatabaseFromAssetsToData (context);

				Bootstrapper.Startup (
					new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid (),
					AppBootstrapper.Instance ());
			}
			_initialized = true;
		}

		private static string GetDBDirectory(Context context)
		{
			return string.Format ("/data/data/{0}/databases/", context.ApplicationContext.ApplicationInfo.PackageName);
		}

		private static String GetDBPath(Context context) {
			if (string.IsNullOrEmpty(DBPath)) {
				
				DBPath = System.IO.Path.Combine(
					GetDBDirectory(context), 
					"shiplocations.sqlite");
			}
			return DBPath;
		}

		private static string DBPath {get; set;}

		private static void CopySQLiteDatabaseFromAssetsToData(Context context)
		{
			// Path to the just created empty db
			String outFileName = GetDBPath(context);

			if (System.IO.File.Exists (outFileName))
				return;

			if (!System.IO.Directory.Exists (GetDBDirectory (context))) {
				System.IO.Directory.CreateDirectory (GetDBDirectory (context));
			}

			//Open your local db as the input stream
			//Open the empty db as the output stream
			using (System.IO.Stream myInput = context.Assets.Open ("shiplocations.sqlite"))
			using (FileOutputStream myOutput = new FileOutputStream (outFileName)) {
				//transfer bytes from the inputfile to the outputfile
				byte[] buffer = new byte[1024];
				int length;
				while ((length = myInput.Read (buffer,0,1024)) > 0) {
					myOutput.Write (buffer, 0, length);
				}
				myOutput.Flush ();
			}

			//Close the streams
			/*myOutput.Flush();
			myOutput.Close();
			myInput.Close();*/
		}

		private static bool _initialized;

		#region IFolderProvider implementation

		public string MapDatabasePath {
			get {
				return DBPath;
			}
		}

		public string DateDatabasePath
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string MasterDatabasePath
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		private static AppBootstrapper Instance()
		{
			if (_instance == null) {
				_instance = new AppBootstrapper ();
			}
			return _instance;
		}

		static AppBootstrapper _instance;
	}
}

