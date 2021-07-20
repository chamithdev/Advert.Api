using AdvertApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;

namespace Advert.Api.Services
{
    public class DynamoDbAdvertStorageService : IAdvertStorageService
    {
        private readonly IMapper mapper;

        public DynamoDbAdvertStorageService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<string> Add(AdvertModel model)
        {
            AdvertDbModel dbModel = mapper.Map<AdvertDbModel>(model);
            dbModel.Id = Guid.NewGuid().ToString();
            dbModel.CreationDateTime = DateTime.UtcNow;
            dbModel.Status = AdvertStatus.Pending;

            using (AmazonDynamoDBClient client = new AmazonDynamoDBClient())
            {
                using (DynamoDBContext context = new DynamoDBContext(client))
                {
                    await context.SaveAsync(dbModel);
                }
            }

            return dbModel.Id;

        }

        public async Task<bool> Confirm(ConfirmAdvertModel model)
        {
            using (AmazonDynamoDBClient client = new AmazonDynamoDBClient())
            {
               
                using (DynamoDBContext context = new DynamoDBContext(client))
                {
                    AdvertDbModel rec = await context.LoadAsync<AdvertDbModel>(model.Id);
                    if(rec == null)
                    {
                        throw new KeyNotFoundException($"Record with id = {model.Id}");
                    }
                    if(model.Status == AdvertStatus.Active)
                    {
                        rec.Status = AdvertStatus.Active;
                        await context.SaveAsync(rec);
                    }
                    else
                    {
                        await context.DeleteAsync(rec);
                    }
                   
                }
            }

            return true;
        }
    }
}
