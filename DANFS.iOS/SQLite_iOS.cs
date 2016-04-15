using System;
using System.Runtime.CompilerServices;
using SQLite.Net.Interop;


[assembly: Dependency (typeof (SQLite_iOS))]

public class SQLite_iOS : ISQLitePlatform {
	public SQLite_iOS () {}

	#region ISQLitePlatform implementation


	public ISQLiteApi SQLiteApi {
		get {
			throw new NotImplementedException ();
		}
	}


	public IStopwatchFactory StopwatchFactory {
		get {
			throw new NotImplementedException ();
		}
	}


	public IReflectionService ReflectionService {
		get {
			throw new NotImplementedException ();
		}
	}


	public IVolatileService VolatileService {
		get {
			throw new NotImplementedException ();
		}
	}
}

