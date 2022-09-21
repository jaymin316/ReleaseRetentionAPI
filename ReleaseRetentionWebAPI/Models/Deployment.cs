namespace ReleaseRetentionWebAPI.Models
{
	public class Deployment
	{
		public string Id { get; set; } = null!;
		public string? ReleaseId { get; set; }
		public string EnvironmentId { get; set; } = null!;
		public DateTime DeployedAt { get; set; }

	}
}
