using System;

namespace DANFS.Services
{
	public interface IFolderProvider
	{
		string DateDatabasePath { get; }
		string MapDatabasePath {get;}
	}
}

