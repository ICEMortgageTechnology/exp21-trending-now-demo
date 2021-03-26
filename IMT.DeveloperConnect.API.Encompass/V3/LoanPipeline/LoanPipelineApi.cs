using IMT.DeveloperConnect.API.Client.Http;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMT.DeveloperConnect.API.Encompass.V3.LoanPipeline
{
    public class LoanPipelineApi : SessionBasedApiBase
    {
        private const string Url = "/encompass/v3/loanPipeline";

        public LoanPipelineApi(IApiSession session, IApiClient apiClient) : base(session, apiClient)
        {
        }

        public Task<LoanPipelineItemContract[]> QueryLoanPipeline(LoanPipelineQueryContract loanPipelineQuery)
        {
            ArgumentChecks.IsNotNull(loanPipelineQuery, nameof(loanPipelineQuery));

            return ExecuteNewRequest<LoanPipelineItemContract[]>(Url, Method.Post, request => request.AddJsonContent(loanPipelineQuery));
        }
    }
}
