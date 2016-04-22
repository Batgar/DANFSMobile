using System;
using SQLite.Net.Interop;
using DANFS.Services;

namespace DANFS.Shared
{
	public class Bootstrapper
	{
		public static void Startup(ISQLitePlatform platform,
			IFolderProvider folderProvider)
		{
			TinyIoC.TinyIoCContainer.Current.Register<IFolderProvider> (folderProvider);
			TinyIoC.TinyIoCContainer.Current.Register<ISQLitePlatform> (platform);
			TinyIoC.TinyIoCContainer.Current.Register<IDataAccess> (new DataAccess.DataAccess ());
		}
	}
}

