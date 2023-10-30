using Amazon.Lambda.Core;
using Amazon.DynamoDBv2.Model;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using Amazon.Lambda.Serialization.SystemTextJson;
using lambda_cadastro_cliente_models;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace lambda_cadastro_cliente;

public class Function
{
    private readonly static AmazonDynamoDBClient client = new AmazonDynamoDBClient();
    
    public static async Task<dynamic> CadastroCliente(Cliente json, ILambdaContext lambdaContext)
    {
        try
        {
            using (DynamoDBContext context = new DynamoDBContext(client))
            {
                var item = PutItem(JsonConvert.SerializeObject(json));
                var resp = await client.PutItemAsync(item);

                return resp;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static async Task<dynamic> AddItem(string json, string input)
    {
        try
        {
            using (DynamoDBContext context = new DynamoDBContext(client))
            {
                var item = PutItem(json);

                var resp = await client.PutItemAsync(item);

                return resp;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static PutItemRequest PutItem(string json)
    {
        try
        {
            var request = new PutItemRequest
            {
                TableName = "tb_cliente",
                Item = new Dictionary<string, AttributeValue>()
                    {
                        { "id_cliente", new AttributeValue { N = new Random().Next().ToString() } },
                        { "json_cadastro", new AttributeValue { S = json } }
                    }
            };

            return request;
        }
        catch (Exception)
        {
            throw;
        }
    }
}