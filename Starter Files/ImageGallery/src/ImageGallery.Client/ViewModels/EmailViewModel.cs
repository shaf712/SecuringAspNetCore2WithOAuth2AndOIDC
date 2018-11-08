using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageGallery.Client.ViewModels
{
	public class EmailViewModel
	{
		public string Email { get; private set; } = String.Empty;
		public EmailViewModel(string email)
		{
			Email = email; 
		}
	}
}
