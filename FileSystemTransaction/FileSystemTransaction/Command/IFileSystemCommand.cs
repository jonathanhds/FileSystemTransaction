using System;
namespace FileSystemTransaction
{
	public interface IFileSystemCommand
	{
		void Execute();
		
		void Rollback();
	}
}