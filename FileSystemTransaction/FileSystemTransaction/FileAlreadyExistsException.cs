using System;
using System.IO;

namespace FileSystemTransaction
{
	public class FileAlreadyExistsException : IOException
	{
		
		public FileAlreadyExistsException () : base()
		{}
		
		public FileAlreadyExistsException (string message) : base(message)
		{}
		
		public FileAlreadyExistsException (string message, Exception innerException) : base (message, innerException)
		{}
		
	}
}

