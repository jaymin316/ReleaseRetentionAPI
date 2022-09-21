namespace ReleaseRetentionWebAPI.Models
{
	public class InputDTO
	{
		public Project Project { get; set; } = null!;
		public IEnumerable<Environment> Environments { get; set; } = null!;
		public IEnumerable<Release> Releases { get; set; } = null!;
		public IEnumerable<Deployment> Deployments { get; set; } = null!;

	}
}
