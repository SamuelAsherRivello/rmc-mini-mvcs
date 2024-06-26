﻿using RMC.Mini.Controller.Commands;

namespace RMC.Mini.Samples.BouncyBall.WithMini.Mini.Controller.Commands
{
    
	/// <summary>
	/// The Command is a stand-alone object containing
	/// all arguments needed to perform a request
	/// </summary>
	public class PlayAudioClipCommand : ICommand
	{
		public string AudioClipName { get { return _audioClipName; } }
		private readonly string _audioClipName;

		public PlayAudioClipCommand (string audioClipName)
		{
			_audioClipName = audioClipName;
		}
	}
}

