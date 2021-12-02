using System;
namespace Mandrilkalender
{
	public interface IToastService 
	{
		void ShowToast(string message, int duration);
	}
}
