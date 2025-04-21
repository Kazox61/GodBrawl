using Godot;

namespace GodBrawl.Config;

[GlobalClass]
public partial class StartConfig : Resource {
	[Export] public bool IsServer;
	
	[Export] public string Ip = "127.0.0.1";
	[Export] public int Port = 12345;
	
	[Export] public int BrawlerId;
}