using AdvertApi.Models;
using AdvertApi.Models.Constants;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;

namespace AdvertApi.Services;

public class DynamoDbAdvertStorage : IAdvertStorageService
{
	private readonly IMapper _mapper;

	public DynamoDbAdvertStorage(IMapper mapper)
	{
		_mapper = mapper;
	}

	public async Task<string> Add(AdvertModel model)
	{
		var dbModel = _mapper.Map<AdvertDbModel>(model);
		dbModel.Id = new Guid().ToString();
		dbModel.CreationDateTime = DateTime.UtcNow;
		dbModel.Status = AdvertStatus.Pending;
		using (var client = new AmazonDynamoDBClient())
		{
			using (var context = new DynamoDBContext(client))
			{
				await context.SaveAsync(dbModel);
			}
		}

		return dbModel.Id;
	}

	public Task<bool> Confirm(ConfirmAdvertModel model)
	{
		throw new NotImplementedException();
	}
}