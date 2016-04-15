using System;
using DANFS.Services;

namespace DANFS.Android
{
	public static class AppBootstrapper
	{
		public static void Initialize()
		{
			IDataAccess dataAccess;
			if (!TinyIoC.TinyIoCContainer.Current.TryResolve<IDataAccess> (out dataAccess)) {
				TinyIoC.TinyIoCContainer.Current.Register<IDataAccess> (new DataAccess.DataAccess ());
			}
		}
	}
}

