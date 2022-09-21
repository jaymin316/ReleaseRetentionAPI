using Microsoft.AspNetCore.Mvc;
using ReleaseRetentionWebAPI.Controllers;
using ReleaseRetentionWebAPI.Models;

namespace ReleaseRetentionAPI.Tests
{
	public class ReleaseRetentionControllerTests
	{
		readonly ReleaseRetentionController _controller;

		public ReleaseRetentionControllerTests()
		{
			_controller = new ReleaseRetentionController();
		}

		[Fact]
		public void OneEnv_OneReleasePerEnv_Valid()
		{
			//Arrange
			InputDTO input = new()
			{
				Project = GetProject(),
				Environments = GetEnvironments(),
				Releases = GetReleases(),
				Deployments = GetDeployments()
			};

			var expectedOutput = new List<OutputDTO>
			{
				new OutputDTO
				{
					ReleaseId = "Release-1",
					EnvironmentId = "Environment-1"
				}
			};
			var expectedOutputMsg = _controller.GenerateDisplayMessage(expectedOutput);

			//Act
			var result = _controller.Get(1, input);

			//Assert
			Assert.IsType<OkObjectResult>(result);
			var resultObject = (ObjectResult)result;
			Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(resultObject.Value, expectedOutputMsg);
		}

		[Fact]
		public void OneEnv_TwoReleasePerEnv_Valid()
		{
			//Arrange
			InputDTO input = new()
			{
				Project = GetProject(),
				Environments = GetEnvironments(),
				Releases = GetReleases(),
				Deployments = GetDeployments()
			};

			var expectedOutput = new List<OutputDTO>
			{
				new OutputDTO
				{
					ReleaseId = "Release-1",
					EnvironmentId = "Environment-1"
				},
				new OutputDTO
				{
					ReleaseId = "Release-2",
					EnvironmentId = "Environment-1"
				}
			};
			var expectedOutputMsg = _controller.GenerateDisplayMessage(expectedOutput);

			//Act
			var result = _controller.Get(2, input);

			//Assert
			Assert.IsType<OkObjectResult>(result);
			var resultObject = (ObjectResult)result;
			Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(resultObject.Value, expectedOutputMsg);
		}

		[Fact]
		public void TwoEnvs_OneReleasePerEnv_Valid()
		{
			//Arrange
			var envs = GetEnvironments().ToList();
			AddSecondEnvironment(envs);

			InputDTO input = new()
			{
				Project = GetProject(),
				Environments = envs.AsEnumerable(),
				Releases = GetReleases(),
				Deployments = GetDeployments()
			};

			var expectedOutput = new List<OutputDTO>
			{
				new OutputDTO
				{
					ReleaseId = "Release-1",
					EnvironmentId = "Environment-1"
				},
				new OutputDTO
				{
					ReleaseId = "Release-2",
					EnvironmentId = "Environment-2"
				}
			};
			var expectedOutputMsg = _controller.GenerateDisplayMessage(expectedOutput);

			//Act
			var result = _controller.Get(1, input);

			//Assert
			Assert.IsType<OkObjectResult>(result);
			var resultObject = (ObjectResult)result;
			Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(resultObject.Value, expectedOutputMsg);
		}

		[Fact]
		public void OneEnv_OneReleasePerEnvExpected_NoReleaseForEnvSuplied_InValid()
		{
			//Arrange
			InputDTO input = new()
			{
				Project = GetProject(),
				Environments = GetInvalidEnvironment(),
				Releases = GetReleases(),
				Deployments = GetDeployments()
			};

			var expectedOutput = new List<OutputDTO>
			{
				new OutputDTO
				{
					ReleaseId = "Release-1",
					EnvironmentId = "Environment-3"
				}
			};
			var expectedOutputMsg = _controller.GenerateDisplayMessage(expectedOutput);

			//Act
			var result = _controller.Get(2, input);

			//Assert
			Assert.IsType<OkObjectResult>(result);
			var resultObject = (ObjectResult)result;
			Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(resultObject.Value, expectedOutputMsg);
		}

