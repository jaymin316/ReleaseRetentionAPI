namespace ReleaseRetentionWebAPI.Models
{
	public class Release
	{
		public string Id { get; set; } = null!;
		public string ProjectId { get; set; } = null!;
		public string Version { get; set; } = null!;
		public DateTime Created { get; set; }
	}
}
