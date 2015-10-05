﻿using System;

namespace ClassicalSharp {

	public sealed class Events {
		
		/// <summary> Raised when an entity is spawned in the current world. </summary>
		public event EventHandler<IdEventArgs> EntityAdded;
		internal void RaiseEntityAdded( byte id ) { idArgs.Id = id; Raise( EntityAdded, idArgs ); }
		
		/// <summary> Raised when an entity is despawned from the current world. </summary>
		public event EventHandler<IdEventArgs> EntityRemoved;
		internal void RaiseEntityRemoved( byte id ) { idArgs.Id = id; Raise( EntityRemoved, idArgs ); }
		
		/// <summary> Raised when a new CPE player list entry is created. </summary>
		public event EventHandler<IdEventArgs> CpeListInfoAdded;
		internal void RaiseCpeListInfoAdded( byte id ) { idArgs.Id = id; Raise( CpeListInfoAdded, idArgs ); }
		
		/// <summary> Raised when a CPE player list entry is modified. </summary>
		public event EventHandler<IdEventArgs> CpeListInfoChanged;
		internal void RaiseCpeListInfoChanged( byte id ) { idArgs.Id = id; Raise( CpeListInfoChanged, idArgs ); }
		
		/// <summary> Raised when a CPE player list entry is removed. </summary>
		public event EventHandler<IdEventArgs> CpeListInfoRemoved;
		internal void RaiseCpeListInfoRemoved( byte id ) { idArgs.Id = id; Raise( CpeListInfoRemoved, idArgs ); }
		
		
		/// <summary> Raised when the client joins and begins loading a new map. </summary>
		public event EventHandler OnNewMap;
		internal void RaiseOnNewMap() { Raise( OnNewMap ); }
		
		/// <summary> Raised when a portion of the map is read and decompressed by the client. </summary>
		public event EventHandler<MapLoadingEventArgs> MapLoading;
		internal void RaiseMapLoading( byte progress ) { loadingArgs.Progress = progress; Raise( MapLoading, loadingArgs ); }
		
		/// <summary> Raised when the client has finished loading a new map and can now interact with it. </summary>
		public event EventHandler OnNewMapLoaded;
		internal void RaiseOnNewMapLoaded() { Raise( OnNewMapLoaded ); }
		
		
		/// <summary> Raised when the terrain atlas ("terrain.png") is changed. </summary>
		public event EventHandler TerrainAtlasChanged;
		internal void RaiseTerrainAtlasChanged() { Raise( TerrainAtlasChanged );  }
		
		/// <summary> Raised when an environment variable is changed by the user, CPE, or WoM config. </summary>
		public event EventHandler<EnvVariableEventArgs> EnvVariableChanged;
		internal void RaiseEnvVariableChanged( EnvVariable envVar ) { envArgs.Var = envVar; Raise( EnvVariableChanged, envArgs ); }
		
		/// <summary> Raised when the user changed their view/fog distance. </summary>
		public event EventHandler ViewDistanceChanged;
		internal void RaiseViewDistanceChanged() { Raise( ViewDistanceChanged );  }
		
		/// <summary> Raised when the held block is changed by the user or by CPE. </summary>
		public event EventHandler HeldBlockChanged;
		internal void RaiseHeldBlockChanged() { Raise( HeldBlockChanged ); }
		
		/// <summary> Raised when the client's block permissions(can place or delete a block) change. </summary>
		public event EventHandler BlockPermissionsChanged;
		internal void RaiseBlockPermissionsChanged() { Raise( BlockPermissionsChanged ); }
		
		/// <summary> Raised when the a block definition is changed. </summary>
		public event EventHandler BlockDefinitionChanged;
		internal void RaiseBlockDefinitionChanged() { Raise( BlockDefinitionChanged ); }
		
		/// <summary> Raised when the server or a client-side command sends a message. </summary>
		public event EventHandler<ChatEventArgs> ChatReceived;	
		internal void RaiseChatReceived( string text, CpeMessage type ) { chatArgs.Type = type; chatArgs.Text = text; Raise( ChatReceived, chatArgs ); }
		
		
	
		// Cache event instances so we don't create needless new objects.
		IdEventArgs idArgs = new IdEventArgs();
		MapLoadingEventArgs loadingArgs = new MapLoadingEventArgs();	
		EnvVariableEventArgs envArgs = new EnvVariableEventArgs();
		ChatEventArgs chatArgs = new ChatEventArgs();
				
		void Raise( EventHandler handler ) {
			if( handler != null ) {
				handler( this, EventArgs.Empty );
			}
		}
		
		void Raise<T>( EventHandler<T> handler, T args ) where T : EventArgs {
			if( handler != null ) {
				handler( this, args );
			}
		}
	}
	
	public sealed class IdEventArgs : EventArgs {
		
		public byte Id;
	}
	
	public sealed class ChatEventArgs : EventArgs {
		
		public CpeMessage Type;
		
		public string Text;
	}
	
	public sealed class MapLoadingEventArgs : EventArgs {
		
		public int Progress;
	}
	
	public sealed class EnvVariableEventArgs : EventArgs {
		
		public EnvVariable Var;
	}
	
	public enum EnvVariable {
		SidesBlock,
		EdgeBlock,
		WaterLevel,
		Weather,

		SkyColour,
		CloudsColour,
		FogColour,
		SunlightColour,
		ShadowlightColour,	
	}
	
	public enum Weather {
		Sunny,
		Rainy,
		Snowy,
	}
}
