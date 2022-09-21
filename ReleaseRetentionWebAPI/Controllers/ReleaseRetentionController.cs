using Microsoft.AspNetCore.Mvc;
using ReleaseRetentionWebAPI.Models;

namespace ReleaseRetentionWebAPI.Controllers
{
	[Route("api/releaseretention")]
	[ApiController]
	public class ReleaseRetentionController : ControllerBase
	{
		[HttpGet("{retentionsToKeep}")]
		public IActionResult Get(int retentionsToKeep, [FromBody] InputDTO input)
		{
			try
			{
				var releases = input.Releases;
				var deployments = input.Deployments;
				var environments = input.Environments;

				// Join the input data and to get a list of deployments that have been released on environments
				var deployedReleases = from d in deployments
									   join r in releases
										   on d.ReleaseId equals r.Id into releasesTmp
									   from rel in releasesTmp.DefaultIfEmpty()
									   join e in environments
										   on d.EnvironmentId equals e.Id into environmentsTmp
									   from env in environmentsTmp.DefaultIfEmpty()
									   select new
									   {
										   d.ReleaseId,
										   d.EnvironmentId,
										   d.DeployedAt,
										   rel.Created
									   };

				// Order (desc) the list by their Deployed Date
				var sortedDeployedReleases = deployedReleases.OrderByDescending(x => x.DeployedAt).ToList();

				// Empty collection to hold the output result
				var output = new List<OutputDTO>();
				foreach (var env in environments)
				{
					// Take required number of retentions per Environment as supplied in input
					var tmp = from t in sortedDeployedReleases
							  where t.EnvironmentId == env.Id
							  select new OutputDTO
							  {
								  ReleaseId = t.ReleaseId,
								  EnvironmentId = t.EnvironmentId
							  };
					output.AddRange(tmp.ToList().Take(retentionsToKeep));
				}

				// Generate the return message
				string msg = GenerateDisplayMessage(output);

				return Ok(msg);
			}
			catch (Exception)
			{
				return NoContent();
			}
		}

		public string GenerateDisplayMessage(List<OutputDTO> output)
		{
			var msg = "";
			foreach (var o in output)
			{
				msg += ("`" + o.ReleaseId + "` kept because it was the most recently deployed to `" + o.EnvironmentId + "` \n");
			}

			return msg;
		}
	}
}
