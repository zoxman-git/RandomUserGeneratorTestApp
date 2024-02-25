using RandomUserGeneratorTestApp.BusinessLogic.ViewModels.Requests;
using Swashbuckle.AspNetCore.Filters;
using System.Text.Json;

namespace RandomUserGeneratorTestApp.Web
{
    public class SwaggerExamplesProvider : IExamplesProvider<UserRequest[]>
    {
        public UserRequest[] GetExamples()
        {
            var usersSampleData = File.ReadAllText(@".\SwaggerSampleRequests\SampleUsersRequest.json"); ;
            
            var users = JsonSerializer.Deserialize<UserRequest[]>(usersSampleData);

            return users;
        }
    }
}
