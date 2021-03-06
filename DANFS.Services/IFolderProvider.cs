﻿using System;
using System.IO;

namespace DANFS.Services
{
	public interface IFolderProvider
	{
		string MasterDatabasePath { get; }

		string DateDatabasePath { get; }
		string MapDatabasePath {get;}

		Stream UniqueLocationsFileStream { get; }
	}
}