		[Fact]
		public void OneEnv_TwoReleasePerEnv_IncorrectSortOrder_InValid()
		{
			//Arrange
			InputDTO input = new()
			{
				Project = GetProject(),
				Environments = GetEnvironments(),
				Releases = GetReleases(),
				Deployments = GetDeployments()
			};

			var expectedOutput = new List<OutputDTO>
			{
				new OutputDTO
				{
					ReleaseId = "Release-2",
					EnvironmentId = "Environment-1"
				},
				new OutputDTO
				{
					ReleaseId = "Release-1",
					EnvironmentId = "Environment-1"
				}
			};
			var expectedOutputMsg = _controller.GenerateDisplayMessage(expectedOutput);

			//Act
			var result = _controller.Get(2, input);

			//Assert
			Assert.IsType<OkObjectResult>(result);
			var resultObject = (ObjectResult)result;
			Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(resultObject.Value, expectedOutputMsg);
		}

		[Fact]
		public void OneEnv_NoReleaseDeployed_InValid()
		{
			//Arrange
			InputDTO input = new()
			{
				Project = GetProject(),
				Environments = GetEnvironments(),
				Releases = GetReleases(),
				Deployments = GetInvalidDeployments()
			};

			//Act
			var result = _controller.Get(1, input);

			//Assert
			Assert.IsType<NoContentResult>(result);
		}

		#region Get Test Data
		private static Project GetProject()
		{
			return new Project
			{
				Id = "Project-1",
				Name = "Random Quotes"
			};
		}

		private static IEnumerable<ReleaseRetentionWebAPI.Models.Environment> GetEnvironments()
		{
			return new List<ReleaseRetentionWebAPI.Models.Environment>
			{
				new ReleaseRetentionWebAPI.Models.Environment
				{
					Id = "Environment-1",
					Name = "Staging"
				}
			};
		}

		private static void AddSecondEnvironment(List<ReleaseRetentionWebAPI.Models.Environment> envs)
		{
			envs.Add(new ReleaseRetentionWebAPI.Models.Environment
			{
				Id = "Environment-2",
				Name = "Production"
			});
		}

		private static IEnumerable<ReleaseRetentionWebAPI.Models.Environment> GetInvalidEnvironment()
		{
			return new List<ReleaseRetentionWebAPI.Models.Environment>
			{
				new ReleaseRetentionWebAPI.Models.Environment
				{
					Id = "Environment-3",
					Name = "Testing"
				}
			};
		}

		private static IEnumerable<Release> GetReleases()
		{
			return new List<Release>
			{
				new Release
				{
					Id = "Release-1",
					ProjectId = "Project-1",
					Version = "1.0.0",
					Created = DateTime.UtcNow,
				},
				new Release
				{
					Id = "Release-2",
					ProjectId = "Project-1",
					Version = "1.0.1",
					Created = DateTime.UtcNow.AddHours(1),

				}
			};
		}

		private static IEnumerable<Deployment> GetDeployments()
		{
			return new List<Deployment>
			{
				new Deployment
				{
					Id = "Deployment-1",
					ReleaseId = "Release-1",
					EnvironmentId = "Environment-1",
					DeployedAt = DateTime.UtcNow.AddDays(2) // latest Deployment
				},
				new Deployment
				{
					Id = "Deployment-3",
					ReleaseId = "Release-2",
					EnvironmentId = "Environment-1",
					DeployedAt = DateTime.UtcNow
				},
				new Deployment
				{
					Id = "Deployment-2",
					ReleaseId = "Release-2",
					EnvironmentId = "Environment-2",
					DeployedAt = DateTime.UtcNow.AddDays(1) 
				}
			};
		}

		private static IEnumerable<Deployment> GetInvalidDeployments()
		{
			return new List<Deployment>
			{
				new Deployment
				{
					Id = "Deployment-4",
					ReleaseId = "Release-4",
					EnvironmentId = "Environment-1",
					DeployedAt = DateTime.UtcNow.AddDays(2) // latest Deployment
				},
				new Deployment
				{
					Id = "Deployment-5",
					ReleaseId = "Release-5",
					EnvironmentId = "Environment-1",
					DeployedAt = DateTime.UtcNow
				}
			};
		}
		#endregion
	}
}
